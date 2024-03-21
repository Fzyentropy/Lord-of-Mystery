using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource_Click_Message : MonoBehaviour
{
    public string messageId;
    private Vector3 click_mouse_position;       // 用于点击时记录鼠标的位置
    private Vector3 lastMousePosition;      // 用于记录鼠标拖拽时，前一帧鼠标的位置

    public Color normal_color;
    public Color highlight_color;

    private void Awake()
    {
        normal_color = GetComponent<SpriteRenderer>().color;
        highlight_color = new Color(normal_color.r - 0.1f, normal_color.g - 0.1f, normal_color.b - 0.1f);
    }

    private void OnMouseOver()      // 鼠标悬停的时候，高亮
    {
        GetComponent<SpriteRenderer>().color = highlight_color;
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = normal_color;
    }

    private void OnMouseDown()      // 按下鼠标左键的时候，记录鼠标位置，调整卡牌的渲染 layer，让其到最上面，取消高亮
    {
        
        // 记录鼠标位置
        click_mouse_position = Input.mousePosition;
        lastMousePosition = Input.mousePosition;

        // 取消高亮
        
        
    }

    private void OnMouseUp() // 如果此时鼠标的位置和先前按下左键时记录的位置差不多，则为点击，触发点击功能（打开 panel）
    {

        if ((Input.mousePosition - click_mouse_position).magnitude < 0.2) // 判断此时鼠标的位置和记录的位置，如果差不多即视为点击，触发点击功能
        {
            GameManager.GM.Generate_Message(messageId);
        }
    }

    
}
