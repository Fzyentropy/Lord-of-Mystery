using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Card_Body_Part_Feature : MonoBehaviour
{

    public Card_Body_Part _CardBodyPart;

    public SpriteRenderer body_part_image;
    public TMP_Text body_part_label;

    void Start()
    {
        Initialize_Body_Part();
    }

    private void OnMouseUp()
    {
        // SpriteRenderer.
    }

    void Initialize_Body_Part()
    {
        body_part_label.text = _CardBodyPart.Label;
        body_part_image.sprite = Resources.Load<Sprite>("Image/" + _CardBodyPart.Image);
    }

    private void OnMouseDown()        ////    2023-11-15：不能使用此方法，因为拖拽时也会触发这个功能
    {
        GameManager.GM.Generate_Message(_CardBodyPart.Produce_Message);
        
        
    }
}
