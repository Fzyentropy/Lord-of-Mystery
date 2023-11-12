using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Card_Loader : MonoBehaviour
{

    public const string PATH_CARD_AUTOMATIC = "Cards/Card_Automatic";

    public List<Card_Automatic> Automatic_Card_List;

    void Start()
    {
        Load_All_Cards_From_JSON();
        print();
    }


    public void Load_All_Cards_From_JSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(PATH_CARD_AUTOMATIC);    // 从路径读取 JSON 文件
        string jsonData = jsonFile.text;    // 将 JSON 文件的文本数据存储在一个 string 参数 jsonData 中

        // 这个步骤可以将 JSON 中 "Card_Automatic" 对应的数组中的每张卡的数据放进 Wrapper中的 Card_Automatic 类型的 list 里面
        // Card_Automatic_Wrapper cardAutomaticWrapper = JsonConvert.DeserializeObject<Card_Automatic_Wrapper>(jsonData);
        Card_Automatic_Wrapper cardAutomaticWrapper = JsonUtility.FromJson<Card_Automatic_Wrapper>(jsonData);
        
        
        foreach (var card in cardAutomaticWrapper.List_Card_Automatic)
        {
            Automatic_Card_List.Add(card);
        }

    }

    void print()
    {
        foreach (var c in Automatic_Card_List)
            Debug.Log(
                c.Id + "\n"
                + c.Label + "\n"
                + c.Image + "\n"
                + c.Description + "\n"
                
                + c.Rank.ToString() + "\n"
                + c.Occupation + "\n"
                + c.Burn.ToString() + "\n"
                + c.Use_Time.ToString() + "\n"
                + c.Only.ToString() + "\n"
                + c.Time.ToString() + "\n"
            );
    }



}
