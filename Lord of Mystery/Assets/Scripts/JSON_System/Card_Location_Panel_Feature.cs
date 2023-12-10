using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Card_Location_Panel_Feature : MonoBehaviour
{
    [Header("Card")]
    public GameObject attached_card;     // 用于指代生成此 panel 的卡牌
    public Card_Location_Feature attached_card_location_feature;   // 生成此 panel 的卡牌的 card_location_feature 脚本
    

    public bool is_mouse_hover_on_panel;
    
    // panel section
    [Header("Panel Section")]
    public GameObject panel_section_resource;       // panel resource section 指代，手动拖拽
    public GameObject panel_section_body_part;      // panel body part section 指代，手动拖拽
    public GameObject panel_section_without_body_part;      // 没有 body part 的 section，手动拖拽
    public GameObject start_button;      // 此 panel 上的 start button   

    [Space(5)] 
    public TMP_Text resource_1_amount_text;
    public TMP_Text resource_2_amount_text;
    public TMP_Text resource_3_amount_text;
    public TMP_Text resource_4_amount_text;
    public TMP_Text resource_5_amount_text;

    // Resource and Body Part
    public Dictionary<int, bool> availableResourceSlot;     // 用于记录 resource section 上 available 的 button slot
    public Dictionary<string, int> requiredResourcesThisPanel;  // 用于记录 required resource 及对应数量的 字典
    public Dictionary<int, bool> availableBodyPartSlot;     // 用于记录 body part section 上 available 的 slot
    public List<string> requiredBodyPartsThisPanel;         // 用于记录需要的 body part 的 list
    
    private int resource_1_amount = -1;                 // resource_X_amount 用于记录该资源需要消耗的 总数
    private int current_resource_1_amount = 0;          // current_resource_X_amount 用于记录 panel 当前吸收的资源数量
    private int resource_2_amount = -1;
    private int current_resource_2_amount = 0;
    private int resource_3_amount = -1;
    private int current_resource_3_amount = 0;
    private int resource_4_amount = -1;
    private int current_resource_4_amount = 0;
    private int resource_5_amount = -1;
    private int current_resource_5_amount = 0;
    private bool is_body_part_1_filled = false;         // 是否用 body part 填充了
    private bool is_body_part_2_filled = false;
    private bool is_body_part_3_filled = false;
    private bool is_body_part_4_filled = false;
    

    // Resource Button Prefab
    [Header("Resource Button Prefab")] 
    public GameObject Button_Fund;
    public GameObject Button_Physical_Energy;
    public GameObject Button_Spirit;
    public GameObject Button_Soul;
    public GameObject Button_Spirituality_Infused_Material;
    public GameObject Button_Knowledge;
    public GameObject Button_Belief;
    public GameObject Button_Putrefaction;
    public GameObject Button_Madness;
    public GameObject Button_Godhood;

    // panel 外观设置
    [Header("Panel Appearance")]
    public SpriteRenderer panel_image;   
    public TMP_Text panel_label;
    public TMP_Text panel_description;


    private void Start()
    {
        Set_Panel_Section();
        Set_Start_Button();
    }

    private void Update()
    {
        Update_Resource_Number();
    }


    /////////////////     设置函数

    public void Set_Attached_Card(GameObject attachedCard)     // 设置生成此 panel 的卡牌指代，顺便设置 feature 指代，实例化 Panel 时从外部设置
    {                                                          // 只能从外部设置，因为从 panel 自身无法查找到生成 panel 的卡是哪张
        attached_card = attachedCard;
        attached_card_location_feature = attached_card.GetComponent<Card_Location_Feature>();
    }
    
    void Set_Panel_Section()        // 根据 是否消耗 resource 和 body part，设置 section 是否出现及出现的位置
    {
        bool isRequireResource = false;
        bool isRequireBodyParts = false;
        requiredResourcesThisPanel = new Dictionary<string, int>() { };
        requiredBodyPartsThisPanel = new List<string>() { };
        
        // 检查所有的 required_resource 值是否大于0，如果有大于0的则说明此 panel 对应的卡牌需要消耗资源，则需要 resource section 
        foreach (KeyValuePair<string,int> required_resource in attached_card_location_feature.required_resources)
        {
            if (required_resource.Value > 0)
            {
                isRequireResource = true;
                requiredResourcesThisPanel.Add(required_resource.Key,required_resource.Value);
            }
        }

        // 检查所有的 required_body_part 值是否大于0，如果有大于0的则说明此 panel 对应的卡牌需要 body part 参与，则需要 body part section
        foreach (KeyValuePair<string,int> required_body_part in attached_card_location_feature.required_body_parts)
        {
            if (required_body_part.Value > 0)
            {
                isRequireBodyParts = true;
                for (int i = required_body_part.Value; i > 0; i--)
                {
                    requiredBodyPartsThisPanel.Add(required_body_part.Key);
                }
            }
        }

        // 根据上方的检查结果，设置 section 是否出现以及位置
        
        if (isRequireBodyParts) // 根据是否需要 body part，设置 start button 所在的 section，粗或细
        {
            panel_section_body_part.SetActive(true);
            start_button.transform.parent = panel_section_body_part.transform;
            start_button.transform.localPosition =
                GameObject.Find("Start_Button_Location_With_BodyParts").transform.localPosition;
        }
        else
        {
            panel_section_without_body_part.SetActive(true);
            start_button.transform.parent = panel_section_without_body_part.transform;
            start_button.transform.localPosition =
                GameObject.Find("Start_Button_Location_Without_BodyParts").transform.localPosition;
        }
        
        if (isRequireResource)  // 如果有 required resource, 则需要 resource section，相应地设置 resource section 和 body part section 的位置
        {
            panel_section_resource.SetActive(true);
            panel_section_resource.transform.localPosition = GameObject.Find("Section_Location_Description_End").transform.localPosition;
            if (isRequireBodyParts)
            {
                panel_section_body_part.transform.localPosition =
                    GameObject.Find("Section_Location_Resource_End").transform.localPosition;
            }
            else
            {
                panel_section_without_body_part.transform.localPosition =
                    GameObject.Find("Section_Location_Resource_End").transform.localPosition;
            }
        }
        else    // 如果不需要 required resource, 则设置 body part section 的位置
        {
            if (isRequireBodyParts)
            {
                panel_section_body_part.transform.localPosition =
                    GameObject.Find("Section_Location_Description_End").transform.localPosition;
            }
            else
            {
                panel_section_without_body_part.transform.localPosition =
                    GameObject.Find("Section_Location_Description_End").transform.localPosition;
            }
        }

        if (requiredResourcesThisPanel.Count > 0)
        {
            Set_Resource_Button();      // 实例化 resource button
        }
        
        Set_Body_Part_Slots();

    }

    public void Set_Resource_Button()       // 具体方法 ：实例化 resource button，并设置 消耗资源的参数
    {
        availableResourceSlot = new Dictionary<int, bool>()     // 初始化 slot 字典
        {
            { 1, true },
            { 2, true },
            { 3, true },
            { 4, true },
            { 5, true }
        };

        // 根据上方初始化方法构建的 require resource 字典，生成相应的 resource button，在相应的 available slots 上
        foreach (var resource in requiredResourcesThisPanel)
        {
            GameObject resource_button = null;
            
            if (resource.Key == "Fund")
            {
                resource_button = Instantiate(Button_Fund, panel_section_resource.transform);    // 实例化 button
            }
            else if (resource.Key == "Physical_Energy")
            {
                resource_button = Instantiate(Button_Physical_Energy, panel_section_resource.transform);    // 实例化 button
            }
            else if (resource.Key == "Spirit")
            {
                resource_button = Instantiate(Button_Spirit, panel_section_resource.transform);    // 实例化 button
            }
            else if (resource.Key == "Soul")
            {
                resource_button = Instantiate(Button_Soul, panel_section_resource.transform);    // 实例化 button
            }
            else if (resource.Key == "Spirituality_Infused_Material")
            {
                resource_button = Instantiate(Button_Spirituality_Infused_Material, panel_section_resource.transform);    // 实例化 button
            }
            else if (resource.Key == "Knowledge")
            {
                resource_button = Instantiate(Button_Knowledge, panel_section_resource.transform);    // 实例化 button
            }
            else if (resource.Key == "Belief")
            {
                resource_button = Instantiate(Button_Belief, panel_section_resource.transform);    // 实例化 button
            }
            else if (resource.Key == "Putrefaction")
            {
                resource_button = Instantiate(Button_Putrefaction, panel_section_resource.transform);    // 实例化 button
            }
            else if (resource.Key == "Madness")
            {
                resource_button = Instantiate(Button_Madness, panel_section_resource.transform);    // 实例化 button
            }
            else if (resource.Key == "Godhood")
            {
                resource_button = Instantiate(Button_Godhood, panel_section_resource.transform);    // 实例化 button
            }
            
            Find_Resource_Available_Slot_And_Set_Value(resource_button, resource.Value);     // 调用方法：寻找 available 的 slot 并将按钮放置在相应的位置
        }
        
    }

    public void Find_Resource_Available_Slot_And_Set_Value(GameObject button, int value)    // 寻找 available 的 slot 并将按钮放置在相应的位置，设置消耗资源总数的值
    {
        for (int i = 1; i < availableResourceSlot.Count; i++)
        {
            if (availableResourceSlot[i])
            {
                availableResourceSlot[i] = false;
                button.transform.localPosition = GameObject.Find("Resource_"+i).transform.localPosition;   // 放置到 available slot 的空物体位置
                
                // 根据 slot 的编号，设置对应 资源总数参数的值
                if (i == 1) { resource_1_amount = value; }
                if (i == 2) { resource_2_amount = value; }
                if (i == 3) { resource_3_amount = value; }
                if (i == 4) { resource_4_amount = value; }
                if (i == 5) { resource_5_amount = value; }
                break;
            }
        }
    }

    public void Set_Body_Part_Slots()
    {
        availableBodyPartSlot = new Dictionary<int, bool>()
        {
            { 1, true },
            { 2, true },
            { 3, true },
            { 4, true }
        };
        
        
    }
    
    void Set_Start_Button()         // 设置按钮的指代
    {
        if (start_button == null)
        {
            start_button = GameObject.Find("Start_Button");
        }

        if (start_button.GetComponent<Start_Button_Script>().attached_panel == null)    // 设置按钮脚本对 panel 的指代
        {
            start_button.GetComponent<Start_Button_Script>().Set_Attached_Panel(gameObject);
        }
          
        
        start_button.GetComponent<Start_Button_Script>().Set_Button_Availability(true); // 让按钮可点击，test用
    }


    // 如果相应的资源 总数大于0（说明需要消耗，不涉及该资源则为 -1）且当前未吸收满（总数 - current > 0), 则显示剩余需要吸收的数量，否则不显示
    private void Update_Resource_Number()       
    {
        if (resource_1_amount > 0 && resource_1_amount - current_resource_1_amount > 0) 
        { resource_1_amount_text.text = (resource_1_amount - current_resource_1_amount).ToString(); }
        else { resource_1_amount_text.text = ""; }
        
        if (resource_2_amount > 0 && resource_2_amount - current_resource_2_amount > 0) 
        { resource_2_amount_text.text = (resource_2_amount - current_resource_2_amount).ToString(); }
        else { resource_2_amount_text.text = ""; }
        
        if (resource_3_amount > 0 && resource_3_amount - current_resource_3_amount > 0) 
        { resource_3_amount_text.text = (resource_3_amount - current_resource_3_amount).ToString(); }
        else { resource_3_amount_text.text = ""; }
        
        if (resource_4_amount > 0 && resource_4_amount - current_resource_4_amount > 0) 
        { resource_4_amount_text.text = (resource_4_amount - current_resource_4_amount).ToString(); }
        else { resource_4_amount_text.text = ""; }
        
        if (resource_5_amount > 0 && resource_5_amount - current_resource_5_amount > 0) 
        { resource_5_amount_text.text = (resource_5_amount - current_resource_5_amount).ToString(); }
        else { resource_5_amount_text.text = ""; }
        
    }

    public void Set_Sprite(Sprite sprite)
    {
        panel_image.sprite = sprite;
    }

    public void Set_Label(string label)
    {
        panel_label.text = label;
    }

    public void Set_Description(string description)
    {
        panel_description.text = description;
    }


    private void OnMouseOver()
    {
        Debug.Log("mouse over big panel");
        is_mouse_hover_on_panel = true;
    }
}
