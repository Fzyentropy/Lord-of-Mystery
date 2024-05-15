using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using random = UnityEngine.Random;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// using UnityEngine.UIElements;

public class Card_Location_Feature_Game_Scene_Exit : MonoBehaviour
{
    
    
    // 卡牌数据
    public Card_Location _cardLocation;         // Card_Location 的卡牌实例

    // 卡牌外观 prefab 各元素指代
    [Header("prefab elements")]
    public SpriteRenderer card_frame;           // 卡牌的 frame，边框
    public SpriteRenderer card_name_tag;        // 卡牌的 name tag，名字栏
    public SpriteRenderer card_image;           // 卡牌的 image，图片
    public TMP_Text card_label;                 // 卡牌的 label，名称
    public SpriteRenderer card_image_mask;      // 卡牌的 image mask，遮罩
    public SpriteRenderer card_shadow;          // 卡牌的 shadow 阴影

    // Panel 相关
    [Header("panel relevance")]
    public GameObject _card_location_panel;       // 点击此卡时，要弹出的 Panel prefab
    public GameObject  showed_panel;             // 打开这张卡对应的 panel 时，记录 打开的 panel 到此参数

    // 是否 Available 相关参数
    public bool card_location_availability = true;     // 此 card location 是否 available
    public bool is_counting_down = false;     // panel 是否在倒计时生产中
    [HideInInspector] public bool is_card_able_to_move;      // 卡牌是否能够 拖拽/移动，根据 _cardLocation 中的 Stable 参数设置
    
    // 进度条
    [Header("Progress Bar")]
    public GameObject progress_bar_prefab;        // 进度条 prefab
    [HideInInspector]public GameObject progress_bar;               // 进度条 指代
    [HideInInspector]public GameObject progress_bar_root;          // 进度条方块的根 指代
    public GameObject progress_bar_position;      // 进度条位置标记 空物体
    [HideInInspector]public TMP_Text countdown_text;               // 显示秒数文本
    

    // Requirement / Outcome 触发条件 字典集 包括 resources 和 body part
    public Dictionary<string, int> required_resources;      // 要消耗的 resources
    public Dictionary<string, int> required_body_parts;     // 要消耗的 body part
    public Dictionary<string, int> produce_resources;
    public Dictionary<string, int> produce_body_parts;      // 产出的 body part

    public List<string> start_effect;       // 卡牌刚生成时的 Effect
    public List<string> start_countdown_effect;     // 卡牌开始 Countdown 时触发的 Effect
    public List<string> special_effect;     // 卡牌 Countdown 结束时触发的 所有 Special Effect

    
    // 特殊卡牌相关

    // Mis Variables
    private Vector3 click_mouse_position;       // 用于点击时记录鼠标的位置
    private Vector3 lastMousePosition;      // 用于记录鼠标拖拽时，前一帧鼠标的位置
    private bool clicking_over_dragging;    // 是 drag 还是 click
    private Vector3 dragging_offset;        // 拖拽时 鼠标位置和物体位置的差值，作 offset

    public int LayerIndex;      // 记录此 GameObject 所处的 layer（“Card_Location")

    public bool isHighlightYellow = false;    // 是否需要黄色高亮，在拖拽需要的 body part 到卡牌上方时为黄色
    public bool isHighlight = false;    // 判断是否需要高亮
    
    private bool yellow_highlight_bodypart_variable_switch = true;     // 用于让当前 card location 被 body part 重叠时，只在第一次重叠时传入此 card location feature 实例到 body part feature 的参数里

    float newCardLocationPositionXOffset = 7f;      // 生成新的 card location 的时候的 X Offset
    float newCardLocationPositionYOffset = -5f;      // 生成新的 card location 的时候的 Y Offset
    private float newBodyPartPositionXOffset = 4f;
    private float newBodyPartPositionYOffset = 8f;

    private bool is_dragging_pick_up_effect_applied;     // 用于记录是否 “抬起” 了卡牌

    private float draw_card_animation_duration = 0.4f;

    [HideInInspector] public Color highlight_color_common = Color.white;
    [HideInInspector] public Color highlight_color_yellow = Color.yellow;

    private bool is_playing_dragging_SFX;       // 是否在播放 drag 音效


    private void Awake()
    {
        Set_Exit_Card_Instance();   // 设置 Exit 卡
    }

