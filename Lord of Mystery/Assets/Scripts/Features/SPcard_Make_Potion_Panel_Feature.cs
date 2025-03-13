using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using TMPro;
using UnityEngine;

public class SPcard_Make_Potion_Panel_Feature : MonoBehaviour
{
    
    // 此 panel 吸收的各资源计数
    public Dictionary<string, int> absorbedResourceThisPanel;
    public List<string> absorbedKnowledgeThisPanel;

    // 记录是否匹配的参数
    [HideInInspector] public bool is_matching_sequence;
    public Sequence matched_sequence_temp_storage;

    [Header("Sections")] 
    public GameObject Resource_Section;
    public GameObject ProgressBar_Section;

    [Header("Text")] 
    public TMP_Text panel_label;
    public TMP_Text panel_description;

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
    
    // 进度条
    [Header("Progress Bar")]
    public GameObject progress_bar_prefab;        // 进度条 prefab
    [HideInInspector]public GameObject progress_bar;               // 进度条 指代
    [HideInInspector]public GameObject progress_bar_root;          // 进度条方块的根 指代
    public GameObject progress_bar_position;      // 进度条位置标记 空物体
    [HideInInspector]public TMP_Text countdown_text;               // 显示秒数文本


    private void Start()
    {
        
        Set_Absorbed_Resource_Dictionary();     // 初始化这个 panel 上吸收的 resource 的 Dictionary
        Set_Make_Potion_Start_Button();
        
        Set_Make_Potion_Panel_Resource_Button_Or_ProgressBar();
        
        Set_Panel_Text();
        
    }

    private void Update()
    {
        Check_If_Absorbed_Resource_Meet_Any_Sequence_Requirements();        // 时刻检测 吸收的资源是否满足 序列需要
        Set_Make_Potion_Start_Button_Availability();        // 动态设置 start 按钮是否可点击
        Update_Resource_Amount_Text();              // 更新 panel 上吸收的 各 resource 的数量，到 TMP text 上
        
    }


    public void Set_Attached_Make_Potion_Card(GameObject attachedCard)     // 设置生成此 panel 的卡牌指代，顺便设置 feature 指代，实例化 Panel 时从外部设置
    {                                                                   // 只能从外部设置，因为从 panel 自身无法查找到生成 panel 的卡是哪张
        attached_make_potion_card = attachedCard;
        attached_make_potion_card_feature = attached_make_potion_card.GetComponent<SPcard_Make_Potion_Feature>();
    }


