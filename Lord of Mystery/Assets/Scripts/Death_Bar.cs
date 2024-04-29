using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Death_Bar : MonoBehaviour
{

    public SpriteMask death_bar_mask;
    public TMP_Text death_number_text;
    public float empty_position_y;
    public float full_position_y;
    
    private Vector3 click_mouse_position;       // 用于点击时记录鼠标的位置
    private Vector3 lastMousePosition;      // 用于记录鼠标拖拽时，前一帧鼠标的位置


    private void Start()
    {
        Set_Death_Bar();
    }


    void Update()
    {
        UpdateDeathBar();
        UpdateDeathText();
    }


    void Set_Death_Bar()
    {
        gameObject.name = "Death_Bar";
    }

    void UpdateDeathBar()
    {
        death_bar_mask.transform.localPosition =
            new Vector3(
                death_bar_mask.transform.localPosition.x,
                empty_position_y + (full_position_y - empty_position_y) * GameManager.GM.ResourceManager.Death_UI_Amount/GameManager.GM.ResourceManager.Max_Death_UI_Amount,
                death_bar_mask.transform.localPosition.z
            );
    }

    void UpdateDeathText()
    {
        death_number_text.text = 
            GameManager.GM.ResourceManager.Death_UI_Amount + "/" + GameManager.GM.ResourceManager.Max_Death_UI_Amount;
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
            GameManager.GM.Generate_Message("Death");
        }
    }
}