    private void Start()
    {
        
        Check_If_Card_Exist();      // 检查卡牌实例是否存在
        Check_Card_JSON_Setting_Soft_Bug();     // 检查卡牌的 JSON 是否有好好设置，有没有设为 0 的参数
        // AddColliderAndRigidbody();      // Exit 卡不需要 Collider 
        Set_Layer_Index();          // 设置 layer 的 index

        
        Initialize_Card();      // 设置卡牌 label，image，初始化卡牌使用次数，等设置
        Initialize_Card_Resource_And_Body_Part();     // 根据 _cardLocation 实例设置 3个字典 - 消耗的 resource，消耗的 body part，生产的 resource，生产的 body part
        Initialize_Start_Countdown_And_Special_Effect();      // Start Effect，Start Countdown Effect，Special Effect 的初始化
        Initialize_Some_Bool_Variables();
        
        // StartCoroutine(Highlight_If_Dragging_Needed_BodyPart());    // 如果拖拽了需要的 body part，则高亮
        // StartCoroutine(Trigger_Any_Start_Effects());        // 如果有任何生成时就要发动的效果，就集成在这里
    }




    void Set_Exit_Card_Instance()
    {
        _cardLocation = GameManager.GM.CardLoader.Get_Card_Location_By_Id("Game_Scene_Card_Exit");
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

        gameObject.name = "Card_" + _cardLocation.Card_Type + "__" +  _cardLocation.Id;     // 设置此 card location 的 GameObject 的名称为 I
        
        
        
        card_frame.color = Color.clear;
        card_name_tag.color = Color.clear;
        card_image.color = Color.clear;
        card_label.text = "";
        card_image_mask.color = Color.clear; 
        card_shadow.color = Color.clear;

    }

    void Initialize_Card_Resource_And_Body_Part()
    {
        
        required_resources = new Dictionary<string, int>
        {
            {"Fund", _cardLocation.Require_Resource.Fund},
            {"Physical_Energy", _cardLocation.Require_Resource.Physical_Energy},
            {"Spiritual_Energy", _cardLocation.Require_Resource.Spiritual_Energy},
            {"Soul", _cardLocation.Require_Resource.Soul},
            {"Spirituality_Infused_Material", _cardLocation.Require_Resource.Spirituality_Infused_Material},
            {"Knowledge", _cardLocation.Require_Resource.Knowledge},
            {"Belief", _cardLocation.Require_Resource.Belief},
            {"Putrefaction", _cardLocation.Require_Resource.Putrefaction},
            {"Madness", _cardLocation.Require_Resource.Madness},
            {"Godhood", _cardLocation.Require_Resource.Godhood},
            {"Death", _cardLocation.Require_Resource.Death}     // Death added
        };

        required_body_parts = new Dictionary<string, int>
        {
            { "Physical_Body", _cardLocation.Require_Body_Part.Physical_Body },
            { "Spirit", _cardLocation.Require_Body_Part.Spirit },
            { "Psyche", _cardLocation.Require_Body_Part.Psyche},
            
            {"Potion", _cardLocation.Require_Body_Part.Potion},
            
            {"Save", _cardLocation.Require_Body_Part.Save}

        };
        
        produce_resources = new Dictionary<string, int>
        {
            {"Fund", _cardLocation.Produce_Resource.Fund},
            {"Physical_Energy", _cardLocation.Produce_Resource.Physical_Energy},
            {"Spiritual_Energy", _cardLocation.Produce_Resource.Spiritual_Energy},
            {"Soul", _cardLocation.Produce_Resource.Soul},
            {"Spirituality_Infused_Material", _cardLocation.Produce_Resource.Spirituality_Infused_Material},
            {"Knowledge", _cardLocation.Produce_Resource.Knowledge},
            {"Belief", _cardLocation.Produce_Resource.Belief},
            {"Putrefaction", _cardLocation.Produce_Resource.Putrefaction},
            {"Madness", _cardLocation.Produce_Resource.Madness},
            {"Godhood", _cardLocation.Produce_Resource.Godhood},
            {"Death", _cardLocation.Produce_Resource.Death}     // Death added
        };
        
        produce_body_parts = new Dictionary<string, int>
        {
            { "Physical_Body", _cardLocation.Produce_Body_Part.Physical_Body },
            { "Spirit", _cardLocation.Produce_Body_Part.Spirit },
            { "Psyche", _cardLocation.Produce_Body_Part.Psyche},
            
            {"Potion", _cardLocation.Produce_Body_Part.Potion},
            
            {"Save", _cardLocation.Require_Body_Part.Save}
        };
        
    }


