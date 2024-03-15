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
    
    public int Rank;
    public string Occupation;

    public int Time;
    public int Counter;
    
    public Resource_Data Require_Resource;
    public Resource_Data Produce_Resource;

    public List<string> Produce_Card_Location;
    public List<string> Produce_Special_Effect;
}


[System.Serializable]
public class Sequence_Wrapper
{
    public List<Sequence> List_Sequence;
}