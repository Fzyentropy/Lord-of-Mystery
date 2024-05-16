using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title_Setting_Language_Dropdown_choice : MonoBehaviour
{
    public GameManager.Language this_choice_of_language;
    
    public SpriteRenderer button;
    public Color idle_color;

    public GameObject triangle_button;

    
    private void Start()
    {
        button.color = idle_color;
    }
    


    private void OnMouseOver()
    {
        button.color = Color.white;
    }

    private void OnMouseExit()
    {
        button.color = idle_color;
    }

    private void OnMouseDown()
    {
        GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Start_Button_Click_MouseDown);
        button.color = idle_color;
    }

    private void OnMouseUp()
    {
        button.color = Color.white;
        GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Start_Button_Click_MouseUp);

        triangle_button.GetComponent<Title_Setting_Language_Dropdown_button>().current_language_choice =
            this_choice_of_language;
        
        triangle_button.GetComponent<Title_Setting_Language_Dropdown_button>().Menu_Drop();
    }
}
