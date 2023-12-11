using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject _card_location_panel;       // 点击此卡时，要弹出的 Panel
    
    public List<Button> resource_buttons;       
    public List<TMP_Text> resource_number;
    public List<GameObject> body_part_slots;
    // public Button start_button;
    
    // 进度条
    public GameObject progress_bar_prefab;        // 进度条 prefab
    public GameObject progress_bar;               // 进度条 指代
    public GameObject progress_bar_root;          // 进度条方块的根 指代
    public GameObject progress_bar_position;      // 进度条位置标记 空物体
    public TMP_Text countdown_text;               // 显示秒数文本
    

    // Requirement 触发条件 字典集 包括 resources 和 body part
    public Dictionary<string, int> required_resources;
    public Dictionary<string, int> required_body_parts;

    // Outcome 触发结果 字典集 resource
    public Dictionary<string, int> produce_resources;

    public int use_time_counter;
    

    private void Start()
    {
        Check_If_Card_Exist();      // 检查卡牌实例是否存在
        Check_Card_JSON_Setting_Soft_Bug();     // 检查卡牌的 JSON 是否有好好设置，有没有设为 0 的参数
        AddColliderAndRigidbody();      // 如果没加 collider 和 rigidbody，则加上
        
        Initialize_Card();
        Initialize_Card_Resource();
        
        // StartCountdown(); // for test
        
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







    // 当卡牌被点击时调用，打开 Panel

    public GameObject Open_Panel()
    {
        InputManager.isInfoPanelOut = true;        // 设置 Input Manager 中的 是否打开panel参数为 true
        
        GameObject panel = Instantiate(_card_location_panel, gameObject.transform.position, Quaternion.identity);  // 实例化 panel
        Card_Location_Panel_Feature panel_feature = panel.GetComponent<Card_Location_Panel_Feature>();    // 指代panel的feature脚本
        
        panel_feature.Set_Attached_Card(gameObject);        // 将生成的 panel 中的对于生成卡牌的指代设置为此卡
        
        panel_feature.Set_Sprite(Resources.Load<Sprite>("Image/" + _cardLocation.Image));   // 设置图片
        panel_feature.Set_Label(_cardLocation.Label);                                            // 设置 Label
        panel_feature.Set_Description(_cardLocation.Description);                                // 设置 description
        
        // panel_feature. Set Resource Buttons
        
        
        // panel_feature. Set Body Parts
        

        return panel;
    }
    
    
    

    
    // 检查 Require_Body_Part 和 Require_Resource 是否满足条件
    public void CheckAndFulfillConditions()
    {
        
        
    }

    
    // 开始倒计时， 或许需要重写，以加入进度条
    public void Start_Countdown()
    {
        StartCoroutine(Counting_Down_For_Card_Effect());
    }
    
    
    
    // 倒计时协程，包括进度条的功能
    
    IEnumerator Counting_Down_For_Card_Effect()
    {
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

        // 触发卡牌效果
        TriggerCardEffects();
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
    
    
    
    
    
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////                 卡牌效果                   //////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    
    /////////////////     触发卡牌效果
    
    void TriggerCardEffects()
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


    void Draw_A_Tarot_Card()
    {
        int randomElement = random.Range(0, GameManager.GM.CardLoader.Body_Part_Card_List.Count - 1);
        GameManager.GM.Generate_Card_Body_Part(GameManager.GM.CardLoader.Body_Part_Card_List[randomElement].Id);
    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
