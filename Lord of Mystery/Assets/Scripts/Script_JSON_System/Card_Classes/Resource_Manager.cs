using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using DG.Tweening; 

public class Resource_Manager : MonoBehaviour
{
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
    
    
    // 资源位置标记和占用情况
    private Dictionary<int, bool> resourceLocationsOccupied = new Dictionary<int, bool>();
    private Dictionary<string, int> resourceLocationIndex = new Dictionary<string, int>();

    public GameObject fund_icon_pure; // Fund资源的图标Prefab
    public GameObject physical_energy_icon_pure; // Physical_Energy资源的图标Prefab
    public GameObject spirit_icon_pure; // Spirit资源的图标Prefab
    public GameObject soul_icon_pure; // Soul资源的图标Prefab
    public GameObject spirituality_infused_material_icon_pure; // Spirituality_Infused_Material资源的图标Prefab
    public GameObject knowledge_icon_pure; // Knowledge资源的图标Prefab
    public GameObject belief_icon_pure; // Belief资源的图标Prefab
    public GameObject putrefaction_icon_pure; // Putrefaction资源的图标Prefab
    public GameObject madness_icon_pure; // Madness资源的图标Prefab
    public GameObject godhood_icon_pure; // Godhood资源的图标Prefab

    



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


    private bool AppearBoolPasser = false;
    
    private GameObject iconInstance;
    

    // 在Start中初始化资源位置占用情况
    void Start()
    {
        for (int i = 1; i <= 10; i++)
        {
            resourceLocationsOccupied[i] = false;
        }

        resourceLocationIndex = new Dictionary<string, int>
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


        // 加载或设置fundIconPrefab
        // fundIconPrefab = ...
    }
    
    
    

    
    // 增加Fund资源，仅在增加资源时调用，因此外部需做好 加减判断
    // 调用需传入当前物体 location
    public void Add_Fund(int amount, Vector3 position)     
    {
        Fund += amount;
        AnimateResourceChange(true, position, "Fund");         // 调用动画效果
    }

    // 减少Fund资源，仅在减少资源时调用，因此外部需做好 加减判断
    // 调用需传入当前物体 location
    public void Reduce_Fund(int amount, Vector3 position)   
    {
        if (Fund - amount >= 0)
        {
            Fund -= amount;
            // 调用动画效果
            AnimateResourceChange(false, position, "Fund");
        }
        else
        {
            // 资源不足，特殊效果触发
            // TODO: 特殊效果的实现
        }
    }
    
        // 增加Physical_Energy资源，仅在增加资源时调用，因此外部需做好 加减判断
        // 调用需传入当前物体 location
        public void Add_Physical_Energy(int amount, Vector3 position)
        {
            Physical_Energy += amount;
            AnimateResourceChange(true, position, "Physical_Energy"); // 调用动画效果
        }

