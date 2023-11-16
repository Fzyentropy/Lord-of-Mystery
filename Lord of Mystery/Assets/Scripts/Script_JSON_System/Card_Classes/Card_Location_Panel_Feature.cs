using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card_Location_Panel_Feature : MonoBehaviour
{

    public GameObject attached_card;     // 用于指代生成此 panel 的卡牌
    public Card_Location_Feature card_location_feature_in_attached_card;   // 生成此 panel 的卡牌的 card_location_feature 脚本
    public GameObject start_button;      // 此 panel 上的 start button   

    public bool is_mouse_hover_on_panel;
    
    // panel 外观设置
    public SpriteRenderer panel_image;   
    public TMP_Text panel_label;
    public TMP_Text panel_description;


    private void Start()
    {
        Set_Start_Button();
    }


    /////////////////     设置函数

    public void Set_Attached_Card(GameObject attachedCard)     // 设置生成此 panel 的卡牌指代，顺便设置 feature 指代，从外部设置
    {
        attached_card = attachedCard;
        card_location_feature_in_attached_card = attached_card.GetComponent<Card_Location_Feature>();
    }

    void Set_Start_Button()
    {
        if (start_button == null)
        {
            start_button = GameObject.Find("Start_Button");
        }

        if (start_button.GetComponent<Start_Button_Script>().attached_panel == null)    // 设置按钮脚本对 panel 的指代
        {
            start_button.GetComponent<Start_Button_Script>().Set_Attached_Panel(gameObject);
        }
          
        
        start_button.GetComponent<Start_Button_Script>().Set_Button_Availability(true); // 让按钮可点击，test用
    }

    public void Set_Sprite(Sprite sprite)
    {
        panel_image.sprite = sprite;
    }

    public void Set_Label(string label)
    {
        panel_label.text = label;
    }

    public void Set_Description(string description)
    {
        panel_description.text = description;
    }


    
    
    




}
