using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Card_Automatic
{
    public string Id;
    public string Label;
    public string Image;
    public string Description;

    public int Rank;
    public string Occupation;

    public bool Burn;
    public int Use_Time;
    public bool Only;
    public int Time;

    public Resource_Data Produce_Resource;

    public List<string> Produce_Message;
    public List<string> Produce_Card_Automatic;
    public List<string> Produce_Card_Action;
    public List<string> Produce_Card_Event;
    public List<string> Produce_Card_Location;

    
    // 构造函数，用于从JSON数据初始化
    /*public Card_Automatic(/* 参数来自JSON对象 #1#)
    {
        // 初始化逻辑
    }*/

    // 可以添加更多方法处理卡牌逻辑
}


[System.Serializable]
public class Card_Automatic_Wrapper
{
    public List<Card_Automatic> List_Card_Automatic;
}