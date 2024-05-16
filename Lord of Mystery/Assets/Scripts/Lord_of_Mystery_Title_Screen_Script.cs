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

        Vector3 physical_body_generate_location = GameObject.Find("Physical_Body_Generate_Location").transform.position;
        GameManager.GM.BodyPartManager.Generate_Body_Part_To_Board("Physical_Body", 
                physical_body_generate_location,
                physical_body_generate_location);
        
        StartCoroutine(GameManager.GM.InputManager.Main_Scene_Fade_In());

        yield return null;
    }
    
    
}
