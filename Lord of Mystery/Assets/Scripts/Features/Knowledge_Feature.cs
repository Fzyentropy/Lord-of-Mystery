using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Knowledge_Feature : MonoBehaviour
{

    // Knowledge 实例
    public Knowledge _Knowledge;
    
    // 卡牌外观 prefab 各元素
    public SpriteRenderer knowledge_frame;      // 框
    public SpriteRenderer knowledge_name_tag;    // label 底
    public TMP_Text knowledge_short_label;       // short label
    public SpriteRenderer knowledge_shadow;     // shadow 阴影
    
    // Important Marks
    public GameObject overlapped_card_location_or_knowledge_slot;   // 与此 Knowledge 重叠的 Card Location 或者 Knowledge Slot，重叠时由 Card Location 或 Knowledge Slot 传递设置
    public Vector3 lastPosition;        // // 用于记录拖拽时最后停下的 location
    
    // Mis Variables
    private Vector3 click_mouse_position;       // 用于点击时记录鼠标的位置
    private Vector3 lastMousePosition;      // 用于记录鼠标拖拽时，前一帧鼠标的位置
    
    private bool dragging_shadow_effect_if_transformed;

    public int LayerIndex;


    private void Start()
    {
        Initialize_Knowledge();
        Set_Layer_Index();
    }




    void Initialize_Knowledge()
    {
        gameObject.name = _Knowledge.Id;
        knowledge_short_label.text = _Knowledge.Short_Label;
        
        if (GameManager.currentLanguage == GameManager.Language.English)        // 设置语言
        {
            knowledge_short_label.font = GameManager.Font_English;
            // card_label.fontSize = 8;
        }
        else if (GameManager.currentLanguage == GameManager.Language.Chinese)
        {
            knowledge_short_label.font = GameManager.Font_Chinese;
            // card_label.fontSize = 8;
        }
        
    }
    
    void Set_Layer_Index()      // 设置 layer 的 index
    {
        LayerIndex = gameObject.layer;
    }
    
    
    
    

    private void OnMouseOver()      // 鼠标悬停的时候，高亮
    {
        // 高亮
        if(GameManager.GM.InputManager.Dragging_Object == null)
            Highlight_Collider();
    }

    private void OnMouseDown()      // 按下鼠标左键的时候，记录鼠标位置，调整卡牌的渲染 layer，让其到最上面，取消高亮
    {
        
        // 记录鼠标位置
        click_mouse_position = Input.mousePosition;
        lastMousePosition = Input.mousePosition;

        // 取消高亮
        
    }

    private void OnMouseDrag()      // 当按住鼠标左键的时候，如果移动鼠标（即拖拽），则卡牌随之移动
    {

        // 调整卡牌的渲染 layer，让其到最上面
    
        gameObject.layer = LayerMask.NameToLayer("DraggingLayer");  // 调用系统方法来找到 "Dragging Layer"对应的 Index，并设置
        IncreaseSortingLayer();     // 增加渲染的 order in layer，将物体渲染在最前面
    
        // 如果鼠标移动，卡牌随之移动

        GameManager.GM.InputManager.Dragging_Object = gameObject;      // 将 Input Manager 中的 正在拖拽物体 记录为此物体
        Clear_Highlight_Collider();                             // 取消高亮
        // float mouse_drag_sensitivity = 0.05f;
        Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(lastMousePosition);
        delta.z = 0;
        gameObject.transform.position += delta;
        lastMousePosition = Input.mousePosition;
            
        
    }
    
    private void OnMouseExit()      // 当鼠标离开卡牌上方时，取消高亮
    {
        // 取消高亮
        Clear_Highlight_Collider();
    }
    
    private void OnMouseUp()        // 如果此时鼠标的位置和先前按下左键时记录的位置差不多，则为点击，触发点击功能（打开 panel）
    {
        
        if ((Input.mousePosition - click_mouse_position).magnitude < 0.8f) // 判断此时鼠标的位置和记录的位置，如果差不多即视为点击，触发点击功能
        {
            Knowledge_Click_Function();                      // 点击功能的封装
        }
        
        GameManager.GM.InputManager.Dragging_Object = null;      // 释放 Input Manager 中的 正在拖拽 GameObject，设置为空
        
        // 调整 卡牌的渲染 layer 让其回到原位
        gameObject.layer = LayerIndex; 
        DecreaseSortingLayer();     // 设置回 原 Order in Layer
        
        
        // 松开时 吸收 Knowledge
        if (overlapped_card_location_or_knowledge_slot != null)     // 如果 overlap 了一个东西
        {

            if (overlapped_card_location_or_knowledge_slot.GetComponent<Card_Location_Feature>() != null)     // overlap 的是 Card Location
            {
                if (overlapped_card_location_or_knowledge_slot.GetComponent<Card_Location_Feature>()
                    .Check_If_Dragging_Knowledge_Is_Need(gameObject))
                {
                    StartCoroutine(overlapped_card_location_or_knowledge_slot.GetComponent<Card_Location_Feature>()
                        .Absorb_Dragging_Knowledge(gameObject));
                }
                
            }
            
            else if (overlapped_card_location_or_knowledge_slot.GetComponent<Card_Location_Panel_Knowledge_Slot>() != null)     // overlap 的是 Knowledge Slot
            {

                if (overlapped_card_location_or_knowledge_slot.GetComponent<Card_Location_Panel_Knowledge_Slot>()
                    .is_make_potion_panel)  // 如果是 make potion panel
                {
                    overlapped_card_location_or_knowledge_slot.GetComponent<Card_Location_Panel_Knowledge_Slot>()
                        .attached_make_potion_card_panel_feature.Absorb_Knowledge_Make_Potion(gameObject);
                }
                else   // 如果是 正常 Card Location Panel
                {
                    // knowledge slot 处理
                    overlapped_card_location_or_knowledge_slot.GetComponent<Card_Location_Panel_Knowledge_Slot>().
                        attached_card_location_panel_feature.Absorb_Knowledge(gameObject);
                }
                
                
            }
            
            
            
        }
        else   // 没 overlap 东西
        {
            lastPosition = transform.position;
        }
        
        
        
    }


    public void Highlight_Collider()
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 5; // 四个角加上第一个点重复一次以闭合

        // 计算BoxCollider2D的四个角
        Vector3[] corners = new Vector3[5];
        corners[0] = collider.offset + new Vector2(-collider.size.x, -collider.size.y) * 0.5f;
        corners[1] = collider.offset + new Vector2(collider.size.x, -collider.size.y) * 0.5f;
        corners[2] = collider.offset + new Vector2(collider.size.x, collider.size.y) * 0.5f;
        corners[3] = collider.offset + new Vector2(-collider.size.x, collider.size.y) * 0.5f;
        corners[4] = corners[0]; // 闭合线段

        // 转换到世界坐标
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = transform.TransformPoint(corners[i]);
            lineRenderer.SetPosition(i, corners[i]);
        }

        // 设置材质和颜色
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white; // 高亮颜色
        lineRenderer.endColor = Color.white; // 高亮颜色
    }

    public void Clear_Highlight_Collider()
    {
        
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.startColor = Color.clear; // 高亮颜色
            lineRenderer.endColor = Color.clear; // 高亮颜色
        }
    }
    
    public void IncreaseSortingLayer() // 提高 卡牌的 Order in Layer 数值，以让卡牌在最上方渲染
    {
        knowledge_short_label.GetComponent<Renderer>().sortingLayerName = "Dragging";
        knowledge_frame.sortingLayerName = "Dragging";
        knowledge_name_tag.sortingLayerName = "Dragging";
        knowledge_shadow.sortingLayerName = "Dragging";

        float x_movement = -0.2f;
        float y_movement = 0.2f;

        if (!dragging_shadow_effect_if_transformed)
        {
            dragging_shadow_effect_if_transformed = true;

            knowledge_short_label.transform.localPosition = new Vector3(
                knowledge_short_label.transform.localPosition.x + x_movement,
                knowledge_short_label.transform.localPosition.y + y_movement,
                knowledge_short_label.transform.localPosition.z);

            knowledge_frame.transform.localPosition = new Vector3(
                knowledge_frame.transform.localPosition.x + x_movement,
                knowledge_frame.transform.localPosition.y + y_movement,
                knowledge_frame.transform.localPosition.z);

            knowledge_name_tag.transform.localPosition = new Vector3(
                knowledge_name_tag.transform.localPosition.x + x_movement,
                knowledge_name_tag.transform.localPosition.y + y_movement,
                knowledge_name_tag.transform.localPosition.z);
        }
    }

    public void DecreaseSortingLayer()       // 减少 卡牌的 Order in Layer 数值，以让卡牌在最上方渲染
    {
        knowledge_short_label.GetComponent<Renderer>().sortingLayerName = "Body Parts";
        knowledge_frame.sortingLayerName = "Body Parts";
        knowledge_name_tag.sortingLayerName = "Body Parts";
        knowledge_shadow.sortingLayerName = "Body Parts";
        
        float x_movement = 0.2f;
        float y_movement = -0.2f;

        if (dragging_shadow_effect_if_transformed)
        {
            dragging_shadow_effect_if_transformed = false;

            knowledge_short_label.transform.localPosition = new Vector3(
                knowledge_short_label.transform.localPosition.x + x_movement,
                knowledge_short_label.transform.localPosition.y + y_movement,
                knowledge_short_label.transform.localPosition.z);

            knowledge_frame.transform.localPosition = new Vector3(
                knowledge_frame.transform.localPosition.x + x_movement,
                knowledge_frame.transform.localPosition.y + y_movement,
                knowledge_frame.transform.localPosition.z);

            knowledge_name_tag.transform.localPosition = new Vector3(
                knowledge_name_tag.transform.localPosition.x + x_movement,
                knowledge_name_tag.transform.localPosition.y + y_movement,
                knowledge_name_tag.transform.localPosition.z);
        }
    }


    public void Knowledge_Click_Function()
    {
        GameManager.GM.Generate_Knowledge_Panel(_Knowledge.Id);
    }
    
    
    
}
