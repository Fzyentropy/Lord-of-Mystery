using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Sequence
{
    public string Id;
    public string Label;
    public string Image;
    public string Description;

    public string Corresponding_Potion_Message_Id;      // 对应的 Potion card 点击生成的 message 的 id
    
    public int Rank;
    public string Occupation;
    
    public Resource_Data Require_Resource;

    public int Time;

    public int Death_Expansion_Amount;
    
    public int Counter;

    public Resource_Data Produce_Resource;

    public List<string> Produce_Card_Location;
    public List<string> Produce_Special_Effect;
}


[System.Serializable]
public class Sequence_Wrapper
{
    public List<Sequence> List_Sequence;
}