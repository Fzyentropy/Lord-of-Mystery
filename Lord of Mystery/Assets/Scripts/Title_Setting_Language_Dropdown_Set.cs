using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_Setting_Language_Dropdown_Set : MonoBehaviour
{
    public SpriteRenderer button;
    public Color idle_color;

    public GameObject triangle_button;

    
    private void Start()
    {
        button.color = idle_color;
    }

    private void Update()
    {

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
        
        Set_Current_Language();
        GameManager.GM.PanelManager.Close_Current_Panel();

        Reload_Scene();

    }

    public void Set_Current_Language()
    {
        GameManager.currentLanguage = triangle_button.GetComponent<Title_Setting_Language_Dropdown_button>()
            .current_language_choice;
    }

    public void Reload_Scene()
    {
        SceneManager.LoadScene("Lord_of_Mystery_Title_Screen");
    }
}
