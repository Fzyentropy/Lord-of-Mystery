using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Resource_Manager : MonoBehaviour
{
    // 各个资源的数量，真正出现在游戏中的计数参数，需要保存
    [HideInInspector]
    public int Fund;
    public int Physical_Energy;
    public int Spiritual_Energy;
    public int Soul;
    public int Spirituality_Infused_Material;
    // public int Knowledge;        // Knowledge 的计数通过 List 的 Count 实现
    public int Belief;
    public int Putrefaction;
    public int Madness;
    public int Godhood;
    
    public int Death;   // Death added
    public int Max_Death = 6;    // the Max of Death, reached => Death
    public bool player_is_dead;     // 玩家是否死亡
    
    // 各个资源的数量，用于显示在 UI 上的计数，会有闪烁动画，改变真实数量后，将滞后闪烁至最终值
    public int Fund_UI_Amount;
    public int Physical_Energy_UI_Amount;
    public int Spiritual_Energy_UI_Amount;
    public int Soul_UI_Amount;
    public int Spirituality_Infused_Material_UI_Amount;
    // public int Knowledge_UI_Amount;      // Knowledge 的计数通过 List 的 Count 实现
    public int Belief_UI_Amount;
    public int Putrefaction_UI_Amount;
    public int Madness_UI_Amount;
    public int Godhood_UI_Amount;
    
    public int Death_UI_Amount;     // Death
    public int Max_Death_UI_Amount = 6;     // 最大 Death
    
    
    // 资源位置标记和占用情况，真正出现在游戏中的标记参数，需要保存
    private Dictionary<int, bool> resourceLocationsOccupied = new Dictionary<int, bool>();      // int : 资源槽位的编号    bool : 是否被占用, true:占用，false:没占用
    private Dictionary<string, int> resourceLocationIndex = new Dictionary<string, int>();      // string : 资源名称    int : 资源占用的槽位编号
    private Dictionary<int, bool> slotAbleToShowText = new Dictionary<int, bool>();      // int : 资源槽位的编号    bool : 是否可以被update资源数量text了，字典用于开锁：是否可以显示资源数量
    
    
    // Knowledge 资源相关
    public List<string> Player_Owned_Current_Knowledge_List;



    // 各资源的 icon prefab
    public GameObject fund_icon_pure;   // Fund资源的图标Prefab
    public GameObject physical_energy_icon_pure;    // Physical_Energy资源的图标Prefab
    public GameObject spiritual_energy_icon_pure;     // Spirit资源的图标Prefab
    public GameObject soul_icon_pure;   // Soul资源的图标Prefab
    public GameObject spirituality_infused_material_icon_pure;  // Spirituality_Infused_Material资源的图标Prefab
    public GameObject knowledge_icon_pure;  // Knowledge资源的图标Prefab
    public GameObject belief_icon_pure;     // Belief资源的图标Prefab
    public GameObject putrefaction_icon_pure;   // Putrefaction资源的图标Prefab
    public GameObject madness_icon_pure;    // Madness资源的图标Prefab
    public GameObject godhood_icon_pure;    // Godhood资源的图标Prefab
    
    public GameObject death_icon_pure;      // Death资源的图标Prefab
    public GameObject death_bar_prefab;     // Death 进度条 prefab！！
    
    // 判定各资源是否出现过
    [HideInInspector] public bool is_fund_ever_appears = false;
    [HideInInspector] public bool is_physical_energy_ever_appears = false; // 检查Physical_Energy资源是否曾出现
    [HideInInspector] public bool is_spiritual_energy_ever_appears = false; // 检查Spirit资源是否曾出现
    [HideInInspector] public bool is_soul_ever_appears = false; // 检查Soul资源是否曾出现
    [HideInInspector] public bool is_spirituality_infused_material_ever_appears = false; // 检查Spirituality_Infused_Material资源是否曾出现
    [HideInInspector] public bool is_knowledge_ever_appears = false; // 检查Knowledge资源是否曾出现
    [HideInInspector] public bool is_belief_ever_appears = false; // 检查Belief资源是否曾出现
    [HideInInspector] public bool is_putrefaction_ever_appears = false; // 检查Putrefaction资源是否曾出现
    [HideInInspector] public bool is_madness_ever_appears = false; // 检查Madness资源是否曾出现
    [HideInInspector] public bool is_godhood_ever_appears = false; // 检查Godhood资源是否曾出现
    [HideInInspector] public bool is_death_ever_appears = false;     // 检查Death资源是否曾出现
    

    // Mis
    private bool FirstTimeBoolPasser = false;      // 用于将上方的 is_XXX_ever_appears 布尔值赋予此参数，来判断是否是第一次


    
    
    private void Awake()
    {
        Set_Max_Death();        // 临时设置 Max Death
    }
    
    

    // 在Start中初始化资源位置占用情况
    void Start()
    {
        Initialize_Dictionaries();       // 初始化 上方建立的字典  // TODO 未来加入保存功能后，可能要更改此方法，因为不能每次加载都初始化
        
        // 加载或设置fundIconPrefab
        // fundIconPrefab = ...

        StartCoroutine(Update_Resource_UI_Amount());      // 更新 resource真实数量 与 UI显示数量 差值 的协程

    }

    private void Update()
    {
        Update_Resource_Amount_In_Slots();
        Check_If_Knowledge_Appears();

        Check_Player_Death();
    }

    
    
    private void Initialize_Dictionaries()   // 初始化 上方建立的字典
    {
        for (int i = 1; i <= 10; i++)       // 初始化 资源槽位是否被占用字典
        {
            resourceLocationsOccupied[i] = false;
        }

        resourceLocationIndex = new Dictionary<string, int>     // 初始化 资源对应槽位字典
        {
            {"Fund", -1},
            {"Physical_Energy", -1},
            {"Spiritual_Energy", -1},
            {"Soul", -1},
            {"Spirituality_Infused_Material", -1},
            {"Knowledge", -1},
            {"Belief", -1},
            {"Putrefaction", -1},
            {"Madness", -1},
            {"Godhood", -1},
            // No Death,
        };
        
        for (int i = 1; i <= 10; i++)       // 初始化 槽位是否能显示数量字典
        {
            slotAbleToShowText[i] = false;
        }

    }

    void Set_Max_Death()        // 临时，设置最大死亡值数值，将来会根据职业和等级做更改
    {
        Max_Death = 6;
    }

    void Check_Player_Death()
    {
        if (Death >= Max_Death && !player_is_dead)
        {
            player_is_dead = true;
            
            // 死亡逻辑
            StartCoroutine(Player_Death());
        }
    }
    
    IEnumerator Player_Death()
    {
        GameManager.GM.InputManager.Move_Camera_To(GameObject.Find("Resource_Location_Death").transform.position, 2f);

        yield return new WaitForSeconds(1f);

        StartCoroutine(GameManager.GM.InputManager.Main_Scene_Fade_Out());

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Lord_of_Mystery_Death");
    }
    
    

    // 增加资源的集成方法，输入资源名称，来调用下面增加特定资源的方法
    public void Add_Resource(string resource_type, int amount, Vector3 sourcePosition)
    {
        if (resource_type == "Fund")        // 如果输入的是 Fund，则 Add Fund
        {
            Add_Fund(amount, sourcePosition);
        }
        else if (resource_type == "Physical_Energy")        // 如果输入的是 Physical_Energy，则 Add Physical_Energy
        {
            Add_Physical_Energy(amount, sourcePosition);
        }
        else if (resource_type == "Spiritual_Energy")        // 如果输入的是 Spirit，则 Add Spirit
        {
            Add_Spiritual_Energy(amount, sourcePosition);
        }
        else if (resource_type == "Soul")        // 如果输入的是 Soul，则 Add Soul
        {
            Add_Soul(amount, sourcePosition);
        }
        else if (resource_type == "Spirituality_Infused_Material")      // 如果输入的是 Spirituality，则 Add Spirituality
        {
            Add_Spirituality_Infused_Material(amount, sourcePosition);
        }
        else if (resource_type == "Knowledge")        // 如果输入的是 Knowledge，则 Add Knowledge
        {
            Add_Knowledge(amount, sourcePosition);
        }
        else if (resource_type == "Belief")        // 如果输入的是 Belief，则 Add Belief
        {
            Add_Belief(amount, sourcePosition);
        }
        else if (resource_type == "Putrefaction")        // 如果输入的是 Putrefaction，则 Add Putrefaction
        {
            Add_Putrefaction(amount, sourcePosition);
        }
        else if (resource_type == "Madness")        // 如果输入的是 Madness，则 Add Madness
        {
            Add_Madness(amount, sourcePosition);
        }
        else if (resource_type == "Godhood")        // 如果输入的是 Godhood，则 Add Godhood
        {
            Add_Godhood(amount, sourcePosition);
        }
        else if (resource_type == "Death")      // Death
        {
            Add_Death(amount, sourcePosition);
        }
        
    }
    
    // 减少资源的集成方法，输入资源名称，来调用下面减少特定资源的方法
    public void Reduce_Resource(string resource_type, int amount, Vector3 sourcePosition)
    {
        if (resource_type == "Fund")        // 如果输入的是 Fund，则 Reduce Fund
        {
            Reduce_Fund(amount, sourcePosition);
        }
        else if (resource_type == "Physical_Energy")        // 如果输入的是 Physical_Energy，则 Reduce Physical_Energy
        {
            Reduce_Physical_Energy(amount, sourcePosition);
        }
        else if (resource_type == "Spiritual_Energy")        // 如果输入的是 Spirit，则 Reduce Spirit
        {
            Reduce_Spiritual_Energy(amount, sourcePosition);
        }
        else if (resource_type == "Soul")        // 如果输入的是 Soul，则 Reduce Soul
        {
            Reduce_Soul(amount, sourcePosition);
        }
        else if (resource_type == "Spirituality_Infused_Material")      // 如果输入的是 Spirituality，则 Reduce Spirituality
        {
            Reduce_Spirituality_Infused_Material(amount, sourcePosition);
        }
        /*else if (resource_type == "Knowledge")        // 如果输入的是 Knowledge，则 Reduce Knowledge
        {
            Reduce_Knowledge(amount, sourcePosition);
        }*/
        else if (resource_type == "Belief")        // 如果输入的是 Belief，则 Reduce Belief
        {
            Reduce_Belief(amount, sourcePosition);
        }
        else if (resource_type == "Putrefaction")        // 如果输入的是 Putrefaction，则 Reduce Putrefaction
        {
            Reduce_Putrefaction(amount, sourcePosition);
        }
        else if (resource_type == "Madness")        // 如果输入的是 Madness，则 Reduce Madness
        {
            Reduce_Madness(amount, sourcePosition);
        }
        else if (resource_type == "Godhood")        // 如果输入的是 Godhood，则 Reduce Godhood
        {
            Reduce_Godhood(amount, sourcePosition);
        }
        else if (resource_type == "Death")        // Death
        {
            Reduce_Death(amount, sourcePosition); 
        }

    }
    

    
    // 增加Fund资源，仅在增加资源时调用，因此输入的 amount 需要为正值                   + Fund
    // 调用需传入当前物体 Position，以追踪来源
        public void Add_Fund(int amount, Vector3 position)     
        {
            // Fund += amount;
            // 如果是增加资源，则在 Animate Resource Change 结束后，该方法会自己调用函数 Resource Amount Change 增加资源，无须再添加
            
            Animate_Resource_Change(true, position, "Fund", amount);         // 调用动画效果
        }

    // 减少Fund资源，仅在减少资源时调用，因此输入的 amount 需要为正值                    - Fund
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Fund(int amount, Vector3 position)   
        {
            if (Fund - amount >= 0)    
            {
                // Fund -= amount;
                // 如果是减少资源，则 Animate Resource Change 不会改变资源数量，需要额外调用 Resource Amount Change
                
                Resource_Amount_Change("Fund", -amount);

                // 调用动画效果，具体执行
                Animate_Resource_Change(false, position, "Fund", amount);
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }
    
    // 增加Physical_Energy资源，仅在增加资源时调用，因此输入的 amount 需要为正值                 + Physical_Energy
    // 调用需传入当前物体 location，以追踪来源
        public void Add_Physical_Energy(int amount, Vector3 position)
        {
            // Physical_Energy += amount;
            // 如果是增加资源，则在 Animate Resource Change 结束后，该方法会自己调用函数 Resource Amount Change 增加资源，无须再添加
            
            Animate_Resource_Change(true, position, "Physical_Energy", amount); // 调用动画效果
        }

        // 减少Physical_Energy资源，仅在减少资源时调用，因此外部需做好 加减判断                   - Physical_Energy
        // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Physical_Energy(int amount, Vector3 position)
        {
            if (Physical_Energy - amount >= 0)
            {
                // Physical_Energy -= amount;
                // 如果是减少资源，则 Animate Resource Change 不会改变资源数量，需要额外调用 Resource Amount Change
                
                Resource_Amount_Change("Physical_Energy", -amount);
                
                Animate_Resource_Change(false, position, "Physical_Energy", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                
                /////////////////////////////    Physical Energy 不足，则不足部分生成 Death
                int amount_physical_energy = Physical_Energy;
                int amount_death = amount - Physical_Energy;
                Resource_Amount_Change("Physical_Energy", -amount_physical_energy);
                Animate_Resource_Change(false,position,"Physical_Energy", amount);
                Add_Death(amount_death,position);
                
                

                // TODO: 特殊效果的实现



            }
        }

    // 增加Spirit资源，仅在增加资源时调用，因此输入的 amount 需要为正值                    + Spirit
    // 调用需传入当前物体 location，以追踪来源
        public void Add_Spiritual_Energy(int amount, Vector3 position)
        {
            // Spirit += amount;
            // 如果是增加资源，则在 Animate Resource Change 结束后，该方法会自己调用函数 Resource Amount Change 增加资源，无须再添加
            
            Animate_Resource_Change(true, position, "Spiritual_Energy", amount); // 调用动画效果
        }

    // 减少Spirit资源，仅在减少资源时调用，因此外部需做好 加减判断                    - Spirit
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Spiritual_Energy(int amount, Vector3 position)
        {
            if (Spiritual_Energy - amount >= 0)
            {
                // Spirit -= amount;
                // 如果是减少资源，则 Animate Resource Change 不会改变资源数量，需要额外调用 Resource Amount Change
                
                Resource_Amount_Change("Spiritual_Energy", -amount);
                
                Animate_Resource_Change(false, position, "Spiritual_Energy", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }

        
    // 增加Soul资源，仅在增加资源时调用，因此外部需做好 加减判断                  + Soul
    // 调用需传入当前物体 location，以追踪来源
        public void Add_Soul(int amount, Vector3 position)
        {
            // Soul += amount;
            // 如果是增加资源，则在 Animate Resource Change 结束后，该方法会自己调用函数 Resource Amount Change 增加资源，无须再添加
            
            Animate_Resource_Change(true, position, "Soul", amount); // 调用动画效果
        }

    // 减少Soul资源，仅在减少资源时调用，因此外部需做好 加减判断                  - Soul
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Soul(int amount, Vector3 position)
        {
            if (Soul - amount >= 0)
            {
                // Soul -= amount;
                // 如果是减少资源，则 Animate Resource Change 不会改变资源数量，需要额外调用 Resource Amount Change
                
                Resource_Amount_Change("Soul", -amount);
                
                Animate_Resource_Change(false, position, "Soul", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }

    // 增加Spirituality_Infused_Material资源，仅在增加资源时调用，因此外部需做好 加减判断     + Spirituality_Infused_Material
    // 调用需传入当前物体 location，以追踪来源
        public void Add_Spirituality_Infused_Material(int amount, Vector3 position)
        {
            // Spirituality_Infused_Material += amount;
            // 如果是增加资源，则在 Animate Resource Change 结束后，该方法会自己调用函数 Resource Amount Change 增加资源，无须再添加
            
            Animate_Resource_Change(true, position, "Spirituality_Infused_Material", amount); // 调用动画效果
        }

    // 减少Spirituality_Infused_Material资源，仅在减少资源时调用，因此外部需做好 加减判断     - Spirituality_Infused_Material
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Spirituality_Infused_Material(int amount, Vector3 position)
        {
            if (Spirituality_Infused_Material - amount >= 0)
            {
                // Spirituality_Infused_Material -= amount;
                // 如果是减少资源，则 Animate Resource Change 不会改变资源数量，需要额外调用 Resource Amount Change
                
                Resource_Amount_Change("Spirituality_Infused_Material", -amount);
                
                Animate_Resource_Change(false, position, "Spirituality_Infused_Material", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }
        
        
        
        
    // 增加Knowledge资源，仅在增加资源时调用，因此外部需做好 加减判断                 + Knowledge
    // 调用需传入当前物体 location，以追踪来源
        /*public void Add_Knowledge(int amount, Vector3 position)
        {
            // Knowledge += amount;
            // 如果是增加资源，则在 Animate Resource Change 结束后，该方法会自己调用函数 Resource Amount Change 增加资源，无须再添加
            
            Animate_Resource_Change(true, position, "Knowledge", amount); // 调用动画效果
        }*/

        // 减少Knowledge资源，仅在减少资源时调用，因此外部需做好 加减判断                     - Knowledge
    // 调用需传入当前物体 location，以追踪来源
    
        /*public void Reduce_Knowledge(int amount, Vector3 position)
        {
            if (Knowledge - amount >= 0)
            {
                // Knowledge -= amount;
                // 如果是减少资源，则 Animate Resource Change 不会改变资源数量，需要额外调用 Resource Amount Change
                
                Resource_Amount_Change("Knowledge", -amount);
                
                Animate_Resource_Change(false, position, "Knowledge", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }*/
        
        ///////////////////////////////////////////////////////////////////               Knowledge 的更新方法
        
        public void Add_Knowledge(int amount, Vector3 position)     // 添加 Knowledge （抽取 Knowledge 卡）
        {
            float newKnowledgeCard_XPos = 3f;
            float newKnowledgeCard_YPos = -4f;

            for (int i = amount; i > 0; i--)
            {
                Draw_A_Knowledge_With_Rarity_Involved(position, position + new Vector3(newKnowledgeCard_XPos,newKnowledgeCard_YPos,0));
                newKnowledgeCard_YPos -= 3f;
            }
        }
        
        public void Reduce_Knowledge(GameObject knowledge_card)
        {

            Player_Owned_Current_Knowledge_List.Remove(knowledge_card.GetComponent<Knowledge_Feature>()._Knowledge.Id);      // 从 Owned Knowledge 里移除

        }
        
        
        

    // 增加Belief资源，仅在增加资源时调用，因此外部需做好 加减判断                + Belief
    // 调用需传入当前物体 location，以追踪来源
        public void Add_Belief(int amount, Vector3 position)
        {
            // Belief += amount;
            // 如果是增加资源，则在 Animate Resource Change 结束后，该方法会自己调用函数 Resource Amount Change 增加资源，无须再添加
            
            Animate_Resource_Change(true, position, "Belief", amount); // 调用动画效果
        }

    // 减少Belief资源，仅在减少资源时调用，因此外部需做好 加减判断                    - Belief
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Belief(int amount, Vector3 position)
        {
            if (Belief - amount >= 0)
            {
                // Belief -= amount;
                // 如果是减少资源，则 Animate Resource Change 不会改变资源数量，需要额外调用 Resource Amount Change
                
                Resource_Amount_Change("Belief", -amount);
                
                Animate_Resource_Change(false, position, "Belief", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }


    // 增加 Putrefaction资源，仅在增加资源时调用，因此外部需做好 加减判断             + Putrefaction
    // 调用需传入当前物体 location，以追踪来源
        public void Add_Putrefaction(int amount, Vector3 position)
        {
            // Putrefaction += amount;
            // 如果是增加资源，则在 Animate Resource Change 结束后，该方法会自己调用函数 Resource Amount Change 增加资源，无须再添加
            
            Animate_Resource_Change(true, position, "Putrefaction", amount); // 调用动画效果
        }

    // 减少Putrefaction资源，仅在减少资源时调用，因此外部需做好 加减判断              - Putrefaction
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Putrefaction(int amount, Vector3 position)
        {
            if (Putrefaction - amount >= 0)
            {
                // Putrefaction -= amount;
                // 如果是减少资源，则 Animate Resource Change 不会改变资源数量，需要额外调用 Resource Amount Change
                
                Resource_Amount_Change("Putrefaction", -amount);
                
                Animate_Resource_Change(false, position, "Putrefaction", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }
        
        
    // 增加Madness资源，仅在增加资源时调用，因此外部需做好 加减判断                   + Madness
    // 调用需传入当前物体 location，以追踪来源
        public void Add_Madness(int amount, Vector3 position)
        {
            // Madness += amount;
            // 如果是增加资源，则在 Animate Resource Change 结束后，该方法会自己调用函数 Resource Amount Change 增加资源，无须再添加
            
            Animate_Resource_Change(true, position, "Madness", amount); // 调用动画效果
        }

    // 减少Madness资源，仅在减少资源时调用，因此外部需做好 加减判断                   - Madness
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Madness(int amount, Vector3 position)
        {
            if (Madness - amount >= 0)
            {
                // Madness -= amount;
                // 如果是减少资源，则 Animate Resource Change 不会改变资源数量，需要额外调用 Resource Amount Change
                
                Resource_Amount_Change("Madness", -amount);
                
                Animate_Resource_Change(false, position, "Madness", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }

    // 增加Godhood资源，仅在增加资源时调用，因此外部需做好 加减判断                   + Godhood
    // 调用需传入当前物体 location，以追踪来源
        public void Add_Godhood(int amount, Vector3 position)
        {
            // Godhood += amount;
            // 如果是增加资源，则在 Animate Resource Change 结束后，该方法会自己调用函数 Resource Amount Change 增加资源，无须再添加
            
            Animate_Resource_Change(true, position, "Godhood", amount); // 调用动画效果
        }

    // 减少Godhood资源，仅在减少资源时调用，因此外部需做好 加减判断                   - Godhood
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Godhood(int amount, Vector3 position)
        {
            if (Godhood - amount >= 0)
            {
                // Godhood -= amount;
                // 如果是减少资源，则 Animate Resource Change 不会改变资源数量，需要额外调用 Resource Amount Change
                
                Resource_Amount_Change("Godhood", -amount);
                
                Animate_Resource_Change(false, position, "Godhood", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }
        
        
        // 增加Death资源，仅在增加资源时调用，因此外部需做好 加减判断                   + Death
        // 调用需传入当前物体 location，以追踪来源
        public void Add_Death(int amount, Vector3 position)
        {
            // Death += amount;
            // 如果是增加资源，则在 Animate Resource Change 结束后，该方法会自己调用函数 Resource Amount Change 增加资源，无须再添加
            
            Animate_Resource_Change(true, position, "Death", amount); // 调用动画效果
        }

        // 减少Death资源，仅在减少资源时调用，因此外部需做好 加减判断                   - Death
        // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Death(int amount, Vector3 position)
        {
            if (Death - amount >= 0)
            {
                // Death -= amount;
                // 如果是减少资源，则 Animate Resource Change 不会改变资源数量，需要额外调用 Resource Amount Change
                
                Resource_Amount_Change("Death", -amount);
                
                Animate_Resource_Change(false, position, "Death", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }

    
        
    

    // 动画效果，游戏表现，生产或消耗资源的执行
    
    private void Animate_Resource_Change(bool isAdding, Vector3 position, string resourceName, int animatedAmount)
    {
        GameObject iconInstance = null;     // 临时变量，存储生成的 resource icon 实例

        float first_time_adding_time = 1f;
        float adding_time = 0.6f;
        float reducing_time = 0.2f;

        if (resourceName == "Fund")
        {
            iconInstance = Instantiate(fund_icon_pure, position, Quaternion.identity);    // 在调用物体位置生成 icon
            
            if (isAdding)     // 如果是增加资源，则用 First Time Bool Passer 获取布尔值，判断是否是第一次，然后将 ever appears 设为 true（即出现过了）
            {
                FirstTimeBoolPasser = is_fund_ever_appears;
                is_fund_ever_appears = true;
            }
            
        }
        if (resourceName == "Physical_Energy")
        {
            iconInstance = Instantiate(physical_energy_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            
            if (isAdding)     // 如果是增加资源，则用 First Time Bool Passer 获取布尔值，判断是否是第一次，然后将 ever appears 设为 true（即出现过了）
            {
                FirstTimeBoolPasser = is_physical_energy_ever_appears;
                is_physical_energy_ever_appears = true;
            }
        }
        if (resourceName == "Spiritual_Energy")
        {
            iconInstance = Instantiate(spiritual_energy_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            
            if (isAdding)     // 如果是增加资源，则用 First Time Bool Passer 获取布尔值，判断是否是第一次，然后将 ever appears 设为 true（即出现过了）
            {
                FirstTimeBoolPasser = is_spiritual_energy_ever_appears;
                is_spiritual_energy_ever_appears = true;
            }
        }
        if (resourceName == "Soul")
        {
            iconInstance = Instantiate(soul_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            
            if (isAdding)     // 如果是增加资源，则用 First Time Bool Passer 获取布尔值，判断是否是第一次，然后将 ever appears 设为 true（即出现过了）
            {
                FirstTimeBoolPasser = is_soul_ever_appears;
                is_soul_ever_appears = true;
            }
        }
        if (resourceName == "Spirituality_Infused_Material")
        {
            iconInstance = Instantiate(spirituality_infused_material_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            
            if (isAdding)     // 如果是增加资源，则用 First Time Bool Passer 获取布尔值，判断是否是第一次，然后将 ever appears 设为 true（即出现过了）
            {
                FirstTimeBoolPasser = is_spirituality_infused_material_ever_appears;
                is_spirituality_infused_material_ever_appears = true;
            }
        }
        
        /*if (resourceName == "Knowledge")
        {
            iconInstance = Instantiate(knowledge_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            
            if (isAdding)     // 如果是增加资源，则用 First Time Bool Passer 获取布尔值，判断是否是第一次，然后将 ever appears 设为 true（即出现过了）
            {
                FirstTimeBoolPasser = is_knowledge_ever_appears;
                is_knowledge_ever_appears = true;
            }
        }*/
        
        
        if (resourceName == "Belief")
        {
            iconInstance = Instantiate(belief_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            
            if (isAdding)     // 如果是增加资源，则用 First Time Bool Passer 获取布尔值，判断是否是第一次，然后将 ever appears 设为 true（即出现过了）
            {
                FirstTimeBoolPasser = is_belief_ever_appears;
                is_belief_ever_appears = true;
            }
        }
        if (resourceName == "Putrefaction")
        {
            iconInstance = Instantiate(putrefaction_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            
            if (isAdding)     // 如果是增加资源，则用 First Time Bool Passer 获取布尔值，判断是否是第一次，然后将 ever appears 设为 true（即出现过了）
            {
                FirstTimeBoolPasser = is_putrefaction_ever_appears;
                is_putrefaction_ever_appears = true;
            }
        }
        if (resourceName == "Madness")
        {
            iconInstance = Instantiate(madness_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            
            if (isAdding)     // 如果是增加资源，则用 First Time Bool Passer 获取布尔值，判断是否是第一次，然后将 ever appears 设为 true（即出现过了）
            {
                FirstTimeBoolPasser = is_madness_ever_appears;
                is_madness_ever_appears = true;
            }
        }
        if (resourceName == "Godhood")
        {
            iconInstance = Instantiate(godhood_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            
            if (isAdding)     // 如果是增加资源，则用 First Time Bool Passer 获取布尔值，判断是否是第一次，然后将 ever appears 设为 true（即出现过了）
            {
                FirstTimeBoolPasser = is_godhood_ever_appears;
                is_godhood_ever_appears = true;
            }
        }
        
        // Death资源 在这个位置的操作可以变得不同，比如生成一个进度条
        // TODO 加入 Death 资源处理
        // if (resourceName == "Death")
        
        if (resourceName == "Death")
        {
            iconInstance = Instantiate(death_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            
            if (isAdding)     // 如果是增加资源，则用 First Time Bool Passer 获取布尔值，判断是否是第一次，然后将 ever appears 设为 true（即出现过了）
            {
                FirstTimeBoolPasser = is_death_ever_appears;
                is_death_ever_appears = true;
            }
        }
        

   
        
        if (isAdding)   // 增加资源时
        {
            if (!FirstTimeBoolPasser)     // 如果是第一次
            {

                if (resourceName == "Death")            // 如果是 Death，添加 Death 进度条生成
                {
                    Vector3 deathPosition = GameObject.Find("Resource_Location_Death").transform.position;      // 找 Death 进度条的 position
                    
                    iconInstance.GetComponent<SpriteRenderer>().sortingLayerName = "Dragging";      // 将 资源icon 的 Sorting层 改为 最上层
                    iconInstance.transform.DOMove(deathPosition, first_time_adding_time).OnComplete(() => // 移动完成后，添加 collider 和脚本组件
                        {
                            Destroy(iconInstance);      // 销毁 death icon
                            
                            // TODO 添加炫酷的进度条出现特效
                            Instantiate(death_bar_prefab, deathPosition, Quaternion.identity);            // 生成 Death 进度条 prefab
                            
                            Resource_Amount_Change(resourceName, animatedAmount);

                        }
                    );

                }

                else
                {
                    int locationIndex = FindAvailableResourceLocation();      // 找到空的 resource_location_X 的 slot，获取到空 slot 的序号 X
                    resourceLocationsOccupied[locationIndex] = true;        // 相应的 resource_location_X slot 被占领
                    resourceLocationIndex[resourceName] = locationIndex;    // 记录资源对应的 resource_location_X 的 X 序号
                
                    Vector3 targetPosition = GetResourceLocationPosition(locationIndex);    // 获取到 resource_location_X 的位置
                    iconInstance.GetComponent<SpriteRenderer>().sortingLayerName = "Dragging";      // 将 资源icon 的 Sorting层 改为 最上层
                    iconInstance.transform.DOMove(targetPosition, first_time_adding_time).OnComplete(() => // 移动完成后，添加 collider 和脚本组件
                        {
                            // 添加脚本组件和 collider
                            iconInstance.name = "Resource_Icon_" + resourceName;
                            iconInstance.AddComponent<Resource_Click_Message>();
                            iconInstance.GetComponent<Resource_Click_Message>().messageId = resourceName;
                            CircleCollider2D circleCollider = iconInstance.AddComponent<CircleCollider2D>();
                            iconInstance.GetComponent<SpriteRenderer>().sortingLayerName = "OnBoards";
                        
                            // 出现 number（暂定不出现资源名称先 2023-12-10）
                            // 更新 2个 Dictionary 的记录，来让 Update 中 "更新各 slot 中资源数量" 的 TMP_text 投入工作
                            slotAbleToShowText[locationIndex] = true;       // 开锁，编号为 X 的槽位可以开始显示 资源数量的 text 了
                            Resource_Amount_Change(resourceName, animatedAmount);

                        }
                    );
                }
                
                
            }
            else        // 如果不是第一次了
            {

                Vector3 targetPosition = new Vector3(0, 0, 0);
                if (resourceName == "Death")
                {
                    // 如果是 Death，则单独寻找 Death 进度条的 position
                    targetPosition = GameObject.Find("Resource_Location_Death").transform.position;      // 找到 Death 的 location
                }
                else
                {
                    targetPosition = GetResourceLocationPosition(resourceLocationIndex[resourceName]);  // 如果不是 Death，则用寻找功能来寻找资源位置
                }
                iconInstance.GetComponent<SpriteRenderer>().sortingLayerName = "Dragging";
                iconInstance.transform.DOMove(targetPosition, adding_time).OnComplete(() =>
                    {
                        Destroy(iconInstance);
                        Resource_Amount_Change(resourceName, animatedAmount);
                    });
                
            }
            
        }
        else   //  减少资源时
        {

            /*if (!AppearBoolPasser)      // 如果是第一次
            {
                int locationIndex = FindAvailableResourceLocation();      // 找到空的 resource_location_X 的 slot，获取到空 slot 的序号 X
                Vector3 targetPosition = GetResourceLocationPosition(locationIndex);    // 获取到 resource_location_X 的位置
                
                // 此处可以加酷炫的 shader TODO 制作动画或者特效让 icon 出现
                // 但目前让它直接出现
                iconInstance.transform.position = targetPosition;        // 将 之前生成的 icon 移动到 resource_location_X 的位置
                
                resourceLocationsOccupied[locationIndex] = true;        // 相应的 resource_location_X slot 被占领
                resourceLocationIndex[resourceName] = locationIndex;    // 记录资源对应的 resource_location_X 的 X 序号
                
            }
            else   // 如果不是第一次*/
            {
                if (resourceName == "Death")        // 如果是 Death，则直接寻找 death 的 position
                {
                    iconInstance.transform.position = GameObject.Find("Resource_Location_Death").transform.position;
                }
                else                               // 如果不是 Death，则调用方法来寻找 资源的 position
                {
                    iconInstance.transform.position = GetResourceLocationPosition(resourceLocationIndex[resourceName]);
                }
                
                iconInstance.transform.DOMove(position, reducing_time).OnComplete(() =>
                    Destroy(iconInstance));
            }
        }
        
        
        // 原本的 将资源 ever appears 参数设置为 true 的方法， 现在弃置不用
        
        /*if (resourceName == "Fund") { is_fund_ever_appears = true; }
        if (resourceName == "Physical_Energy") { is_physical_energy_ever_appears = true; }
        if (resourceName == "Spirit") { is_spirit_ever_appears = true; }
        if (resourceName == "Soul") { is_soul_ever_appears = true; }
        if (resourceName == "Spirituality_Infused_Material") { is_spirituality_infused_material_ever_appears = true; }
        if (resourceName == "Knowledge") { is_knowledge_ever_appears = true;}
        if (resourceName == "Belief") { is_belief_ever_appears = true; }
        if (resourceName == "Putrefaction") { is_putrefaction_ever_appears = true; }
        if (resourceName == "Madness") { is_madness_ever_appears = true; }
        if (resourceName == "Godhood") { is_godhood_ever_appears = true; }*/
        
    }
    
    
    
    
    ///////////////////////////////////////////////      Knowledge，相关系列方法
    
    public void Draw_A_Knowledge_With_Rarity_Involved(Vector3 sourcePosition, Vector3 targetPosition)      // 抽一张 Knowledge，based on 稀有度
    {
        // 根据稀有度 抽卡
        
        // 概率
        // Common : 6 / 10
        // Rare : 3 / 10
        // Mythic : 1 / 10

        List<string> rarity_draw_list = new List<string>()
        {
            "Common",
            "Common",
            "Common",
            "Common",
            "Common",
            "Common",
            "Rare",
            "Rare",
            "Rare",
            "Mythic"
        };

        int index = Random.Range(0, rarity_draw_list.Count);
        
        Draw_A_Knowledge("", rarity_draw_list[index], sourcePosition, targetPosition);

    }

    public void Draw_A_Knowledge_Of_Certain_Rarity(string rarity, Vector3 sourcePosition, Vector3 targetPosition)          // 抽一张 特定稀有度的 Knowledge
    {
        Draw_A_Knowledge("", rarity, sourcePosition, targetPosition);
    }

    public void Draw_A_Knowledge_By_Name(string Knowledge_Name, Vector3 sourcePosition, Vector3 targetPosition)    // 抽一张 特定的 Knowledge
    {
        Draw_A_Knowledge(Knowledge_Name,"", sourcePosition, targetPosition);
    }
    
    
    public void Draw_A_Knowledge(string Name, string Rarity, Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (Name != "")      // 如果有具体指名，则直接抽取
        {
            GameManager.GM.Generate_Knowledge_Card(Name, targetPosition);
            
            Player_Owned_Current_Knowledge_List.Add(Name);
        }

        else if (Rarity != "")      // 如果没有指名，但有稀有度输入，则抽取一张相应稀有度的 Knowledge

        {

            List<string> filtered_knowledge_list_of_certain_rarity = Get_All_Knowledge_Of_Certain_Rarity(Rarity);       // Get ALL 方法过滤了曾经拥有过的 Knowledge

            
            if (filtered_knowledge_list_of_certain_rarity.Count > 0)        // 如果 还有 Knowledge 可以抽
            {
                int index = Random.Range(0, filtered_knowledge_list_of_certain_rarity.Count);           // 从这个 过滤的 list 中抽一张
            
                GameManager.GM.Generate_Knowledge_Card(filtered_knowledge_list_of_certain_rarity[index], targetPosition);       // 根据 index 找到 string 元素并生成卡
            
                Player_Owned_Current_Knowledge_List.Add(filtered_knowledge_list_of_certain_rarity[index]);   // 将抽到的 Knowledge 记录到当前拥有的 Knowledge list，全局的在 Generate_Knowledge 方法里已经加了
            }
            else
            {
                Debug.Log("hi -knowledge");
            }
            
        }
    
        else    // Name 和 Rarity 都为空时的特殊处理
        
        {
            
        }
        
    }

    public List<string> Get_All_Knowledge_Of_Certain_Rarity(string Rarity)
    {
        List<string> list_of_knowledge_of_certain_rarity = new List<string>() { };

        foreach (var knowledge_instance in GameManager.GM.CardLoader.Knowledge_List)
        {
            if (
                knowledge_instance.Rarity == Rarity // 如果 稀有度 匹配
                && !GameManager.GM.Player_Owned_All_Knowledge.Contains(knowledge_instance.Id) // 且不曾拥有过 这个 Knowledge
            )
            {
                list_of_knowledge_of_certain_rarity.Add(knowledge_instance.Id);
            }
        }

        return list_of_knowledge_of_certain_rarity;
    }



    void Check_If_Knowledge_Appears()
    {
        if (Player_Owned_Current_Knowledge_List.Count > 0 && !is_knowledge_ever_appears)
        {
            is_knowledge_ever_appears = true;
            
            // Knowledge Icon 的出现
            int locationIndex = FindAvailableResourceLocation();      // 找到空的 resource_location_X 的 slot，获取到空 slot 的序号 X
            resourceLocationsOccupied[locationIndex] = true;        // 相应的 resource_location_X slot 被占领
            resourceLocationIndex["Knowledge"] = locationIndex;    // 记录资源对应的 resource_location_X 的 X 序号
            
            
            Move_Resource_Markers_OnBoard_For_Knowledge(locationIndex);     // 移动 Board 上的 资源位置 Marker
            
                
            Vector3 targetPosition = GetResourceLocationPosition(locationIndex);    // 获取到 resource_location_X 的位置
            
            // Knowledge Icon 出现的炫酷动画
            
            GameObject iconInstance = Instantiate(knowledge_icon_pure, targetPosition, Quaternion.identity);
            
            // 添加脚本组件和 collider
            iconInstance.name = "Resource_Icon_Knowledge";
            iconInstance.AddComponent<Resource_Click_Message>();
            iconInstance.GetComponent<Resource_Click_Message>().messageId = "Knowledge";
            BoxCollider2D circleCollider = iconInstance.AddComponent<BoxCollider2D>();
            iconInstance.GetComponent<SpriteRenderer>().sortingLayerName = "OnBoards";
            slotAbleToShowText[locationIndex] = true;       // 开锁，编号为 X 的槽位可以开始显示 资源数量的 text 了
        }
    }


    // 当玩家点击 Panel 上的 Knowledge icon 时， 吸收一个 Knowledge，按照 Rarity 优先级 来先吸收低级的
    void Remove_Knowledge_Based_On_Rarity()
    {
        
        
    }


    void Move_Resource_Markers_OnBoard_For_Knowledge(int knowledge_slot_X)
    {

        float knowledge_icon_move_distance = 0.4f;
        float knowledge_text_move_distance = 1.6f;
        float other_markers_move_distance = 1.25f;

        GameObject.Find($"Resource_Location_{knowledge_slot_X}").
                transform.position += new Vector3(knowledge_icon_move_distance, 0, 0);

        GameObject.Find($"Resource_Amount_Text_{knowledge_slot_X}").
                transform.position += new Vector3(knowledge_text_move_distance, 0, 0);

        int index = knowledge_slot_X + 1;

        while (true)
        {
            if (GameObject.Find($"Resource_Location_{index}") == null)
                break;
            
            GameObject.Find($"Resource_Location_{index}").
                transform.position += new Vector3(other_markers_move_distance, 0, 0);
            GameObject.Find($"Resource_Amount_Text_{index}").
                transform.position += new Vector3(other_markers_move_distance, 0, 0);

            index++;
        }

    }





    ///////////////////////////////////////////////      Knowledge，相关系列方法  END





    // 寻找可用的资源位置
    private int FindAvailableResourceLocation()
    {
        foreach (var location in resourceLocationsOccupied)
        {
            if (!location.Value) return location.Key;
        }
        return -1; // 如果没有空位
    }

    // 获取资源位置的世界坐标
    private Vector3 GetResourceLocationPosition(int locationIndex)
    {
        GameObject locationObject = GameObject.Find("Resource_Location_" + locationIndex);
        return locationObject != null ? locationObject.transform.position : Vector3.zero;
    }
    
    // 原更改资源真实数量的方法，让 resource 在改变数量时数字闪烁，  archived 2024-2-28
    /*private void Resource_Amount_Flash(string flash_resource, int flash_amount) //输入有正有负
    {
        StartCoroutine(Number_Flash(flash_resource, flash_amount));
    }*/
    /*private IEnumerator Number_Flash(string flash_resource, int flash_amount)
    {
        float flash_one_time = 0.1f;    // 一次闪烁的时间
        
        if (flash_amount > 0)       // 大于 0 时，加数值
        {
            if (flash_resource == "Fund")
            {
                float finalAmount = Fund + flash_amount;
                while (Fund < finalAmount)
                {
                    Fund++;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Physical_Energy")
            {
                float finalAmount = Physical_Energy + flash_amount;
                while (Physical_Energy < finalAmount)
                {
                    Physical_Energy++;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Spirit")
            {
                float finalAmount = Spirit + flash_amount;
                while (Spirit < finalAmount)
                {
                    Spirit++;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Soul")
            {
                float finalAmount = Soul + flash_amount;
                while (Soul < finalAmount)
                {
                    Soul++;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Spirituality_Infused_Material")
            {
                float finalAmount = Spirituality_Infused_Material + flash_amount;
                while (Spirituality_Infused_Material < finalAmount)
                {
                    Spirituality_Infused_Material++;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Knowledge")
            {
                float finalAmount = Knowledge + flash_amount;
                while (Knowledge < finalAmount)
                {
                    Knowledge++;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Belief")
            {
                float finalAmount = Belief + flash_amount;
                while (Belief < finalAmount)
                {
                    Belief++;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Putrefaction")
            {
                float finalAmount = Putrefaction + flash_amount;
                while (Putrefaction < finalAmount)
                {
                    Putrefaction++;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Madness")
            {
                float finalAmount = Madness + flash_amount;
                while (Madness < finalAmount)
                {
                    Madness++;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Godhood")
            {
                float finalAmount = Godhood + flash_amount;
                while (Godhood < finalAmount)
                {
                    Godhood++;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
        }
        
        else if (flash_amount < 0)       // 小于 0 时，减数值
        {
            if (flash_resource == "Fund")
            {
                float finalAmount = Fund + flash_amount;
                while (Fund > finalAmount)
                {
                    Fund--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Physical_Energy")
            {
                float finalAmount = Physical_Energy + flash_amount;
                while (Physical_Energy > finalAmount)
                {
                    Physical_Energy--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Spirit")
            {
                float finalAmount = Spirit + flash_amount;
                while (Spirit > finalAmount)
                {
                    Spirit--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Soul")
            {
                float finalAmount = Soul + flash_amount;
                while (Soul > finalAmount)
                {
                    Soul--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Spirituality_Infused_Material")
            {
                float finalAmount = Spirituality_Infused_Material + flash_amount;
                while (Spirituality_Infused_Material > finalAmount)
                {
                    Spirituality_Infused_Material--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Knowledge")
            {
                float finalAmount = Knowledge + flash_amount;
                while (Knowledge > finalAmount)
                {
                    Knowledge--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Belief")
            {
                float finalAmount = Belief + flash_amount;
                while (Belief > finalAmount)
                {
                    Belief--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Putrefaction")
            {
                float finalAmount = Putrefaction + flash_amount;
                while (Putrefaction > finalAmount)
                {
                    Putrefaction--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Madness")
            {
                float finalAmount = Madness + flash_amount;
                while (Madness > finalAmount)
                {
                    Madness--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Godhood")
            {
                float finalAmount = Godhood + flash_amount;
                while (Godhood > finalAmount)
                {
                    Godhood--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
        }
        
    }*/
    
    
    // ** 实际更改资源 真实数量 的方法 
    private void Resource_Amount_Change(string change_resource, int change_amount) //输入有正有负
    {
        
        if (change_resource == "Fund")
        {
            Fund += change_amount;
        }
        if (change_resource == "Physical_Energy")
        {
            Physical_Energy += change_amount;
        }
        if (change_resource == "Spiritual_Energy")
        {
            Spiritual_Energy += change_amount;
        }
        if (change_resource == "Soul")
        {
            Soul += change_amount;
        }
        if (change_resource == "Spirituality_Infused_Material")
        {
            Spirituality_Infused_Material += change_amount;
        }
        /*if (change_resource == "Knowledge")       // Knowledge 用数 List 的 Count 的方式来更新数量
        {
            Knowledge += change_amount;
        }*/
        if (change_resource == "Belief")
        {
            Belief += change_amount;
        }
        if (change_resource == "Putrefaction")
        {
            Putrefaction += change_amount;
        }
        if (change_resource == "Madness")
        {
            Madness += change_amount;
        }
        if (change_resource == "Godhood")
        {
            Godhood += change_amount;
        }
        if (change_resource == "Death")
        {
            Death += change_amount;
        }
    }
    
    
    // 在 Update 中，如果一个资源 slot 被占领，则显示并实时更新资源的 数量 text
    private void Update_Resource_Amount_In_Slots()
    {
        foreach (var slot in resourceLocationsOccupied)     // 对于每个槽位
        {
            string resourceName = "";
            if (slot.Value && slotAbleToShowText[slot.Key])     // 如果槽位被占用 且 槽位对应的资源数量 text 能够显示
            {
                
                // 则获取槽位号 X 对应的资源名称
                foreach (var slotResource in resourceLocationIndex)
                {
                    if (slotResource.Value == slot.Key)
                    {
                        resourceName = slotResource.Key;
                        break;
                    }
                }
                
                // 根据资源的名称，将相应的槽位对应的 amount text 的文本更新为 该资源拥有的数量

                if (resourceName == "Fund")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Fund_UI_Amount.ToString();
                }
                if (resourceName == "Physical_Energy")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Physical_Energy_UI_Amount.ToString();
                }
                if (resourceName == "Spiritual_Energy")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Spiritual_Energy_UI_Amount.ToString();
                }
                if (resourceName == "Soul")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Soul_UI_Amount.ToString();
                }
                if (resourceName == "Spirituality_Infused_Material")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Spirituality_Infused_Material_UI_Amount.ToString();
                }
                if (resourceName == "Knowledge")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Player_Owned_Current_Knowledge_List.Count.ToString();       // Knowledge 的数量直接 数 Knowledge List 的 Count
                }
                if (resourceName == "Belief")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Belief_UI_Amount.ToString();
                }
                if (resourceName == "Putrefaction")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Putrefaction_UI_Amount.ToString();
                }
                if (resourceName == "Madness")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Madness_UI_Amount.ToString();
                }
                if (resourceName == "Godhood")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Godhood_UI_Amount.ToString();
                }
                
            }
        }

    }

    private IEnumerator Update_Resource_UI_Amount()
    {
        int flash_amount_each_time = 1;
        float flash_time = 0.1f;
        
        
        while (true)
        {

            if (Fund_UI_Amount != Fund)     // 假如 resource数量 有更新，即 resource真实数量 和 UI显示数量 不一样 
            {
                int Change = Fund - Fund_UI_Amount;
                Fund_UI_Amount += flash_amount_each_time * Math.Sign(Change);  // Math.Sign 返回一个数的正负值
            }
            if (Physical_Energy_UI_Amount != Physical_Energy)     
            {
                int Change = Physical_Energy - Physical_Energy_UI_Amount;
                Physical_Energy_UI_Amount += flash_amount_each_time * Math.Sign(Change);  
            }
            if (Spiritual_Energy_UI_Amount != Spiritual_Energy)
            {
                int Change = Spiritual_Energy - Spiritual_Energy_UI_Amount;
                Spiritual_Energy_UI_Amount += flash_amount_each_time * Math.Sign(Change); 
            }
            if (Soul_UI_Amount != Soul) 
            {
                int Change = Soul - Soul_UI_Amount;
                Soul_UI_Amount += flash_amount_each_time * Math.Sign(Change);
            }
            if (Spirituality_Infused_Material_UI_Amount != Spirituality_Infused_Material) 
            {
                int Change = Spirituality_Infused_Material - Spirituality_Infused_Material_UI_Amount;
                Spirituality_Infused_Material_UI_Amount += flash_amount_each_time * Math.Sign(Change); 
            }
            
            // Knowledge 的 UI 数量通过变更 Knowledge List 并识别 Count 来实现，不在此处写
            
            if (Belief_UI_Amount != Belief) 
            {
                int Change = Belief - Belief_UI_Amount;
                Belief_UI_Amount += flash_amount_each_time * Math.Sign(Change); 
            }
            if (Putrefaction_UI_Amount != Putrefaction)
            {
                int Change = Putrefaction - Putrefaction_UI_Amount;
                Putrefaction_UI_Amount += flash_amount_each_time * Math.Sign(Change);
            }
            if (Madness_UI_Amount != Madness)
            {
                int Change = Madness - Madness_UI_Amount;
                Madness_UI_Amount += flash_amount_each_time * Math.Sign(Change);
            }
            if (Godhood_UI_Amount != Godhood)
            {
                int Change = Godhood - Godhood_UI_Amount;
                Godhood_UI_Amount += flash_amount_each_time * Math.Sign(Change);
            }
            
            
            
            if (Max_Death_UI_Amount != Max_Death)       // 更新 Max Death 数量
            {
                int Change = Max_Death - Max_Death_UI_Amount;
                Max_Death_UI_Amount += flash_amount_each_time * Math.Sign(Change);
            }
            if (Death_UI_Amount != Death)       // 更新 Death 数量
            {
                int Change = Death - Death_UI_Amount;
                Death_UI_Amount += flash_amount_each_time * Math.Sign(Change);
            }
            


            yield return new WaitForSeconds(flash_time);
        }
    }
    
    
    
    
    
    
    
    
    
    
    
}

