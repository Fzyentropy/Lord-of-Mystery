using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card_Location_Panel_Feature : MonoBehaviour
{

    public SpriteRenderer panel_image;
    public TMP_Text panel_label;
    public TMP_Text panel_description;






    /////////////////     设置函数
    
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