    void Initialize_Start_Countdown_And_Special_Effect()
    {
        start_effect = new List<string>() { };
        start_countdown_effect = new List<string>() { };
        special_effect = new List<string>() { };

        foreach (var effect in _cardLocation.Start_Effect)
        {
            start_effect.Add(effect);
        }

        foreach (var countdown_effect in _cardLocation.Start_Countdown_Effect)
        {
            start_countdown_effect.Add(countdown_effect);
        }

        foreach (var sp_effect in _cardLocation.Produce_Special_Effect)
        {
            special_effect.Add(sp_effect);
        }
        
    }


    void Initialize_Some_Bool_Variables()
    {
        is_card_able_to_move = !_cardLocation.Stable;        // 根据 Stable，设置 is_card_able_to_drag
        
    }
    
    
    
    
    /*/////////////////////////////////////////////////////////////////////////     I Pointer Handler 方法
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("左键点击");

            if (card_location_availability   // 此卡处于 available 的状态
                // && (Input.mousePosition - click_mouse_position).magnitude < 0.4f)
                && clicking_over_dragging)
            {
                Open_Panel();
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("右键点击");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("鼠标悬停进入");
        
        if (card_location_availability
            && GameManager.GM.InputManager.Dragging_Object == null)     // 鼠标悬停时，如果没拖拽着其他卡牌，
        {
            isHighlight = true;     // 高亮
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("鼠标悬停退出");
        
        isHighlight = false;    // 取消高亮, by 设定高亮参数为 false
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("开始拖拽");
        
        if (card_location_availability)
            GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Card_Click);

        clicking_over_dragging = false;

        dragging_offset = transform.position - eventData.pointerCurrentRaycast.worldPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isHighlight = false; // 取消高亮

        if (card_location_availability)
        {
            IncreaseOrderInLayer();
            
            if (!_cardLocation.Stable)
            {
                // 将 Input Manager 中的 正在拖拽物体 记录为此物体
                GameManager.GM.InputManager.Dragging_Object = gameObject; 
            
                // 更新物体位置逻辑
                transform.position = eventData.pointerCurrentRaycast.worldPosition + dragging_offset;
            }

        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("结束拖拽");

        
        if (card_location_availability)
        {
            // 放下音效
            GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Card_Drop);
        
            // 调整 卡牌的渲染 layer 让其回到原位
            gameObject.layer = LayerIndex; 
            
            // 设置回 原 Order in Layer
            DecreaseOrderInLayer();  
        }
        
        // 释放 Input Manager 中的 正在拖拽 GameObject，设置为空
        GameManager.GM.InputManager.Dragging_Object = null;
        
        
        clicking_over_dragging = true;
    }
    
    
    /////////////////////////////////////////////////////////////////////////     I Pointer Handler 方法        END*/
    
    

