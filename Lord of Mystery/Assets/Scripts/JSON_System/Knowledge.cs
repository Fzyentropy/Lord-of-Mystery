using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Knowledge
{
    public string Id;
    public string Short_Label;
    public string Label;
    public string Image;
    public string Knowledge_Content;
}


[System.Serializable]
public class Knowledge_Wrapper
{
    public List<Knowledge> List_Knowledge;
}