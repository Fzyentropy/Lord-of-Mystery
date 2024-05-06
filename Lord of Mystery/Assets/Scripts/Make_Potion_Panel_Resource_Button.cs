using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Make_Potion_Panel_Resource_Button : MonoBehaviour, IPointerClickHandler
{

   public enum Resources
    {
        Fund,
        Physical_Energy,
        Spiritual_Energy,
        Soul,
        Spirituality_Infused_Material,
        Knowledge,
        Belief,
        Putrefaction,
        Madness,
        Godhood,
        Death
        ////////////////////// TODO 其他资源扩展，如有
    }
   

    public Resources Current_Resource;     // 用于标记当前 button 对应的资源是哪种
    public bool is_resource_set = false;        // 判断这个 button 对应的 resource 是否设置了

    private bool is_button_available;       // button 是否 available，根据 is_ever_appears 设置

    public GameObject empty_slot;       // 不 available 的时候的 empty slot
    
    [HideInInspector] public GameObject attached_make_potion_panel;       // 用于指代这个 button 附着的 panel 的 GameObject
    [HideInInspector] public SPcard_Make_Potion_Panel_Feature attached_make_potion_panel_feature;     // 这个 button 附着的 panel 的 panel_feature 脚本

    [HideInInspector] public int panel_resource_slot_number = 0;      // 这个按钮资源 对应的槽位 slot 编号 X

    // Sprite Highlight
    public SpriteRenderer highlightSprite;      // 用于粗糙地制作 描边效果
    
    
    // Mis
    private Vector2 click_mouse_position;       // 用于检测鼠标是 点击还是拖拽 的临时参数，记录鼠标按下时的位置
    private Color highlight_color = new Color(1, 1, 1, 1);
    private Color transparent_color = Color.clear;
    
    


    private void Start()
    {
        Check_If_Initialized_Well();
    }

    private void Update()
    {
        Set_Available_Based_On_Ever_Appears();
        Set_Active_Based_On_Availability();
    }


    public void Set_Attached_Make_Potion_Panel(GameObject panel)    // 设置这个 button 附着的 panel 和其 panel_feature 脚本指代， 外部设置
    {
        attached_make_potion_panel = panel;
        attached_make_potion_panel_feature = attached_make_potion_panel.GetComponent<SPcard_Make_Potion_Panel_Feature>();
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
        if (resource == "Spiritual_Energy")
        {
            Current_Resource = Resources.Spiritual_Energy;
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
        if (resource == "Death")        // Death
        {
            Current_Resource = Resources.Death;
            is_resource_set = true;
        }
        
        ////////////////////// TODO 其他资源扩展，如有

    }

    private void Check_If_Initialized_Well()
    {
        if (attached_make_potion_panel_feature == null || !is_resource_set)
        {
            throw new ArgumentException("资源按钮未被正确设置");
        }
    }

    private void Set_Available_Based_On_Ever_Appears()
    {
        if (Current_Resource == Resources.Physical_Energy)          
        {
            if (GameManager.GM.ResourceManager.is_physical_energy_ever_appears)
            {
                is_button_available = true;
            }
            else
            {
                is_button_available = false;
            }
        }
        if (Current_Resource == Resources.Spiritual_Energy)          
        {
            if (GameManager.GM.ResourceManager.is_spiritual_energy_ever_appears)
            {
                is_button_available = true;
            }
            else
            {
                is_button_available = false;
            }
        }
        if (Current_Resource == Resources.Soul)          
        {
            if (GameManager.GM.ResourceManager.is_soul_ever_appears)
            {
                is_button_available = true;
            }
            else
            {
                is_button_available = false;
            }
        }
        if (Current_Resource == Resources.Spirituality_Infused_Material)          
        {
            if (GameManager.GM.ResourceManager.is_spirituality_infused_material_ever_appears)
            {
                is_button_available = true;
            }
            else
            {
                is_button_available = false;
            }
        }
        if (Current_Resource == Resources.Belief)          
        {
            if (GameManager.GM.ResourceManager.is_belief_ever_appears)
            {
                is_button_available = true;
            }
            else
            {
                is_button_available = false;
            }
        }
        if (Current_Resource == Resources.Putrefaction)          
        {
            if (GameManager.GM.ResourceManager.is_putrefaction_ever_appears)
            {
                is_button_available = true;
            }
            else
            {
                is_button_available = false;
            }
        }
        if (Current_Resource == Resources.Madness)          
        {
            if (GameManager.GM.ResourceManager.is_madness_ever_appears)
            {
                is_button_available = true;
            }
            else
            {
                is_button_available = false;
            }
        }
        if (Current_Resource == Resources.Godhood)          
        {
            if (GameManager.GM.ResourceManager.is_godhood_ever_appears)
            {
                is_button_available = true;
            }
            else
            {
                is_button_available = false;
            }
        }


    }

    void Set_Active_Based_On_Availability()
    {
        if (is_button_available)
        {
            empty_slot.SetActive(false);
        }
        else
        {
            empty_slot.SetActive(true);
        }
    }
    

    public void AbsorbResource()        // 点击时执行 ：吸收资源，到 panel 上
    {
        if (Current_Resource == Resources.Physical_Energy)     // 如果这个按钮被设置为 Physical_Energy 按钮，则吸收 Physical_Energy
        {
            if (GameManager.GM.ResourceManager.Physical_Energy > 0)  // 当你拥有此资源（资源数量>0),且此panel对于该资源未吸收满时，触发
            {
                GameManager.GM.ResourceManager.Reduce_Physical_Energy(1, gameObject.transform.position);
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Physical_Energy"]++;
            }
        }
        if (Current_Resource == Resources.Spiritual_Energy)     // 如果这个按钮被设置为 Spirit 按钮，则吸收 Spirit
        {
            if (GameManager.GM.ResourceManager.Spiritual_Energy > 0)  // 当你拥有此资源（资源数量>0),且此panel对于该资源未吸收满时，触发
            {
                GameManager.GM.ResourceManager.Reduce_Spiritual_Energy(1, gameObject.transform.position);
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Spiritual_Energy"]++;
            }
        }
        if (Current_Resource == Resources.Soul)     // 如果这个按钮被设置为 Soul 按钮，则吸收 Soul
        {
            if (GameManager.GM.ResourceManager.Soul > 0)  // 当你拥有此资源（资源数量>0),且此panel对于该资源未吸收满时，触发
            {
                GameManager.GM.ResourceManager.Reduce_Soul(1, gameObject.transform.position);
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Soul"]++;
            }
        }
        if (Current_Resource == Resources.Spirituality_Infused_Material)     // 如果这个按钮被设置为 Spirituality_Infused_Material 按钮，则吸收 Spirituality_Infused_Material
        {
            if (GameManager.GM.ResourceManager.Spirituality_Infused_Material > 0)  // 当你拥有此资源（资源数量>0),且此panel对于该资源未吸收满时，触发
            {
                GameManager.GM.ResourceManager.Reduce_Spirituality_Infused_Material(1, gameObject.transform.position);
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Spirituality_Infused_Material"]++;
            }
        }
        /*if (Current_Resource == Resources.Knowledge)     // 如果这个按钮被设置为 Knowledge 按钮，则吸收 Knowledge
        {
            if (GameManager.GM.ResourceManager.Knowledge > 0)  // 当你拥有此资源（资源数量>0),且此panel对于该资源未吸收满时，触发
            {
                GameManager.GM.ResourceManager.Reduce_Knowledge(1, gameObject.transform.position);
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Knowledge"]++;
            }
        }*/
        if (Current_Resource == Resources.Belief)     // 如果这个按钮被设置为 Belief 按钮，则吸收 Belief
        {
            if (GameManager.GM.ResourceManager.Belief > 0)  // 当你拥有此资源（资源数量>0),且此panel对于该资源未吸收满时，触发
            {
                GameManager.GM.ResourceManager.Reduce_Belief(1, gameObject.transform.position);
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Belief"]++;
            }
        }
        if (Current_Resource == Resources.Putrefaction)     // 如果这个按钮被设置为 Putrefaction 按钮，则吸收 Putrefaction
        {
            if (GameManager.GM.ResourceManager.Putrefaction > 0)  // 当你拥有此资源（资源数量>0),且此panel对于该资源未吸收满时，触发
            {
                GameManager.GM.ResourceManager.Reduce_Putrefaction(1, gameObject.transform.position);
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Putrefaction"]++;
            }
        }
        if (Current_Resource == Resources.Madness)     // 如果这个按钮被设置为 Madness 按钮，则吸收 Madness
        {
            if (GameManager.GM.ResourceManager.Madness > 0)  // 当你拥有此资源（资源数量>0),且此panel对于该资源未吸收满时，触发
            {
                GameManager.GM.ResourceManager.Reduce_Madness(1, gameObject.transform.position);
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Madness"]++;
            }
        }
        if (Current_Resource == Resources.Godhood)     // 如果这个按钮被设置为 Godhood 按钮，则吸收 Godhood
        {
            if (GameManager.GM.ResourceManager.Godhood > 0)  // 当你拥有此资源（资源数量>0),且此panel对于该资源未吸收满时，触发
            {
                GameManager.GM.ResourceManager.Reduce_Godhood(1, gameObject.transform.position);
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Godhood"]++;
            }
        }
        
        ////////////////////// TODO 其他资源扩展，如有
    }
    
    
    public void ReduceResource()        // 点击时执行 ：吸收资源，到 panel 上
    {
        if (Current_Resource == Resources.Fund)     // 如果这个按钮被设置为 fund 按钮，则吸收 fund
        {
            if (attached_make_potion_panel_feature.absorbedResourceThisPanel["Fund"] > 0)
            {
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Fund"]--;
                GameManager.GM.ResourceManager.Add_Fund(1, gameObject.transform.position);
            }
        }
        if (Current_Resource == Resources.Physical_Energy)     // 如果这个按钮被设置为 Physical_Energy 按钮，则吸收 Physical_Energy
        {

            if (attached_make_potion_panel_feature.absorbedResourceThisPanel["Physical_Energy"] > 0)
            {
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Physical_Energy"]--;
                GameManager.GM.ResourceManager.Add_Physical_Energy(1, gameObject.transform.position);
            }
            
        }
        if (Current_Resource == Resources.Spiritual_Energy)     // 如果这个按钮被设置为 Spiritual_Energy 按钮，则吸收 Spirit
        {
            if (attached_make_potion_panel_feature.absorbedResourceThisPanel["Spiritual_Energy"] > 0)
            {
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Spiritual_Energy"]--;
                GameManager.GM.ResourceManager.Add_Spiritual_Energy(1, gameObject.transform.position);
            }
            
        }
        if (Current_Resource == Resources.Soul)     // 如果这个按钮被设置为 Soul 按钮，则吸收 Soul
        {

            if (attached_make_potion_panel_feature.absorbedResourceThisPanel["Soul"] > 0)
            {
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Soul"]--;
                GameManager.GM.ResourceManager.Add_Soul(1, gameObject.transform.position);
            }
            
        }
        if (Current_Resource == Resources.Spirituality_Infused_Material)     // 如果这个按钮被设置为 Spirituality_Infused_Material 按钮，则吸收 Spirituality_Infused_Material
        {

            if (attached_make_potion_panel_feature.absorbedResourceThisPanel["Spirituality_Infused_Material"] > 0)
            {
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Spirituality_Infused_Material"]--;
                GameManager.GM.ResourceManager.Add_Spirituality_Infused_Material(1, gameObject.transform.position);
            }
            
        }
        /*if (Current_Resource == Resources.Knowledge)     // 如果这个按钮被设置为 Knowledge 按钮，则吸收 Knowledge
        {
            if (GameManager.GM.ResourceManager.Knowledge > 0 && Check_If_Absorb_Full())  // 当你拥有此资源（资源数量>0),且此panel对于该资源未吸收满时，触发
            {
                GameManager.GM.ResourceManager.Reduce_Knowledge(1, gameObject.transform.position);
                Add_Panel_Absorb_Amount();
            }
        }*/
        if (Current_Resource == Resources.Belief)     // 如果这个按钮被设置为 Belief 按钮，则吸收 Belief
        {

            if (attached_make_potion_panel_feature.absorbedResourceThisPanel["Belief"] > 0)
            {
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Belief"]--;
                GameManager.GM.ResourceManager.Add_Belief(1, gameObject.transform.position);
            }
            
        }
        if (Current_Resource == Resources.Putrefaction)     // 如果这个按钮被设置为 Putrefaction 按钮，则吸收 Putrefaction
        {

            if (attached_make_potion_panel_feature.absorbedResourceThisPanel["Putrefaction"] > 0)
            {
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Putrefaction"]--;
                GameManager.GM.ResourceManager.Add_Putrefaction(1, gameObject.transform.position);
            }
            
        }
        if (Current_Resource == Resources.Madness)     // 如果这个按钮被设置为 Madness 按钮，则吸收 Madness
        {

            if (attached_make_potion_panel_feature.absorbedResourceThisPanel["Madness"] > 0)
            {
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Madness"]--;
                GameManager.GM.ResourceManager.Add_Madness(1, gameObject.transform.position);
            }
            
        }
        if (Current_Resource == Resources.Godhood)     // 如果这个按钮被设置为 Godhood 按钮，则吸收 Godhood
        {

            if (attached_make_potion_panel_feature.absorbedResourceThisPanel["Godhood"] > 0)
            {
                attached_make_potion_panel_feature.absorbedResourceThisPanel["Godhood"]--;
                GameManager.GM.ResourceManager.Add_Godhood(1, gameObject.transform.position);
            }
            
        }
        
        ////////////////////// TODO 其他资源扩展，如有
    }
    
    

    private void OnMouseOver()
    {
        if (is_button_available)
            highlightSprite.color = highlight_color;  // 高亮
    }

    private void OnMouseExit()
    {
        highlightSprite.color = transparent_color;  // 鼠标移开，取消高亮 
    }

    private void OnMouseDown()
    {
        if (is_button_available)
            click_mouse_position = Input.mousePosition;
        
    }

    private void OnMouseUp()
    {
        if (is_button_available
            && ((Vector2)Input.mousePosition - click_mouse_position).magnitude < 0.6f)  // 鼠标位置基本没变
        {
            
            // 播放 按钮音效
            GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Resource_Button);
            
            AbsorbResource();
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (is_button_available)
        {
            
            if (eventData.button == PointerEventData.InputButton.Left)
            {
            
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("右键点击");

                ReduceResource();
            
                // 播放 按钮音效
                GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Resource_Button);

            }
            
            
        }
    }

    
    
    
}