    /////////////////////////////////////////////////////////////////////////     On Mouse 系列方法

    
    /*private void OnMouseOver()      // 鼠标悬停的时候，高亮
    {
        if (card_location_availability
            && GameManager.GM.InputManager.Dragging_Object == null)     // 鼠标悬停时，如果没拖拽着其他卡牌，
        {
            isHighlight = true;     // 高亮
        }
    }

    private void OnMouseDown()      // 按下鼠标左键的时候，记录鼠标位置，调整卡牌的渲染 layer，让其到最上面，取消高亮
    {

        if (card_location_availability)
        {
            GameManager.GM.InputManager.Start_Dragging(gameObject);

        }
        
    }

    private void OnMouseDrag() // 当按住鼠标左键的时候，如果移动鼠标（即拖拽），则卡牌随之移动
    {
        
        if (card_location_availability)     
            
        {
            
            // Clear_Highlight_Collider();                             // 取消高亮
            isHighlight = false; // 取消高亮



            // Increase Order In Layer
            // 改变物体的 Layer，Sorting Layer，和 "抬起卡牌" 效果
            {
                
                // 调整卡牌的渲染 layer 层为 "DraggingLayer"
                GameManager.GM.CardEffects.Change_GameObject_Layer(gameObject, "DraggingLayer");

                // IncreaseOrderInLayer();
                GameManager.GM.CardEffects.Change_Order_In_Layer("Dragging",
                    card_frame, card_name_tag, card_image, card_label, card_image_mask, card_shadow);

                // "抬起卡牌"效果
                if (is_card_able_to_move        // 如果这卡能够移动才可以 "抬起"
                    && !is_dragging_pick_up_effect_applied)
                {
                    is_dragging_pick_up_effect_applied = true;
                    GameManager.GM.CardEffects.Apply_Dragging_Pick_Up_Effect(
                        card_frame, card_name_tag, card_image, card_label, card_image_mask);
                }
                
            }
            
            

            // 如果鼠标移动，卡牌随之移动        // 临时，如果 Moveable 才可以移动，为了临时代替 序列 Sequence
            if (is_card_able_to_move)
            {
                
                GameManager.GM.InputManager.On_Dragging();
                
            }
            
        }
        
    }

    private void OnMouseUp()        // 如果此时鼠标的位置和先前按下左键时记录的位置差不多，则为点击，触发点击功能（打开 panel）
    {

        if (card_location_availability) // 此卡处于 available 的状态
        {
            
            GameManager.GM.InputManager.End_Dragging();
            
            
            // Decrease Order In Layer
            // 改变物体的 Layer，Sorting Layer，和 "放下卡牌" 效果
            {
                
                // 调整卡牌的渲染 layer 层为 "Card Location"
                GameManager.GM.CardEffects.Change_GameObject_Layer(gameObject, "Card Location");

                // DecreaseOrderInLayer();   
                GameManager.GM.CardEffects.Change_Order_In_Layer("Cards",
                    card_frame, card_name_tag, card_image, card_label, card_image_mask, card_shadow);

                // "放下卡牌"效果
                if (is_card_able_to_move
                    && is_dragging_pick_up_effect_applied)
                {
                    is_dragging_pick_up_effect_applied = false;
                    GameManager.GM.CardEffects.Apply_Dragging_Put_Down_Effect(
                        card_frame, card_name_tag, card_image, card_label, card_image_mask);
                }
                
            }
            

        }
        
    }

    
    private void OnMouseExit()      // 当鼠标离开卡牌上方时，取消高亮
    {
        isHighlight = false;    // 取消高亮, by 设定高亮参数为 false
    }*/
    
    
    /////////////////////////////////////////////////////////////////////////     On Mouse 系列方法     END
    
    void OnTriggerEnter2D(Collider2D other)     
    {

    }

    /*private void OnTriggerStay2D(Collider2D other)      // 当有 Collider2D 悬停时
    {
        
        if (card_location_availability          // 如果此卡 available
            && !is_counting_down)                   // 如果没在 倒计时
        {
            
            if (other.GetComponent<Card_Body_Part_Feature>() != null)   // 如果该 Collider2D 是 body part
            {
                
                if (GameManager.GM.InputManager.Dragging_Object == other.gameObject // 如果正在拖拽那张 body part
                    && Check_If_Dragging_BodyPart_Is_Need(other.gameObject)) // 且拖拽的那张 Body part 是需要的 Body part
                {
                    other.GetComponent<Card_Body_Part_Feature>().overlapped_card_location_or_panel_slot = gameObject; // 向与此卡重叠的 body part 传入此 card location feature, 确保当前卡被吸收时的唯一性

                    if (other.GetComponent<Card_Body_Part_Feature>().overlapped_card_location_or_panel_slot == gameObject)
                    {
                        isHighlightYellow = true; // 将高亮改为黄色
                    }
                    else
                    {
                        isHighlightYellow = false;
                    }

                }

            }

            if (other.GetComponent<Knowledge_Feature>() != null)    // 如果该 Collider 2D 是 Knowledge Card
            {

                if (GameManager.GM.InputManager.Dragging_Object == other.gameObject // 如果正在拖拽那张 Knowledge Card
                    && Check_If_Dragging_Knowledge_Is_Need(other.gameObject)) // 且拖拽的那张 Knowledge Card 是需要的 Knowledge Card
                {
                    other.GetComponent<Knowledge_Feature>().overlapped_card_location_or_knowledge_slot = gameObject; // 向与此卡重叠的 body part 传入此 card location feature, 确保当前卡被吸收时的唯一性

                    if (other.GetComponent<Knowledge_Feature>().overlapped_card_location_or_knowledge_slot ==
                        gameObject)
                    {
                        isHighlightYellow = true;
                    }
                    else
                    {
                        isHighlightYellow = false;
                    }
                    
                }

            }
            
            
            
        }
        
    }*/

