using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using random = UnityEngine.Random;
using Random = Unity.Mathematics.Random;

public class Start_Button_Script : MonoBehaviour
{
    public GameObject attached_panel;         // 对于这个按钮所在的 panel 的指代，可通过此 panel 指代找到相应的卡牌，并触发countdown功能
    // public Card_Location_Panel_Feature attached_panel_feature;    // 对所在 panel 上的 panel_feature 进行指代
    
    public SpriteRenderer button_sprite_renderer;       // Button 的 SpriteRenderer 指代，方便调用以调整颜色
    private bool is_button_available;                      // Button 是否能够点击，当条件满足时，可以点击
    private bool is_button_hover;
    private bool is_button_clicking;

    // Button 各状态颜色
    public Color button_idle_color;
    public Color button_hover_color;
    public Color button_click_color;
    public Color button_unavailable_color;
    
    // Mis
    public Vector3 mouse_click_position;                  // 点击时记录鼠标位置，方便判定点击还是拖拽


    private void Awake()
    {
        Set_Button_Renderer();    // 设置 button 的 SpriteRenderer 指代
        // Set_Button_Availability(false);
    }

    private void Update()
    {
        Update_Button_Color();
    }


    public void Set_Attached_Panel(GameObject attachedPanel)            // 在 panel 初始化的时候调用，外部设置此按钮对应的 panel 实例
    {
        attached_panel = attachedPanel;
        // attached_panel_feature = attached_panel.GetComponent<Card_Location_Panel_Feature>();
    }
    
    private void Set_Button_Renderer()
    {
        button_sprite_renderer = GetComponent<SpriteRenderer>();
    }
    
    public void Set_Button_Availability(bool availability)    //  设置 button 是否 available 的方法，从外部设置，顺带设置颜色
    {
        is_button_available = availability;
        
    }
    

    public void Set_Button_Text(string text)
    {
        TMP_Text buttonText = GetComponentInChildren<TMP_Text>();
        buttonText.text = text;
    }



    void Update_Button_Color()
    {
        if (!is_button_available)       // 如果 Button 不 Available，则设置 Button 颜色为灰色
        {
            button_sprite_renderer.color = button_unavailable_color;
        }

        else 
        {
            if (is_button_clicking)
            {
                button_sprite_renderer.color = button_click_color;
            }
            else 
            {
                if (is_button_hover)
                    button_sprite_renderer.color = button_hover_color;
                else
                    button_sprite_renderer.color = button_idle_color;
            }
            
        }
        
    }
    

    
    //////////    Mouse 点击相关方法
    
    private void OnMouseOver()
    {
        is_button_hover = true;
    }

    private void OnMouseExit()
    {
        is_button_hover = false;
    }
    
    private void OnMouseDown()
    {
        mouse_click_position = Input.mousePosition;

        is_button_clicking = true;
        
        // 播放 Start Button 按下 音效
        if (is_button_available)
            GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Start_Button_Click_MouseDown);

    }

    private void OnMouseUp()
    {
        is_button_clicking = false;
        
        if (is_button_available && (Input.mousePosition - mouse_click_position).magnitude < 0.6f)
        {
            /////// 点击后的执行逻辑
            
            // 播放 按钮抬起 音效 
            GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Start_Button_Click_MouseUp);
        
            // Randomly_Generate_A_Body_Part();
        
            attached_panel.
                GetComponent<Card_Location_Panel_Feature>().
                    attached_card.
                        GetComponent<Card_Location_Feature>().
                            Start_Countdown();
            
            // 播放 倒计时 音效
            GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Start_Countdown);
            
            GameManager.GM.PanelManager.isPanelOpen = false;
            GameManager.GM.PanelManager.current_panel = null;
            
            Destroy(attached_panel);

        }
        

    }
    
    
    
    
    
    
    
    
}
