using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using random = UnityEngine.Random;
// using UnityEngine.UIElements;

public class Card_Location_Feature : MonoBehaviour
{

    public Card_Location _cardLocation;         // Card_Location 的卡牌实例

    public Resource_Data _ResourceData;         // 用于跟 资源管理脚本实例 传值
    
    // 卡牌数据
    public TMP_Text card_label;                 // 卡牌的 label，名称
    public SpriteRenderer card_image;           // 卡牌的 image，图片

    // Panel 相关
    public GameObject _card_location_panel;       // 点击此卡时，要弹出的 Panel prefab
    public GameObject  showed_panel;             // 打开这张卡对应的 panel 时，记录 打开的 panel 到此参数
    
    public List<Button> resource_buttons;       
    public List<TMP_Text> resource_number;
    public List<GameObject> body_part_slots;
    // public Button start_button;
    
    // 进度
    public bool is_counting_down = false;     // panel 是否在倒计时生产中
    
    // 进度条
    public GameObject progress_bar_prefab;        // 进度条 prefab
    public GameObject progress_bar;               // 进度条 指代
    public GameObject progress_bar_root;          // 进度条方块的根 指代
    public GameObject progress_bar_position;      // 进度条位置标记 空物体
    public TMP_Text countdown_text;               // 显示秒数文本
    

    // Requirement 触发条件 字典集 包括 resources 和 body part
    public Dictionary<string, int> required_resources;      // 要消耗的 resources
    public Dictionary<string, int> required_body_parts;     // 要消耗的 body part

    // Outcome 触发结果 字典集 resource
    public Dictionary<string, int> produce_resources;

    public int use_time_counter;
    
    
    // Mis Variables
    private Vector3 click_mouse_position;       // 用于点击时记录鼠标的位置
    private Vector3 lastMousePosition;      // 用于记录鼠标拖拽时，前一帧鼠标的位置

    public int LayerIndex;      // 记录此 GameObject 所处的 layer（“Card_Location")
    
    public GameObject cardFrame;       // 临时 原 physical body 卡牌图片，用于调整 Order in Layer

    public bool isHighlightYellow = false;    // 是否需要黄色高亮，在拖拽需要的 body part 到卡牌上方时为黄色
    public bool isHighlight = false;    // 判断是否需要高亮
    
    private bool yellow_highlight_bodypart_variable_switch = true;     // 用于让当前 card location 被 body part 重叠时，只在第一次重叠时传入此 card location feature 实例到 body part feature 的参数里


    private void Start()
    {
        Check_If_Card_Exist();      // 检查卡牌实例是否存在
        Check_Card_JSON_Setting_Soft_Bug();     // 检查卡牌的 JSON 是否有好好设置，有没有设为 0 的参数
        AddColliderAndRigidbody();      // 如果没加 collider 和 rigidbody，则加上
        Set_Layer_Index();          // 设置 layer 的 index
        {
            LayerIndex = gameObject.layer;
        }
        
        Initialize_Card();      // 设置卡牌 label，image，初始化卡牌使用次数，等设置
        Initialize_Card_Resource();     // 根据 Card_Location 实例设置 3个字典 - 消耗的resource，消耗的body part，生产的resource

        
        StartCoroutine(Highlight_If_Dragging_Needed_BodyPart());    // 如果拖拽了需要的 body part，则高亮

    }

    
    
    
    
    void Check_If_Card_Exist()      // 检查 _cardLocation 卡牌实例是否为空，如果 _cardlocation 卡牌实例为空，则报错
    {
        if (_cardLocation == null)
        {
            throw new NullReferenceException("Card Location 卡牌实例不存在");
        }
    }