    /*void OnTriggerExit2D(Collider2D other)      // 当悬停的卡牌离开时
    {

        if (card_location_availability)
        {
            if (other.GetComponent<Card_Body_Part_Feature>() != null)
            {
                // 取消黄色 Highlight
                isHighlightYellow = false;

                // “ 取出 ” body part 重叠物体
                other.GetComponent<Card_Body_Part_Feature>().overlapped_card_location_or_panel_slot = null;

            }
            
            if (other.GetComponent<Knowledge_Feature>() != null)
            {
                // 取消黄色 Highlight
                isHighlightYellow = false;

                // “ 取出 ” body part 重叠物体
                other.GetComponent<Knowledge_Feature>().overlapped_card_location_or_knowledge_slot = null;

            }
        }
    }
    */

    
    
    
    // 当卡牌被点击时调用，打开 Panel

    public void Open_Panel()
    {
        if (GameManager.GM.PanelManager.current_panel == null       // 如果 当前没有打开的 panel
            ||                                                      // 或者
            GameManager.GM.PanelManager.current_panel != null &&    // 当前有 panel 已经打开，但不是 card location panel (是其他类型 panel）
            GameManager.GM.PanelManager.current_panel.GetComponent<Card_Location_Panel_Feature>() == null 
            ||                                                      // 或者
            GameManager.GM.PanelManager.current_panel != null &&    // 当前有 panel 已经打开，且是 card location panel，但不是此卡打开的
            GameManager.GM.PanelManager.current_panel.GetComponent<Card_Location_Panel_Feature>() != null &&
            GameManager.GM.PanelManager.current_panel.GetComponent<Card_Location_Panel_Feature>()   
                .attached_card_location_feature != this
            )      
        {

            if (GameManager.GM.PanelManager.current_panel != null)
                GameManager.GM.PanelManager.Close_Current_Panel();              // 有打开的 panel 则关闭 panel
            
            // 生成新的 panel
            GameObject panel = Instantiate(_card_location_panel,        // 实例化 panel
                new Vector3(
                    gameObject.transform.position.x,
                    gameObject.transform.position.y,
                    gameObject.transform.position.z - 2), Quaternion.identity);  
            
            Card_Location_Panel_Feature panel_feature = panel.GetComponent<Card_Location_Panel_Feature>();    // 指代panel的feature脚本
            
            // 播放 Panel 打开 音效
            panel_feature.Play_Panel_Show_Up_Audio();

            panel_feature.Set_Attached_Card(gameObject);        // 将生成的 panel 中的对于生成卡牌的指代设置为此卡
            panel_feature.Set_Sprite(Resources.Load<Sprite>("Image/" + _cardLocation.Image));   // 设置图片
            panel_feature.Set_Label(_cardLocation.Label);                                            // 设置 Label
            panel_feature.Set_Description(_cardLocation.Description);                                // 设置 description
            panel.name = "Card_Location_Panel__" + _cardLocation.Id;                    // 设置 panel 的 GameObject 名称

            showed_panel = panel;       // 记录打开的 panel 实例到 showed panel 参数
            // GameManager.GM.PanelManager.current_panel = panel;      // 将 "打开的 panel" 设置为刚打开的 panel


            if (SceneManager.GetActiveScene().name == "Lord_of_Mystery_Title_Screen")   // 若在 Title Scene，将 Panel 移至中央
            {
                panel.transform.position = GameObject.Find("Title_Screen_Panel_Location").transform.position;
            }
            GameManager.GM.PanelManager.Set_Panel_Reference_And_Scale_Up(panel);
            GameManager.GM.PanelManager.isPanelOpen = true;
            
            

            // panel_feature. Set Resource Buttons

            // panel_feature. Set Body Parts

            // return panel;
            
        }

    }
    
    
    
    
    // 检测当前拖拽的卡牌是否是这张卡能够吸收的 body part，此判定若通过，则可以直接调用 card location panel feature 中的 吸收 body part 方法
    public bool Check_If_Dragging_BodyPart_Is_Need(GameObject draggingObject)     // 传入卡牌 GameObject，比如当前拖拽的卡牌，看是否是 body part，如果是，则判定是不是需要的 body part
    {
        if (draggingObject != null)   // 如果正在 dragging Object
        {
            Card_Body_Part_Feature bodyPartFeature =
                draggingObject.GetComponent<Card_Body_Part_Feature>();     // 尝试获取 body part feature

            if (bodyPartFeature != null)            // 且如果 dragging Object 是 body part，通过检测是否存在 body part feature 判断
            {


                if (GameManager.GM.PanelManager.isPanelOpen)   //  情况 1/2 ： 如果 panel 打开着
                {
                    if (GameManager.GM.PanelManager.current_panel.GetComponent<Card_Location_Panel_Feature>() != null  // 如果打开的 panel 是个 card location panel 
                        && GameManager.GM.PanelManager.current_panel.GetComponent<Card_Location_Panel_Feature>().attached_card == gameObject  // 如果打开这个 panel 的卡是 这张卡
                        && GameManager.GM.PanelManager.current_panel.GetComponent<Card_Location_Panel_Feature>().
                            Find_First_Empty_Body_Part_Type_In_Slots(bodyPartFeature._CardBodyPart.Id) > 0)   // 如果在 panel 上能找到 空的，同类型的 Body Part Slot
                        
                    {
                        
                        
                        if (bodyPartFeature._CardBodyPart.Id == "Potion")       // 如果是 Potion，额外判定等级 （TODO 加上职业判定）
                        {
                                
                            if (bodyPartFeature.gameObject.GetComponent<Potion_Info>() != null        // 如果能 检测到 Potion Info
                                && bodyPartFeature.gameObject.GetComponent<Potion_Info>().potion_sequence != null     // 如果 Potion 中的 序列实例存在
                                && bodyPartFeature.gameObject.GetComponent<Potion_Info>().potion_sequence.Rank     // 如果 Potion 升级的等级 刚好比当前 序列等级高1级
                                == GameManager.GM.Current_Rank - 1)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                                
                        }
                            
                            
                        else       // 如果不是 Potion，就可以过了
                        {
                            return true;
                        }
                        
                    }
                }
                
                
                else        // 情况 2/2 ： 如果 panel 没打开，则直接检测 Card_Location_Feature 的 dictionary 是否标记了需要这个 type 的 body part
                {
                    foreach (var bodyPart in required_body_parts)
                    {
                        if (bodyPartFeature._CardBodyPart.Id == bodyPart.Key && bodyPart.Value > 0)     // 检测当前拖拽的 body part 的 id 是否跟需要的 body part 中的一样
                        {
                            
                            
                            if (bodyPart.Key == "Potion")       // 如果是 Potion，额外判定等级 （TODO 加上职业判定）
                            {
                                
                                if (bodyPartFeature.gameObject.GetComponent<Potion_Info>() != null        // 如果能 检测到 Potion Info
                                    && bodyPartFeature.gameObject.GetComponent<Potion_Info>().potion_sequence != null     // 如果 Potion 中的 序列实例存在
                                    && bodyPartFeature.gameObject.GetComponent<Potion_Info>().potion_sequence.Rank     // 如果 Potion 升级的等级 刚好比当前 序列等级高1级
                                    == GameManager.GM.Current_Rank - 1)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                                
                            }
                            
                            
                            else       // 如果不是 Potion，就可以过了
                            {
                                return true;
                            }
                            
                            
                        }
                    }
                }

                
                
            }
            
        }

        return false;
    }


