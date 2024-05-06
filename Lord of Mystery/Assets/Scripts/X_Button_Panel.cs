using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class X_Button_Panel : MonoBehaviour
{

    public Color idle_color;
    public Color hover_color;

    public TMP_Text xButton;
    
    private Vector3 click_mouse_position;       // 用于点击时记录鼠标的位置


    private void OnMouseOver()
    {
        xButton.color = hover_color;
    }

    private void OnMouseDown()
    {
        click_mouse_position = Input.mousePosition;
        xButton.color = idle_color;
    }

    private void OnMouseExit()
    {
        xButton.color = idle_color;
    }

    private void OnMouseUp()
    {
        if ((Input.mousePosition - click_mouse_position).magnitude < 0.3f)
            GameManager.GM.PanelManager.Close_Current_Panel();
    }
}
