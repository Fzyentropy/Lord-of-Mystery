using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource_Click_Message : MonoBehaviour
{
    public string messageId = "Born";
    
    private void OnMouseDown()
    {
        GameManager.GM.Generate_Message(messageId);
    }
    
}