    // 如果 drag 的是需要的 Body Part，则卡牌高亮
    /*public IEnumerator Highlight_If_Dragging_Needed_BodyPart()
    {
        while (true)
        {
            if (card_location_availability)      // 如果此卡 available
                // && !is_counting_down)      // 如果没有在 倒计时
            {
                
                if (Check_If_Dragging_BodyPart_Is_Need(GameManager.GM.InputManager.Dragging_Object)   // 如果拖拽的是需要的 body part，则高亮，根据是否跟 card location 重叠来判断是否是黄色
                    || Check_If_Dragging_Knowledge_Is_Need(GameManager.GM.InputManager.Dragging_Object))    // 或者拖拽的是需要的 Knowledge，则高亮，根据是否跟 card location 重叠来判断是否是黄色
                {
                    if (!isHighlightYellow)
                        GameManager.GM.CardEffects.Highlight_Collider(GetComponent<BoxCollider2D>(), GetComponent<LineRenderer>(), highlight_color_common);
                    else
                        GameManager.GM.CardEffects.Highlight_Collider(GetComponent<BoxCollider2D>(), GetComponent<LineRenderer>(), highlight_color_yellow);
                }
                else if (isHighlight)       // 如果鼠标悬停让 isHighlight 参数为 true 了，也高亮，高亮白色
                {
                    GameManager.GM.CardEffects.Highlight_Collider(GetComponent<BoxCollider2D>(), GetComponent<LineRenderer>(), highlight_color_common);
                }
                else
                {
                    // Clear Highlight
                    GameManager.GM.CardEffects.Clear_Highlight_Collider(GetComponent<LineRenderer>());
                }
                
            }
            else
            {
                // Clear Highlight
                GameManager.GM.CardEffects.Clear_Highlight_Collider(GetComponent<LineRenderer>());
            }
            
            
            yield return null;
        }
    }*/
    
    
    
    
    
    
    
