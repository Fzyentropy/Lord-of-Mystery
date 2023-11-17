using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public static int Number_SoulPiece;

    public static int Amount_Fund;
    public static int Amount_PhysicalStrength;
    public static int Amount_Spirit;
    public static int Amount_Soul;
    public static int Amount_SpiritualityInfusedMaterial;
    public static int Amount_Knowledge;
    
    public static int Amount_Belief;
    public static int Amount_Madness;
    public static int Amount_Putrefaction;
    public static int Amount_Godhood;

    
    

    /////////////////////////////////////////////////////////////////////////////////////////

    
    public static GameManager GM;                   // 方便在其他脚本 调用 Game Manager 中方法和变量的 static 自身指代
    
    // 一些其他 Manager
    public Card_Loader CardLoader;                  // 处理 从 JSON 文件到卡牌类结构的操作的脚本，提供一系列方法来获取卡牌
    public Resource_Manager ResourceManager;        // 资源管理脚本 Resource Manager

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


    public void Generate_Card_Location(string id)
    {
        GameObject cardLocation = Instantiate(Card_Location_Prefab, new Vector3(15,-10,0), Quaternion.identity);
        cardLocation.GetComponent<Card_Location_Feature>()._cardLocation = CardLoader.Get_Card_Location_By_Id(id);
    }
    
    public void Generate_Message(string id)
    {
        GameObject messagePanel = Instantiate(message_Panel,GameObject.Find("Canvas").transform);
        messagePanel.GetComponent<Message_Feature>()._message = CardLoader.Get_Message_By_Id(id);
    }

    public void Generate_Card_Body_Part(string id)
    {
        GameObject cardBodyPart = Instantiate(Card_Body_Part_Prefab, new Vector3(random.Range(-5,5),random.Range(-5,5),1), Quaternion.identity);
        cardBodyPart.GetComponent<Card_Body_Part_Feature>()._CardBodyPart = CardLoader.Get_Card_Body_Part_By_Id(id);
    }



























}
