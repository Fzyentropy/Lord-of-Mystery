using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;

public class Resource_Manager : MonoBehaviour
{
    // 各个资源的数量，真正出现在游戏中的计数参数，需要保存
    public int Fund;
    public int Physical_Energy;
    public int Spirit;
    public int Soul;
    public int Spirituality_Infused_Material;
    public int Knowledge;
    public int Belief;
    public int Putrefaction;
    public int Madness;
    public int Godhood;
    
    
    // 资源位置标记和占用情况，真正出现在游戏中的标记参数，需要保存
    private Dictionary<int, bool> resourceLocationsOccupied = new Dictionary<int, bool>();      // int : 资源槽位的编号    bool : 是否被占用, true:占用，false:没占用
    private Dictionary<string, int> resourceLocationIndex = new Dictionary<string, int>();      // string : 资源名称    int : 资源占用的槽位编号
    private Dictionary<int, bool> slotAbleToShowText = new Dictionary<int, bool>();      // int : 资源槽位的编号    bool : 是否可以被update资源数量text了，字典用于开锁：是否可以显示资源数量

    public GameObject fund_icon_pure;   // Fund资源的图标Prefab
    public GameObject physical_energy_icon_pure;    // Physical_Energy资源的图标Prefab
    public GameObject spirit_icon_pure;     // Spirit资源的图标Prefab
    public GameObject soul_icon_pure;   // Soul资源的图标Prefab
    public GameObject spirituality_infused_material_icon_pure;  // Spirituality_Infused_Material资源的图标Prefab
    public GameObject knowledge_icon_pure;  // Knowledge资源的图标Prefab
    public GameObject belief_icon_pure;     // Belief资源的图标Prefab
    public GameObject putrefaction_icon_pure;   // Putrefaction资源的图标Prefab
    public GameObject madness_icon_pure;    // Madness资源的图标Prefab
    public GameObject godhood_icon_pure;    // Godhood资源的图标Prefab
    

    private bool is_fund_ever_appears = false;
    private bool is_physical_energy_ever_appears = false; // 检查Physical_Energy资源是否曾出现
    private bool is_spirit_ever_appears = false; // 检查Spirit资源是否曾出现
    private bool is_soul_ever_appears = false; // 检查Soul资源是否曾出现
    private bool is_spirituality_infused_material_ever_appears = false; // 检查Spirituality_Infused_Material资源是否曾出现
    private bool is_knowledge_ever_appears = false; // 检查Knowledge资源是否曾出现
    private bool is_belief_ever_appears = false; // 检查Belief资源是否曾出现
    private bool is_putrefaction_ever_appears = false; // 检查Putrefaction资源是否曾出现
    private bool is_madness_ever_appears = false; // 检查Madness资源是否曾出现
    private bool is_godhood_ever_appears = false; // 检查Godhood资源是否曾出现

    public TMP_Text resource_1_Amount_text;
    public TMP_Text resource_2_Amount_text;
    public TMP_Text resource_3_Amount_text;
    public TMP_Text resource_4_Amount_text;
    public TMP_Text resource_5_Amount_text;
    public TMP_Text resource_6_Amount_text;
    public TMP_Text resource_7_Amount_text;
    public TMP_Text resource_8_Amount_text;
    public TMP_Text resource_9_Amount_text;


    private bool FirstTimeBoolPasser = false;      // 用于将上方的 is_XXX_ever_appears 布尔值赋予此参数，来判断是否是第一次

    private GameObject iconInstance;
    

    // 在Start中初始化资源位置占用情况
    void Start()
    {
        InitializeDictionaries();       // 初始化 上方建立的字典


        // 加载或设置fundIconPrefab
        // fundIconPrefab = ...
    }

    private void Update()
    {
        Update_Resource_Amount_In_Slots();
    }

    
    