    public IEnumerator Absorb_Dragging_Body_Parts(GameObject bodyPartToAbsorb)         // 被 card body part feature 调用的 吸收 body part 方法
    {
        Open_Panel();   // 自带是否打开的检测

        yield return new WaitUntil(() => showed_panel.GetComponent<Card_Location_Panel_Feature>().isPanelWellSet);  // 等到 panel well set 了
        
        showed_panel.GetComponent<Card_Location_Panel_Feature>().Absorb_Body_Part_Based_On_Type(bodyPartToAbsorb);        // 调用 panel 中的方法

    }
    
    
    

    
    ////////////////////////////////////////////////////////////////////////////////      开始倒计时 方法
    
    public void Start_Countdown()
    {
        if (card_location_availability          // 如果此卡 available，且没在 countdown，则开始倒计时
            && !is_counting_down)
        {

            // 开始倒计时
            StartCoroutine(Counting_Down_For_Card_Effect());
            
        }
        
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
            
            {
                remainingTime -= timeInterval * GameManager.GM.InputManager.Time_X_Speed;     // 加入时间加速参数！！
            }
            
            // 更新进度条和时间显示
            float progress = (totalTime - remainingTime) / totalTime;
            progress_bar_root.transform.localScale = new Vector3(progress, originalScale.y, originalScale.z);
            countdown_text.text = $"{remainingTime:0.0} s";
        }
        
        
        // 销毁 进度条 prefab
        Destroy(progress_bar);
        is_counting_down = false;   // 设置 "正在倒计时" 为否
        
        
        // 如果倒计时结束时 panel 开着，则也关闭 panel
        if (GameManager.GM.PanelManager.isPanelOpen
            && GameManager.GM.PanelManager.current_panel.GetComponent<Card_Location_Panel_Feature>() != null
            && GameManager.GM.PanelManager.current_panel.GetComponent<Card_Location_Panel_Feature>()
                .attached_card_location_feature == this)
        {
            GameManager.GM.PanelManager.Close_Current_Panel();
        }