    void Set_Panel_Text()
    {
        if (GameManager.currentLanguage == GameManager.Language.English)        // 设置语言
        {
            panel_label.text = "MAKE POTION";
            panel_label.font = GameManager.Font_English;
            panel_description.text = "Consuming the potion is the sole path for humans to become Beyonders. By combining the essences of malevolent spirits, dragons, monsters, and enchanted flora and crystals with various mental and energetic forces, diverse potions are crafted, bestowing unique extraordinary abilities upon their users. \n\n[Left-click to add ingredients, right-click to remove. Whether you have a formula or not, as long as you input the correct proportions, the potion will be complete...]";
            panel_description.font = GameManager.Font_English;
            panel_description.fontSize = 8;
        }
        else if (GameManager.currentLanguage == GameManager.Language.Chinese)
        {
            panel_label.text = "制作魔药";
            panel_label.font = GameManager.Font_Chinese;
            panel_description.text = "服食魔药是人类成为非凡者的唯一途径。用恶灵，用巨龙，用怪物，用神奇树木、花朵或结晶等多种灵性材料，与不同的精神、能量相组合，能够配制出不同性相的魔药，并赋予使用者不同的非凡能力。\n\n【左键添加材料，右键减少材料，无论你有没有配方，只要你输入了正确的比例，魔药就可以完成…】";
            panel_description.font = GameManager.Font_Chinese;
            panel_description.fontSize = 7;
            panel_description.characterSpacing = -2;
            panel_description.lineSpacing = -20;
        }
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

    
    void Set_Make_Potion_Panel_Resource_Button_Or_ProgressBar()        // 根据 board 上已经获得的资源来设置 资源按钮的出现
    {
        
        if (attached_make_potion_card_feature.is_counting_down)
        {

            Resource_Section.SetActive(false);
            ProgressBar_Section.SetActive(true);
            
            make_potion_start_button.transform.localPosition =
                GameObject.Find("Start_Button_Location_Without_BodyParts").transform.localPosition;
        
            // 实例化 进度条 prefab
            progress_bar = Instantiate(progress_bar_prefab, transform.position, Quaternion.identity);
            progress_bar.transform.parent = transform;
            progress_bar.transform.localPosition = progress_bar_position.transform.localPosition;
            progress_bar_root = progress_bar.transform.Find("Bar_Root").gameObject;
            countdown_text = progress_bar.GetComponentInChildren<TMP_Text>();

            StartCoroutine(panel_progress_bar_sync());
            
        }

        else
        {
            Resource_Section.SetActive(true);
            ProgressBar_Section.SetActive(false);
            
            make_potion_start_button.transform.localPosition =
                GameObject.Find("Start_Button_Location_Resource").transform.localPosition;
            
            
            // make_potion_button_physical_energy.SetActive(GameManager.GM.ResourceManager.is_physical_energy_ever_appears);
            // make_potion_button_spiritual_energy.SetActive(GameManager.GM.ResourceManager.is_spiritual_energy_ever_appears);
            // make_potion_button_soul.SetActive(GameManager.GM.ResourceManager.is_soul_ever_appears);
            // make_potion_button_spirituality_infused_material.SetActive(GameManager.GM.ResourceManager.is_spirituality_infused_material_ever_appears);
            // // make_potion_button_knowledge.SetActive(GameManager.GM.ResourceManager.is_knowledge_ever_appears);
            make_potion_button_knowledge.SetActive(false);
            // make_potion_button_belief.SetActive(GameManager.GM.ResourceManager.is_belief_ever_appears);
            // make_potion_button_putrefaction.SetActive(GameManager.GM.ResourceManager.is_putrefaction_ever_appears);
            // make_potion_button_madness.SetActive(GameManager.GM.ResourceManager.is_madness_ever_appears);
            // make_potion_button_godhood.SetActive(GameManager.GM.ResourceManager.is_godhood_ever_appears);


            make_potion_button_physical_energy.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
            make_potion_button_spiritual_energy.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
            make_potion_button_soul.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
            make_potion_button_spirituality_infused_material.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
            make_potion_button_belief.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
            make_potion_button_putrefaction.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
            make_potion_button_madness.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
            make_potion_button_godhood.GetComponent<Make_Potion_Panel_Resource_Button>().Set_Attached_Make_Potion_Panel(gameObject);
            
            make_potion_button_knowledge.GetComponent<Card_Location_Panel_Knowledge_Slot>().Set_Attached_Panel(gameObject);
        }
        
        

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

    
    IEnumerator panel_progress_bar_sync()
    {
        while (true)

        {
            if (progress_bar != null)
            {
                progress_bar_root.transform.localScale =
                    attached_make_potion_card_feature.progress_bar_root.transform.localScale;
                countdown_text.text = attached_make_potion_card_feature.countdown_text.text;
            }

            yield return new WaitForSeconds(0.05f);
        }
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
                matched_sequence_temp_storage = GameManager.GM.CardLoader.Get_Sequence_By_Id(sequence.Id);
                return true;
            }
            
        }

        // 失败
        is_matching_sequence = false;               // (如果 is_match_sequence 为 false，Start 按钮无法点击，所以 matched_sequence_temp_storage 传递时不会是 null)
        matched_sequence_temp_storage = null;
        return false;

    }

    public Sequence Pass_Matched_Sequence_To_Manager()          // 在点击了 Make Potion panel 的 Start 之后调用，把临时存储的 sequence 实例传递到 manager
    {
        GameManager.GM.BodyPartManager.matched_sequence = matched_sequence_temp_storage;
        return matched_sequence_temp_storage;
    }
    
    

    // 根据 put into 的 resource 动态设置按钮的 availability
    void Set_Make_Potion_Start_Button_Availability()             // 设置 按钮 availability
    {

        make_potion_start_button.GetComponent<Start_Button_Script_Make_Potion>()
            .Set_Button_Availability(is_matching_sequence);                        
        
    }

    
    // Knowledge 的 吸收

