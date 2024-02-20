using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    

    
    

    /////////////////////////////////////////////////////////////////////////////////////////

    
    public static GameManager GM;                   // 方便在其他脚本 调用 Game Manager 中方法和变量的 static 自身指代
    
    // 一些其他 Manager
    public Card_Loader CardLoader;                  // 处理 从 JSON 文件到卡牌类结构的操作的脚本，提供一系列方法来获取卡牌
    public Resource_Manager ResourceManager;        // 资源管理脚本 Resource Manager
    public Body_Part_Manager BodyPartManager;       // body part 管理脚本
    public Panel_Manager PanelManager;              // panel 管理 manager
    public Input_Manager InputManager;              // Input Manager
    public Card_Effects CardEffects;                // 卡牌效果 功能集成

    // 卡牌 prefab
    public GameObject message_Panel;                // 左下角 message panel 的 prefab
    public GameObject Card_Location_Prefab;         // Card_Location 的 prefab
    public GameObject Card_Body_Part_Prefab;        // Card_Body_Part 的 prefab


    private void Awake()
    {
        SetGameManager();
    }

    private void Start()
    {
        SetCardLoader();
    }
    
    
    /////////////////////////////////////////////////////////////////////////////////////////   Initial Set up
    
    void SetGameManager()
    {
        GM = this;
    }

    void SetCardLoader()
    {
        if (CardLoader == null)
        {
            CardLoader = GameObject.Find("Card_Loader").GetComponent<Card_Loader>();
        }
    }


    public void Generate_Card_Location(string id, Vector3 position)   // 实例化 Card_Location， 根据 id 从 Card_Loader 的卡牌 list 中找到卡牌实例，并赋予生成的卡牌 prefab
    {
        GameObject cardLocation = Instantiate(Card_Location_Prefab, position, Quaternion.identity);    
        cardLocation.GetComponent<Card_Location_Feature>()._cardLocation = CardLoader.Get_Card_Location_By_Id(id); 
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



























}
