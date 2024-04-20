using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SPcard_Make_Potion_Feature : MonoBehaviour
{
    
    // Prefab
    [Header("make potion panel")]
    public GameObject _make_potion_panel;       // 弹出的 Make Potion 的 panel
    
    // 打开 Panel 指代
    [Header("panel reference")]
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
    [HideInInspector]public bool is_counting_down = false;     // panel 是否在倒计时生产中
    private float make_potion_time = 15f;
    
    // 进度条
    [Header("Progress Bar")]
    public GameObject progress_bar_prefab;        // 进度条 prefab
    [HideInInspector]public GameObject progress_bar;               // 进度条 指代
    [HideInInspector]public GameObject progress_bar_root;          // 进度条方块的根 指代
    public GameObject progress_bar_position;      // 进度条位置标记 空物体
    [HideInInspector]public TMP_Text countdown_text;               // 显示秒数文本
    
    
    // Movement Variables
    private Vector3 click_mouse_position;       // 用于点击时记录鼠标的位置
    private Vector3 lastMousePosition;      // 用于记录鼠标拖拽时，前一帧鼠标的位置

    public int LayerIndex;      // 记录此 GameObject 所处的 layer（“Card_Location")

    public bool isHighlightYellow = false;    // 是否需要黄色高亮，在拖拽需要的 body part 到卡牌上方时为黄色
    public bool isHighlight = false;    // 判断是否需要高亮
    
    private bool yellow_highlight_bodypart_variable_switch = true;     // 用于让当前 card location 被 body part 重叠时，只在第一次重叠时传入此 card location feature 实例到 body part feature 的参数里

    private bool dragging_shadow_effect_if_transformed = false;     // 用于记录是否 “抬起” 了卡牌

    
    

    private void Start()
    {
        AddColliderAndRigidbody();      // 如果没加 collider 和 rigidbody，则加上
        Set_Layer_Index();          // 设置 layer 的 index

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

    void Set_Layer_Index()      // 设置 layer 的 index
    {
        LayerIndex = gameObject.layer;
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
            // 记录鼠标位置
            click_mouse_position = Input.mousePosition;
            lastMousePosition = Input.mousePosition;

            // 取消高亮

            
            
        }
        
    }

    private void OnMouseDrag() // 当按住鼠标左键的时候，如果移动鼠标（即拖拽），则卡牌随之移动
    {
        // if (GameManager.GM.InputManager.raycast_top_object == gameObject)   //只有当射线检测的 top GameObject 是这张卡时
        {

            
            // Clear_Highlight_Collider();                             // 取消高亮
            isHighlight = false; // 取消高亮


            // 调整卡牌的渲染 layer，让其到最上面
            
            IncreaseOrderInLayer();


            // 如果鼠标移动，卡牌随之移动        // 临时，如果 Moveable 才可以移动，为了临时代替 序列 Sequence
            // if (!_cardLocation.Stable)   
            {
                // float mouse_drag_sensitivity = 0.05f;
                GameManager.GM.InputManager.Dragging_Object = gameObject; // 将 Input Manager 中的 正在拖拽物体 记录为此物体
                Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) -
                                Camera.main.ScreenToWorldPoint(lastMousePosition);
                delta.z = 0;
                gameObject.transform.position += delta;
                lastMousePosition = Input.mousePosition;
            }
            
        }
    }
    
    
    private void OnMouseUp()        // 如果此时鼠标的位置和先前按下左键时记录的位置差不多，则为点击，触发点击功能（打开 panel）
    {
        
        if ((Input.mousePosition - click_mouse_position).magnitude < 0.8f) // 判断此时鼠标的位置和记录的位置，如果差不多即视为点击，触发点击功能
        {
            Click_Effect();
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


    
    public IEnumerator Highlight_If_Mouse_Hover()
    {
        while (true)
        {
            if (isHighlight)       // 如果鼠标悬停让 isHighlight 参数为 true 了，也高亮，高亮白色
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

    
    
    
    
    public void IncreaseOrderInLayer()       // 提高 卡牌的 Order in Layer 数值，以让卡牌在最上方渲染
    {
        gameObject.layer = LayerMask.NameToLayer("DraggingLayer"); // 调用系统方法来找到 "Dragging Layer"对应的 Index，并设置
        
        card_frame.sortingLayerName = "Dragging";
        card_name_tag.sortingLayerName = "Dragging";
        card_image.sortingLayerName = "Dragging";
        card_label.GetComponent<Renderer>().sortingLayerName = "Dragging";
        card_image_mask.sortingLayerName = "Dragging";
        card_shadow.sortingLayerName = "Dragging";

        float x_movement = -0.1f;
        float y_movement = 0.1f;

        if (!dragging_shadow_effect_if_transformed)
        {
            dragging_shadow_effect_if_transformed = true;
            
            card_frame.transform.localPosition = new Vector3(
                card_frame.transform.localPosition.x + x_movement,
                card_frame.transform.localPosition.y + y_movement,
                card_frame.transform.localPosition.z);
        
            card_name_tag.transform.localPosition = new Vector3(
                card_name_tag.transform.localPosition.x + x_movement,
                card_name_tag.transform.localPosition.y + y_movement,
                card_name_tag.transform.localPosition.z);
        
            card_image.transform.localPosition = new Vector3(
                card_image.transform.localPosition.x + x_movement,
                card_image.transform.localPosition.y + y_movement,
                card_image.transform.localPosition.z);
        
            card_label.transform.localPosition = new Vector3(
                card_label.transform.localPosition.x + x_movement,
                card_label.transform.localPosition.y + y_movement,
                card_label.transform.localPosition.z);
        
            card_image_mask.transform.localPosition = new Vector3(
                card_image_mask.transform.localPosition.x + x_movement,
                card_image_mask.transform.localPosition.y + y_movement,
                card_image_mask.transform.localPosition.z);
        }
        
        
    }
    public void DecreaseOrderInLayer()       // 提高 卡牌的 Order in Layer 数值，以让卡牌在最上方渲染
    {
        gameObject.layer = LayerMask.NameToLayer("Card Location"); // 调用系统方法来找到 "Dragging Layer"对应的 Index，并设置
        
        card_frame.sortingLayerName = "Cards";
        card_name_tag.sortingLayerName = "Cards";
        card_image.sortingLayerName = "Cards";
        card_label.GetComponent<Renderer>().sortingLayerName = "Cards";
        card_image_mask.sortingLayerName = "Cards";
        card_shadow.sortingLayerName = "Cards";

        float x_movement = 0.1f;
        float y_movement = -0.1f;
        

        if (dragging_shadow_effect_if_transformed)
        {
            dragging_shadow_effect_if_transformed = false;
            
            card_frame.transform.localPosition = new Vector3(
                card_frame.transform.localPosition.x + x_movement,
                card_frame.transform.localPosition.y + y_movement,
                card_frame.transform.localPosition.z);
        
            card_name_tag.transform.localPosition = new Vector3(
                card_name_tag.transform.localPosition.x + x_movement,
                card_name_tag.transform.localPosition.y + y_movement,
                card_name_tag.transform.localPosition.z);
        
            card_image.transform.localPosition = new Vector3(
                card_image.transform.localPosition.x + x_movement,
                card_image.transform.localPosition.y + y_movement,
                card_image.transform.localPosition.z);
        
            card_label.transform.localPosition = new Vector3(
                card_label.transform.localPosition.x + x_movement,
                card_label.transform.localPosition.y + y_movement,
                card_label.transform.localPosition.z);
        
            card_image_mask.transform.localPosition = new Vector3(
                card_image_mask.transform.localPosition.x + x_movement,
                card_image_mask.transform.localPosition.y + y_movement,
                card_image_mask.transform.localPosition.z);
        }
    }


    void Click_Effect()         // 点击时执行的逻辑集成
    {
        Open_Make_Potion_Panel();
        
    }

    void Open_Make_Potion_Panel()
    {
        
        Debug.Log("SPcard_Potion_Clicked ");
        
        if (GameManager.GM.PanelManager.current_panel == null       // 如果 当前 panel 为空（没有已经打开的 panel）
            ||                                                      // 或者
            GameManager.GM.PanelManager.current_panel != null &&    // 当前 panel 不为空（说明有 panel 已经打开），且打开的 panel 不是 Make Potion Panel
            GameManager.GM.PanelManager.current_panel.GetComponent<SPcard_Make_Potion_Panel_Feature>() == null)      
            
        {
            if (GameManager.GM.PanelManager.current_panel != null)
                GameManager.GM.PanelManager.Close_Current_Panel();              // 有打开的 panel 则关闭 panel
            
            // 生成新的 panel
            GameObject make_potion_panel = Instantiate(_make_potion_panel,        // 实例化 panel
                new Vector3(
                    gameObject.transform.position.x,
                    gameObject.transform.position.y,
                    gameObject.transform.position.z - 1), Quaternion.identity);  
            
            
            SPcard_Make_Potion_Panel_Feature make_potion_panel_feature = make_potion_panel.GetComponent<SPcard_Make_Potion_Panel_Feature>();    // 指代 panel 的 feature 脚本

            make_potion_panel_feature.Set_Attached_Make_Potion_Card(gameObject);        // 将生成的 panel 中的对于生成卡牌的指代设置为此卡

            showed_panel = make_potion_panel;       // 记录打开的 panel 实例到 showed panel 参数
            // GameManager.GM.PanelManager.current_panel = panel;      // 将 "打开的 panel" 设置为刚打开的 panel
            GameManager.GM.PanelManager.Set_Panel_Reference_And_Scale_Up(make_potion_panel);
            GameManager.GM.PanelManager.isPanelOpen = true;
            

            // panel_feature. Set Resource Buttons

            // panel_feature. Set Body Parts

            // return panel;
            
        }
        
        
        
    }
    
    
    ////////////////////////////////////////////////////////////////////////////////      开始倒计时 方法
    
    public void Start_Make_Potion()
    {
        StartCoroutine(Counting_Down_For_Make_Potion());
    }
    
    
    // 倒计时协程，包括进度条的功能
    
    IEnumerator Counting_Down_For_Make_Potion()
    {
        is_counting_down = true;   // 设置 "正在倒计时" 为是
        
        float totalTime = make_potion_time;   // 配置魔药计时
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
            remainingTime -= timeInterval * GameManager.GM.InputManager.Time_X_Speed;     // 加入时间加速参数！！

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
            && GameManager.GM.PanelManager.current_panel.GetComponent<SPcard_Make_Potion_Panel_Feature>() != null)
        {
            GameManager.GM.PanelManager.Close_Current_Panel();
        }

        // 触发卡牌效果
        Make_The_Potion();
        
    }


    void Make_The_Potion()
    {
        
        // 生成 Potion Body Part，设置 该 Potion 特殊标签

        float newPotionPositionYOffset = 8f;

        GameObject new_potion = GameManager.GM.BodyPartManager.Generate_Body_Part_To_Board("Potion",
            transform.position,
            new Vector3(transform.position.x,
                transform.position.y - newPotionPositionYOffset,
                transform.position.z - 1));




    }



}
