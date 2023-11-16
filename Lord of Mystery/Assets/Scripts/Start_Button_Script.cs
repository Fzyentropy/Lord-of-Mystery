using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using random = UnityEngine.Random;
using Random = Unity.Mathematics.Random;

public class Start_Button_Script : MonoBehaviour
{
    public GameObject attached_panel;         // 对于这个按钮所在的 panel 的指代，可通过此 panel 指代找到相应的卡牌，并触发countdown功能
    // public Card_Location_Panel_Feature attached_panel_feature;    // 对所在 panel 上的 panel_feature 进行指代
    
    public bool is_button_available;                      // Button 是否能够点击，当条件满足时，可以点击
    public SpriteRenderer button_sprite_renderer;
    public Vector3 mouse_click_position;                  // 点击时记录鼠标位置，方便判定点击还是拖拽
    
    // Button 各状态颜色
    public Color button_idle_color;
    public Color button_hover_color;
    public Color button_click_color;
    public Color button_unavailable_color;
    
    
    
    
    private void Awake()
    {
        Set_Button_Renderer();    // 设置 button 的 SpriteRenderer 指代
        // Set_Button_Availability(false);
    }


    public void Set_Attached_Panel(GameObject attachedPanel)
    {
        attached_panel = attachedPanel;
        // attached_panel_feature = attached_panel.GetComponent<Card_Location_Panel_Feature>();
    }
    
    public void Set_Button_Availability(bool availability)    //  设置 button 是否 available 的方法，从外部设置，顺带设置颜色
    {
        is_button_available = availability;
        
        if (!is_button_available)
        {
            button_sprite_renderer.color = button_unavailable_color;
        }
        else
        {
            button_sprite_renderer.color = button_idle_color;
        }
    }

    private void Set_Button_Renderer()
    {
        button_sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
    }
    

    
    //////////    Mouse 点击相关方法
    
    private void OnMouseOver()
    {
        Debug.Log("is over the button");
        if (is_button_available)
        {
            button_sprite_renderer.color = button_hover_color;
        }
        
    }

    private void OnMouseExit()
    {
        if (is_button_available)
        {
            button_sprite_renderer.color = button_idle_color;
        }
        
    }
    
    private void OnMouseDown()
    {
        mouse_click_position = Input.mousePosition;
        if (is_button_available)
        {
            button_sprite_renderer.color = button_click_color;
        }
        
    }

    private void OnMouseUp()
    {
        if (is_button_available)
        {
            if ((Input.mousePosition - mouse_click_position).magnitude < 0.1)
            {
            
                // 点击后的执行逻辑
            
                // Randomly_Generate_A_Body_Part();
            
                attached_panel.GetComponent<Card_Location_Panel_Feature>().attached_card.GetComponent<Card_Location_Feature>().Start_Countdown();
                
            
            }
        }
        

    }


    public void Randomly_Generate_A_Body_Part()
    {
        int randomElement = random.Range(0, GameManager.GM.CardLoader.Body_Part_Card_List.Count - 1);
        GameManager.GM.Generate_Card_Body_Part(GameManager.GM.CardLoader.Body_Part_Card_List[randomElement].Id);
        // new Vector3(
        //     random.Range(-12,12),random.Range(-12,12),1));
    }
    
    
    
    
    
    
    
}
