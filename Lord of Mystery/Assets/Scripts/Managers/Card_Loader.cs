using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Card_Loader : MonoBehaviour
{

    public const string PATH_CARD_AUTOMATIC = "Cards/Card_Automatic";
    public const string PATH_CARD_LOCATION = "Cards/Card_Location";
    public const string PATH_MESSAGE = "Cards/Message";
    public const string PATH_CARD_BODY_PART = "Cards/Card_Body_Part";
    public const string PATH_SEQUENCE = "Cards/Sequence";

    [SerializeField] public List<Card_Automatic> Automatic_Card_List;
    [SerializeField] public List<Card_Location> Location_Card_List;
    [SerializeField] public List<Message> Message_List;
    [SerializeField] public List<Card_Body_Part> Body_Part_Card_List;
    [SerializeField] public List<Sequence> Sequence_List;

    void Start()
    {
        Load_All_Card_Automatic_From_JSON();
        Load_All_Card_Location_From_JSON();
        Load_All_Message_From_JSON();
        Load_All_Card_Body_Part_From_JSON();
        // Load_All_Sequence_From_JSON();
    }

    

    ////////////////////////////////////////////////////////////////////     加载 JSON 文件的脚本

    
    public void Load_All_Card_Automatic_From_JSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(PATH_CARD_AUTOMATIC);    // 从路径读取 JSON 文件
        string jsonData = jsonFile.text;    // 将 JSON 文件的文本数据存储在一个 string 参数 jsonData 中

        // 这个步骤可以将 JSON 中 "Card_Automatic" 对应的数组中的每张卡的数据放进 Wrapper中的 Card_Automatic 类型的 list 里面
        // 备选功能： JsonConvert.DeserializeObject<Card_Automatic_Wrapper>(jsonData);
        Card_Automatic_Wrapper cardAutomaticWrapper = JsonUtility.FromJson<Card_Automatic_Wrapper>(jsonData);

        foreach (var card_automatic in cardAutomaticWrapper.List_Card_Automatic)
        {
            Automatic_Card_List.Add(card_automatic);
        }
    }
    
    public void Load_All_Card_Location_From_JSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(PATH_CARD_LOCATION);    // 从路径读取 JSON 文件
        string jsonData = jsonFile.text;    // 将 JSON 文件的文本数据存储在一个 string 参数 jsonData 中

        // 这个步骤可以将 JSON 中 "Card_Location" 对应的数组中的每张卡的数据放进 Wrapper中的 Card_Location 类型的 list 里面
        Card_Location_Wrapper cardLocationWrapper = JsonUtility.FromJson<Card_Location_Wrapper>(jsonData);

        foreach (var card_location in cardLocationWrapper.List_Card_Location)
        {
            Location_Card_List.Add(card_location);
        }
    }
    
    public void Load_All_Message_From_JSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(PATH_MESSAGE);    // 从路径读取 JSON 文件
        string jsonData = jsonFile.text;    // 将 JSON 文件的文本数据存储在一个 string 参数 jsonData 中

        // 这个步骤可以将 JSON 中 "Card_Location" 对应的数组中的每张卡的数据放进 Wrapper中的 Card_Location 类型的 list 里面
        Message_Wrapper messageWrapper = JsonUtility.FromJson<Message_Wrapper>(jsonData);

        foreach (var message in messageWrapper.List_Message)
        {
            Message_List.Add(message);
        }
    }

    public void Load_All_Card_Body_Part_From_JSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(PATH_CARD_BODY_PART);    // 从路径读取 JSON 文件
        string jsonData = jsonFile.text;    // 将 JSON 文件的文本数据存储在一个 string 参数 jsonData 中
        
        // 这个步骤可以将 JSON 中 "Card_Location" 对应的数组中的每张卡的数据放进 Wrapper中的 Card_Location 类型的 list 里面
        Card_Body_Part_Wrapper bodyPartWrapper = JsonUtility.FromJson<Card_Body_Part_Wrapper>(jsonData);
        
        foreach (var card_body_part in bodyPartWrapper.List_Card_Body_Part)
        {
            Body_Part_Card_List.Add(card_body_part);
        }
    }

    /*public void Load_All_Sequence_From_JSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(PATH_SEQUENCE);    // 从路径读取 JSON 文件
        string jsonData = jsonFile.text;    // 将 JSON 文件的文本数据存储在一个 string 参数 jsonData 中
        
        // 这个步骤可以将 JSON 中 "Card_Location" 对应的数组中的每张卡的数据放进 Wrapper中的 Card_Location 类型的 list 里面
        Sequence_Wrapper sequenceWrapper = JsonUtility.FromJson<Sequence_Wrapper>(jsonData);

        foreach (var sequence in sequenceWrapper.List_Sequence)
        {
            Sequence_List.Add(sequence);
        }
    }*/
    
    
    ////////////////////////////////////////////////////////////////////     获取 list 中的卡牌对象 by ID


    public Card_Automatic Get_Card_Automatic_By_Id(string id)
    {
        return Automatic_Card_List.Find(card_automatic => card_automatic.Id == id);     // 根据 id 返回 Card_Automatic 实例
    }

    public Card_Location Get_Card_Location_By_Id(string id)
    {
        return Location_Card_List.Find(card_location => card_location.Id == id);     // 根据 id 返回 Card_Location 实例
    }
    
    public Message Get_Message_By_Id(string id)
    {
        return Message_List.Find(message => message.Id == id);     // 根据 id 返回 Message 实例
    }

    public Card_Body_Part Get_Card_Body_Part_By_Id(string id)
    {
        return Body_Part_Card_List.Find(card_body_part => card_body_part.Id == id);     // 根据 id 返回 Body_Part 实例
    }

    public Sequence Get_Sequence_By_Id(string id)
    {
        return Sequence_List.Find(sequence => sequence.Id == id);
    }
    
    
    ////////////////////////////////////////////////////////////////////     
    

    
    
    
    
    
    
    


}
