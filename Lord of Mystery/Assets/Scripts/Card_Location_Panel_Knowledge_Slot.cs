using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Location_Panel_Knowledge_Slot : MonoBehaviour
{
    
    // 附着的 Panel指代，以及 Panel Feature 脚本指代 
    [HideInInspector] public GameObject attached_panel;       // 用于指代这个 button 附着的 panel 的 GameObject
    [HideInInspector] public Card_Location_Panel_Feature attached_card_location_panel_feature;     // 这个 button 附着的 panel 的 panel_feature 脚本
    
    // 记录
    [HideInInspector] public int panel_resource_slot_number_knowledge = -1;   // 记录 Knowledge 资源在 Panel 上的 slot 编号
    

    // 高亮边缘相关
    public SpriteRenderer highlightSprite;
    private Color highlight_color = Color.white;
    private Color transparent_color = Color.clear;
    private Color dragging_needed_knowledge_color = Color.yellow;
    private bool isHighlight;
    private bool isHighlightYellow;
    
    // Mis
    private Vector2 click_mouse_position;       // 用于检测鼠标是 点击还是拖拽 的临时参数，记录鼠标按下时的位置
    
    
    
    
    
    
    
    void Start()
    {
        Check_If_Set_Well();
        StartCoroutine(Highlight_If_Dragging_Needed_Knowledge());
    }

    private void Update()
    {
        Update_Knowledge_Amount_In_Attached_Panel();
    }


    public void Set_Attached_Panel(GameObject panel)    // 设置这个 button 附着的 panel 和其 panel_feature 脚本指代， 外部设置
    {
        attached_panel = panel;
        attached_card_location_panel_feature = attached_panel.GetComponent<Card_Location_Panel_Feature>();
    }

    public void Set_Resource_Slot_Number_Knowledge(int number)
    {
        panel_resource_slot_number_knowledge = number;
    }
    
    
    void Check_If_Set_Well()      // 检查 _cardLocation 卡牌实例是否为空，如果 _cardlocation 卡牌实例为空，则报错
    {
        if (attached_card_location_panel_feature == null)
        {
            throw new NullReferenceException("slot 生成 panel 丢失");
        }
        
    }
    
    
    
    public void Highlight_Slot(Color color)         // 改变 slot highlight 颜色的方法
    {
        highlightSprite.color = color;
    }
    
    
    public IEnumerator Highlight_If_Dragging_Needed_Knowledge()
    {
        while (true)
        {
            if (attached_card_location_panel_feature.
                    attached_card_location_feature.
                        Check_If_Dragging_Knowledge_Is_Need(GameManager.GM.InputManager.Dragging_Object)   // 如果拖拽的是需要的 Knowledge Card
                && attached_card_location_panel_feature.requiredResourcesThisPanel["Knowledge"] > 
                    attached_card_location_panel_feature.absorbed_knowledge_list.Count)      // 且需要的 Knowledge 大于 已经吸收的 Knowledge
            {
                if (!isHighlightYellow)                                                              // 则高亮，根据是否跟 slot 重叠来判断是否是黄色
                    Highlight_Slot(highlight_color);
                else
                    Highlight_Slot(dragging_needed_knowledge_color);
            }
            else if (isHighlight)
            {
                Highlight_Slot(highlight_color);
            }
            else  // 如果拖拽的不是 Knowledge 或 Knowledge 已经吸收满
            {
                Highlight_Slot(transparent_color);
            }
            
            yield return null;
        }
    }


    void Update_Knowledge_Amount_In_Attached_Panel()     // 如果需要 Knowledge 资源，则将 Card Location Panel 上 Knowledge 对应编号的资源数量更新为 吸收的 Knowledge 数量
    {

        if (panel_resource_slot_number_knowledge == 1)      // 如果 slot 编号是 1，则将 current resource 1 的数量更新为 吸收的 Knowledge 数量
        {
            attached_card_location_panel_feature.current_resource_1_amount =
                attached_card_location_panel_feature.absorbed_knowledge_list.Count;
        }
        if (panel_resource_slot_number_knowledge == 2)      // 如果 slot 编号是 2，则将 current resource 2 的数量更新为 吸收的 Knowledge 数量
        {
            attached_card_location_panel_feature.current_resource_2_amount =
                attached_card_location_panel_feature.absorbed_knowledge_list.Count;
        }
        if (panel_resource_slot_number_knowledge == 3)      // 如果 slot 编号是 3，则将 current resource 3 的数量更新为 吸收的 Knowledge 数量
        {
            attached_card_location_panel_feature.current_resource_3_amount =
                attached_card_location_panel_feature.absorbed_knowledge_list.Count;
        }
        if (panel_resource_slot_number_knowledge == 4)      // 如果 slot 编号是 4，则将 current resource 4 的数量更新为 吸收的 Knowledge 数量
        {
            attached_card_location_panel_feature.current_resource_4_amount =
                attached_card_location_panel_feature.absorbed_knowledge_list.Count;
        }
        if (panel_resource_slot_number_knowledge == 5)      // 如果 slot 编号是 5，则将 current resource 5 的数量更新为 吸收的 Knowledge 数量
        {
            attached_card_location_panel_feature.current_resource_5_amount =
                attached_card_location_panel_feature.absorbed_knowledge_list.Count;
        }
        
        // 如果后续有更多 资源按钮 slot，在此处添加
        
    }



    private void OnMouseOver()
    {
        if (GameManager.GM.InputManager.Dragging_Object == null     // 如果 没在 drag 东西
            && attached_card_location_panel_feature.requiredResourcesThisPanel["Knowledge"] > 
            attached_card_location_panel_feature.absorbed_knowledge_list.Count)      // 且需要的 Knowledge 大于 已经吸收的 Knowledge
        {
            isHighlight = true;        // 高亮
        }  
    }

    private void OnMouseExit()
    {
        isHighlight = false; // 鼠标移开，取消高亮 
    }

    private void OnMouseDown()
    {
        click_mouse_position = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        
        if (((Vector2)Input.mousePosition - click_mouse_position).magnitude < 0.6f)         // 鼠标位置基本没变
        {
            GameManager.GM.Generate_Message("Panel_Knowledge_Slot_Click_Show_Requirement");     // 弹出一个 Message 来告诉玩家需要 拖入 Knowledge
        }
        
        
    }
    
    
    
    private void OnTriggerStay2D(Collider2D other)      // 当有 Collider2D 悬停时
    {

        if (GameManager.GM.InputManager.Dragging_Object == other.gameObject     // 如果正在拖拽那个东西
            && other.GetComponent<Knowledge_Feature>() != null    // 而且在拖拽的是 Knowledge Card
            && attached_card_location_panel_feature.
                attached_card_location_feature.
                Check_If_Dragging_Knowledge_Is_Need(other.gameObject))    // 且是需要的 Knowledge Card
        {
            
            Debug.Log("Knowledge ABSORBED");
            other.GetComponent<Knowledge_Feature>().overlapped_card_location_or_knowledge_slot = gameObject;    // 向与此卡重叠的 body part 传入此 body part slot, 确保当前卡被吸收时的唯一性

            if (other.GetComponent<Knowledge_Feature>().overlapped_card_location_or_knowledge_slot == gameObject)
            {
                isHighlightYellow = true;   // 将高亮改为黄色
            }
            else
            {
                isHighlightYellow = false;
            }
            
        }
        
        
    }

    void OnTriggerExit2D(Collider2D other)      // 当悬停的卡牌离开时
    {
        
        if (GameManager.GM.InputManager.Dragging_Object == other.gameObject     // 如果正在拖拽那个东西
            && other.GetComponent<Knowledge_Feature>() != null)    // 而且在拖拽的是 body part
        {
            // 取消黄色 Highlight
            isHighlightYellow = false;

            // “ 取出 ” body part 重叠物体
            other.GetComponent<Knowledge_Feature>().overlapped_card_location_or_knowledge_slot = null;

        }
    }
    
    
}
