using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using TMPro;
using Unity.VisualScripting;
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
    public TMP_Text resource_1_amount_text;         // 场景中各资源对应的 TMP_text 数量文本
    public TMP_Text resource_2_amount_text;
    public TMP_Text resource_3_amount_text;
    public TMP_Text resource_4_amount_text;
    public TMP_Text resource_5_amount_text;
    // 添加更多的资源槽位，如果需要 - resource_X_amount_text，需要在 prefab 中添加对应的 TMP_text 和 icon

    // Resource 相关的 Dictionary
    [Header("Dictionary")]
    public Dictionary<int, bool> availableResourceSlot;     // 用于记录 resource section 上 available 的 button slot
    public Dictionary<string, int> requiredResourcesThisPanel;  // 用于记录 required resource 及对应数量的 字典
    public Dictionary<string, int> resourceSlotNumber = new Dictionary<string, int>() {};    // 用于记录 required resource 及对应的 slot 编号
    
    // Body Part 相关的 Dictionary
    public Dictionary<int, string> requiredBodyPartsThisPanel;         // 用于记录需要的 body part 的 Dictionary, int:槽位编号, string:Body Part类型的string
    // public List<string> currentlyAbsorbedBodyPart;          // 用于记录当前已经吸收的 Body Part，临时，可能没必要
    public Dictionary<int, bool> currentlyAbosorbedBodyPartSlots;       // 用于记录 body part 的各个 slot 是否已经吸收, int:槽位编号, bool:是否已经吸收 body part
    public Dictionary<int, GameObject> absorbedBodyPartGameObjects;     // 用于记录 已经吸收的 body part 的 Game Object，方便访问

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
    
    public bool is_body_part_1_filled = false;         // 是否用 body part 填充了
    public bool is_body_part_2_filled = false;
    public bool is_body_part_3_filled = false;
    public bool is_body_part_4_filled = false;
    
    // Panel Status
    private bool is_resource_ok = false;
    private bool is_bodypart_ok = false;
    
    

    // Resource Button Prefab，panel 上的资源按钮
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

    // panel 外观设置
    [Header("Panel Appearance")]
    public SpriteRenderer panel_image;   
    public TMP_Text panel_label;
    public TMP_Text panel_description;
    
    // Mis Variables
    private Vector3 click_mouse_position;       // 用于点击时记录鼠标的位置
    private Vector3 lastMousePosition;      // 用于记录鼠标拖拽时，前一帧鼠标的位置
    public bool isPanelWellSet = false;     // 用于判定这个 panel 是否已经初始化完全，用以调用 Find_First_Empty_Body_Part_Type_In_Slots 方法

    
    

    private void Start()
    {
        Set_Panel_Section();
        Set_Start_Button();
    }

    private void Update()
    { 
        Update_Resource_Number();
        Set_Start_Button_Availablitity();

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
        

        if (requiredResourcesThisPanel.Count > 0)   // 如果需要消耗 resource，则根据需要的 resource 生成 resource 按钮
        {
            Set_Resource_Button();    
        }

        if (requiredBodyPartsThisPanel.Count > 0)         // 如果需要消耗 body part，则根据需要的 body part 生成 body part 的槽位
        {
            Set_Body_Part_Slots();       
        }

        isPanelWellSet = true;
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
                // resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Current_Resource("Fund");  //设置按钮操纵的资源为 Fund
            }
            else if (resource.Key == "Physical_Energy")
            {
                resource_button = Instantiate(Button_Physical_Energy, panel_section_resource.transform);    // 实例化 button
                // resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Current_Resource("Physical_Energy");  //设置按钮操纵的资源为Physical_Energy
            }
            else if (resource.Key == "Spirit")
            {
                resource_button = Instantiate(Button_Spirit, panel_section_resource.transform);    // 实例化 button
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
            
            resource_button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Attached_Panel(gameObject); // 设置 resource button 的 panel 指代为此
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
                button.transform.localPosition = GameObject.Find("Resource_"+i).transform.localPosition;   // 放置到 available slot 的空物体位置
                
                // 根据 slot 的编号，设置对应 资源总数 参数的值, 并调用 resource button 中的方法，设置 resource button 中对应在此 panel 上资源编号的参数
                if (i == 1)
                {
                    resource_1_amount = value;      // 设置 resource 1 的数量为 目前引用的资源数量
                    button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Resource_Slot_Number(1);   // 设置 resource Button 对应的资源 slot 为 1
                    resourceSlotNumber.Add(resourceName,1);     // 记录 当前资源对应的 slot 编号 1，到字典 resourceSlotNumber
                }

                if (i == 2)
                {
                    resource_2_amount = value; 
                    button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Resource_Slot_Number(2);
                    resourceSlotNumber.Add(resourceName,2);
                }

                if (i == 3)
                {
                    resource_3_amount = value; 
                    button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Resource_Slot_Number(3);
                    resourceSlotNumber.Add(resourceName,3);
                }

                if (i == 4)
                {
                    resource_4_amount = value; 
                    button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Resource_Slot_Number(4);
                    resourceSlotNumber.Add(resourceName,4);
                }

                if (i == 5)
                {
                    resource_5_amount = value; 
                    button.GetComponent<Card_Location_Panel_Resource_Button>().Set_Resource_Slot_Number(5);
                    resourceSlotNumber.Add(resourceName,5);
                }
                break;
            }
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
            }
            if (body_part.Value == "Spirit")
            {
                bodyPartSlot = Instantiate(Slot_Spirit, panel_section_body_part.transform);
            }
            if (body_part.Value == "Psyche")
            {
                bodyPartSlot = Instantiate(Slot_Psyche, panel_section_body_part.transform);
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
    
    ///////////////////////////////////////////////////     设置函数结束
    
    
    
    
    ///////////////////////////////////////////////////     On 事件函数

    private void OnMouseOver()
    {
        // Debug.Log("is over the LOCATION PANEL");
        is_mouse_hover_on_panel = true;
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
    }
    
    
    ///////////////////////////////////////////////////     On 事件函数结束
    
    
    
    ///////////////////////////////////////////////////     判定函数
    

    public bool Check_If_Absorb_All_Requirements()      // 检查是否吸收完了所有需要的 resource 和 body part
    {
        return Check_If_Absorb_All_Resource() && Check_If_Absorb_All_BodyParts();
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
                if (!slot.Value)
                {
                    is_bodypart_ok = false;
                    return false;
                }
            }
        }

        is_bodypart_ok = true;
        return true;

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
    
    
    // 吸收一个给定类型的 body part 的细节操作
        // 在拖拽 body part 到卡牌时调用此方法
    public void Absorb_Body_Part_Based_On_Type(GameObject bodyPartToAbsorb)
    {
        if (bodyPartToAbsorb.GetComponent<Card_Body_Part_Feature>() != null)    // 确保输入的是 body part
        {

            // panel 上的 body part 增加

            string bodyPartToAbsorbType = bodyPartToAbsorb.GetComponent<Card_Body_Part_Feature>()._CardBodyPart.Id; // 获取 body part 类型 string

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
                bodyPartToAbsorb.GetComponent<Card_Body_Part_Feature>().lastPosition;
            
            // 调整 panel 上生成 body part 卡牌的 缩放 Adjust Scale of Instantiate card

            float scaleOffset = 0.8f;

            instantiatedBodyPartOnPanel.transform.localScale *=
                GameManager.GM.PanelManager.current_panel.transform.lossyScale.x * scaleOffset / GameManager.GM.PanelManager.panel_original_scale.x;
            
            // 设置 panel 上生成的 body part 卡牌的 父层级为 panel 上 body part Section
            
            instantiatedBodyPartOnPanel.transform.parent = panel_section_body_part.transform;

            foreach (var spriteRenderer in instantiatedBodyPartOnPanel.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.sortingLayerName = "Panel";
                spriteRenderer.sortingOrder = 3;
            }

            foreach (var tmpText in instantiatedBodyPartOnPanel.GetComponentsInChildren<TMP_Text>())
            {
                tmpText.GetComponent<Renderer>().sortingLayerName = "Panel";
                tmpText.GetComponent<Renderer>().sortingOrder = 3;
            }
            
            
            // 获取空 Slot X 的编号

            int EmptySlotNumber = Find_First_Empty_Body_Part_Type_In_Slots(bodyPartToAbsorbType);
            
            
            // 调整 panel 上生成 body part 卡牌的 位置，到 slot X 的位置
            
            instantiatedBodyPartOnPanel.transform.localPosition = 
                GameObject.Find("Body_Part_Slot_" + EmptySlotNumber).transform.localPosition;   
            
            
            // 将新生成的 body part GameObject 记录进 absorbed Body Part 字典
            
            absorbedBodyPartGameObjects.Add(EmptySlotNumber, instantiatedBodyPartOnPanel);
            
            
            // 设置 currentlyAbsorbedBodyPartSlots 字典中相应的槽位为 “被占据 ” ，即将值设置为 true
            
            currentlyAbosorbedBodyPartSlots[EmptySlotNumber] = true;
            
            
            // board 上的 body part 减少
            
            GameManager.GM.BodyPartManager.Take_Body_Part_Away_From_Board(bodyPartToAbsorb);


        }

    }
    
    // 吸收一个特定 slot 的 body part 的操作
        // 点击 panel 上的 body part 槽位时调用此方法

    public void Absorb_Body_Part_Based_On_Slot(int slotNumber)
    {
        
        
        
    }
    
    // return body part，从 slot 返回桌面
    

    
    public void Return_Resource()              // 关闭 panel 时，返还所有资源，执行逻辑，被 Panel_Manager 在关闭 panel 时调用
    {
        
        if (current_resource_1_amount > 0)
        {
            string resource_name = "";
            foreach (var resource_slot in resourceSlotNumber)
            {
                if (resource_slot.Value == 1)
                {
                    resource_name = resource_slot.Key;
                }
            }

            GameManager.GM.ResourceManager.Add_Resource(resource_name, current_resource_1_amount, gameObject.transform.position);
            current_resource_1_amount = 0;
        }
        if (current_resource_2_amount > 0)
        {
            string resource_name = "";
            foreach (var resource_slot in resourceSlotNumber)
            {
                if (resource_slot.Value == 2)
                {
                    resource_name = resource_slot.Key;
                }
            }

            GameManager.GM.ResourceManager.Add_Resource(resource_name, current_resource_2_amount, gameObject.transform.position);
            current_resource_2_amount = 0;
        }
        if (current_resource_3_amount > 0)
        {
            string resource_name = "";
            foreach (var resource_slot in resourceSlotNumber)
            {
                if (resource_slot.Value == 3)
                {
                    resource_name = resource_slot.Key;
                }
            }

            GameManager.GM.ResourceManager.Add_Resource(resource_name, current_resource_3_amount, gameObject.transform.position);
            current_resource_3_amount = 0;
        }
        if (current_resource_4_amount > 0)
        {
            string resource_name = "";
            foreach (var resource_slot in resourceSlotNumber)
            {
                if (resource_slot.Value == 4)
                {
                    resource_name = resource_slot.Key;
                }
            }

            GameManager.GM.ResourceManager.Add_Resource(resource_name, current_resource_4_amount, gameObject.transform.position);
            current_resource_4_amount = 0;
        }
        if (current_resource_5_amount > 0)
        {
            string resource_name = "";
            foreach (var resource_slot in resourceSlotNumber)
            {
                if (resource_slot.Value == 5)
                {
                    resource_name = resource_slot.Key;
                }
            }

            GameManager.GM.ResourceManager.Add_Resource(resource_name, current_resource_5_amount, gameObject.transform.position);
            current_resource_5_amount = 0;
        }
        
    }

    public void Return_Body_Part()
    {
        foreach (var slot in currentlyAbosorbedBodyPartSlots)
        {
            if (slot.Value)
            {
                GameManager.GM.BodyPartManager.Generate_Body_Part_To_Board(
                    requiredBodyPartsThisPanel[slot.Key],
                    absorbedBodyPartGameObjects[slot.Key].transform.position,
                    absorbedBodyPartGameObjects[slot.Key].GetComponent<Card_Body_Part_Feature>().lastPosition);
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
        if (Check_If_Absorb_All_Requirements())     // 如果吸收了 所有需要的资源
        {
            if (attached_card_location_feature.is_counting_down)    // 如果正在倒计时
            {
                start_button.GetComponent<Start_Button_Script>().Set_Button_Availability(false);
                start_button.GetComponent<Start_Button_Script>().Set_Button_Text("Arriving");
            }
            else      // 如果没在倒计时  
            {
                start_button.GetComponent<Start_Button_Script>().Set_Button_Availability(true);
                start_button.GetComponent<Start_Button_Script>().Set_Button_Text("Start");
            }
        }
        else  // 如果没吸收够 所有需要的资源
        {
            start_button.GetComponent<Start_Button_Script>().Set_Button_Availability(false);
            start_button.GetComponent<Start_Button_Script>().Set_Button_Text("Start");
        }
    }
    
    
    ///////////////////////////////////////////////////     更新 panel 状态 结束
    
    
    
    ///////////////////////////////////////////////////     其他函数
    
    
    
    
    
    
    
    
    
    
}