    void Check_Card_JSON_Setting_Soft_Bug()     // 检查卡牌的 JSON 是否有好好设置
    {
        if (_cardLocation.Time <= 0)
        {
            Debug.LogWarning("警告：卡牌的倒计时时间设置为0");
        }

        if (_cardLocation.Use_Time == 0)
        {
            Debug.LogWarning("警告：卡牌的使用次数设置为0");
        }
    }
    
    
    // 检测是否存在 Collider 和 Rigidbody，没有则添加并设置
    private void AddColliderAndRigidbody()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            boxCollider.size = new Vector2(5.6f, 7.8f);
        }

        // 检查是否存在Rigidbody2D组件，如果不存在则添加它并设置为Kinematic
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            rb2d = gameObject.AddComponent<Rigidbody2D>();
            rb2d.isKinematic = true;
        }
    }

    void Set_Layer_Index()      // 设置 layer 的 index
    {
        LayerIndex = gameObject.layer;
    }
    
    
    // 初始化卡牌图片和描述，根据 _cardlocation 实例中的数据设置相关参数
    void Initialize_Card()      
    {
        card_label.text = _cardLocation.Label;        // 设置 游戏场景中卡牌 名称
        card_image.sprite = Resources.Load<Sprite>("Image/" + _cardLocation.Image);          // 加载 id 对应的图片
        use_time_counter = _cardLocation.Use_Time == -1 ? int.MaxValue : _cardLocation.Use_Time;    // 初始化卡牌的使用次数

    }

    void Initialize_Card_Resource()
    {
        
        required_resources = new Dictionary<string, int>
        {
            {"Fund", _cardLocation.Require_Resource.Fund},
            {"Physical_Energy", _cardLocation.Require_Resource.Physical_Energy},
            {"Spirit", _cardLocation.Require_Resource.Spirit},
            {"Soul", _cardLocation.Require_Resource.Soul},
            {"Spirituality_Infused_Material", _cardLocation.Require_Resource.Spirituality_Infused_Material},
            {"Knowledge", _cardLocation.Require_Resource.Knowledge},
            {"Belief", _cardLocation.Require_Resource.Belief},
            {"Putrefaction", _cardLocation.Require_Resource.Putrefaction},
            {"Madness", _cardLocation.Require_Resource.Madness},
            {"Godhood", _cardLocation.Require_Resource.Godhood}
        };

        required_body_parts = new Dictionary<string, int>
        {
            { "Physical_Body", _cardLocation.Require_Body_Part.Physical_Body },
            { "Spirit", _cardLocation.Require_Body_Part.Spirit },
            { "Psyche", _cardLocation.Require_Body_Part.Psyche}

        };
        
        produce_resources = new Dictionary<string, int>
        {
            {"Fund", _cardLocation.Produce_Resource.Fund},
            {"Physical_Energy", _cardLocation.Produce_Resource.Physical_Energy},
            {"Spirit", _cardLocation.Produce_Resource.Spirit},
            {"Soul", _cardLocation.Produce_Resource.Soul},
            {"Spirituality_Infused_Material", _cardLocation.Produce_Resource.Spirituality_Infused_Material},
            {"Knowledge", _cardLocation.Produce_Resource.Knowledge},
            {"Belief", _cardLocation.Produce_Resource.Belief},
            {"Putrefaction", _cardLocation.Produce_Resource.Putrefaction},
            {"Madness", _cardLocation.Produce_Resource.Madness},
            {"Godhood", _cardLocation.Produce_Resource.Godhood}
        };
        
    }


    private void OnMouseOver()      // 鼠标悬停的时候，高亮
    {
        // 高亮
        if (GameManager.GM.InputManager.Dragging_Object == null) // 鼠标悬停时，如果没拖拽着其他卡牌，则高亮
            isHighlight = true;
    }

    private void OnMouseDown()      // 按下鼠标左键的时候，记录鼠标位置，调整卡牌的渲染 layer，让其到最上面，取消高亮
    {
        // 记录鼠标位置
        click_mouse_position = Input.mousePosition;
        lastMousePosition = Input.mousePosition;

        // 取消高亮
        

        // 调整卡牌的渲染 layer，让其到最上面
        gameObject.layer = LayerMask.NameToLayer("DraggingLayer");  // 调用系统方法来找到 "Dragging Layer"对应的 Index，并设置
        IncreaseOrderInLayer();
    }

    private void OnMouseDrag()      // 当按住鼠标左键的时候，如果移动鼠标（即拖拽），则卡牌随之移动
    {
        GameManager.GM.InputManager.Dragging_Object = gameObject;       // 将 Input Manager 中的 正在拖拽物体 记录为此物体
        // Clear_Highlight_Collider();                             // 取消高亮
        isHighlight = false;                             // 取消高亮
        
        // 如果鼠标移动，卡牌随之移动
        // float mouse_drag_sensitivity = 0.05f;
        Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(lastMousePosition);
        delta.z = 0;
        gameObject.transform.position += delta;
        lastMousePosition = Input.mousePosition;
    }

    private void OnMouseUp()        // 如果此时鼠标的位置和先前按下左键时记录的位置差不多，则为点击，触发点击功能（打开 panel）
    {
        
        if ((Input.mousePosition - click_mouse_position).magnitude < 0.4) // 判断此时鼠标的位置和记录的位置，如果差不多即视为点击，触发点击功能
        {
            Open_Panel();   // 打开 panel
        }

        GameManager.GM.InputManager.Dragging_Object = null;     // 释放 Input Manager 中的 正在拖拽 GameObject，设置为空
        
        // 调整 卡牌的渲染 layer 让其回到原位
        gameObject.layer = LayerIndex; 
        DecreaseOrderInLayer();     // 设置回 原 Order in Layer
    }

    private void OnMouseExit()      // 当鼠标离开卡牌上方时，取消高亮
    {
        isHighlight = false;    // 取消高亮, by 设定高亮参数为 false
    }
    
    void OnTriggerEnter2D(Collider2D other)     
    {

    }

    private void OnTriggerStay2D(Collider2D other)      // 当有卡牌悬停时
    {
        if (other.CompareTag("Body_Parts"))
        {
            if (GameManager.GM.InputManager.Dragging_Object == other.gameObject)
            {
                other.GetComponent<Card_Body_Part_Feature>().overlapped_card_location = gameObject;    // 向与此卡重叠的 body part 传入此 card location feature, 确保当前卡被吸收时的唯一性

                if (other.GetComponent<Card_Body_Part_Feature>().overlapped_card_location == gameObject)
                {
                    isHighlightYellow = true;   // 将高亮改为黄色
                }
                else
                {
                    isHighlightYellow = false;
                }
                
            }
            
        }
        
    }

    void OnTriggerExit2D(Collider2D other)      // 当悬停的卡牌离开时
    {
        
        if (other.gameObject.CompareTag("Body_Parts"))
        {
            // 取消黄色 Highlight
            isHighlightYellow = false;

            // “ 取出 ” body part 重叠物体
            other.GetComponent<Card_Body_Part_Feature>().overlapped_card_location = null;

        }
    }
    
    
    


    // 当卡牌被点击时调用，打开 Panel -2023_12_13

    public GameObject Open_Panel_2023_12_13()
    {
        
        GameObject panel = Instantiate(_card_location_panel, gameObject.transform.position, Quaternion.identity);  // 实例化 panel
        Card_Location_Panel_Feature panel_feature = panel.GetComponent<Card_Location_Panel_Feature>();    // 指代panel的feature脚本
        showed_panel = panel;
        
        if (GameManager.GM.PanelManager.current_panel != null && GameManager.GM.PanelManager.current_panel != showed_panel) // 如果有 panel 打开但不是此 panel，关闭它
        {
            GameManager.GM.PanelManager.Close_Current_Panel();
        }
        GameManager.GM.PanelManager.isPanelOpen = true;        // 设置 Panel Manager 中的 是否打开panel参数为 true
        
        
        

        panel_feature.Set_Attached_Card(gameObject);        // 将生成的 panel 中的对于生成卡牌的指代设置为此卡
        
        panel_feature.Set_Sprite(Resources.Load<Sprite>("Image/" + _cardLocation.Image));   // 设置图片
        panel_feature.Set_Label(_cardLocation.Label);                                            // 设置 Label
        panel_feature.Set_Description(_cardLocation.Description);                                // 设置 description
        
        // panel_feature. Set Resource Buttons
        
        
        // panel_feature. Set Body Parts
        

        return panel;
    }
    
    // 当卡牌被点击时调用，打开 Panel

    public void Open_Panel()
    {
        if (GameManager.GM.PanelManager.current_panel == null       // 如果 当前 panel 为空（没有已经打开的 panel）
            ||                                                      // 或者
            GameManager.GM.PanelManager.current_panel != null &&    // 当前 panel 不为空（说明有 panel 已经打开），且打开的 panel 不是此卡打开的
            GameManager.GM.PanelManager.current_panel.GetComponent<Card_Location_Panel_Feature>()   
                .attached_card_location_feature != this)      
        {
            if (GameManager.GM.PanelManager.current_panel != null)
                GameManager.GM.PanelManager.Close_Current_Panel();              // 有打开的 panel 则关闭 panel
            
            // 生成新的 panel
            GameObject panel = Instantiate(_card_location_panel, gameObject.transform.position, Quaternion.identity);  // 实例化 panel
            Card_Location_Panel_Feature panel_feature = panel.GetComponent<Card_Location_Panel_Feature>();    // 指代panel的feature脚本

            panel_feature.Set_Attached_Card(gameObject);        // 将生成的 panel 中的对于生成卡牌的指代设置为此卡
            panel_feature.Set_Sprite(Resources.Load<Sprite>("Image/" + _cardLocation.Image));   // 设置图片
            panel_feature.Set_Label(_cardLocation.Label);                                            // 设置 Label
            panel_feature.Set_Description(_cardLocation.Description);                                // 设置 description

            showed_panel = panel;       // 记录打开的 panel 实例到 showed panel 参数
            // GameManager.GM.PanelManager.current_panel = panel;      // 将 "打开的 panel" 设置为刚打开的 panel
            GameManager.GM.PanelManager.Set_Panel_Reference_And_Scale_Up(panel);
            GameManager.GM.PanelManager.isPanelOpen = true;
            

            // panel_feature. Set Resource Buttons

            // panel_feature. Set Body Parts

            // return panel;
            
        }

    }



    public void Highlight_Collider(Color color)        // 利用 collider和 Line Renderer 来高亮 collider 边缘
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 5; // 四个角加上第一个点重复一次以闭合

        // 计算BoxCollider2D的四个角
        Vector3[] corners = new Vector3[5];
        corners[0] = collider.offset + new Vector2(-collider.size.x, -collider.size.y) * 0.5f;
        corners[1] = collider.offset + new Vector2(collider.size.x, -collider.size.y) * 0.5f;
        corners[2] = collider.offset + new Vector2(collider.size.x, collider.size.y) * 0.5f;
        corners[3] = collider.offset + new Vector2(-collider.size.x, collider.size.y) * 0.5f;
        corners[4] = corners[0]; // 闭合线段

        // 转换到世界坐标
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = transform.TransformPoint(corners[i]);
            lineRenderer.SetPosition(i, corners[i]);
        }

        // 设置材质和颜色
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = color; // 高亮颜色
        lineRenderer.endColor = color; // 高亮颜色
    }

    public void Clear_Highlight_Collider()      // 将 Line Renderer 的颜色设置为 透明
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.startColor = Color.clear; // 高亮颜色
            lineRenderer.endColor = Color.clear; // 高亮颜色
        }
    }
    

    /////////////啰嗦版 检测当前拖拽的卡牌是否是这张卡能够吸收的 body part，此判定若通过，则可以直接调用 card location panel feature 中的 吸收 body part 方法
    /*public bool Check_If_Dragging_BodyPart_Is_Need()     
    {
        if (GameManager.GM.InputManager.Dragging_Object != null)   // 如果正在 dragging Object
        {
            Card_Body_Part_Feature bodyPartFeature =
                GameManager.GM.InputManager.Dragging_Object.GetComponent<Card_Body_Part_Feature>();     // 尝试获取 body part feature
            

            

            if (bodyPartFeature != null)            // 且如果 dragging Object 是 body part，通过检测是否存在 body part feature 判断
            {
                Debug.Log("Body Part Type： " + bodyPartFeature._CardBodyPart.Id);
                foreach (var GG in required_body_parts)
                {
                    Debug.Log("Required body part : "+ GG.Key);
                }
                
                
                if (GameManager.GM.PanelManager.isPanelOpen)        // 如果 panel 打开着
                {

                    Card_Location_Panel_Feature cardLocationPanelFeature =
                        GameManager.GM.PanelManager.current_panel.GetComponent<Card_Location_Panel_Feature>();

                    if (cardLocationPanelFeature != null)      // 如果打开的 panel 是个 card location panel, 通过检测是否存在 card location panel feature 判断
                    {

                        if (cardLocationPanelFeature.attached_card == gameObject)   // 如果打开这个 card location panel 的卡是 这张卡
                        {
                            
                            if (cardLocationPanelFeature.Find_First_Body_Part_Type_In_Slots
                                    (bodyPartFeature._CardBodyPart.Id)      // 传入当前拖拽的 body part 的类型 string
                                > 0) // 如果打开的这个 card location panel 中，仍可以吸收 该类型的 body part，则返回 true 
                            {
                                Debug.Log(cardLocationPanelFeature.Find_First_Body_Part_Type_In_Slots
                                    (bodyPartFeature._CardBodyPart.Id));
                                return true;
                            }
                            
                        }
                        else   // 打开的 card location panel 不是跟这张卡绑定的
                        {
                            foreach (var bodyPart in required_body_parts)
                            {
                                if (bodyPartFeature._CardBodyPart.Id == bodyPart.Key && bodyPart.Value > 0)     // 检测当前拖拽的 body part 的 id 是否跟需要的 body part 中的一样
                                {                                                           // TODO 将来还要加上对是否吸收满了 body part 的检测
                                    return true;
                                }
                            }
                        }

                    }
                    else   // 打开的 panel 不是 card location panel 的话
                    {
                        foreach (var bodyPart in required_body_parts)
                        {
                            if (bodyPartFeature._CardBodyPart.Id == bodyPart.Key && bodyPart.Value > 0)     // 检测当前拖拽的 body part 的 id 是否跟需要的 body part 中的一样
                            {                                                           // TODO 将来还要加上对是否吸收满了 body part 的检测
                                return true;
                            }
                        }
                    }
                    
                }
                else   // panel 没打卡的话
                {
                    foreach (var bodyPart in required_body_parts)
                    {
                        if (bodyPartFeature._CardBodyPart.Id == bodyPart.Key && bodyPart.Value > 0)     // 检测当前拖拽的 body part 的 id 是否跟需要的 body part 中的一样
                        {                                                           // TODO 将来还要加上对是否吸收满了 body part 的检测
                            return true;
                        }
                    }
                }

            }
            
        }

        return false;
    }*/
    
    // 检测当前拖拽的卡牌是否是这张卡能够吸收的 body part，此判定若通过，则可以直接调用 card location panel feature 中的 吸收 body part 方法
    public bool Check_If_Dragging_BodyPart_Is_Need(GameObject draggingObject)     // 传入卡牌 GameObject，比如当前拖拽的卡牌，看是否是 body part，如果是，则判定是不是需要的 body part
    {
        if (draggingObject != null)   // 如果正在 dragging Object
        {
            Card_Body_Part_Feature bodyPartFeature =
                draggingObject.GetComponent<Card_Body_Part_Feature>();     // 尝试获取 body part feature

            if (bodyPartFeature != null)            // 且如果 dragging Object 是 body part，通过检测是否存在 body part feature 判断
            {
                
                if (GameManager.GM.PanelManager.isPanelOpen &&      // 如果 panel 打开着
                    GameManager.GM.PanelManager.current_panel.GetComponent<Card_Location_Panel_Feature>() != null &&    // 如果打开的 panel 是个 card location panel 
                    GameManager.GM.PanelManager.current_panel.GetComponent<Card_Location_Panel_Feature>().attached_card == gameObject &&  // 如果打开这个 panel 的卡是 这张卡
                    GameManager.GM.PanelManager.current_panel.GetComponent<Card_Location_Panel_Feature>().
                        Find_First_Empty_Body_Part_Type_In_Slots(bodyPartFeature._CardBodyPart.Id) > 0)     // 传入当前拖拽的 body part 的类型 string)  
                {
                    return true;
                }
                else
                {
                    foreach (var bodyPart in required_body_parts)
                    {
                        if (bodyPartFeature._CardBodyPart.Id == bodyPart.Key && bodyPart.Value > 0)     // 检测当前拖拽的 body part 的 id 是否跟需要的 body part 中的一样
                        {                                                          
                            return true;
                        }
                    }
                }

            }
            
        }

        return false;
    }

    public IEnumerator Highlight_If_Dragging_Needed_BodyPart()
    {
        while (true)
        {
            if (Check_If_Dragging_BodyPart_Is_Need(GameManager.GM.InputManager.Dragging_Object))   // 如果拖拽的是需要的 body part，则高亮，根据是否跟 card location 重叠来判断是否是黄色
            {
                if (!isHighlightYellow)
                    Highlight_Collider(Color.white);
                else
                    Highlight_Collider(Color.yellow);
            }
            else if (isHighlight)       // 如果鼠标悬停让 isHighlight 参数为 true 了，也高亮，高亮白色
            {
                Highlight_Collider(Color.white);
            }
            else
            {
                Clear_Highlight_Collider();
            }
            
            yield return null;
        }
    }
    
    
    public void IncreaseOrderInLayer()       // 提高 卡牌的 Order in Layer 数值，以让卡牌在最上方渲染
    {
        card_label.GetComponent<Renderer>().sortingLayerName = "Dragging"; 
        card_image.sortingLayerName = "Dragging"; 
        cardFrame.GetComponent<SpriteRenderer>().sortingLayerName = "Dragging"; 
    }
    public void DecreaseOrderInLayer()       // 提高 卡牌的 Order in Layer 数值，以让卡牌在最上方渲染
    {
        card_label.GetComponent<Renderer>().sortingLayerName = "Cards";
        card_image.sortingLayerName = "Cards";
        cardFrame.GetComponent<SpriteRenderer>().sortingLayerName = "Cards";
    }
    
    
    
    
    
    
    public IEnumerator Absorb_Dragging_Body_Parts(GameObject bodyPartToAbsorb)         // 被 card body part feature 调用的 吸收 body part 方法
    {
        Open_Panel();   // 自带是否打开的检测

        yield return new WaitUntil(() => showed_panel.GetComponent<Card_Location_Panel_Feature>().isPanelWellSet);  // 等到 panel well set 了
        
        showed_panel.GetComponent<Card_Location_Panel_Feature>().Absorb_Body_Part_Based_On_Type(bodyPartToAbsorb);        // 调用 panel 中的方法

    }

     
    // 检查 Require_Body_Part 和 Require_Resource 是否满足条件
    public void CheckAndFulfillConditions()
    {
        
        
    }

    
    ////////////////////////////////////////////////////////////////////////////////      开始倒计时 方法
    public void Start_Countdown()
    {
        StartCoroutine(Counting_Down_For_Card_Effect());
    }
    
    
    // 倒计时协程，包括进度条的功能
    
    IEnumerator Counting_Down_For_Card_Effect()
    {
        is_counting_down = true;   // 设置 "正在倒计时" 为是
        
        float totalTime = _cardLocation.Time;   // 从 JSON 获取总计时
        float remainingTime = totalTime;        // 设置倒计时参数
        float timeInterval = 0.05f;              // 设置进度条更新的时间间隔

        // 实例化 进度条 prefab
        progress_bar = Instantiate(progress_bar_prefab, transform.position, Quaternion.identity);
        progress_bar.transform.localPosition = progress_bar_position.transform.position;
        progress_bar.transform.parent = transform;
        progress_bar_root = progress_bar.transform.Find("Bar_Root").gameObject;
        countdown_text = progress_bar.GetComponentInChildren<TMP_Text>();
        
        // 初始化进度条和时间显示
        Vector3 originalScale = progress_bar_root.transform.localScale;      // 记录 original scale 设置为 1
        progress_bar_root.transform.localScale = new Vector3(0, originalScale.y, originalScale.z);  // 进度条 scale 设置为 0
        countdown_text.text = $"{remainingTime:0.0} S";                   // 将 剩余时间 用 小数点后 1位 格式来显示

        while (remainingTime > 0)
        {
            // 等待 timeInterval时间长度 - 0.05 秒
            yield return new WaitForSeconds(timeInterval);
            remainingTime -= timeInterval;

            // 更新进度条和时间显示
            float progress = (totalTime - remainingTime) / totalTime;
            progress_bar_root.transform.localScale = new Vector3(progress, originalScale.y, originalScale.z);
            countdown_text.text = $"{remainingTime:0.0} S";
        }
        
        
        // 销毁 进度条 prefab
        Destroy(progress_bar);
        is_counting_down = false;   // 设置 "正在倒计时" 为否

        // 触发卡牌效果
        Trigger_Card_Effects();
    }
    
    
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////                 卡牌效果                   //////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    
    /////////////////     触发卡牌效果
    
    void Trigger_Card_Effects()
    {
        // 根据_cardLocation触发相应的效果
        // 比如：Produce_Resource, Produce_Message 等
        // ...
        
        
        
        // Produce Resource

        foreach (var resource in produce_resources)     // 对于资源字典里的每个资源
        {
            if (resource.Value > 0)         // 假如要 produce 的资源大于 0，则触发 resource manager 里的 add 方法
            {
                GameManager.GM.ResourceManager.Add_Resource(resource.Key, resource.Value, gameObject.transform.position);
            }
        }

        
        // Special Effect
        
        if (_cardLocation.Produce_Special_Effect.Count > 0)       // 简单写的，根据 Special Effect 的 list 触发 Special Effect 
        {
            foreach (var special_effect in _cardLocation.Produce_Special_Effect)
            {
                if (special_effect == "Draw_A_Tarot_Card")
                {
                    Draw_A_Tarot_Card();
                }

                if (special_effect == "")
                {
                    
                }
                
                if (special_effect == "")
                {
                    
                }
                
                if (special_effect == "")
                {
                    
                }
                
                if (special_effect == "")
                {
                    
                }
                
                if (special_effect == "")
                {
                    
                }
                
                if (special_effect == "")
                {
                    
                }
            }
        }
        
        use_time_counter--;         // 记一次使用次数，假如 use time 有限，则使用一定次数就销毁
        Check_Use_Time();
    }

    
    // 检查卡牌效果使用次数 是否超过 限制的次数，超过即销毁（TODO 销毁动画）
    // 需要在 每次使用卡牌效果时 调用
    void Check_Use_Time()
    {
        if (use_time_counter <= 0)
        {
            // TODO 销毁动画，写一个方法，或者做一个特效，或者shader
            Destroy(gameObject);
        }
    }

    
    void Draw_A_Tarot_Card()
    {
        int randomElement = random.Range(0, GameManager.GM.CardLoader.Body_Part_Card_List.Count - 1);
        GameManager.GM.Generate_Card_Body_Part(GameManager.GM.CardLoader.Body_Part_Card_List[randomElement].Id);
    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
