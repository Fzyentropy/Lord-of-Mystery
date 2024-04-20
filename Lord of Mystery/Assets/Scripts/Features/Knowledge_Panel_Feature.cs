using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
// using UnityEngine.UIElements;
using UnityEngine.UI;

public class Knowledge_Panel_Feature : MonoBehaviour
{

    public Knowledge _Knowledge;

    public TMP_Text knowledge_label;
    public TMP_Text knowledge_content;
    public Image knowledge_image;

    
    private void Start()
    {
        Check_If_Knowledge_Set_Well();
        Set_Knowledge();
    }
    

    void Check_If_Knowledge_Set_Well()
    {
        if (_Knowledge == null)
        {
            throw new NotImplementedException("Knowledge 为空");
        }
    }

    void Set_Knowledge()
    {
        knowledge_label.text = _Knowledge.Label;
        knowledge_image.sprite = Resources.Load<Sprite>("Image/" + _Knowledge.Image);
        knowledge_content.text = _Knowledge.Knowledge_Content;

    }
    



}
