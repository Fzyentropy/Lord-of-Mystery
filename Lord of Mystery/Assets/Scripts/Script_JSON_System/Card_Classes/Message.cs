using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Message
{
    public string Id;
    public string Label;
    public string Image;
    public string Message_Content;

}


// Message 的包装类，便于用 JsonUtility 反序列化
[System.Serializable]
public class Message_Wrapper
{
    public List<Message> List_Message;
}