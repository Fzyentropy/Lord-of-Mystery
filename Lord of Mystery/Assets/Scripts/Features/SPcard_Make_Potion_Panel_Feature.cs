using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SPcard_Make_Potion_Panel_Feature : MonoBehaviour
{
    
    // 此 panel 吸收的各资源计数
    public Dictionary<string, int> absorbedResourceThisPanel;
    
    // 记录是否匹配的参数
    [HideInInspector] public bool is_matching_sequence;
    [HideInInspector] public string matched_sequence;

    // 各资源 button prefab
    [Header("Resource Button Prefab")]
    public GameObject make_potion_button_physical_energy;
    public GameObject make_potion_button_spiritual_energy;
    public GameObject make_potion_button_soul;
    public GameObject make_potion_button_spirituality_infused_material;
    public GameObject make_potion_button_knowledge;
    public GameObject make_potion_button_belief;
    public GameObject make_potion_button_putrefaction;
    public GameObject make_potion_button_madness;
    public GameObject make_potion_button_godhood;
    
    // 各资源对应的 吸收数量 text
    [Header("resource absorb amount text")]
    public TMP_Text absorbed_amount_text_physical_energy;
    public TMP_Text absorbed_amount_text_spiritual_energy;
    public TMP_Text absorbed_amount_text_soul;
    public TMP_Text absorbed_amount_text_spirituality_infused_material;
    public TMP_Text absorbed_amount_text_knowledge;
    public TMP_Text absorbed_amount_text_belief;
    public TMP_Text absorbed_amount_text_putrefaction;
    public TMP_Text absorbed_amount_text_madness;
    public TMP_Text absorbed_amount_text_godhood;
    
    // 生成此 panel 的 Make Potion 卡
    [HideInInspector] public GameObject attached_make_potion_card;
    [HideInInspector] public SPcard_Make_Potion_Feature attached_make_potion_card_feature;
    
    // Start Button
    [Header("Make Potion Start Button")]
    public GameObject make_potion_start_button;


    private void Start()
    {
        Set_Absorbed_Resource_Dictionary();     // 初始化这个 panel 上吸收的 resource 的 Dictionary
        Set_Make_Potion_Panel_Resource_Button();
        Set_Make_Potion_Start_Button();
    }

    private void Update()
    {
        Check_If_Absorbed_Resource_Meet_Any_Sequence_Requirements();        // 时刻检测 吸收的资源是否满足 序列需要
        Set_Make_Potion_Start_Button_Availability();        // 动态设置 start 按钮是否可点击
        
    }


    public void Set_Attached_Make_Potion_Card(GameObject attachedCard)     // 设置生成此 panel 的卡牌指代，顺便设置 feature 指代，实例化 Panel 时从外部设置
    {                                                                   // 只能从外部设置，因为从 panel 自身无法查找到生成 panel 的卡是哪张
        attached_make_potion_card = attachedCard;
        attached_make_potion_card_feature = attached_make_potion_card.GetComponent<SPcard_Make_Potion_Feature>();
    }


    void Set_Absorbed_Resource_Dictionary()
    {

        absorbedResourceThisPanel = new Dictionary<string, int>()
        {
            
            { "Physical_Energy", 0},
            { "Spiritual_Energy", 0},
            { "Soul", 0},
            { "Spirituality_Infused_Material", 0},
            { "Knowledge", 0},
            { "Belief", 0},
            { "Putrefaction", 0},
            { "Madness", 0},
            { "Godhood", 0},

        };


    }

    
    void Set_Make_Potion_Panel_Resource_Button()        // 根据 board 上已经获得的资源来设置 资源按钮的出现
    {
        
        make_potion_button_physical_energy.SetActive(GameManager.GM.ResourceManager.is_physical_energy_ever_appears);
        make_potion_button_spiritual_energy.SetActive(GameManager.GM.ResourceManager.is_spiritual_energy_ever_appears);
        make_potion_button_soul.SetActive(GameManager.GM.ResourceManager.is_soul_ever_appears);
        make_potion_button_spirituality_infused_material.SetActive(GameManager.GM.ResourceManager.is_spirituality_infused_material_ever_appears);
        make_potion_button_knowledge.SetActive(GameManager.GM.ResourceManager.is_knowledge_ever_appears);
        make_potion_button_belief.SetActive(GameManager.GM.ResourceManager.is_belief_ever_appears);
        make_potion_button_putrefaction.SetActive(GameManager.GM.ResourceManager.is_putrefaction_ever_appears);
        make_potion_button_madness.SetActive(GameManager.GM.ResourceManager.is_madness_ever_appears);
        make_potion_button_godhood.SetActive(GameManager.GM.ResourceManager.is_godhood_ever_appears);

        make_potion_button_physical_energy.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
        make_potion_button_spiritual_energy.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
        make_potion_button_soul.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
        make_potion_button_spirituality_infused_material.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
        make_potion_button_knowledge.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
        make_potion_button_belief.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
        make_potion_button_putrefaction.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
        make_potion_button_madness.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
        make_potion_button_godhood.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);

    }
    
    void Set_Make_Potion_Start_Button()         // 设置start按钮的指代
    {
        if (make_potion_start_button == null)
        {
            make_potion_start_button = GameObject.Find("Start_Button_Make_Potion");
        }

        if (make_potion_start_button.GetComponent<Start_Button_Script_Make_Potion>().attached_make_potion_panel == null)    // 设置按钮脚本对 panel 的指代
        {
            make_potion_start_button.GetComponent<Start_Button_Script_Make_Potion>().Set_Attached_Make_Potion_Panel(gameObject);
        }
          
        
        // start_button.GetComponent<Start_Button_Script>().Set_Button_Availability(true); // 让按钮可点击，test用
    }



    bool Check_If_Absorbed_Resource_Meet_Any_Sequence_Requirements()
    {

        foreach (var sequence in GameManager.GM.CardLoader.Sequence_List)
        {
            if
            (
                absorbedResourceThisPanel["Physical_Energy"] == sequence.Require_Resource.Physical_Energy &&
                absorbedResourceThisPanel["Spiritual_Energy"] == sequence.Require_Resource.Spiritual_Energy &&
                absorbedResourceThisPanel["Soul"] == sequence.Require_Resource.Soul &&
                absorbedResourceThisPanel["Spirituality_Infused_Material"] == sequence.Require_Resource.Spirituality_Infused_Material &&
                absorbedResourceThisPanel["Knowledge"] == sequence.Require_Resource.Knowledge &&
                absorbedResourceThisPanel["Belief"] == sequence.Require_Resource.Belief &&
                absorbedResourceThisPanel["Putrefaction"] == sequence.Require_Resource.Putrefaction &&
                absorbedResourceThisPanel["Madness"] == sequence.Require_Resource.Madness &&
                absorbedResourceThisPanel["Godhood"] == sequence.Require_Resource.Godhood
            )
            {
                // 成功
                is_matching_sequence = true;
                matched_sequence = sequence.Id;
                return true;
            }
            
        }

        // 失败
        is_matching_sequence = true;
        matched_sequence = "";
        return false;

    }
    
    

    // 根据 put into 的 resource 动态设置按钮的 availability
    void Set_Make_Potion_Start_Button_Availability()             // 设置 按钮 availability
    {

        make_potion_start_button.GetComponent<Start_Button_Script_Make_Potion>()
            .Set_Button_Availability(is_matching_sequence);                        
        
    }


    public void Absorb_Resource_To_This_Panel(string resource_type, int amount)         // 用于 吸收，返还 资源的方法
    {

        foreach (var resource in absorbedResourceThisPanel)
        {

            if (resource.Key == resource_type)
            {
                absorbedResourceThisPanel[resource.Key] += amount;
            }
            
        }
        
    }


    public void Return_Absorbed_Resource()
    {

        foreach (var resource in absorbedResourceThisPanel)
        {

            if (resource.Key == "Physical_Energy")
            {
                GameManager.GM.ResourceManager.Add_Physical_Energy(resource.Value, transform.position);
            }
            if (resource.Key == "Spiritual_Energy")
            {
                GameManager.GM.ResourceManager.Add_Spiritual_Energy(resource.Value, transform.position);
            }
            if (resource.Key == "Soul")
            {
                GameManager.GM.ResourceManager.Add_Soul(resource.Value, transform.position);
            }
            if (resource.Key == "Spirituality_Infused_Material")
            {
                GameManager.GM.ResourceManager.Add_Spirituality_Infused_Material(resource.Value, transform.position);
            }
            if (resource.Key == "Knowledge")
            {
                GameManager.GM.ResourceManager.Add_Knowledge(resource.Value, transform.position);
            }
            if (resource.Key == "Belief")
            {
                GameManager.GM.ResourceManager.Add_Belief(resource.Value, transform.position);
            }
            if (resource.Key == "Putrefaction")
            {
                GameManager.GM.ResourceManager.Add_Putrefaction(resource.Value, transform.position);
            }
            if (resource.Key == "Madness")
            {
                GameManager.GM.ResourceManager.Add_Madness(resource.Value, transform.position);
            }
            if (resource.Key == "Godhood")
            {
                GameManager.GM.ResourceManager.Add_Godhood(resource.Value, transform.position);
            }

        }
        
        
    }
    
    
}
