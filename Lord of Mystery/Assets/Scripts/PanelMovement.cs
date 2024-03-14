using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMovement : MonoBehaviour
{
 
    private Vector3 click_mouse_position;       // 用于点击时记录鼠标的位置
    private Vector3 lastMousePosition;      // 用于记录鼠标拖拽时，前一帧鼠标的位置
    
    
    ///////////////////////////////////////////////////     On 事件函数 写在了另外的 component上

    private void OnMouseOver()
    {

    }

    private void OnMouseDown()
    {
        // 记录鼠标位置
        click_mouse_position = Input.mousePosition;
        lastMousePosition = Input.mousePosition;
        
    }

    private void OnMouseDrag()
    {
        
        GameManager.GM.InputManager.Dragging_Object = gameObject;      // 将 Input Manager 中的 正在拖拽物体 记录为此物体
        // float mouse_drag_sensitivity = 0.05f;
        
        // 如果鼠标移动，卡牌随之移动
        Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(lastMousePosition);
        delta.z = 0;
        FindUpParent(transform).position += delta;      // 找到最上层父物体（即 panel）并改变其位置
        lastMousePosition = Input.mousePosition;
        
    }

    private void OnMouseUp()
    {
        GameManager.GM.InputManager.Dragging_Object = null;      // 释放 Input Manager 中的 正在拖拽 GameObject，设置为空
    }

    private void OnMouseExit()
    {
        
    }
    
    private void OnDestroy()        // 当 Panel 被销毁的时候， 返还 resource 和 body part
    {
        // Debug.Log("panel 销毁了");
        // Return_Resource();
    }
    
    
    ///////////////////////////////////////////////////     On 事件函数结束


    Transform FindUpParent(Transform child)
    {
        if (child.parent == null)
            return child;
        else
            return FindUpParent(child.parent);
    }
    
    
}