    private void InitializeDictionaries()   // 初始化 上方建立的字典
    {
        for (int i = 1; i <= 10; i++)       // 初始化 资源槽位是否被占用字典
        {
            resourceLocationsOccupied[i] = false;
        }

        resourceLocationIndex = new Dictionary<string, int>     // 初始化 资源对应槽位字典
        {
            {"Fund", -1},
            {"Physical_Energy", -1},
            {"Spirit", -1},
            {"Soul", -1},
            {"Spirituality_Infused_Material", -1},
            {"Knowledge", -1},
            {"Belief", -1},
            {"Putrefaction", -1},
            {"Madness", -1},
            {"Godhood", -1}
        };
        
        for (int i = 1; i <= 10; i++)       // 初始化 槽位是否能显示数量字典
        {
            slotAbleToShowText[i] = false;
        }

    }


    // 增加资源的集成方法，输入资源名称，来调用下面增加特定资源的方法
    public void Add_Resource(string resource_type, int amount, Vector3 position)
    {
        if (resource_type == "Fund")        // 如果输入的是 Fund，则 Add Fund
        {
            Add_Fund(amount, position);
        }
        else if (resource_type == "Physical_Energy")        // 如果输入的是 Physical_Energy，则 Add Physical_Energy
        {
            Add_Physical_Energy(amount, position);
        }
        else if (resource_type == "Spirit")        // 如果输入的是 Spirit，则 Add Spirit
        {
            Add_Spirit(amount, position);
        }
        else if (resource_type == "Soul")        // 如果输入的是 Soul，则 Add Soul
        {
            Add_Soul(amount, position);
        }
        else if (resource_type == "Spirituality_Infused_Material")      // 如果输入的是 Spirituality，则 Add Spirituality
        {
            Add_Spirituality_Infused_Material(amount, position);
        }
        else if (resource_type == "Knowledge")        // 如果输入的是 Knowledge，则 Add Knowledge
        {
            Add_Knowledge(amount, position);
        }
        else if (resource_type == "Belief")        // 如果输入的是 Belief，则 Add Belief
        {
            Add_Belief(amount, position);
        }
        else if (resource_type == "Putrefaction")        // 如果输入的是 Putrefaction，则 Add Putrefaction
        {
            Add_Putrefaction(amount, position);
        }
        else if (resource_type == "Madness")        // 如果输入的是 Madness，则 Add Madness
        {
            Add_Madness(amount, position);
        }
        else if (resource_type == "Godhood")        // 如果输入的是 Godhood，则 Add Godhood
        {
            Add_Godhood(amount, position);
        }
        
    }
    
    // 减少资源的集成方法，输入资源名称，来调用下面减少特定资源的方法
    public void Reduce_Resource(string resource_type, int amount, Vector3 position)
    {
        if (resource_type == "Fund")        // 如果输入的是 Fund，则 Reduce Fund
        {
            Reduce_Fund(amount, position);
        }
        else if (resource_type == "Physical_Energy")        // 如果输入的是 Physical_Energy，则 Reduce Physical_Energy
        {
            Reduce_Physical_Energy(amount, position);
        }
        else if (resource_type == "Spirit")        // 如果输入的是 Spirit，则 Reduce Spirit
        {
            Reduce_Spirit(amount, position);
        }
        else if (resource_type == "Soul")        // 如果输入的是 Soul，则 Reduce Soul
        {
            Reduce_Soul(amount, position);
        }
        else if (resource_type == "Spirituality_Infused_Material")      // 如果输入的是 Spirituality，则 Reduce Spirituality
        {
            Reduce_Spirituality_Infused_Material(amount, position);
        }
        else if (resource_type == "Knowledge")        // 如果输入的是 Knowledge，则 Reduce Knowledge
        {
            Reduce_Knowledge(amount, position);
        }
        else if (resource_type == "Belief")        // 如果输入的是 Belief，则 Reduce Belief
        {
            Reduce_Belief(amount, position);
        }
        else if (resource_type == "Putrefaction")        // 如果输入的是 Putrefaction，则 Reduce Putrefaction
        {
            Reduce_Putrefaction(amount, position);
        }
        else if (resource_type == "Madness")        // 如果输入的是 Madness，则 Reduce Madness
        {
            Reduce_Madness(amount, position);
        }
        else if (resource_type == "Godhood")        // 如果输入的是 Godhood，则 Reduce Godhood
        {
            Reduce_Godhood(amount, position);
        }

    }
    

    
    // 增加Fund资源，仅在增加资源时调用，因此输入的 amount 需要为正值                   + Fund
    // 调用需传入当前物体 Position，以追踪来源
        public void Add_Fund(int amount, Vector3 position)     
        {
            // Fund += amount;       // 在 Animate Resource Change 结束后，再增加
            
            AnimateResourceChange(true, position, "Fund", amount);         // 调用动画效果
        }