        // 减少Physical_Energy资源，仅在减少资源时调用，因此外部需做好 加减判断
        // 调用需传入当前物体 location
        public void Reduce_Physical_Energy(int amount, Vector3 position)
        {
            if (Physical_Energy - amount >= 0)
            {
                Physical_Energy -= amount;
                AnimateResourceChange(false, position, "Physical_Energy"); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }

    // 增加Spirit资源，仅在增加资源时调用，因此外部需做好 加减判断
    // 调用需传入当前物体 location
        public void Add_Spirit(int amount, Vector3 position)
        {
            Spirit += amount;
            AnimateResourceChange(true, position, "Spirit"); // 调用动画效果
        }

    // 减少Spirit资源，仅在减少资源时调用，因此外部需做好 加减判断
    // 调用需传入当前物体 location
        public void Reduce_Spirit(int amount, Vector3 position)
        {
            if (Spirit - amount >= 0)
            {
                Spirit -= amount;
                AnimateResourceChange(false, position, "Spirit"); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }

        
    // 增加Soul资源，仅在增加资源时调用，因此外部需做好 加减判断
    // 调用需传入当前物体 location
        public void Add_Soul(int amount, Vector3 position)
        {
            Soul += amount;
            AnimateResourceChange(true, position, "Soul"); // 调用动画效果
        }

    // 减少Soul资源，仅在减少资源时调用，因此外部需做好 加减判断
    // 调用需传入当前物体 location
        public void Reduce_Soul(int amount, Vector3 position)
        {
            if (Soul - amount >= 0)
            {
                Soul -= amount;
                AnimateResourceChange(false, position, "Soul"); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }

    // 增加Spirituality_Infused_Material资源，仅在增加资源时调用，因此外部需做好 加减判断
    // 调用需传入当前物体 location
        public void Add_Spirituality_Infused_Material(int amount, Vector3 position)
        {
            Spirituality_Infused_Material += amount;
            AnimateResourceChange(true, position, "Spirituality_Infused_Material"); // 调用动画效果
        }

    // 减少Spirituality_Infused_Material资源，仅在减少资源时调用，因此外部需做好 加减判断
    // 调用需传入当前物体 location
        public void Reduce_Spirituality_Infused_Material(int amount, Vector3 position)
        {
            if (Spirituality_Infused_Material - amount >= 0)
            {
                Spirituality_Infused_Material -= amount;
                AnimateResourceChange(false, position, "Spirituality_Infused_Material"); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }
        
        // 增加Knowledge资源，仅在增加资源时调用，因此外部需做好 加减判断
// 调用需传入当前物体 location
        public void Add_Knowledge(int amount, Vector3 position)
        {
            Knowledge += amount;
            AnimateResourceChange(true, position, "Knowledge"); // 调用动画效果
        }

// 减少Knowledge资源，仅在减少资源时调用，因此外部需做好 加减判断
// 调用需传入当前物体 location
        public void Reduce_Knowledge(int amount, Vector3 position)
        {
            if (Knowledge - amount >= 0)
            {
                Knowledge -= amount;
                AnimateResourceChange(false, position, "Knowledge"); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }

        // 增加Belief资源，仅在增加资源时调用，因此外部需做好 加减判断
// 调用需传入当前物体 location
        public void Add_Belief(int amount, Vector3 position)
        {
            Belief += amount;
            AnimateResourceChange(true, position, "Belief"); // 调用动画效果
        }

// 减少Belief资源，仅在减少资源时调用，因此外部需做好 加减判断
// 调用需传入当前物体 location
        public void Reduce_Belief(int amount, Vector3 position)
        {
            if (Belief - amount >= 0)
            {
                Belief -= amount;
                AnimateResourceChange(false, position, "Belief"); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }


        // 增加Putrefaction资源，仅在增加资源时调用，因此外部需做好 加减判断
// 调用需传入当前物体 location
        public void Add_Putrefaction(int amount, Vector3 position)
        {
            Putrefaction += amount;
            AnimateResourceChange(true, position, "Putrefaction"); // 调用动画效果
        }

// 减少Putrefaction资源，仅在减少资源时调用，因此外部需做好 加减判断
// 调用需传入当前物体 location
        public void Reduce_Putrefaction(int amount, Vector3 position)
        {
            if (Putrefaction - amount >= 0)
            {
                Putrefaction -= amount;
                AnimateResourceChange(false, position, "Putrefaction"); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }
        
        
        // 增加Madness资源，仅在增加资源时调用，因此外部需做好 加减判断
// 调用需传入当前物体 location
        public void Add_Madness(int amount, Vector3 position)
        {
            Madness += amount;
            AnimateResourceChange(true, position, "Madness"); // 调用动画效果
        }

// 减少Madness资源，仅在减少资源时调用，因此外部需做好 加减判断
// 调用需传入当前物体 location
        public void Reduce_Madness(int amount, Vector3 position)
        {
            if (Madness - amount >= 0)
            {
                Madness -= amount;
                AnimateResourceChange(false, position, "Madness"); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }

// 增加Godhood资源，仅在增加资源时调用，因此外部需做好 加减判断
// 调用需传入当前物体 location
        public void Add_Godhood(int amount, Vector3 position)
        {
            Godhood += amount;
            AnimateResourceChange(true, position, "Godhood"); // 调用动画效果
        }

// 减少Godhood资源，仅在减少资源时调用，因此外部需做好 加减判断
// 调用需传入当前物体 location
        public void Reduce_Godhood(int amount, Vector3 position)
        {
            if (Godhood - amount >= 0)
            {
                Godhood -= amount;
                AnimateResourceChange(false, position, "Godhood"); // 调用动画效果
            }
            else
            {
                // 资源不足，特殊效果触发
                // TODO: 特殊效果的实现
            }
        }

        
        
        
        
    
    
    
    

    // 动画效果
    private void AnimateResourceChange(bool isAdding, Vector3 position, string resourceName)
    {

        if (resourceName == "Fund")
        {
            iconInstance = Instantiate(fund_icon_pure, position, Quaternion.identity);    // 在调用物体位置生成 icon
            AppearBoolPasser = is_fund_ever_appears;
        }
        if (resourceName == "Physical_Energy")
        {
            iconInstance = Instantiate(physical_energy_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            AppearBoolPasser = is_physical_energy_ever_appears;
        }
        if (resourceName == "Spirit")
        {
            iconInstance = Instantiate(spirit_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            AppearBoolPasser = is_spirit_ever_appears;
        }
        if (resourceName == "Soul")
        {
            iconInstance = Instantiate(soul_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            AppearBoolPasser = is_soul_ever_appears;
        }
        if (resourceName == "Spirituality_Infused_Material")
        {
            iconInstance = Instantiate(spirituality_infused_material_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            AppearBoolPasser = is_spirituality_infused_material_ever_appears;
        }
        if (resourceName == "Knowledge")
        {
            iconInstance = Instantiate(knowledge_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            AppearBoolPasser = is_knowledge_ever_appears;
        }
        if (resourceName == "Belief")
        {
            iconInstance = Instantiate(belief_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            AppearBoolPasser = is_belief_ever_appears;
        }
        if (resourceName == "Putrefaction")
        {
            iconInstance = Instantiate(putrefaction_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            AppearBoolPasser = is_putrefaction_ever_appears;
        }
        if (resourceName == "Madness")
        {
            iconInstance = Instantiate(madness_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            AppearBoolPasser = is_madness_ever_appears;
        }
        if (resourceName == "Godhood")
        {
            iconInstance = Instantiate(godhood_icon_pure, position, Quaternion.identity); // 在调用物体位置生成 icon
            AppearBoolPasser = is_godhood_ever_appears;
        }

   
        
        if (isAdding)   // 增加资源时
        {
            if (!AppearBoolPasser)     // 如果是第一次
            {
                int locationIndex = FindAvailableResourceLocation();      // 找到空的 resource_location_X 的 slot，获取到空 slot 的序号 X
                Vector3 targetPosition = GetResourceLocationPosition(locationIndex);    // 获取到 resource_location_X 的位置
                iconInstance.transform.DOMove(targetPosition, 2f).OnComplete(() => // 移动完成后，添加 collider 和脚本组件

                    // 添加 collider 和脚本组件
                    iconInstance.AddComponent<Resource_Click_Message>()
                    // ,iconInstance.GetComponent<Resource_Click_Message>().messageId = resourceName

                    );
                resourceLocationsOccupied[locationIndex] = true;        // 相应的 resource_location_X slot 被占领
                resourceLocationIndex[resourceName] = locationIndex;    // 记录资源对应的 resource_location_X 的 X 序号
            }
            else        // 如果不是第一次了
            {
                Vector3 targetPosition = GetResourceLocationPosition(resourceLocationIndex[resourceName]);
                iconInstance.transform.DOMove(targetPosition, 2f).OnComplete(() =>
                    Destroy(iconInstance));
                
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
                iconInstance.transform.DOMove(position, 2f).OnComplete(() =>
                    Destroy(iconInstance));
            }
        }
        
        if (resourceName == "Fund")
        {
            is_fund_ever_appears = true;
        }
        if (resourceName == "Physical_Energy")
        {
            is_physical_energy_ever_appears = true;
        }
        if (resourceName == "Spirit")
        {
            is_spirit_ever_appears = true;
        }
        if (resourceName == "Soul")
        {
            is_soul_ever_appears = true;
        }
        if (resourceName == "Spirituality_Infused_Material")
        {
            is_spirituality_infused_material_ever_appears = true;
        }
        if (resourceName == "Knowledge")
        {
            is_knowledge_ever_appears = true;
        }
        if (resourceName == "Belief")
        {
            is_belief_ever_appears = true;
        }
        if (resourceName == "Putrefaction")
        {
            is_putrefaction_ever_appears = true;
        }
        if (resourceName == "Madness")
        {
            is_madness_ever_appears = true;
        }
        if (resourceName == "Godhood")
        {
            is_godhood_ever_appears = true;
        }



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
}

