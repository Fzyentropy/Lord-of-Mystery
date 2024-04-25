using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lord_of_Mystery_Scene_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Game_Start());
    }

    IEnumerator Game_Start()
    {
        
        GameManager.GM.Generate_Card_Location("Loen", new Vector3(10,-8,0));
       
        GameManager.GM.BodyPartManager.Generate_Body_Part_To_Board("Physical_Body", new Vector3(20,-8,-1), new Vector3(20,-8,-1));

        yield return new WaitForSeconds(0.5f);
        
        GameManager.GM.Generate_Message("Born");

    }
    
    
}