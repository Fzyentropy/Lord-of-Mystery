using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Card_Location_Panel_Feature : MonoBehaviour
{
    [Header("Card")]
    public GameObject attached_card;     // 用于指代生成此 panel 的卡牌
    public Card_Location_Feature attached_card_location_feature;   // 生成此 panel 的卡牌的 card_location_feature 脚本
    
    
    // panel section
    [Header("Panel Section")] 
    public GameObject panel_section_description;    // panel Description 板块的指代
    public GameObject panel_section_resource;       // panel resource section 指代，手动拖拽
    public GameObject panel_section_body_part;      // panel body part section 指代，手动拖拽
    public GameObject panel_section_without_body_part;      // 没有 body part 的 section，手动拖拽
    public GameObject start_button;      // 此 panel 上的 start button   
    
    // 进度条
    [Header("Progress Bar")]
    public GameObject progress_bar_prefab;        // 进度条 prefab
    [HideInInspector]public GameObject progress_bar;               // 进度条 指代
    [HideInInspector]public GameObject progress_bar_root;          // 进度条方块的根 指代
    public GameObject progress_bar_position;      // 进度条位置标记 空物体
    [HideInInspector]public TMP_Text countdown_text;               // 显示秒数文本
    

    [Space(5)] 
    [Header("Resource Text")]
    public TMP_Text resource_1_amount_text;         // 场景中各资源对应的 TMP_text 数量文本
    public TMP_Text resource_2_amount_text;
    public TMP_Text resource_3_amount_text;
    public TMP_Text resource_4_amount_text;
    public TMP_Text resource_5_amount_text;
    // 添加更多的资源槽位，如果需要 - resource_X_amount_text，需要在 prefab 中添加对应的 TMP_text 和 icon

    // Resource 相关的 Dictionary
    [Header("Dictionary")]
    public Dictionary<int, bool> availableResourceSlot;     // 用于记录 resource section 上 available 的 button slot
    public Dictionary<string, int> requiredResourcesThisPanel;  // 用于记录 required resource 及对应数量的 字典    string: 资源名称， int: 资源需要的数量
    public Dictionary<string, int> resourceSlotNumber = new Dictionary<string, int>() {};    // 用于记录 required resource 及对应的 slot 编号,  string: 资源名称， int: 资源按钮的 slot 的编号/位置
    
    // Body Part 相关的 Dictionary
    public Dictionary<int, string> requiredBodyPartsThisPanel;         // 用于记录需要的 body part 的 Dictionary, int:槽位编号, string:Body Part类型的string
    // public List<string> currentlyAbsorbedBodyPart;          // 用于记录当前已经吸收的 Body Part，临时，可能没必要
    public Dictionary<int, bool> currentlyAbosorbedBodyPartSlots;       // 用于记录body part的各个slot是否已经吸收, int:槽位编号, bool:是否已经吸收body part (true: 吸收了, false: 没吸收)
    public Dictionary<int, GameObject> absorbedBodyPartGameObjects;     // 用于记录 已经吸收的 body part 的 Game Object，方便访问

    [Header("Resource Amount")]
    public int resource_1_amount = -1;                 // resource_X_amount 用于记录该资源需要消耗的 总数
    public int current_resource_1_amount = 0;          // current_resource_X_amount 用于记录 panel 当前吸收的资源数量
    public int resource_2_amount = -1;
    public int current_resource_2_amount = 0;
    public int resource_3_amount = -1; 
    public int current_resource_3_amount = 0;
    public int resource_4_amount = -1;
    public int current_resource_4_amount = 0;
    public int resource_5_amount = -1;
    public int current_resource_5_amount = 0;

    // Knowledge 资源相关
    public List<string> absorbed_knowledge_list; 

    // Panel Status
    private bool is_resource_ok;
    private bool is_bodypart_ok;
    
    

    // Resource Button Prefab，panel 上的资源按钮
    [Header("Resource Button Prefab")] 
    public GameObject Button_Fund;
    public GameObject Button_Physical_Energy;
    public GameObject Button_Spiritual_Energy;
    public GameObject Button_Soul;
    public GameObject Button_Spirituality_Infused_Material;
    public GameObject Button_Knowledge;
    public GameObject Button_Belief;
    public GameObject Button_Putrefaction;
    public GameObject Button_Madness;
    public GameObject Button_Godhood;
    public GameObject Button_Death;     // Death button added
    
    [Header("Panel Effect Description Resource Icon Prefab")] 
    public GameObject Icon_Fund;
    public GameObject Icon_Physical_Energy;
    public GameObject Icon_Spiritual_Energy;
    public GameObject Icon_Soul;
    public GameObject Icon_Spirituality_Infused_Material;
    public GameObject Icon_Knowledge;
    public GameObject Icon_Belief;
    public GameObject Icon_Putrefaction;
    public GameObject Icon_Madness;
    public GameObject Icon_Godhood;
    public GameObject Icon_Death;     // Death button added

    [Header("Other Prefab")] 
    public GameObject question_mark;    // 用于 Effect Description，描述未知效果
    public GameObject special_effect_icon;    // 用于 Effect Description，描述 special effect
    public GameObject draw_card_icon;   // 用于 Effect Description，描述抽新卡的效果
    
    // Body Part prefab, Body part 的 Card prefab
    [Header("Body Part Prefab")]
    public GameObject Body_Part_Prefab_Physical_Body;
    public GameObject Body_Part_Prefab_Spirit;
    public GameObject Body_Part_Prefab_Psyche;
     
    // Body Part prefab，panel 上的 body part 槽位（也是按钮）
    [Header("Body Part Slot Prefab")] 
    public GameObject Slot_Physical_Body;
    public GameObject Slot_Spirit;
    public GameObject Slot_Psyche;
    public GameObject Slot_Potion;
    public GameObject Slot_Save;

    // panel 外观设置
    [Header("Panel Appearance")]
    public SpriteRenderer panel_image;   
    public TMP_Text panel_label;
    public TMP_Text panel_description;
    public TMP_Text panel_effect_description;


    // Mis Variables
    private Vector3 click_mouse_position;       // 用于点击时记录鼠标的位置
    private Vector3 lastMousePosition;      // 用于记录鼠标拖拽时，前一帧鼠标的位置
    public bool isPanelWellSet = false;     // 用于判定这个 panel 是否已经初始化完全，用以调用 Find_First_Empty_Body_Part_Type_In_Slots 方法

    


    private void Start()
    {
        Set_Panel_Section();
        Set_Start_Button();
        Set_Description_Font_Size();    // 若是特殊卡，设置 description 字体大小
        // StartCoroutine(Shift_Label_And_Description_If_Absorbed());    // 切换 Label 和 Description， 如果吸收满了  // 此方法目前会导致无限循环，卡死，先注掉
        
    }

    private void Update()
    {
        
        if (!attached_card_location_feature.is_counting_down)    // 如果不在 countdown，则检测
        {
            Update_Resource_Number();
            Check_If_Absorb_All_Resource();
            Check_If_Absorb_All_BodyParts();
        }
        
        Set_Start_Button_Availablitity();
        Set_Start_Button_Text();

    }

    

    ///////////////////////////////////////////////////     设置函数
    

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
        requiredBodyPartsThisPanel = new Dictionary<int, string>() { };
        currentlyAbosorbedBodyPartSlots = new Dictionary<int, bool>() { };
        absorbedBodyPartGameObjects = new Dictionary<int, GameObject>() {};
        absorbed_knowledge_list = new List<string>() { };
        int temporaryBodyPartSlotNumber = 1;            // 用于初始化 Body Part 槽位 Dictionary<int, string> 中的 int 参数
        
        
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
                for (int i = required_body_part.Value; i > 0; i--)      // 将需要的 Body Part 记录进 Body Part 槽位字典 Dictionary<int,string>
                {
                    requiredBodyPartsThisPanel.Add(temporaryBodyPartSlotNumber,required_body_part.Key);
                    temporaryBodyPartSlotNumber++;
                }
            } 
        }

        
        // 显示 这张卡的效果，生产多少资源，生产卡牌或者特殊效果，未知效果

        /*if (attached_card_location_feature.produce_resources.Count > 0)        
            {
                string effect_text = "";
                
                foreach (var resource in attached_card_location_feature.produce_resources)
                {
                    if (resource.Value != 0)
                    {
                        effect_text += resource.Key + " x " + resource.Value + " ";
                    }
                }
                
                if (effect_text != "")
                    panel_effect_description.text = "Produce: " + effect_text;
            }*/

        
        // 显示 这张卡的效果，生产多少资源，生产卡牌或者特殊效果，未知效果
        {

            Vector3 description_position = GameObject.Find("Effect_Description_Root").transform.localPosition;
            GameObject icon = null;
            List<GameObject> icon_to_generate = new List<GameObject>() { };

            
            if (attached_card_location_feature._cardLocation.Invisible_Description)     // 如果是 Invisible，则生成 ？
            {
                icon = Instantiate(question_mark, panel_section_description.transform);
                icon.transform.localPosition = description_position;
                icon_to_generate.Add(icon);
            }
            
            else    // 如果不 Invisible，则根据 produce 的东西生成 icon
            {
                
                // 生成 resource icon
                foreach (var resource in attached_card_location_feature.produce_resources)
                {
                    if (resource.Value != 0)
                    {
                    
                        if (resource.Key == "Physical_Energy")
                        {
                            icon = Instantiate(Icon_Physical_Energy, panel_section_description.transform);
                        }
                        if (resource.Key == "Fund")
                        {
                            icon = Instantiate(Icon_Fund, panel_section_description.transform);
                        }
                        if (resource.Key == "Spiritual_Energy")
                        {
                            icon = Instantiate(Icon_Spiritual_Energy, panel_section_description.transform);
                        }
                        if (resource.Key == "Soul")
                        {
                            icon = Instantiate(Icon_Soul, panel_section_description.transform);
                        }
                        if (resource.Key == "Spirituality_Infused_Material")
                        {
                            icon = Instantiate(Icon_Spirituality_Infused_Material, panel_section_description.transform);
                        }
                        if (resource.Key == "Knowledge")
                        {
                            icon = Instantiate(Icon_Knowledge, panel_section_description.transform);
                        }
                        if (resource.Key == "Belief")
                        {
                            icon = Instantiate(Icon_Belief, panel_section_description.transform);
                        }
                        if (resource.Key == "Putrefaction")
                        {
                            icon = Instantiate(Icon_Putrefaction, panel_section_description.transform);
                        }
                        if (resource.Key == "Madness")
                        {
                            icon = Instantiate(Icon_Madness, panel_section_description.transform);
                        }
                        if (resource.Key == "Godhood")
                        {
                            icon = Instantiate(Icon_Godhood, panel_section_description.transform);
                        }
                        if (resource.Key == "Death")
                        {
                            icon = Instantiate(Icon_Death, panel_section_description.transform);
                        }
                        // ...

                        icon.transform.localPosition = description_position;

                        // 加 Text 标明数量
                        
                        icon_to_generate.Add(icon);

                    }
                }

                if (attached_card_location_feature._cardLocation.Produce_Card_Location.Count > 0)
                {
                    icon = Instantiate(draw_card_icon, panel_section_description.transform);
                    icon.transform.localPosition = description_position;
                    icon_to_generate.Add(icon);
                }

                if (attached_card_location_feature.special_effect.Count > 0)
                {
                    icon = Instantiate(special_effect_icon, panel_section_description.transform);
                    icon.transform.localPosition = description_position;
                    icon_to_generate.Add(icon);
                }
            }


            if (icon_to_generate.Count > 0)
            {
                Vector3 icon_position_offset = new Vector3(0, 0, 0);
                Vector3 icon_position_interval = new Vector3(-1.68f, 0, 0);

                for (int i = icon_to_generate.Count - 1; i >= 0; i--)
                {
                    icon_to_generate[i].transform.localPosition += icon_position_offset;
                    icon_position_offset += icon_position_interval;

                    icon_to_generate[i].GetComponent<SpriteRenderer>().sortingLayerName = "Panel";
                    icon_to_generate[i].GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
                
            }
            
            
            
        }
        


        if (attached_card_location_feature.is_counting_down)        // 如果在 Count down，则 显示倒计时
        {
            panel_section_without_body_part.SetActive(true);
            start_button.transform.parent = panel_section_without_body_part.transform;
            start_button.transform.localPosition =
                GameObject.Find("Start_Button_Location_Without_BodyParts").transform.localPosition;
            
            // 实例化 进度条 prefab
            progress_bar = Instantiate(progress_bar_prefab, transform.position, Quaternion.identity);
            progress_bar.transform.parent = panel_section_without_body_part.transform;
            progress_bar.transform.localPosition = progress_bar_position.transform.localPosition;
            progress_bar_root = progress_bar.transform.Find("Bar_Root").gameObject;
            countdown_text = progress_bar.GetComponentInChildren<TMP_Text>();

            StartCoroutine(panel_progress_bar_sync());

        }
        
        
        else          // 如果没在 Countdown，则正常设置 panel sections
        
        {

            // 根据上方的检查结果，设置 section 是否出现以及位置
            
            // 根据是否需要 body part，设置 start button 所在的 section，粗或细
            
            if (isRequireBodyParts) 
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
            
            start_button.transform.localPosition = new Vector3(         // 调整 start button z轴坐标 -1
                start_button.transform.localPosition.x,
                start_button.transform.localPosition.y,
            start_button.transform.localPosition.z -1);
            
            
            
            // 如果有 required resource, 则需要 resource section，相应地设置 resource section 和 body part section 的位置

            if (isRequireResource) 
            {
                panel_section_resource.SetActive(true);
                panel_section_resource.transform.localPosition =
                    GameObject.Find("Section_Location_Description_End").transform.localPosition;
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
            else // 如果不需要 required resource, 则设置 body part section 的位置
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

            

            if (requiredResourcesThisPanel.Count > 0) // 如果需要消耗 resource，则根据需要的 resource 生成 resource 按钮
            {
                Set_Resource_Button();
            }

            if (requiredBodyPartsThisPanel.Count > 0) // 如果需要消耗 body part，则根据需要的 body part 生成 body part 的槽位
            {
                Set_Body_Part_Slots();
            }
            
        }
        

        isPanelWellSet = true;
    }

    IEnumerator panel_progress_bar_sync()
    {
        while (true)

        {
            if (progress_bar != null)
            {
                progress_bar_root.transform.localScale =
                    attached_card_location_feature.progress_bar_root.transform.localScale;
                countdown_text.text = attached_card_location_feature.countdown_text.text;
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
    

    public void Set_Resource_Button()       // 具体方法 ：实例化 resource button，并设置 消耗资源的参数
    {
        availableResourceSlot = new Dictionary<int, bool>()     // 初始化 slot 字典 (暂定5个slot，将来或许添加更多)
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
                // resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Current_Resource("Fund");  //设置按钮操纵的资源为 Fund
            }
            else if (resource.Key == "Physical_Energy")
            {
                resource_button = Instantiate(Button_Physical_Energy, panel_section_resource.transform);    // 实例化 button
                // resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Current_Resource("Physical_Energy");  //设置按钮操纵的资源为Physical_Energy
            }
            else if (resource.Key == "Spiritual_Energy")
            {
                resource_button = Instantiate(Button_Spiritual_Energy, panel_section_resource.transform);    // 实例化 button
                // resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Current_Resource("Spirit");  //设置按钮操纵的资源为 Spirit
            }
            else if (resource.Key == "Soul")
            {
                resource_button = Instantiate(Button_Soul, panel_section_resource.transform);    // 实例化 button
                // resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Current_Resource("Soul");  //设置按钮操纵的资源为 Soul
            }
            else if (resource.Key == "Spirituality_Infused_Material")
            {
                resource_button = Instantiate(Button_Spirituality_Infused_Material, panel_section_resource.transform);    // 实例化 button
                // resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Current_Resource("Spirituality_Infused_Material");  //设置按钮操纵的资源为
            }
            else if (resource.Key == "Knowledge")
            {
                resource_button = Instantiate(Button_Knowledge, panel_section_resource.transform);    // 实例化 button
                // resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Current_Resource("Knowledge");  //设置按钮操纵的资源为 Knowledge
            }
            else if (resource.Key == "Belief")
            {
                resource_button = Instantiate(Button_Belief, panel_section_resource.transform);    // 实例化 button
                // resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Current_Resource("Belief");  //设置按钮操纵的资源为 Belief
            }
            else if (resource.Key == "Putrefaction")
            {
                resource_button = Instantiate(Button_Putrefaction, panel_section_resource.transform);    // 实例化 button
                // resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Current_Resource("Putrefaction");  //设置按钮操纵的资源为 Putrefaction
            }
            else if (resource.Key == "Madness")
            {
                resource_button = Instantiate(Button_Madness, panel_section_resource.transform);    // 实例化 button
                // resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Current_Resource("Madness");  //设置按钮操纵的资源为 Madness
            }
            else if (resource.Key == "Godhood")
            {
                resource_button = Instantiate(Button_Godhood, panel_section_resource.transform);    // 实例化 button
                // resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Current_Resource("Godhood");  //设置按钮操纵的资源为 Godhood
            }
            else if (resource.Key == "Death")
            {
                resource_button = Instantiate(Button_Death, panel_section_resource.transform);    // 实例化 button
                // resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Current_Resource("Godhood");  //设置按钮操纵的资源为 Godhood
            }
            
            

            if (resource.Key == "Knowledge")
            {
                resource_button.GetComponent<Card_Location_Panel_Knowledge_Slot>().Set_Attached_Panel(gameObject);  // 设置 knowledge slot 的 panel 指代为此
            }
            else
            {
                resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Attached_Panel(gameObject); // 设置 resource button 的 panel 指代为此
            }
            
            Find_Resource_Available_Slot_And_Set_Value(resource_button, resource.Key, resource.Value);   // 调用方法：寻找 available 的 slot 并将按钮放置在相应的位置，设置字典
        }
        
    }

    // 寻找 available 的 slot 并将按钮放置在 slot 对应的位置，
    // 根据 slot 编号 X 设置消耗资源总数的值 resource_X_amount，
    // 记录资源及对应编号到字典 resourceSlotNumber 中
    public void Find_Resource_Available_Slot_And_Set_Value(GameObject button, string resourceName, int value)    
    {
        for (int i = 1; i < availableResourceSlot.Count; i++)
        {
            if (availableResourceSlot[i])
            {
                availableResourceSlot[i] = false;

                if (resourceName == "Knowledge")
                {
                    Move_Resource_Button_Markers_InPanel_For_Knowledge(i);      // 如果是 Knowledge，则移动 Marker 位置
                }
                
                button.transform.localPosition = GameObject.Find("Resource_"+i).transform.localPosition;   // 放置到 available slot 的空物体位置
                button.transform.localPosition = new Vector3(           // 设置 Z轴坐标为 -1
                    button.transform.localPosition.x,
                    button.transform.localPosition.y,
                    button.transform.localPosition.z - 1);
                
                
                // 根据 slot 的编号，设置对应 资源总数 参数的值, 并调用 resource button 中的方法，设置 resource button 中对应在此 panel 上资源编号的参数
                if (i == 1)
                {
                    resource_1_amount = value;      // 设置 resource 1 的数量为 目前引用的资源数量
                    
                    if (resourceName == "Knowledge")
                        button.GetComponent<Card_Location_Panel_Knowledge_Slot>().Set_Resource_Slot_Number_Knowledge(1);
                    else
                        button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Resource_Slot_Number(1);    // 设置 resource Button 对应的资源 slot 为 1

                    resourceSlotNumber.Add(resourceName,1);     // 记录 当前资源对应的 slot 编号 1，到字典 resourceSlotNumber
                }

                if (i == 2)
                {
                    resource_2_amount = value; 
                    
                    if (resourceName == "Knowledge")
                        button.GetComponent<Card_Location_Panel_Knowledge_Slot>().Set_Resource_Slot_Number_Knowledge(2);
                    else
                        button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Resource_Slot_Number(2);

                    resourceSlotNumber.Add(resourceName,2);
                }

                if (i == 3)
                {
                    resource_3_amount = value; 
                    
                    if (resourceName == "Knowledge")
                        button.GetComponent<Card_Location_Panel_Knowledge_Slot>().Set_Resource_Slot_Number_Knowledge(3);
                    else
                        button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Resource_Slot_Number(3);
                    
                    resourceSlotNumber.Add(resourceName,3);
                }

                if (i == 4)
                {
                    resource_4_amount = value; 
                    
                    if (resourceName == "Knowledge")
                        button.GetComponent<Card_Location_Panel_Knowledge_Slot>().Set_Resource_Slot_Number_Knowledge(4);
                    else
                        button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Resource_Slot_Number(4);
                    
                    resourceSlotNumber.Add(resourceName,4);
                }

                if (i == 5)
                {
                    resource_5_amount = value; 
                    
                    if (resourceName == "Knowledge")
                        button.GetComponent<Card_Location_Panel_Knowledge_Slot>().Set_Resource_Slot_Number_Knowledge(5);
                    else
                        button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Resource_Slot_Number(5);
                    
                    resourceSlotNumber.Add(resourceName,5);
                }
                
                // 如果将来需要更多的 Slot，则在这里添加
                
                break;
            }
        }
    }
    
    
    void Move_Resource_Button_Markers_InPanel_For_Knowledge(int knowledge_button_X)
    {

        float knowledge_icon_move_distance = 1f;
        float knowledge_text_move_distance = 2f;
        float other_markers_move_distance = 2.28f;

        GameObject.Find($"Resource_{knowledge_button_X}").
            transform.localPosition += new Vector3(knowledge_icon_move_distance, 0, 0);

        GameObject.Find($"Resource_Text_{knowledge_button_X}").
            transform.localPosition += new Vector3(knowledge_text_move_distance, 0, 0);

        int index = knowledge_button_X + 1;

        while (true)
        {
            if (GameObject.Find($"Resource_{index}") == null)
                break;
            
            GameObject.Find($"Resource_{index}").
                transform.localPosition += new Vector3(other_markers_move_distance, 0, 0);
            GameObject.Find($"Resource_Text_{index}").
                transform.localPosition += new Vector3(other_markers_move_distance, 0, 0);

            index++;
        }

    }

    public void Set_Body_Part_Slots()           // 根据 这张卡需要的 body part 设置 body part slot   // TODO 有 bug ： 打开 panel 的时候 null reference
    {

        foreach (var body_part in requiredBodyPartsThisPanel)   // 对于每一个需要的 body part
        {
         
            // 初始化记录当前 body part slot 占用情况字典，将所有 body part 槽位对应的占用设置为 false(尚未占用），保证了记录是否占用槽位的字典与 required body part 序号相等
            currentlyAbosorbedBodyPartSlots.Add(body_part.Key, false);      

            
            // Instantiate body part
            // place it to the slot position

            GameObject bodyPartSlot = null;

            if (body_part.Value == "Physical_Body")             // 根据其类型，生成不同的槽位 prefab
            {
                bodyPartSlot = Instantiate(Slot_Physical_Body, panel_section_body_part.transform);
                
                // 设置 Body Part slot 脚本的各参数
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().attached_card_location_panel_feature = this;
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().slot_number_in_panel = body_part.Key;
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().BodyPartSlotType =
                    Card_Location_Panel_Body_Part_Slot.Body_Part_Slot_Type.Physical_Body;
            }
            if (body_part.Value == "Spirit")
            {
                bodyPartSlot = Instantiate(Slot_Spirit, panel_section_body_part.transform);
                
                // 设置 Body Part slot 脚本的各参数
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().attached_card_location_panel_feature = this;
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().slot_number_in_panel = body_part.Key;
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().BodyPartSlotType =
                    Card_Location_Panel_Body_Part_Slot.Body_Part_Slot_Type.Spirit;
            }
            if (body_part.Value == "Psyche")
            {
                bodyPartSlot = Instantiate(Slot_Psyche, panel_section_body_part.transform);
                
                // 设置 Body Part slot 脚本的各参数
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().attached_card_location_panel_feature = this;
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().slot_number_in_panel = body_part.Key;
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().BodyPartSlotType =
                    Card_Location_Panel_Body_Part_Slot.Body_Part_Slot_Type.Psyche;
            }
            if (body_part.Value == "Potion")
            {
                bodyPartSlot = Instantiate(Slot_Potion, panel_section_body_part.transform);
                
                // 设置 Body Part slot 脚本的各参数
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().attached_card_location_panel_feature = this;
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().slot_number_in_panel = body_part.Key;
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().BodyPartSlotType =
                    Card_Location_Panel_Body_Part_Slot.Body_Part_Slot_Type.Potion;
            }
            
            if (body_part.Value == "Save")
            {
                bodyPartSlot = Instantiate(Slot_Save, panel_section_body_part.transform);
                
                // 设置 Body Part slot 脚本的各参数
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().attached_card_location_panel_feature = this;
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().slot_number_in_panel = body_part.Key;
                bodyPartSlot.GetComponent<Card_Location_Panel_Body_Part_Slot>().BodyPartSlotType =
                    Card_Location_Panel_Body_Part_Slot.Body_Part_Slot_Type.Save;
            }
            
            // TODO 设置 body part slot 的 panel 指代为此 panel
            
            bodyPartSlot.transform.localPosition =
                GameObject.Find("Body_Part_Slot_" + body_part.Key).transform.localPosition;     // 将生成的 slot 移动至 slot 标记点 X

        }

        
    }
    
    void Set_Start_Button()         // 设置start按钮的指代
    {
        if (start_button == null)
        {
            start_button = GameObject.Find("Start_Button");
        }

        if (start_button.GetComponent<Start_Button_Script>().attached_panel == null)    // 设置按钮脚本对 panel 的指代
        {
            start_button.GetComponent<Start_Button_Script>().Set_Attached_Panel(gameObject);
        }
          
        
        // start_button.GetComponent<Start_Button_Script>().Set_Button_Availability(true); // 让按钮可点击，test用
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

    public void Set_Description_Font_Size()
    {
        if (attached_card_location_feature._cardLocation.Card_Type == "Title")
        {
            panel_description.fontSize = 10;
        }
    }
    
    ///////////////////////////////////////////////////     设置函数结束
    
    
    
    
    ///////////////////////////////////////////////////     On 事件函数 写在了另外的 component上
    /*private void OnMouseOver()
    {

    }

    private void OnMouseDown()
    {
        // 记录鼠标位置
        click_mouse_position = Input.mousePosition;
        lastMousePosition = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        
        GameManager.GM.InputManager.Dragging_Object = gameObject;      // 将 Input Manager 中的 正在拖拽物体 记录为此物体
        // float mouse_drag_sensitivity = 0.05f;
        
        // 如果鼠标移动，卡牌随之移动
        Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(lastMousePosition);
        delta.z = 0;
        gameObject.transform.position += delta;
        lastMousePosition = Input.mousePosition;
        
    }

    private void OnMouseUp()
    {
        GameManager.GM.InputManager.Dragging_Object = null;      // 释放 Input Manager 中的 正在拖拽 GameObject，设置为空
    }

    private void OnMouseExit()
    {
        
    }
    
    private void OnDestroy()        // 当 Panel 被销毁的时候， 返还 resource 和 body part
    {
        // Debug.Log("panel 销毁了");
        // Return_Resource();
    }*/
    ///////////////////////////////////////////////////     On 事件函数结束
    
    
    
    ///////////////////////////////////////////////////     判定函数
    

    public bool Check_If_Absorb_All_Requirements()      // 检查是否吸收完了所有需要的 resource 和 body part
    {
        return is_resource_ok && is_bodypart_ok;
    }

    public bool Check_If_Absorb_All_Resource()      // 检查是否吸收完了所有需要的 resource
    {
        if (requiredResourcesThisPanel.Count > 0)
        {
            bool resource_1 = true;
            bool resource_2 = true;
            bool resource_3 = true;
            bool resource_4 = true;
            bool resource_5 = true;
            
            foreach (var resource in requiredResourcesThisPanel)     // 检查每一个需要消耗的资源
            {
                if (resourceSlotNumber[resource.Key] == 1)       // 如果它对应的 slot 为 1
                {
                    if (resource.Value - current_resource_1_amount > 0)    // 则检查 资源 1 的数量是否达到了 资源需要的总数
                    {
                        resource_1 = false;
                    }
                }
                else

                if (resourceSlotNumber[resource.Key] == 2)       // 如果它对应的 slot 为 2
                {
                    if (resource.Value - current_resource_2_amount > 0)    // 则检查 资源 2 的数量是否达到了 资源需要的总数
                    {
                        resource_2 = false;
                    }
                }
                else

                if (resourceSlotNumber[resource.Key] == 3)       // 如果它对应的 slot 为 3
                {
                    if (resource.Value - current_resource_3_amount > 0)    // 则检查 资源 3 的数量是否达到了 资源需要的总数
                    {
                        resource_3 = false;
                    }
                }
                else

                if (resourceSlotNumber[resource.Key] == 4)       // 如果它对应的 slot 为 4
                {
                    if (resource.Value - current_resource_4_amount > 0)    // 则检查 资源 4 的数量是否达到了 资源需要的总数
                    {
                        resource_4 = false;
                    }
                }
                else

                if (resourceSlotNumber[resource.Key] == 5)       // 如果它对应的 slot 为 5
                {
                    if (resource.Value - current_resource_5_amount > 0)    // 则检查 资源 5 的数量是否达到了 资源需要的总数
                    {
                        resource_5 = false;
                    }
                }

            }

            
            if (resource_1 && resource_2 && resource_3 && resource_4 && resource_5)
            {
                is_resource_ok = true;
                return true;
            }
            else
            {
                is_resource_ok = false;
                return false;
            }
            
        }
        else // 如果不需要消耗任何资源，即 requiredResourcesThisPanel 的字典为空，则返回 true
        {
            is_resource_ok = true;
            return true;
        }
    }



    public bool Check_If_Absorb_All_BodyParts()     // 检查是否吸收完了所有需要的 body part
    {

        if (requiredBodyPartsThisPanel.Count > 0)
        {
            foreach (var slot in currentlyAbosorbedBodyPartSlots)
            {
                if (!slot.Value
                    && requiredBodyPartsThisPanel[slot.Key] != "Save")      // 此句判定为临时，TODO 过滤掉所有 Save
                {
                    is_bodypart_ok = false;
                    return false;
                }
            }
        }

        is_bodypart_ok = true;
        return true;

    }


    public bool Check_If_Start_Absorb_Any_Resource_Or_BodyParts()
    {

        bool startAbsorbResource = false;
        bool startAbsorbBodyPart = false;

        // 检测是否吸收 any Body Part
        foreach (var slot in currentlyAbosorbedBodyPartSlots)
        {
            if (slot.Value)
            {
                startAbsorbBodyPart = true;
            }
        }

        // 检测是否吸收 any Resource
        if (
            current_resource_1_amount > 0
            || current_resource_2_amount > 0
            || current_resource_3_amount > 0
            || current_resource_4_amount > 0
            || current_resource_5_amount > 0)
        {
            startAbsorbResource = true;
        }

        return startAbsorbResource || startAbsorbBodyPart;
    }
    
    // 给定一个 body part，判定 board 上是否有该类型的 body part 卡牌存在
    public bool Have_Giving_Type_Of_Body_Part_On_Board(string bodyPartType)
    {
        if (bodyPartType == "Physical_Body")
        {
            return GameManager.GM.BodyPartManager.Body_Part_Physical_Body > 0;
        }
        else if (bodyPartType == "Spirit")
        {
            return GameManager.GM.BodyPartManager.Body_Part_Spirit > 0;
        }
        else if (bodyPartType == "Psyche")
        {
            return GameManager.GM.BodyPartManager.Body_Part_Psyche > 0;
        }
        else
        {
            return false;
        }
    }
    
    
    ///////////////////////////////////////////////////     判定函数 结束
    
    
    
    ///////////////////////////////////////////////////     资源，body part，Manipulation 方法
    
    
    // 给定一个类型的 body part，寻找此 panel 中第一个该类型的 slot
    public int Find_First_Empty_Body_Part_Type_In_Slots(string bodyPartType)
    {
        if (requiredBodyPartsThisPanel.Count > 0)
        {
            for (int i = 1; i <= requiredBodyPartsThisPanel.Count; i++)     // 对于每个此 panel 需要的 body part 槽位
            {
                if (requiredBodyPartsThisPanel[i] == bodyPartType       // 如果槽位上对应的 body part 类型，与要寻找的类型相等
                    && !currentlyAbosorbedBodyPartSlots[i])             // 而且该槽位没有被占用
                {
                    return i;                                       // 则返回 slot 编号 i
                }

            }
        }
        
        return 0;           // 没找到则返回 0
    }
    
    
    // 给定一个类型的 body part，寻找此 panel 中第一个该类型的 slot
    public List<int> Find_All_Empty_Body_Part_Slot_Of_Type(string bodyPartType)
    {
        List<int> slot_list = new List<int>() { };
        
        if (requiredBodyPartsThisPanel.Count > 0)
        {
            for (int i = 1; i <= requiredBodyPartsThisPanel.Count; i++)     // 对于每个此 panel 需要的 body part 槽位
            {
                if (requiredBodyPartsThisPanel[i] == bodyPartType       // 如果槽位上对应的 body part 类型，与要寻找的类型相等
                    && !currentlyAbosorbedBodyPartSlots[i])             // 而且该槽位没有被占用
                {
                    slot_list.Add(i);                                       // 则将 slot 编号 i 加入 list
                }

            }
        }
        
        return slot_list;           // 没找到则返回 0
    }
    
    
    
    // 吸收一个给定类型的 body part 的细节操作                      // 原先的代码，含复杂的卡牌 merge 细节操作（已封装）
        // 在拖拽 body part 到卡牌时调用此方法
    /*public void Absorb_Body_Part_Based_On_Type(GameObject body_part_card_to_absorb)
    {
        if (body_part_card_to_absorb.GetComponent<Card_Body_Part_Feature>() != null)    // 确保输入的是 body part
        {

            // panel 上的 body part 增加

            string bodyPartToAbsorbType = body_part_card_to_absorb.GetComponent<Card_Body_Part_Feature>()._CardBodyPart.Id; // 获取 body part 类型 string
            
            // 获取空 Slot X 的编号

            int EmptySlotNumber = Find_First_Empty_Body_Part_Type_In_Slots(bodyPartToAbsorbType);
            

            GameObject instantiatedBodyPartOnPanel = null;

            if (bodyPartToAbsorbType == "Physical_Body")
            {
                instantiatedBodyPartOnPanel = GameManager.GM.Generate_Card_Body_Part("Physical_Body");
            }
            
            if (bodyPartToAbsorbType == "Spirit")
            {
                instantiatedBodyPartOnPanel = GameManager.GM.Generate_Card_Body_Part("Spirit");
            }
            
            if (bodyPartToAbsorbType == "Psyche")
            {
                instantiatedBodyPartOnPanel = GameManager.GM.Generate_Card_Body_Part("Psyche");
            }
            
            // 传递 last position
            
            instantiatedBodyPartOnPanel.GetComponent<Card_Body_Part_Feature>().lastPosition =      
                body_part_card_to_absorb.GetComponent<Card_Body_Part_Feature>().lastPosition;
            
            // 调整 panel 上生成 body part 卡牌的 缩放 Adjust Scale of Instantiate card

            float scaleOffset = 0.8f;

            instantiatedBodyPartOnPanel.transform.localScale *=
                GameManager.GM.PanelManager.current_panel.transform.lossyScale.x * scaleOffset / GameManager.GM.PanelManager.panel_original_scale.x;
            
            // 设置 panel 上生成的 body part 卡牌的 父层级为 panel 上 body part Section
            
            instantiatedBodyPartOnPanel.transform.parent = panel_section_body_part.transform;
            
            // 设置 新生成的 body part 的各个 Sprite 元素的 Sorting Layer

            foreach (var spriteRenderer in instantiatedBodyPartOnPanel.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.sortingLayerName = "Panel";
                spriteRenderer.sortingOrder += 4;    // (panel上 bodypart 的 slot sorting order 为 2)
            }

            foreach (var tmpText in instantiatedBodyPartOnPanel.GetComponentsInChildren<TMP_Text>())
            {
                tmpText.GetComponent<Renderer>().sortingLayerName = "Panel";
                tmpText.GetComponent<Renderer>().sortingOrder += 4;  // (panel上 bodypart 的 slot sorting order 为 2)
            }
            
            
            
            // 调整 panel 上生成 body part 卡牌的 位置，到 slot X 的位置
            
            instantiatedBodyPartOnPanel.transform.localPosition = 
                GameObject.Find("Body_Part_Slot_" + EmptySlotNumber).transform.localPosition;   
            
            
            // 将新生成的 body part GameObject 记录进 absorbed Body Part 字典
            
            absorbedBodyPartGameObjects.Add(EmptySlotNumber, instantiatedBodyPartOnPanel);
            
            
            // 设置 currentlyAbsorbedBodyPartSlots 字典中相应的槽位为 “被占据 ” ，即将值设置为 true
            
            currentlyAbosorbedBodyPartSlots[EmptySlotNumber] = true;
            
            
            // board 上的 body part 减少
            
            GameManager.GM.BodyPartManager.Take_Body_Part_Away_From_Board(body_part_card_to_absorb);


        }

    }*/
    
    
    // 吸收一个特定 slot 的 body part 的操作                     // 原先的代码，含复杂的卡牌 merge 细节操作（已封装）
        // 点击 panel 上的 body part 槽位时调用此方法
    /*public void Absorb_Body_Part_Based_On_Slot(int slot_number)
        {
            
            // 从板子上找到一个该 slot 对应类型的 body part卡，销毁它
            Card_Body_Part_Feature[] bodyParts = FindObjectsOfType<Card_Body_Part_Feature>();
            List<GameObject> bodyPartGameObject = new List<GameObject>() { };
            foreach (var body_part_feature in bodyParts)
            {
                bodyPartGameObject.Add(body_part_feature.gameObject);
            }
    
            GameObject selected_body_part = bodyPartGameObject.Find(
                game_object => game_object.GetComponent<Card_Body_Part_Feature>()._CardBodyPart.Id == requiredBodyPartsThisPanel[slot_number]);
    
            
        // 在 panel 上生成一个（可从上面扒）
    
            // 生成一个新的 body part 实例
            
            GameObject instantiatedBodyPartOnPanel =
                GameManager.GM.Generate_Card_Body_Part(requiredBodyPartsThisPanel[slot_number]);     // TODO 替换成 一个 body part 的空壳
    
            // 传递 last position
                
            instantiatedBodyPartOnPanel.GetComponent<Card_Body_Part_Feature>().lastPosition =      
                selected_body_part.GetComponent<Card_Body_Part_Feature>().lastPosition;
            
            
            // 调整 panel 上生成 body part 卡牌的 缩放 Adjust Scale of Instantiate card
    
            float scaleOffset = 0.8f;
    
            instantiatedBodyPartOnPanel.transform.localScale *=
                GameManager.GM.PanelManager.current_panel.transform.lossyScale.x * scaleOffset / GameManager.GM.PanelManager.panel_original_scale.x;
                
            // 设置 panel 上生成的 body part 卡牌的 父层级为 panel 上 body part Section
                
            instantiatedBodyPartOnPanel.transform.parent = panel_section_body_part.transform;
            
            // 设置 新生成的 body part 的各个 Sprite 元素的 Sorting Layer
    
            foreach (var spriteRenderer in instantiatedBodyPartOnPanel.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.sortingLayerName = "Panel";
                spriteRenderer.sortingOrder += 4;    // (panel上 bodypart 的 slot sorting order 为 2)
            }
    
            foreach (var tmpText in instantiatedBodyPartOnPanel.GetComponentsInChildren<TMP_Text>())
            {
                tmpText.GetComponent<Renderer>().sortingLayerName = "Panel";
                tmpText.GetComponent<Renderer>().sortingOrder += 4;  // (panel上 bodypart 的 slot sorting order 为 2)
            }
            
            
            // 调整 panel 上生成 body part 卡牌的 位置，到 slot X 的位置
                
            instantiatedBodyPartOnPanel.transform.localPosition = 
                GameObject.Find("Body_Part_Slot_" + slot_number).transform.localPosition;  
            
            // 将新生成的 body part GameObject 记录进 absorbed Body Part 字典
                
            absorbedBodyPartGameObjects.Add(slot_number, instantiatedBodyPartOnPanel);
            
            // 设置 currentlyAbsorbedBodyPartSlots 字典中相应的槽位为 “被占据 ” ，即将值设置为 true
                
            currentlyAbosorbedBodyPartSlots[slot_number] = true;
    
            // board 上的 body part 减少
    
            GameManager.GM.BodyPartManager.Take_Body_Part_Away_From_Board(selected_body_part);      // 最后销毁
    
        }*/
    
    
    
    
    // 吸收一个给定类型的 body part 的细节操作
        // 在拖拽 body part 到卡牌时调用此方法
    public void Absorb_Body_Part_Based_On_Type(GameObject body_part_card_to_absorb)
    {
        if (body_part_card_to_absorb.GetComponent<Card_Body_Part_Feature>() != null)    // 确保输入的是 body part
        {
            
            // 获取空 Slot X 的编号
            int EmptySlotNumber = Find_First_Empty_Body_Part_Type_In_Slots(
                body_part_card_to_absorb.GetComponent<Card_Body_Part_Feature>()._CardBodyPart.Id);
            
            // 调用 Merge 方法进行 merge 细节操作
            Merge_Body_Part_To_Slot(body_part_card_to_absorb, EmptySlotNumber);
            
        }

    }
    
    
    // 吸收一个特定 slot 的 body part 的操作
        // 点击 panel 上的 body part 槽位时调用此方法

    public void Absorb_Body_Part_Based_On_Slot(int slot_number)
    {

        GameObject selected_body_part = GameManager.GM.BodyPartManager.Find_All_Body_Parts_On_Board().
            Find(game_object => game_object.GetComponent<Card_Body_Part_Feature>()._CardBodyPart.Id == requiredBodyPartsThisPanel[slot_number]);
        
        
        // 调用 Merge 方法进行 merge 细节操作
        Merge_Body_Part_To_Slot(selected_body_part, slot_number);

    }
    
    
    
    
    // 吸收一个给定类型 且在特定 slot 上的 body part 的操作
        // 拖拽特定 body part 卡牌到 panel 上的 body part 槽位时调用此方法 
    public void Absorb_Body_Part_Based_On_Type_And_Slot(GameObject body_part_card_to_absorb, int slot_number)
    {
        Merge_Body_Part_To_Slot(body_part_card_to_absorb, slot_number);
    }


    // 给定一个body part的GameObject (此GameObject为生成的空壳)，和 Slot number，将 body part 卡牌 Merge 上去
    public void Merge_Body_Part_To_Slot(GameObject bodyPartToMerge_Original, int slotNumber)    
    {
        
        
        
        // 播放 merge slot 音效
        GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Panel_Body_Part_Merge_Slot);

        // 生成一个新的 body part 实例
        
        GameObject bodyPartToMerge_Instantiated =
            GameManager.GM.Generate_Card_Body_Part(requiredBodyPartsThisPanel[slotNumber]);     // TODO 替换成 一个 body part 的空壳
        
        
        // 传递 last position
            
        bodyPartToMerge_Instantiated.GetComponent<Card_Body_Part_Feature>().lastPosition =      
            bodyPartToMerge_Original.GetComponent<Card_Body_Part_Feature>().lastPosition;
        
        
        // 如果是 Potion，传递 Potion_Info 里的 potion sequence 实例到 card location feature 上的一个存储参数上
        // 为了吸收 Potion 的时候，能够清楚这个 Potion 是哪个序列的 Potion，方便 1) Return 的时候重新赋予新生成的 Potion Body Part， 2) 
        
        if (bodyPartToMerge_Original.GetComponent<Potion_Info>() != null)
        {
            attached_card_location_feature.current_potion_card_sequence =
                bodyPartToMerge_Original.GetComponent<Potion_Info>().potion_sequence;   // 将 potion sequence 实例记录到 card location feature 上的存储参数
            
            
            bodyPartToMerge_Instantiated.AddComponent<Potion_Info>().potion_sequence = 
                    bodyPartToMerge_Original.GetComponent<Potion_Info>().potion_sequence;   // 将 potion sequence 实例 赋予 新生成的 panel 上的 potion card
        }


        // 调整 panel 上生成 body part 卡牌的 缩放 Adjust Scale of Instantiate card

        float scaleOffset = 0.8f;

        bodyPartToMerge_Instantiated.transform.localScale *=
            GameManager.GM.PanelManager.current_panel.transform.lossyScale.x * scaleOffset / GameManager.GM.PanelManager.panel_original_scale.x;
        
        
        // 设置 panel 上生成的 body part 卡牌的 父层级为 panel 上 body part Section
            
        bodyPartToMerge_Instantiated.transform.parent = panel_section_body_part.transform;
        
        
        // 设置 新生成的 body part 的各个 Sprite 元素的 Sorting Layer

        foreach (var spriteRenderer in bodyPartToMerge_Instantiated.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.sortingLayerName = "Panel";
            spriteRenderer.sortingOrder += 4;    // (panel上 bodypart 的 slot sorting order 为 2)
        }

        foreach (var tmpText in bodyPartToMerge_Instantiated.GetComponentsInChildren<TMP_Text>())
        {
            tmpText.GetComponent<Renderer>().sortingLayerName = "Panel";
            tmpText.GetComponent<Renderer>().sortingOrder += 4;  // (panel上 bodypart 的 slot sorting order 为 2)
        }
        
        
        // 调整 panel 上生成 body part 卡牌的 位置，到 slot X 的位置
            
        bodyPartToMerge_Instantiated.transform.localPosition = 
            GameObject.Find("Body_Part_Slot_" + slotNumber).transform.localPosition;   
            
            
        // 将新生成的 body part GameObject 记录进 absorbed Body Part 字典
            
        absorbedBodyPartGameObjects.Add(slotNumber, bodyPartToMerge_Instantiated);
            
            
        // 设置 currentlyAbsorbedBodyPartSlots 字典中相应的槽位为 “被占据 ” ，即将值设置为 true
            
        currentlyAbosorbedBodyPartSlots[slotNumber] = true;
        
        
        // board 上的 body part 减少

        GameManager.GM.BodyPartManager.Take_Body_Part_Away_From_Board(bodyPartToMerge_Original);      // 最后销毁
        
    }

    
    
    // Knowledge 的 吸收

    public void Absorb_Knowledge(GameObject knowledgeToAbsorb)      // 仅吸收操作，判定要在外部写
    {
        
        GameManager.GM.ResourceManager.Reduce_Knowledge(knowledgeToAbsorb);     // 将 knowledge 的 ID 从 Resource Manager 中的 Owned Knowledge list 中移除
        
        absorbed_knowledge_list.Add(knowledgeToAbsorb.GetComponent<Knowledge_Feature>()._Knowledge.Id);     // Panel 上的 List 添加 这个 Knowledge 的 ID

        // 炫酷的 吸收动画
        
        Destroy(knowledgeToAbsorb);
        
    }
    
    
    
    
    
    
    
    
    // return Resources，从 panel 返回桌面
    
    public void Return_Resource()              // 关闭 panel 时，返还所有资源，执行逻辑，被 Panel_Manager 在关闭 panel 时调用
    {
        
        if (current_resource_1_amount > 0)
        {
            // 对于每一个 slot，获取到 slot 上对应的 资源名称   
            string resource_name = "";
            foreach (var resource_slot in resourceSlotNumber)
            {
                if (resource_slot.Value == 1)
                {
                    resource_name = resource_slot.Key;
                }
            }

            // 再根据资源名称，以及 slot 上记录的吸收数量，加回相应数量的相应资源
            if (resource_name == "Knowledge")
                Return_Knowledge();
            else
                GameManager.GM.ResourceManager.Add_Resource(resource_name, current_resource_1_amount, gameObject.transform.position);
            
            current_resource_1_amount = 0;
        }
        
        
        if (current_resource_2_amount > 0)
        {
            // 对于每一个 slot，获取到 slot 上对应的 资源名称   
            string resource_name = "";
            foreach (var resource_slot in resourceSlotNumber)
            {
                if (resource_slot.Value == 2)
                {
                    resource_name = resource_slot.Key;
                }
            }

            // 再根据资源名称，以及 slot 上记录的吸收数量，加回相应数量的相应资源
            if (resource_name == "Knowledge")
                Return_Knowledge();
            else
                GameManager.GM.ResourceManager.Add_Resource(resource_name, current_resource_2_amount, gameObject.transform.position);
            
            current_resource_2_amount = 0;
        }
        
        
        if (current_resource_3_amount > 0)
        {
            // 对于每一个 slot，获取到 slot 上对应的 资源名称   
            string resource_name = "";
            foreach (var resource_slot in resourceSlotNumber)
            {
                if (resource_slot.Value == 3)
                {
                    resource_name = resource_slot.Key;
                }
            }

            // 再根据资源名称，以及 slot 上记录的吸收数量，加回相应数量的相应资源
            if (resource_name == "Knowledge")
                Return_Knowledge();
            else
                GameManager.GM.ResourceManager.Add_Resource(resource_name, current_resource_3_amount, gameObject.transform.position);
            
            current_resource_3_amount = 0;
        }
        
        
        if (current_resource_4_amount > 0)
        {
            // 对于每一个 slot，获取到 slot 上对应的 资源名称   
            string resource_name = "";
            foreach (var resource_slot in resourceSlotNumber)
            {
                if (resource_slot.Value == 4)
                {
                    resource_name = resource_slot.Key;
                }
            }

            // 再根据资源名称，以及 slot 上记录的吸收数量，加回相应数量的相应资源
            if (resource_name == "Knowledge")
                Return_Knowledge();
            else
                GameManager.GM.ResourceManager.Add_Resource(resource_name, current_resource_4_amount, gameObject.transform.position);
            
            current_resource_4_amount = 0;
        }
        
        
        if (current_resource_5_amount > 0)
        {
            // 对于每一个 slot，获取到 slot 上对应的 资源名称   
            string resource_name = "";
            foreach (var resource_slot in resourceSlotNumber)
            {
                if (resource_slot.Value == 5)
                {
                    resource_name = resource_slot.Key;
                }
            }

            // 再根据资源名称，以及 slot 上记录的吸收数量，加回相应数量的相应资源
            if (resource_name == "Knowledge")
                Return_Knowledge();
            else
                GameManager.GM.ResourceManager.Add_Resource(resource_name, current_resource_5_amount, gameObject.transform.position);
            
            current_resource_5_amount = 0;
        }
        
    }

    
    // return Knowledge，从 panel 返回桌面
    
    public void Return_Knowledge()
    {
        float newKnowledge_XOffset = 3f;
        float newKnowledge_YOffset = -3f;
        
        foreach (var knowledge_string in absorbed_knowledge_list)
        {
            GameManager.GM.ResourceManager.Draw_A_Knowledge_By_Name(knowledge_string, transform.position, 
                new Vector3(
                    transform.position.x + newKnowledge_XOffset,
                    transform.position.y + newKnowledge_YOffset,
                    transform.position.z + 1));

            newKnowledge_YOffset -= 2f;
        }

    }


    // return body part，从 panel slot 返回桌面

    public void Return_Body_Part()
    {
        
            
            
        foreach (var slot in currentlyAbosorbedBodyPartSlots)
        {
            if (slot.Value)
            {
                GameObject returned_body_part = GameManager.GM.BodyPartManager.Generate_Body_Part_To_Board(
                    requiredBodyPartsThisPanel[slot.Key],
                    absorbedBodyPartGameObjects[slot.Key].transform.position,
                    absorbedBodyPartGameObjects[slot.Key].GetComponent<Card_Body_Part_Feature>().lastPosition);

            
                if (requiredBodyPartsThisPanel[slot.Key] == "Potion")       // 如果 return 的是 Potion，则加上记录的 potion sequence 实例
                {
                    returned_body_part.AddComponent<Potion_Info>().potion_sequence =
                        attached_card_location_feature.current_potion_card_sequence;    // 塞入存储的 sequence 实例
                
                    attached_card_location_feature.current_potion_card_sequence = null;   // 清空 sequence 实例
                }

                // TODO 加上 Save 数据的传递
                if (requiredBodyPartsThisPanel[slot.Key] == "Save")
                {
                    
                }
            
            }
        }
            
        
        
    }
    
    
    ///////////////////////////////////////////////////     资源，body part，Manipulation 方法   结束
    
    
    
    ///////////////////////////////////////////////////     更新 panel 状态
     
    
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

    void Set_Start_Button_Availablitity()       // 设置 Start 按钮是否可点击，以及按钮的 text
    {
        if (Check_If_Absorb_All_Requirements()          // 如果吸收了 所有需要的资源
            && !attached_card_location_feature.is_counting_down)     // 且没在 Countdown
        {
            start_button.GetComponent<Start_Button_Script>().Set_Button_Availability(true);
        }
        else 
        {
            start_button.GetComponent<Start_Button_Script>().Set_Button_Availability(false);
        }
    }

    void Set_Start_Button_Text()
    {

        if (attached_card_location_feature._cardLocation.Id == "Title_Card_Exit")          // 为特殊的卡牌设置特殊 button 文本
        {
            start_button.GetComponent<Start_Button_Script>().Set_Button_Text("Exit");
        }
        
        else if (attached_card_location_feature._cardLocation.Id == "")
        {
            
        }



        else       // 如果不是特殊卡牌，则正常设置 button 文本
        {
            
            if (attached_card_location_feature.is_counting_down) // 如果正在倒计时
            {
                start_button.GetComponent<Start_Button_Script>().Set_Button_Text("Running..");
            }
            else
            {
                start_button.GetComponent<Start_Button_Script>().Set_Button_Text("Start");
            }
            
            
        }
        
        
        
        
    }
    
    
    
    // 切换 Label 和 Description
    public IEnumerator Shift_Label_And_Description_If_Absorbed()        // 此方法目前会导致无限循环，卡死，先注掉
    {
        bool lastStatusIdle = true;     // 硬币参数
        bool isShifting = false;
        
        while (true)
        {
            
            if (Check_If_Start_Absorb_Any_Resource_Or_BodyParts() && lastStatusIdle)    // Idle -> Absorb
            {
                lastStatusIdle = false;     // 翻硬币

                panel_label.text = attached_card_location_feature._cardLocation.Absorbed_Label;     // 改变 Label
                panel_description.text = attached_card_location_feature._cardLocation.Absorbed_Description;     // 改变 Description

                /*if (isShifting)               // TODO 后面再加 Fade In 动画
                {
                    // 如果正在变换时的处理
                }
                else
                {
                    // 直接开始变换 Description
                    isShifting = true;
                    
                }*/
            }

            else if (!Check_If_Start_Absorb_Any_Resource_Or_BodyParts() && !lastStatusIdle)
            {
                lastStatusIdle = true;
                
                panel_label.text = attached_card_location_feature._cardLocation.Label;     // 改变 Label
                panel_description.text = attached_card_location_feature._cardLocation.Description;     // 改变 Description
            }


        }
        
    }
    
    
    ///////////////////////////////////////////////////     更新 panel 状态     END
    
    
    
    /////////////////////////////////////////////////         Audio 相关函数


    public void Play_Panel_Show_Up_Audio()
    {
        GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Panel_Showup);
    }

    public void Play_Panel_Close_Audio()
    {
        GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Panel_Close);
    }
    
    
    
    
    
    
    
    
    
    
    
    /////////////////////////////////////////////////         Audio 相关函数    END
    
    
    ///////////////////////////////////////////////////     其他函数
    
    
    
    
    
    
    
    
    
    
}
