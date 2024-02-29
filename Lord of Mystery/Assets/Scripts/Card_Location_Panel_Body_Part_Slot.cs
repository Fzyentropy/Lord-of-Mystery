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
        Psyche
    }
    public Body_Part_Slot_Type BodyPartSlotType;
    public Card_Location_Panel_Feature attached_card_location_panel_feature;
    public int slot_number_in_panel;

    // 高亮边缘相关
    public SpriteRenderer highlightSprite;
    private Color highlight_color = Color.white;
    private Color transparent_color = Color.clear;
    
    // Mis
    private Vector2 click_mouse_position;       // 用于检测鼠标是 点击还是拖拽 的临时参数，记录鼠标按下时的位置


    
    private void Start()
    {
        Check_If_Set_Well();
    }
    


    void Check_If_Set_Well()      // 检查 _cardLocation 卡牌实例是否为空，如果 _cardlocation 卡牌实例为空，则报错
    {
        if (attached_card_location_panel_feature == null || slot_number_in_panel == null || BodyPartSlotType == null)
        {
            throw new NullReferenceException("slot 生成 panel 丢失 || slot number 丢失 || body part 类型未设置");
        }
    }


    private void OnMouseOver()
    {
        if(!attached_card_location_panel_feature.currentlyAbosorbedBodyPartSlots[slot_number_in_panel])     // 如果该槽位还没吸收
            {highlightSprite.color = highlight_color;}  // 没吸收满，则高亮
    }

    private void OnMouseExit()
    {
        highlightSprite.color = transparent_color;  // 鼠标移开，取消高亮 
    }

    private void OnMouseDown()
    {
        click_mouse_position = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        if (((Vector2)Input.mousePosition - click_mouse_position).magnitude < 0.2         // 鼠标位置基本没变
            && !attached_card_location_panel_feature.currentlyAbosorbedBodyPartSlots[slot_number_in_panel]   // 且该槽位没吸收 body part
            && attached_card_location_panel_feature.Have_Giving_Type_Of_Body_Part_On_Board(BodyPartSlotType.ToString()))    // 且 board 上还有相应类型的 body part
        {
            attached_card_location_panel_feature.Absorb_Body_Part_Based_On_Slot(slot_number_in_panel);
        }
    }

}
