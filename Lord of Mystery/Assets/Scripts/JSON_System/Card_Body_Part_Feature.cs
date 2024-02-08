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

    public GameObject overlapped_card_location; // 用于记录，当前这个 body part 跟哪张卡是重合的，用于被吸收，且用此参数判定可保证重叠的卡唯一

    public Vector3 lastPosition;        // 用于记录拖拽时最后停下的 location
 
    // Mis Variables
    private Vector3 click_mouse_position;       // 用于点击时记录鼠标的位置
    private Vector3 lastMousePosition;      // 用于记录鼠标拖拽时，前一帧鼠标的位置
    
    public GameObject cardBottom;       // 临时 原 physical body 卡牌图片，用于调整 Order in Layer
    public GameObject cover;    // 临时，用于 cover 原 physical body 卡牌名称的 小方块，用于调整 Order in Layer

    public int LayerIndex;
    public const string OriginalSortingLayer = "Body Parts";
    
    

    void Start()
    {
        Initialize_Body_Part();
        Set_Layer_Index();      // 设置 layer 的 index
    }

    private void Update()
    {
        Debug.Log(overlapped_card_location);
    }


    void Initialize_Body_Part()
    {
        body_part_label.text = _CardBodyPart.Label;
        body_part_image.sprite = Resources.Load<Sprite>("Image/" + _CardBodyPart.Image);
    }

    void Set_Layer_Index()      // 设置 layer 的 index
    {
        LayerIndex = gameObject.layer;
    }

    
    private void OnMouseOver()      // 鼠标悬停的时候，高亮
    {
        // 高亮
        if (GameManager.GM.InputManager.Dragging_Object == null)
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

    private void OnMouseUp()        // 如果此时鼠标的位置和先前按下左键时记录的位置差不多，则为点击，触发点击功能（打开 panel）
    {
        
        if ((Input.mousePosition - click_mouse_position).magnitude < 0.2) // 判断此时鼠标的位置和记录的位置，如果差不多即视为点击，触发点击功能
        {
            Card_Body_Part_Mouse_Click_Function();                      // 点击功能的封装
        }
        
        GameManager.GM.InputManager.Dragging_Object = null;      // 释放 Input Manager 中的 正在拖拽 GameObject，设置为空
        
        // 调整 卡牌的渲染 layer 让其回到原位
        gameObject.layer = LayerIndex; 
        DecreaseSortingLayer();     // 设置回 原 Order in Layer
        
        
        // 如果在一个 card location 上面, 触发对应 card location 中的方法，来打开panel，然后将这张卡 merge 到其中一个 slot 上
        // 判定是否能吸收
        // TODO 要加上 是否重叠的判定
        if (overlapped_card_location != null && 
            overlapped_card_location.GetComponent<Card_Location_Feature>().Check_If_Dragging_BodyPart_Is_Need(gameObject))     
        {
            StartCoroutine(overlapped_card_location.GetComponent<Card_Location_Feature>().Absorb_Dragging_Body_Parts(gameObject));
        }
        else
        {
            lastPosition = transform.position;
        }
        
        
    }

    private void OnMouseExit()      // 当鼠标离开卡牌上方时，取消高亮
    {
        // 取消高亮
        Clear_Highlight_Collider();
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
    
    public void IncreaseSortingLayer()       // 提高 卡牌的 Order in Layer 数值，以让卡牌在最上方渲染
    {
        body_part_label.GetComponent<Renderer>().sortingLayerName = "Dragging";
        body_part_image.sortingLayerName = "Dragging";
        cardBottom.GetComponent<SpriteRenderer>().sortingLayerName = "Dragging";
        cover.GetComponent<SpriteRenderer>().sortingLayerName = "Dragging";
    }
    public void DecreaseSortingLayer()       // 减少 卡牌的 Order in Layer 数值，以让卡牌在最上方渲染
    {
        body_part_label.GetComponent<Renderer>().sortingLayerName = OriginalSortingLayer;
        body_part_image.sortingLayerName = OriginalSortingLayer;
        cardBottom.GetComponent<SpriteRenderer>().sortingLayerName = OriginalSortingLayer;
        cover.GetComponent<SpriteRenderer>().sortingLayerName = OriginalSortingLayer;
    }




    public void Card_Body_Part_Mouse_Click_Function()
    {
        
        //  当前如果点击一张 body part 卡，则弹出介绍信息
        {
            /*      TODO 替换成 message 判定和关闭 message            // 如果当前有其他 message，先关闭
            if (GameManager.GM.PanelManager.isPanelOpen)
            {
                GameManager.GM.PanelManager.Close_Current_Panel();
            }
            */

            GameManager.GM.Generate_Message(_CardBodyPart.Produce_Message);      // 生成此 body part 卡对应的 message
        }
        
        
        
        
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
