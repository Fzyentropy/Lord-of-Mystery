using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Message_Feature : MonoBehaviour
{
    public Message _message;                   // message 卡牌的实例，用 GameManager 中的Generate Message方法赋予
    
    public TMP_Text message_label;
    public Image message_image;
    public TMP_Text message_content;


    private void Start()
    {
        if (_message == null)
        {
            throw new NotImplementedException();
        }

        message_label.text = _message.Label;
        message_image.sprite = Resources.Load<Sprite>("Image/" + _message.Image);
        message_content.text = _message.Message_Content;

    }
    
}
