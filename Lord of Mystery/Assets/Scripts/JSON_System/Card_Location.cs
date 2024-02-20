using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card_Location
{
    public string Id;
    public string Label;
    public string Image;
    public string Description;

    public int Rank;
    public string Occupation;

    public bool Burn;
    public int Use_Time;
    public int Counter;
    public bool Only;
    public int Time;
    
    // 临时，用于 prototype，模拟其他卡功能
    public bool Auto_Start;
    public bool Moveable;
    public bool Repeatable;

    public Body_Part_Data Require_Body_Part;
    public Resource_Data Require_Resource;
    public Resource_Data Produce_Resource;

    public List<string> Produce_Message;
    public List<string> Produce_Card_Automatic;
    public List<string> Produce_Card_Action;
    public List<string> Produce_Card_Event;
    public List<string> Produce_Card_Location;
    public List<string> Produce_Special_Effect;
}


// Card_Location 的包装类，便于用 JsonUtility 反序列化
[System.Serializable]
public class Card_Location_Wrapper
{
    public List<Card_Location> List_Card_Location;
}