    // 减少Fund资源，仅在减少资源时调用，因此输入的 amount 需要为正值                    - Fund
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Fund(int amount, Vector3 position)   
        {
            if (Fund - amount >= 0)
            {
                // Fund -= amount;
                Resource_Amount_Flash("Fund", -amount);
                
                // 调用动画效果，具体执行
                AnimateResourceChange(false, position, "Fund", amount);
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
            // Physical_Energy += amount;      // 在 Animate Resource Change 结束后，再增加
            
            AnimateResourceChange(true, position, "Physical_Energy", amount); // 调用动画效果
        }

        // 减少Physical_Energy资源，仅在减少资源时调用，因此外部需做好 加减判断                   - Physical_Energy
        // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Physical_Energy(int amount, Vector3 position)
        {
            if (Physical_Energy - amount >= 0)
            {
                // Physical_Energy -= amount;
                Resource_Amount_Flash("Physical_Energy", -amount);
                
                AnimateResourceChange(false, position, "Physical_Energy", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }

    // 增加Spirit资源，仅在增加资源时调用，因此输入的 amount 需要为正值                    + Spirit
    // 调用需传入当前物体 location，以追踪来源
        public void Add_Spirit(int amount, Vector3 position)
        {
            // Spirit += amount;      // 在 Animate Resource Change 结束后，再增加
            
            AnimateResourceChange(true, position, "Spirit", amount); // 调用动画效果
        }

    // 减少Spirit资源，仅在减少资源时调用，因此外部需做好 加减判断                    - Spirit
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Spirit(int amount, Vector3 position)
        {
            if (Spirit - amount >= 0)
            {
                // Spirit -= amount;
                Resource_Amount_Flash("Spirit", -amount);
                
                AnimateResourceChange(false, position, "Spirit", amount); // 调用动画效果
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
            // Soul += amount;      // 在 Animate Resource Change 结束后，再增加
            
            AnimateResourceChange(true, position, "Soul", amount); // 调用动画效果
        }

    // 减少Soul资源，仅在减少资源时调用，因此外部需做好 加减判断                  - Soul
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Soul(int amount, Vector3 position)
        {
            if (Soul - amount >= 0)
            {
                // Soul -= amount;
                Resource_Amount_Flash("Soul", -amount);
                
                AnimateResourceChange(false, position, "Soul", amount); // 调用动画效果
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
            // Spirituality_Infused_Material += amount;      // 在 Animate Resource Change 结束后，再增加
            
            AnimateResourceChange(true, position, "Spirituality_Infused_Material", amount); // 调用动画效果
        }

    // 减少Spirituality_Infused_Material资源，仅在减少资源时调用，因此外部需做好 加减判断     - Spirituality_Infused_Material
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Spirituality_Infused_Material(int amount, Vector3 position)
        {
            if (Spirituality_Infused_Material - amount >= 0)
            {
                // Spirituality_Infused_Material -= amount;
                Resource_Amount_Flash("Spirituality_Infused_Material", -amount);
                
                AnimateResourceChange(false, position, "Spirituality_Infused_Material", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }
        
    // 增加Knowledge资源，仅在增加资源时调用，因此外部需做好 加减判断                 + Knowledge
    // 调用需传入当前物体 location，以追踪来源
        public void Add_Knowledge(int amount, Vector3 position)
        {
            // Knowledge += amount;      // 在 Animate Resource Change 结束后，再增加
            
            AnimateResourceChange(true, position, "Knowledge", amount); // 调用动画效果
        }

    // 减少Knowledge资源，仅在减少资源时调用，因此外部需做好 加减判断                     - Knowledge
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Knowledge(int amount, Vector3 position)
        {
            if (Knowledge - amount >= 0)
            {
                // Knowledge -= amount;
                Resource_Amount_Flash("Knowledge", -amount);
                
                AnimateResourceChange(false, position, "Knowledge", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }

    // 增加Belief资源，仅在增加资源时调用，因此外部需做好 加减判断                + Belief
    // 调用需传入当前物体 location，以追踪来源
        public void Add_Belief(int amount, Vector3 position)
        {
            // Belief += amount;      // 在 Animate Resource Change 结束后，再增加
            
            AnimateResourceChange(true, position, "Belief", amount); // 调用动画效果
        }

    // 减少Belief资源，仅在减少资源时调用，因此外部需做好 加减判断                    - Belief
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Belief(int amount, Vector3 position)
        {
            if (Belief - amount >= 0)
            {
                // Belief -= amount;
                Resource_Amount_Flash("Belief", -amount);
                
                AnimateResourceChange(false, position, "Belief", amount); // 调用动画效果
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
            // Putrefaction += amount;      // 在 Animate Resource Change 结束后，再增加
            
            AnimateResourceChange(true, position, "Putrefaction", amount); // 调用动画效果
        }

    // 减少Putrefaction资源，仅在减少资源时调用，因此外部需做好 加减判断              - Putrefaction
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Putrefaction(int amount, Vector3 position)
        {
            if (Putrefaction - amount >= 0)
            {
                // Putrefaction -= amount;
                Resource_Amount_Flash("Putrefaction", -amount);
                
                AnimateResourceChange(false, position, "Putrefaction", amount); // 调用动画效果
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
            // Madness += amount;      // 在 Animate Resource Change 结束后，再增加
            
            AnimateResourceChange(true, position, "Madness", amount); // 调用动画效果
        }

    // 减少Madness资源，仅在减少资源时调用，因此外部需做好 加减判断                   - Madness
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Madness(int amount, Vector3 position)
        {
            if (Madness - amount >= 0)
            {
                // Madness -= amount;
                Resource_Amount_Flash("Madness", -amount);
                
                AnimateResourceChange(false, position, "Madness", amount); // 调用动画效果
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
            // Godhood += amount;      // 在 Animate Resource Change 结束后，再增加
            
            AnimateResourceChange(true, position, "Godhood", amount); // 调用动画效果
        }

    // 减少Godhood资源，仅在减少资源时调用，因此外部需做好 加减判断                   - Godhood
    // 调用需传入当前物体 location，以追踪来源
        public void Reduce_Godhood(int amount, Vector3 position)
        {
            if (Godhood - amount >= 0)
            {
                // Godhood -= amount;
                Resource_Amount_Flash("Godhood", -amount);
                
                AnimateResourceChange(false, position, "Godhood", amount); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }

    
        
    

    // 动画效果，游戏表现，生产或消耗资源的执行
    
    private void AnimateResourceChange(bool isAdding, Vector3 position, string resourceName, int animatedAmount)
    {

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
        if (resourceName == "Spirit")
        {
            iconInstance = Instantiate(spirit_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            
            if (isAdding)     // 如果是增加资源，则用 First Time Bool Passer 获取布尔值，判断是否是第一次，然后将 ever appears 设为 true（即出现过了）
            {
                FirstTimeBoolPasser = is_spirit_ever_appears;
                is_spirit_ever_appears = true;
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
        if (resourceName == "Knowledge")
        {
            iconInstance = Instantiate(knowledge_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            
            if (isAdding)     // 如果是增加资源，则用 First Time Bool Passer 获取布尔值，判断是否是第一次，然后将 ever appears 设为 true（即出现过了）
            {
                FirstTimeBoolPasser = is_knowledge_ever_appears;
                is_knowledge_ever_appears = true;
            }
        }
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

   
        
        if (isAdding)   // 增加资源时
        {
            if (!FirstTimeBoolPasser)     // 如果是第一次
            {
                int locationIndex = FindAvailableResourceLocation();      // 找到空的 resource_location_X 的 slot，获取到空 slot 的序号 X
                resourceLocationsOccupied[locationIndex] = true;        // 相应的 resource_location_X slot 被占领
                resourceLocationIndex[resourceName] = locationIndex;    // 记录资源对应的 resource_location_X 的 X 序号
                
                Vector3 targetPosition = GetResourceLocationPosition(locationIndex);    // 获取到 resource_location_X 的位置
                iconInstance.transform.DOMove(targetPosition, 2f).OnComplete(() => // 移动完成后，添加 collider 和脚本组件
                    {
                        // 添加脚本组件和 collider
                        iconInstance.AddComponent<Resource_Click_Message>();
                        iconInstance.GetComponent<Resource_Click_Message>().messageId = resourceName;
                        CircleCollider2D circleCollider = iconInstance.AddComponent<CircleCollider2D>();
                        circleCollider.isTrigger = true;
                        
                        // 出现 number（暂定不出现资源名称先 2023-12-10）
                        // 更新 2个 Dictionary 的记录，来让 Update 中 "更新各 slot 中资源数量" 的 TMP_text 投入工作
                        slotAbleToShowText[locationIndex] = true;       // 开锁，编号为 X 的槽位可以开始显示 资源数量的 text 了
                        Resource_Amount_Flash(resourceName, animatedAmount);
                    }
                );
                
                
            }
            else        // 如果不是第一次了
            {
                Vector3 targetPosition = GetResourceLocationPosition(resourceLocationIndex[resourceName]);
                iconInstance.transform.DOMove(targetPosition, 1f).OnComplete(() =>
                    {
                        Destroy(iconInstance);
                        Resource_Amount_Flash(resourceName, animatedAmount);
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
                iconInstance.transform.position = GetResourceLocationPosition(resourceLocationIndex[resourceName]);
                iconInstance.transform.DOMove(position, 1f).OnComplete(() =>
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
                    amount_text.text = Fund.ToString();
                }
                if (resourceName == "Physical_Energy")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Physical_Energy.ToString();
                }
                if (resourceName == "Spirit")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Spirit.ToString();
                }
                if (resourceName == "Soul")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Soul.ToString();
                }
                if (resourceName == "Spirituality_Infused_Material")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Spirituality_Infused_Material.ToString();
                }
                if (resourceName == "Knowledge")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Knowledge.ToString();
                }
                if (resourceName == "Belief")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Belief.ToString();
                }
                if (resourceName == "Putrefaction")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Putrefaction.ToString();
                }
                if (resourceName == "Madness")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Madness.ToString();
                }
                if (resourceName == "Godhood")
                {
                    TMP_Text amount_text = GameObject.Find("Resource_Amount_Text_" + slot.Key).GetComponent<TMP_Text>();
                    amount_text.text = Godhood.ToString();
                }
                
            }
        }
        
    }


    private void Resource_Amount_Flash(string flash_resource, int flash_amount)
    {
        StartCoroutine(Number_Flash(flash_resource, flash_amount));
    }

    private IEnumerator Number_Flash(string flash_resource, int flash_amount)
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
                float finalAmount = Fund - flash_amount;
                while (Fund > finalAmount)
                {
                    Fund--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Physical_Energy")
            {
                float finalAmount = Physical_Energy - flash_amount;
                while (Physical_Energy > finalAmount)
                {
                    Physical_Energy--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Spirit")
            {
                float finalAmount = Spirit - flash_amount;
                while (Spirit > finalAmount)
                {
                    Spirit--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Soul")
            {
                float finalAmount = Soul - flash_amount;
                while (Soul > finalAmount)
                {
                    Soul--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Spirituality_Infused_Material")
            {
                float finalAmount = Spirituality_Infused_Material - flash_amount;
                while (Spirituality_Infused_Material > finalAmount)
                {
                    Spirituality_Infused_Material--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Knowledge")
            {
                float finalAmount = Knowledge - flash_amount;
                while (Knowledge > finalAmount)
                {
                    Knowledge--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Belief")
            {
                float finalAmount = Belief - flash_amount;
                while (Belief > finalAmount)
                {
                    Belief--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Putrefaction")
            {
                float finalAmount = Putrefaction - flash_amount;
                while (Putrefaction > finalAmount)
                {
                    Putrefaction--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Madness")
            {
                float finalAmount = Madness - flash_amount;
                while (Madness > finalAmount)
                {
                    Madness--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
            if (flash_resource == "Godhood")
            {
                float finalAmount = Godhood - flash_amount;
                while (Godhood > finalAmount)
                {
                    Godhood--;
                    yield return new WaitForSeconds(flash_one_time);
                }
            }
        }
        
    }
    
    
    
    
    
    
    
    
    
    
    
}

