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
        Check_If_Message_Set_Well();
        Set_Message();
    }
    

    void Check_If_Message_Set_Well()
    {
        if (_message == null)
        {
            throw new NotImplementedException("Message 为空");
        }
    }
    

    void Set_Message()
    {
        message_label.text = _message.Label;
        message_image.sprite = Resources.Load<Sprite>("Image/" + _message.Image);
        message_content.text = _message.Message_Content;
        
        if (GameManager.currentLanguage == GameManager.Language.English)        // 设置语言
        {
            message_label.font = GameManager.Font_English;
            message_content.font = GameManager.Font_English;
            // card_label.fontSize = 8;
        }
        else if (GameManager.currentLanguage == GameManager.Language.Chinese)
        {
            message_label.font = GameManager.Font_Chinese;
            message_content.font = GameManager.Font_Chinese;

            message_content.fontSize = 17;
            message_content.characterSpacing = -2;
            message_content.lineSpacing = -20;
        }
        
    }
    
    
}
