using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card_Body_Part
{
    public string Id;
    public string Label;
    public string Image;
    public string Produce_Message;
}

[System.Serializable]
public class Card_Body_Part_Wrapper
{
    public List<Card_Body_Part> List_Card_Body_Part;
}

