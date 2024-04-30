using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Card_Location_Panel_Body_Part_Slot : MonoBehaviour
{

    // 重要 slot depedency 设置
    public enum Body_Part_Slot_Type
    {
        Physical_Body,
        Spirit,
        Psyche,
        
        Potion
    }
    public Body_Part_Slot_Type BodyPartSlotType;
    public Card_Location_Panel_Feature attached_card_location_panel_feature;
    public int slot_number_in_panel;

    // 高亮边缘相关
    public SpriteRenderer highlightSprite;
    private Color highlight_color = Color.white;
    private Color transparent_color = Color.clear;
    private Color dragging_needed_body_part_color = Color.yellow;
    private bool isHighlight;
    private bool isHighlightYellow;
    
    // Mis
    private Vector2 click_mouse_position;       // 用于检测鼠标是 点击还是拖拽 的临时参数，记录鼠标按下时的位置



    private void Start()
    {
        Check_If_Set_Well();
        StartCoroutine(Highlight_If_Dragging_Needed_BodyPart());
    }
    


    void Check_If_Set_Well()      // 检查 _cardLocation 卡牌实例是否为空，如果 _cardlocation 卡牌实例为空，则报错
    {
        if (attached_card_location_panel_feature == null || slot_number_in_panel == null || BodyPartSlotType == null)
        {
            throw new NullReferenceException("slot 生成 panel 丢失 || slot number 丢失 || body part 类型未设置");
        }

        if (BodyPartSlotType == Body_Part_Slot_Type.Potion)     // 添加 Potion 相关设置的判定
        {
            
        }
    }


    public void Highlight_Slot(Color color)         // 改变 slot highlight 颜色的方法
    {
        highlightSprite.color = color;
    }
    
    
    public IEnumerator Highlight_If_Dragging_Needed_BodyPart()
    {
        while (true)
        {
            if (attached_card_location_panel_feature.
                    attached_card_location_feature.
                        Check_If_Dragging_BodyPart_Is_Need(GameManager.GM.InputManager.Dragging_Object)   // 如果拖拽的是需要的 body part
                && !attached_card_location_panel_feature.currentlyAbosorbedBodyPartSlots[slot_number_in_panel])      // 且该槽位还没吸收
            {
                if (!isHighlightYellow)                                                              // 则高亮，根据是否跟 slot 重叠来判断是否是黄色
                    Highlight_Slot(highlight_color);
                else
                    Highlight_Slot(dragging_needed_body_part_color);
            }
            else if (isHighlight)
            {
                Highlight_Slot(highlight_color);
            }
            else  // 如果拖拽的不是 body part 或不需要的类型的 body part 或 槽位已经吸收了
            {
                Highlight_Slot(transparent_color);
            }
            
            yield return null;
        }
    }

    
    private void OnMouseOver()
    {
        if (GameManager.GM.InputManager.Dragging_Object == null         // 如果 没在 drag 东西
            && !attached_card_location_panel_feature.currentlyAbosorbedBodyPartSlots[slot_number_in_panel]) // 如果该槽位还没吸收
        {
            isHighlight = true;        // 没吸收满，则高亮
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
        
        // 弹出一个 Message 来告诉玩家需要 某个类型的 Body Part
        if (BodyPartSlotType == Body_Part_Slot_Type.Physical_Body)
        {
            GameManager.GM.Generate_Message("Panel_Body_Part_Slot_Click_Show_Requirement_Physical_Body");
        }
        if (BodyPartSlotType == Body_Part_Slot_Type.Spirit)
        {
            GameManager.GM.Generate_Message("Panel_Body_Part_Slot_Click_Show_Requirement_Spirit");
        }
        if (BodyPartSlotType == Body_Part_Slot_Type.Psyche)
        {
            GameManager.GM.Generate_Message("Panel_Body_Part_Slot_Click_Show_Requirement_Psyche");
        }
        if (BodyPartSlotType == Body_Part_Slot_Type.Potion)
        {
            GameManager.GM.Generate_Message("Panel_Body_Part_Slot_Click_Show_Requirement_Potion");
        }
        
        
        
        // 原代码：点击 Slot 时会从 board 上吸收一个相应的 Body Part
        /*if (((Vector2)Input.mousePosition - click_mouse_position).magnitude < 0.6f         // 鼠标位置基本没变
            && !attached_card_location_panel_feature.currentlyAbosorbedBodyPartSlots[slot_number_in_panel]   // 且该槽位没吸收 body part
            && attached_card_location_panel_feature.Have_Giving_Type_Of_Body_Part_On_Board(BodyPartSlotType.ToString())    // 且 board 上还有相应类型的 body part
            // && GameManager.GM.InputManager.Dragging_Object == null         // 且没在 drag 东西  // 此判定不能加，因为 Panel Movement 脚本会让 dragging Object = panel
           )
        {
            attached_card_location_panel_feature.Absorb_Body_Part_Based_On_Slot(slot_number_in_panel);
        }*/
    }
    
    
    
    private void OnTriggerStay2D(Collider2D other)      // 当有 Collider2D 悬停时
    {

        if (GameManager.GM.InputManager.Dragging_Object == other.gameObject     // 如果正在拖拽那个东西
            && other.GetComponent<Card_Body_Part_Feature>() != null    // 而且在拖拽的是 body part
            && attached_card_location_panel_feature.
                attached_card_location_feature.
                Check_If_Dragging_BodyPart_Is_Need(other.gameObject)    // 且是需要的 body part
            && !attached_card_location_panel_feature.currentlyAbosorbedBodyPartSlots[slot_number_in_panel])     // 且该槽位还没吸收
        {
            
            Debug.Log("trigggered and passed the check");
            other.GetComponent<Card_Body_Part_Feature>().overlapped_card_location_or_panel_slot = gameObject;    // 向与此卡重叠的 body part 传入此 body part slot, 确保当前卡被吸收时的唯一性

            if (other.GetComponent<Card_Body_Part_Feature>().overlapped_card_location_or_panel_slot == gameObject)
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
            && other.GetComponent<Card_Body_Part_Feature>() != null)    // 而且在拖拽的是 body part
        {
            // 取消黄色 Highlight
            isHighlightYellow = false;

            // “ 取出 ” body part 重叠物体
            other.GetComponent<Card_Body_Part_Feature>().overlapped_card_location_or_panel_slot = null;

        }
    }
    
    
    

}