    public void Absorb_Knowledge_Make_Potion(GameObject knowledgeToAbsorb)      // 仅吸收操作，判定要在外部写
    {
        
        GameManager.GM.ResourceManager.Reduce_Knowledge(knowledgeToAbsorb);     // 将 knowledge 的 ID 从 Resource Manager 中的 Owned Knowledge list 中移除
        
        absorbedKnowledgeThisPanel.Add(knowledgeToAbsorb.GetComponent<Knowledge_Feature>()._Knowledge.Id);     // Panel 上的 List 添加 这个 Knowledge 的 ID

        // 炫酷的 吸收动画
        
        Destroy(knowledgeToAbsorb);
        
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

            if (resource.Key == "Physical_Energy" && resource.Value > 0)
            {
                GameManager.GM.ResourceManager.Add_Physical_Energy(resource.Value, transform.position);
            }
            if (resource.Key == "Spiritual_Energy" && resource.Value > 0)
            {
                GameManager.GM.ResourceManager.Add_Spiritual_Energy(resource.Value, transform.position);
            }
            if (resource.Key == "Soul" && resource.Value > 0)
            {
                GameManager.GM.ResourceManager.Add_Soul(resource.Value, transform.position);
            }
            if (resource.Key == "Spirituality_Infused_Material" && resource.Value > 0)
            {
                GameManager.GM.ResourceManager.Add_Spirituality_Infused_Material(resource.Value, transform.position);
            }
            if (resource.Key == "Knowledge" && resource.Value > 0)
            {
                Return_Knowledge_Make_Potion();
            }
            if (resource.Key == "Belief" && resource.Value > 0)
            {
                GameManager.GM.ResourceManager.Add_Belief(resource.Value, transform.position);
            }
            if (resource.Key == "Putrefaction" && resource.Value > 0)
            {
                GameManager.GM.ResourceManager.Add_Putrefaction(resource.Value, transform.position);
            }
            if (resource.Key == "Madness" && resource.Value > 0)
            {
                GameManager.GM.ResourceManager.Add_Madness(resource.Value, transform.position);
            }
            if (resource.Key == "Godhood" && resource.Value > 0)
            {
                GameManager.GM.ResourceManager.Add_Godhood(resource.Value, transform.position);
            }

        }
        
        
    }


    void Return_Knowledge_Make_Potion()
    {

        float newKnowledge_XOffset = 3f;
        float newKnowledge_YOffset = -3f;
    
        foreach (var knowledge_string in absorbedKnowledgeThisPanel)
        {
            GameManager.GM.ResourceManager.Draw_A_Knowledge_By_Name(knowledge_string, transform.position, 
                new Vector3(
                    transform.position.x + newKnowledge_XOffset,
                    transform.position.y + newKnowledge_YOffset,
                    transform.position.z + 1));

            newKnowledge_YOffset -= 2f;
        }

        
    }
    
    

    // 更新 panel 上吸收的 各 resource 的数量，到 TMP text 上
    void Update_Resource_Amount_Text()
    {

        // Physical_Energy
        if (absorbedResourceThisPanel["Physical_Energy"] == 0)
        {
            absorbed_amount_text_physical_energy.text = "";
        }
        else if (absorbedResourceThisPanel["Physical_Energy"] > 0)
        {
            absorbed_amount_text_physical_energy.text = absorbedResourceThisPanel["Physical_Energy"].ToString();
        }
        
        // Spiritual_Energy
        if (absorbedResourceThisPanel["Spiritual_Energy"] == 0)
        {
            absorbed_amount_text_spiritual_energy.text = "";
        }
        else if (absorbedResourceThisPanel["Spiritual_Energy"] > 0)
        {
            absorbed_amount_text_spiritual_energy.text = absorbedResourceThisPanel["Spiritual_Energy"].ToString();
        }
        
        // Soul
        if (absorbedResourceThisPanel["Soul"] == 0)
        {
            absorbed_amount_text_soul.text = "";
        }
        else if (absorbedResourceThisPanel["Soul"] > 0)
        {
            absorbed_amount_text_soul.text = absorbedResourceThisPanel["Soul"].ToString();
        }
        
        // Spirituality_Infused_Material
        if (absorbedResourceThisPanel["Spirituality_Infused_Material"] == 0)
        {
            absorbed_amount_text_spirituality_infused_material.text = "";
        }
        else if (absorbedResourceThisPanel["Spirituality_Infused_Material"] > 0)
        {
            absorbed_amount_text_spirituality_infused_material.text = absorbedResourceThisPanel["Spirituality_Infused_Material"].ToString();
        }
        
        // Knowledge
        if (absorbedResourceThisPanel["Knowledge"] == 0)
        {
            absorbed_amount_text_knowledge.text = "";
        }
        else if (absorbedResourceThisPanel["Knowledge"] > 0)
        {
            absorbed_amount_text_knowledge.text = absorbedResourceThisPanel["Knowledge"].ToString();
        }
        
        // Belief
        if (absorbedResourceThisPanel["Belief"] == 0)
        {
            absorbed_amount_text_belief.text = "";
        }
        else if (absorbedResourceThisPanel["Belief"] > 0)
        {
            absorbed_amount_text_belief.text = absorbedResourceThisPanel["Belief"].ToString();
        }
        
        // Putrefaction
        if (absorbedResourceThisPanel["Putrefaction"] == 0)
        {
            absorbed_amount_text_putrefaction.text = "";
        }
        else if (absorbedResourceThisPanel["Putrefaction"] > 0)
        {
            absorbed_amount_text_putrefaction.text = absorbedResourceThisPanel["Putrefaction"].ToString();
        }
        
        // Madness
        if (absorbedResourceThisPanel["Madness"] == 0)
        {
            absorbed_amount_text_madness.text = "";
        }
        else if (absorbedResourceThisPanel["Madness"] > 0)
        {
            absorbed_amount_text_madness.text = absorbedResourceThisPanel["Madness"].ToString();
        }
        
        // Godhood
        if (absorbedResourceThisPanel["Godhood"] == 0)
        {
            absorbed_amount_text_godhood.text = "";
        }
        else if (absorbedResourceThisPanel["Godhood"] > 0)
        {
            absorbed_amount_text_godhood.text = absorbedResourceThisPanel["Godhood"].ToString();
        }
        
        
        
        
    }
    
 
    
    
    
    
    
    
}
