using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    
    // 玩家状态 Player Status
    public int Current_Rank;            // 玩家当前处于序列几 Sequence X
    public string Current_Occupation;       // 玩家当前的职业名称 Occupation
    
    
    // 玩家拥有的卡牌 Player's Cards
    public List<string> Player_Owned_Card_Location_List;


    // 一些计数器 Counters
    public int Draw_New_Card_Location_Times;     // 抽取新 Card Location 卡的次数，用于 Lens 的计数
    
    

    /////////////////////////////////////////////////////////////////////////////////////////

    
    public static GameManager GM;                   // 方便在其他脚本 调用 Game Manager 中方法和变量的 static 自身指代
    
    // 一些其他 Manager
    public Card_Loader CardLoader;                  // 处理 从 JSON 文件到卡牌类结构的操作的脚本，提供一系列方法来获取卡牌
    public Resource_Manager ResourceManager;        // 资源管理脚本 Resource Manager
    public Body_Part_Manager BodyPartManager;       // body part 管理脚本
    public Panel_Manager PanelManager;              // panel 管理 manager
    public Input_Manager InputManager;              // Input Manager
    public Card_Effects CardEffects;                // 卡牌效果 功能集成
    public Event_Manager EventManager;              // 事件触发和管理 Manager

    // 卡牌 prefab
    public GameObject message_Panel;                // 左下角 message panel 的 prefab
    public GameObject Card_Location_Prefab;         // Card_Location 的 prefab
    public GameObject Card_Body_Part_Prefab;        // Card_Body_Part 的 prefab


    private void Awake()
    {
        Set_Game_Manager();     // 设置 GM 为 static 唯一 GameManager
    }

    private void Start()
    {
        Set_Card_Loader();                  // 指代 Card Loader
        Set_Player_Status();                // 设置玩家状态
        Set_Player_Owned_Cards();           // 设置玩家当前拥有的卡牌
        Set_Counters();                 // 设置各种计数器
    }
    
    
    /////////////////////////////////////////////////////////////////////////////////////////   Initial Set up
    
    // 设置 GM 为 static 唯一 GameManager
    void Set_Game_Manager()
    {
        GM = this;
    }

    // 指代 Card Loader
    void Set_Card_Loader()
    {
        if (CardLoader == null)
        {
            CardLoader = GameObject.Find("Card_Loader").GetComponent<Card_Loader>();
        }
    }

    // 设置玩家状态 Set Player Status
    void Set_Player_Status()        
    {

        Current_Rank = 10;                  // 临时
        Current_Occupation = "All";         // 临时


        ///// TODO 加上 其他玩家状态设置，以及保存和读取功能
    }


    // 设置玩家当前拥有的卡牌 Player Owned Cards
    void Set_Player_Owned_Cards()
    {
        Player_Owned_Card_Location_List = new List<string>() { };   // 临时

        ///// TODO 加上 保存和读取功能
    }

    
    // 设置各种计数器 Set Counters
    void Set_Counters()         
    {
        Draw_New_Card_Location_Times = 0;       // 临时



        ///// TODO 加上 其他计数器设置，以及保存和读取功能
    }
    
    
    
    /////////////////////////////////////////////////////////////////////////////////////////   Set up END
    


    public void Generate_Card_Location(string id, Vector3 position)   // 实例化 Card_Location， 根据 id 从 Card_Loader 的卡牌 list 中找到卡牌实例，并赋予生成的卡牌 prefab
    {
        GameObject cardLocation = Instantiate(Card_Location_Prefab, position, Quaternion.identity);    
        cardLocation.GetComponent<Card_Location_Feature>()._cardLocation = CardLoader.Get_Card_Location_By_Id(id); 
        
        // 将添加的 card location 的 Id string 记录到 Player Owned Card list 里面
        Player_Owned_Card_Location_List.Add(id);
    }
    
    public void Generate_Message(string id)     // 实例化 message，根据 id 从 Card_Loader 中的 message list 中找到 message 实例，并赋予生成的 message prefab
    {
        GameObject messagePanel = Instantiate(message_Panel,GameObject.Find("Canvas").transform);
        messagePanel.GetComponent<Message_Feature>()._message = CardLoader.Get_Message_By_Id(id);
    }

    public GameObject Generate_Card_Body_Part(string id)  // 实例化 Body_Part， 根据 id 从 Card_Loader 的卡牌 list 中找到卡牌实例，并赋予生成的卡牌 prefab
    {
        GameObject cardBodyPart = Instantiate(Card_Body_Part_Prefab, new Vector3(random.Range(-3,3),random.Range(-3,3),1), Quaternion.identity);
        cardBodyPart.GetComponent<Card_Body_Part_Feature>()._CardBodyPart = CardLoader.Get_Card_Body_Part_By_Id(id);
        return cardBodyPart;
    }


    
    // 获取 匹配当前 Rank 和 Occupation 的 Card Location 的 Id，返回 Id 的 string list
    public List<string> Get_Card_Location_Ids_Based_On_Rank_And_Occupation()        
    {
        List<string> Ids = new List<string>() { };

        foreach (var card_location_instance in CardLoader.Location_Card_List)
        {
            if(card_location_instance.Rank == Current_Rank &&               // 若 card location 实例的 rank 与 当前rank 相等
               card_location_instance.Occupation == Current_Occupation)     // 若 card location 实例的 occupation 与当前 occupation 相等
            {
                Ids.Add(card_location_instance.Id);
            }
        }

        return Ids;
    }

    // 获取 匹配当前 Rank 和 Occupation 的 随机一张 Card Location， 调用 Ids 方法，（有唯一性判定）
    public string Get_Random_Card_Location_Id_Based_On_Rank_And_Occupation()
    {
        List<string> list_of_id = Get_Card_Location_Ids_Based_On_Rank_And_Occupation();
        
        int index = random.Range(0, list_of_id.Count - 1);
        
        
        // 判定新抽的卡牌是不是 已经拥有的并且是 Only 属性的卡牌，是则重新抽取
        {
            bool if_contains_only_card = false;    // 用于在一次判定中检测是否有 only属性 且存在的卡牌，true表示有，false表示没有
            bool check_only_passer = false;         // 用于最终确定 id string 没有重复之后的 放行 bool 参数

            while (true)
            {
                if (check_only_passer)
                {
                    break;
                }
            
                while (true)
                {
                    if_contains_only_card = false;      // 重置判定参数
                
                    foreach (var card_location_id_string in Player_Owned_Card_Location_List)        // 卡牌唯一性 Only 判定
                    {
                        if (card_location_id_string == list_of_id[index] &&                 // 若已经有了同名称卡牌
                            CardLoader.Get_Card_Location_By_Id(card_location_id_string).Only)   // 且此卡是 Only 唯一的
                        {
                            if_contains_only_card = true;
                            break;
                        }
                    }

                    if (!if_contains_only_card)         // 如果全程没检测到 only card，则判定通过，break
                    {
                        check_only_passer = true;
                        break;
                    }
                    else
                    {
                        index = random.Range(0, list_of_id.Count - 1);      // 重新抽一张
                    }
                
                }
            
            
            }
        }
        
        
        return list_of_id[index];

    }


























}
