using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Title_Setting_Language_Dropdown_button : MonoBehaviour
{

    public GameManager.Language current_language_choice = GameManager.Language.English;
    public bool menu_dropped;

    public GameObject choice_English;
    public GameObject choice_Chinese;
    public TMP_Text currentLanguageText;

    public SpriteRenderer button;
    public Color idle_color;

    
    private void Start()
    {
        button.color = idle_color;
        current_language_choice = GameManager.currentLanguage;
    }

    private void Update()
    {
        if (current_language_choice == GameManager.Language.Chinese)
        {
            currentLanguageText.text = "中文";
        }
        if (current_language_choice == GameManager.Language.English)
        {
            currentLanguageText.text = "English";
        }
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
        Menu_Drop();
    }

    public void Menu_Drop()
    {
        if (!menu_dropped)
        {
            menu_dropped = true;
            choice_Chinese.SetActive(true);
            choice_English.SetActive(true);
        }
        else
        {
            menu_dropped = false;
            choice_Chinese.SetActive(false);
            choice_English.SetActive(false);
        }
    }
}
