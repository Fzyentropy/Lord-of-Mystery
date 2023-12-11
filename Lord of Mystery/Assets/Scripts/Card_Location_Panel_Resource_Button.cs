using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Location_Panel_Resource_Button : MonoBehaviour
{

   public enum Resources
    {
        Fund,
        Physical_Energy,
        Spirit,
        Soul,
        Spirituality_Infused_Material,
        Knowledge,
        Belief,
        Putrefaction,
        Madness,
        Godhood
    }

    public Resources Current_Resource;     // 用于标记当前 button 对应的资源是哪种
    public bool is_resource_set = false;        // 判断这个 button 对应的 resource 是否设置了
    
    public GameObject attached_panel;       // 用于指代这个 button 附着的 panel 的 GameObject
    private Card_Location_Panel_Feature attached_panel_feature;     // 这个 button 附着的 panel 的 panel_feature 脚本

    public int panel_resource_slot_number = 0;

    private Vector2 click_mouse_position;       // 用于检测鼠标是 点击还是拖拽 的临时参数，记录鼠标按下时的位置


    private void Start()
    {
        Check_If_Initialized_Well();
    }
    


    public void Set_Attached_Panel(GameObject panel)    // 设置这个 button 附着的 panel 和其 panel_feature 脚本指代
    {
        attached_panel = panel;
        attached_panel_feature = attached_panel.GetComponent<Card_Location_Panel_Feature>();
    }
    
    public void Set_Resource_Slot_Number(int number)    // 设置这个 button 在 panel 上的槽位编号，实例化此按钮时，从外部设置
    {
        panel_resource_slot_number = number;
    }

    public void Set_Current_Resource(string resource)   // 设置这个 button 涉及的 resource 类型，实例化此按钮时，从外部设置
    {
        if (resource == "Fund")
        {
            Current_Resource = Resources.Fund;
            is_resource_set = true;
        }
        if (resource == "Physical_Energy")
        {
            Current_Resource = Resources.Physical_Energy;
            is_resource_set = true;
        }
        if (resource == "Spirit")
        {
            Current_Resource = Resources.Spirit;
            is_resource_set = true;
        }
        if (resource == "Soul")
        {
            Current_Resource = Resources.Soul;
            is_resource_set = true;
        }
        if (resource == "Spirituality_Infused_Material")
        {
            Current_Resource = Resources.Spirituality_Infused_Material;
            is_resource_set = true;
        }
        if (resource == "Knowledge")
        {
            Current_Resource = Resources.Knowledge;
            is_resource_set = true;
        }
        if (resource == "Belief")
        {
            Current_Resource = Resources.Belief;
            is_resource_set = true;
        }
        if (resource == "Putrefaction")
        {
            Current_Resource = Resources.Putrefaction;
            is_resource_set = true;
        }
        if (resource == "Madness")
        {
            Current_Resource = Resources.Madness;
            is_resource_set = true;
        }
        if (resource == "Godhood")
        {
            Current_Resource = Resources.Godhood;
            is_resource_set = true;
        }

    }

    private void Check_If_Initialized_Well()
    {
        if (attached_panel_feature == null || panel_resource_slot_number <= 0 || !is_resource_set)
        {
            throw new ArgumentException("资源按钮未被正确设置");
        }
    }


    public bool Check_If_Absorb_Full()      // 根据此资源对应的 panel 资源槽位是哪个，判断 panel 对这个资源的吸收是否满了
    {
        switch (panel_resource_slot_number)
        {
            case 1 :
            {
                return attached_panel_feature.resource_1_amount - attached_panel_feature.current_resource_1_amount > 0;
            }
            case 2 :
            {
                return attached_panel_feature.resource_2_amount - attached_panel_feature.current_resource_2_amount > 0;
            }
            case 3 :
            {
                return attached_panel_feature.resource_3_amount - attached_panel_feature.current_resource_3_amount > 0;
            }
            case 4 :
            {
                return attached_panel_feature.resource_4_amount - attached_panel_feature.current_resource_4_amount > 0;
            }
            case 5 :
            {
                return attached_panel_feature.resource_5_amount - attached_panel_feature.current_resource_5_amount > 0;
            }
            default:
                return false;
        }

    }

    public void Add_Panel_Absorb_Amount()
    {
        switch (panel_resource_slot_number)
        {
            case 1 :
            {
                attached_panel_feature.current_resource_1_amount ++;
                break;
            }
            case 2 :
            {
                attached_panel_feature.current_resource_2_amount ++;
                break;
            }
            case 3 :
            {
                attached_panel_feature.current_resource_3_amount ++;
                break;
            }
            case 4 :
            {
                attached_panel_feature.current_resource_4_amount ++;
                break;
            }
            case 5 :
            {
                attached_panel_feature.current_resource_5_amount ++;
                break;
            }
        }
    }

    public void AbsorbResource()        // 点击时执行 ：吸收资源，到 panel 上
    {
        if (Current_Resource == Resources.Fund)     // 如果这个按钮被设置为 fund 按钮，则吸收 fund
        {
            if (GameManager.GM.ResourceManager.Fund > 0 && Check_If_Absorb_Full())  // 当你拥有此资源（资源数量>0),且此panel对于该资源未吸收满时，触发
            {
                GameManager.GM.ResourceManager.Reduce_Fund(1, gameObject.transform.position);
                Add_Panel_Absorb_Amount();
            }
        }
    }

    private void OnMouseOver()
    {
        // 改变颜色
    }

    private void OnMouseDown()
    {
        click_mouse_position = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        if (((Vector2)Input.mousePosition - click_mouse_position).magnitude < 0.05)  // 鼠标位置基本没变
        {
            AbsorbResource();
        }
    }
}
