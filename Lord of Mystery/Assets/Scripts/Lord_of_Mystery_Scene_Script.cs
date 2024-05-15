using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lord_of_Mystery_Scene_Script : MonoBehaviour
{

    public Image black_screen;
    public GameObject Game_Scene_Exit_card;

    void Start()
    {
        StartCoroutine(Game_Start());
    }

    IEnumerator Game_Start()
    {

        GameManager.GM.Generate_Card_Location("Loen", new Vector3(10,-8,0));
       
        GameManager.GM.BodyPartManager.Generate_Body_Part_To_Board("Physical_Body", new Vector3(20,-8,-1), new Vector3(20,-8,-1));
        
        GameManager.GM.Generate_Card_Location("Game_Scene_Card_Exit", GameObject.Find("Game_Scene_Exit_Card_Location").transform.position);


        StartCoroutine(GameManager.GM.InputManager.Main_Scene_Fade_In());


        yield return new WaitUntil(() => GameManager.GM.InputManager.black_screen.color == Color.clear);
        yield return new WaitForSeconds(0.5f);
        
        GameManager.GM.Generate_Message("Born");

    }
    
    
    
    
}