        // 触发卡牌效果
        Trigger_Card_Effects();
        
        
        // 如果需要 body part，则吐出用完的 body part
        if (_cardLocation.Id != "Title_Card_Start")
            Return_BodyParts_After_Progress();

        
        if (_cardLocation.Repeatable)   // 临时，如果是 可重复的，则反复循环
        {
            Start_Countdown();
        }
    }


    /*void Show_Absorbed_Body_Part_Icon_On_Side()
    {

        root_of_absorbed_body_part_icon = new GameObject("Root_Of_Body_Part_Icons");
        root_of_absorbed_body_part_icon.transform.parent = this.transform;

        int icon_counter = 1;
        float icon_image_scale = 0.33f;

        foreach (var bodyPart in required_body_parts)
        {
            if (bodyPart.Value > 0)
            {
                // 每一个 value 都生成 icon
                for (int i = bodyPart.Value; i > 0; i--)
                {
                    GameObject icon = new GameObject("Icon"+icon_counter);
                    icon.AddComponent<SpriteRenderer>();
                    
                    
                    if (bodyPart.Key == "Physical_Body")        // 寻找 Physical Body icon
                    {
                        icon.GetComponent<SpriteRenderer>().sprite =
                            Resources.Load<Sprite>("Image/Image_Physical_Body");
                    }
                    if (bodyPart.Key == "Spirit")        // 寻找 Spirit icon
                    {
                        icon.GetComponent<SpriteRenderer>().sprite =
                            Resources.Load<Sprite>("Image/Image_Spirit");
                    }
                    if (bodyPart.Key == "Psyche")        // 寻找 Psyche icon
                    {
                        icon.GetComponent<SpriteRenderer>().sprite =
                            Resources.Load<Sprite>("Image/Image_Psyche");
                    }
                    /*if (bodyPart.Key == "Save")        // 寻找 Save icon
                    {
                        icon.GetComponent<SpriteRenderer>().sprite =
                            Resources.Load<Sprite>("Image/Image_Save");
                    }#1#

                    icon.GetComponent<SpriteRenderer>().sortingLayerName = "Cards";     // 调整 Sorting Layer

                    icon.transform.localScale = new Vector3(                    // 调整图片 scale
                        icon_image_scale * icon.transform.localScale.x,
                        icon_image_scale * icon.transform.localScale.y,
                        icon.transform.localScale.z);

                    icon.transform.parent = gameObject.transform;       // 变成此卡的子物体
                    
                    icon.transform.localPosition =
                        GameObject.Find("Body_Part_Location_" + icon_counter).transform.localPosition;   // 调整位置
                    
                    icon.transform.parent = root_of_absorbed_body_part_icon.transform;      // 变成 root 的子物体
                    
                    icon_counter++;     // 计数 +1


                }
            }
        }


    }*/


    /*void Hide_Absorbed_Body_Part_Icon_On_Side()     
    {
        Destroy(root_of_absorbed_body_part_icon);

    }*/
    
    
    
    
    // 吐出 body part 实际执行
    public void Return_BodyParts_After_Progress()
    {

        float bodyPartReturnNumberCount = 0;        // body part 计数，用于计算生成的 body part 的 x 轴间距
        float bodyPartReturnXOffset = 5f;           // 如果吐出多个 body part，每个之间的 x 轴间距 
        float bodyPartReturnYOffset = -9f;       // 吐出 body part 时候 body part 距离这个 card location 的 y 轴位移
        
        foreach (var body_part in required_body_parts)
        {

            if (body_part.Value > 0        // 如果是需要的 Body Part
                && body_part.Key != "Potion"      // 且 不是 Potion (Potion 不会被返还)
                && body_part.Key != "Save")       // 且 不是 Save (Save 不会被返还)
            {

                for (int i = 1; i <= body_part.Value; i++)
                {
                    GameManager.GM.BodyPartManager.Generate_Body_Part_To_Board(
                        body_part.Key,
                        gameObject.transform.position, 
                        new Vector3(
                            gameObject.transform.position.x + bodyPartReturnNumberCount * bodyPartReturnXOffset, 
                            gameObject.transform.position.y + bodyPartReturnYOffset,
                            gameObject.transform.position.z - 1 
                            )
                        );

                    bodyPartReturnNumberCount++;

                }
            }
        }
        
        
        
    }
    
    
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////                 卡牌效果                   //////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    
    
    
    
    /////////////////     倒计时结束后，触发卡牌效果
    
    
    void Trigger_Card_Effects()
    {
        // 根据_cardLocation触发相应的效果
        // 比如：Produce_Resource, Produce_Message 等
        // ...
        
        
        // Card Effect 音效
        GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Trigger_Card_Effect);
        
        
        // 高优先级 Special Effect， 比如加载场景，退出游戏等

        if (special_effect.Count > 0) // 简单写的，根据 Special Effect 的 list 触发 Special Effect 
        {
            foreach (var special_effect in special_effect)
            {
                if (special_effect == "Start_Game")
                {
                    Start_Game();
                }
                
                if (special_effect == "Exit_Game")
                {
                    Exit_Game();
                }


            }
        }
        
        
        

        // Produce Message

        foreach (var MessageString in _cardLocation.Produce_Message)
        {
            GameManager.GM.Generate_Message(MessageString);
        }
        
        
        
        // Special Effect
        
        if (special_effect.Count > 0)       // 简单写的，根据 Special Effect 的 list 触发 Special Effect 
        {
            foreach (var special_effect in special_effect)
            {

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



    }   // Trigger_Card_Effect  END

    
    


    void Start_Game()
    {
        SceneManager.LoadScene("Lord_of_Mystery");
        Destroy(gameObject);
    }

    void Exit_Game()
    {
        Application.Quit();
    }
    

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
