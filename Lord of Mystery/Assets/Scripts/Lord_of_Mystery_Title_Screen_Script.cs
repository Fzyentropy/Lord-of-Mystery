using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lord_of_Mystery_Title_Screen_Script : MonoBehaviour
{
    private bool is_dio;
    void Start()
    {
        StartCoroutine(Title_Start());
    }

    IEnumerator Title_Start()
    {
        
        GameManager.GM.Generate_Card_Location("Title_Card_Start", new Vector3(-8.5f,-13.26f,0));
       
        GameManager.GM.Generate_Card_Location("Title_Card_Exit", new Vector3(8.5f,-13.26f,0));

 
        GameManager.GM.BodyPartManager.Generate_Body_Part_To_Board("Physical_Body", 
                new Vector3(24.7f, 12.14f, 0),
                new Vector3(24.7f, 12.14f, 0));
        
        StartCoroutine(GameManager.GM.InputManager.Main_Scene_Fade_In());

        yield return null;
    }
    
    
}
