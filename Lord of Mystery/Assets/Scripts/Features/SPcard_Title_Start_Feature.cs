using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SPcard_Title_Start_Feature : MonoBehaviour
{
    
    // Prefab
    [Header("Panel")]
    public GameObject panel_prefab;       // 弹出的 Make Potion 的 panel
    
    // 打开 Panel 指代
    [HideInInspector]
    public GameObject  showed_panel;             // 打开这张卡对应的 panel 时，记录 打开的 panel 到此参数
    
    // 卡牌 prefab 各元素指代
    [Header("prefab elements")]
    public SpriteRenderer card_frame;           // 卡牌的 frame，边框
    public SpriteRenderer card_name_tag;        // 卡牌的 name tag，名字栏
    public SpriteRenderer card_image;           // 卡牌的 image，图片
    public TMP_Text card_label;                 // 卡牌的 label，名称
    public SpriteRenderer card_image_mask;      // 卡牌的 image mask，遮罩
    public SpriteRenderer card_shadow;          // 卡牌的 shadow 阴影
    
    
    // 进度
    [HideInInspector] public bool is_counting_down = false;     // panel 是否在倒计时生产中

    // 进度条
    [Header("Progress Bar")]
    public GameObject progress_bar_prefab;        // 进度条 prefab
    [HideInInspector]public GameObject progress_bar;               // 进度条 指代
    [HideInInspector]public GameObject progress_bar_root;          // 进度条方块的根 指代
    public GameObject progress_bar_position;      // 进度条位置标记 空物体
    [HideInInspector]public TMP_Text countdown_text;               // 显示秒数文本
    
    
    // Mis
    public int LayerIndex;      // 记录此 GameObject 所处的 layer（“Card_Location")

    public bool isHighlightYellow = false;    // 是否需要黄色高亮，在拖拽需要的 body part 到卡牌上方时为黄色
    public bool isHighlight = false;    // 判断是否需要高亮
    
    private bool yellow_highlight_bodypart_variable_switch = true;     // 用于让当前 card location 被 body part 重叠时，只在第一次重叠时传入此 card location feature 实例到 body part feature 的参数里

    private bool is_dragging_pick_up_effect_applied;     // 用于记录是否 “抬起” 了卡牌
    

    
    

    private void Start()
    {
        AddColliderAndRigidbody();      // 如果没加 collider 和 rigidbody，则加上
        Initialize_Title_Start_Card();
        
        StartCoroutine(Highlight_If_Mouse_Hover());
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


    void Initialize_Title_Start_Card()
    {

    }
    
    
    public IEnumerator Highlight_If_Mouse_Hover()
    {
        while (true)
        {
            if (isHighlight)       // 如果鼠标悬停让 isHighlight 参数为 true 了，也高亮，高亮白色
            {
                GameManager.GM.CardEffects.Highlight_Collider(GetComponent<BoxCollider2D>(), GetComponent<LineRenderer>(), Color.white);
            }
            else
            {
                GameManager.GM.CardEffects.Clear_Highlight_Collider(GetComponent<LineRenderer>());
            }
            
            yield return null;
        }
    }
    
    
    
    
    
    private void OnMouseOver()      // 鼠标悬停的时候，高亮
    {
        // 高亮
        if (GameManager.GM.InputManager.Dragging_Object == null)         // 鼠标悬停时，如果没拖拽着其他卡牌，
        {
            isHighlight = true;
        }
    }

    private void OnMouseDown()      // 按下鼠标左键的时候，记录鼠标位置，调整卡牌的渲染 layer，让其到最上面，取消高亮
    {
        // if (GameManager.GM.InputManager.raycast_top_object == gameObject)   //只有当射线检测的 top GameObject 是这张卡时
        {

            GameManager.GM.InputManager.Start_Dragging(gameObject);
            
            
        }
        
    }

    private void OnMouseDrag() // 当按住鼠标左键的时候，如果移动鼠标（即拖拽），则卡牌随之移动
    {
        
        // Clear_Highlight_Collider();                             // 取消高亮
        isHighlight = false; // 取消高亮
        
        
    }
    
    
    private void OnMouseUp()        // 如果此时鼠标的位置和先前按下左键时记录的位置差不多，则为点击，触发点击功能（打开 panel）
    {
        
        GameManager.GM.InputManager.End_Dragging();


        // Increase Order In Layer 改变物体的 Layer，Sorting Layer，和 "放下卡牌" 效果
        {
            
            // 调整卡牌的渲染 layer 层为 "Card Location"
            GameManager.GM.CardEffects.Change_GameObject_Layer(gameObject, "Card Location");

            // DecreaseOrderInLayer();   
            GameManager.GM.CardEffects.Change_Order_In_Layer("Cards",
                card_frame, card_name_tag, card_image, card_label, card_image_mask, card_shadow);

            // "放下卡牌"效果
            if (is_dragging_pick_up_effect_applied)
            {
                is_dragging_pick_up_effect_applied = false;
                GameManager.GM.CardEffects.Apply_Dragging_Put_Down_Effect(
                    card_frame, card_name_tag, card_image, card_label, card_image_mask);
            }
            
        }
        

    }

    
    private void OnMouseExit()      // 当鼠标离开卡牌上方时，取消高亮
    {
        isHighlight = false;    // 取消高亮, by 设定高亮参数为 false
    }


    
    

    


    void Click_Effect()         // 点击时执行的逻辑集成
    {
        Debug.Log("You just clicked the card.");
        
        
        
    }


    

    
    







}
