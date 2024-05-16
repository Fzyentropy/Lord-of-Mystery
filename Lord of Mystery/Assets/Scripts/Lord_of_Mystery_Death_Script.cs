using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lord_of_Mystery_Death_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Death_Scene());
    }


    IEnumerator Death_Scene()
    {

        yield return new WaitForSeconds(1f);

        GameManager.GM.Generate_Card_Location("Player_Death", GameObject.Find("Death_Card_Location").transform.position);

        Vector3 physical_body_generate_location = GameObject.Find("Spirit_Generate_Location").transform.position;
        GameManager.GM.BodyPartManager.Generate_Body_Part_To_Board("Spirit", 
            physical_body_generate_location,
            physical_body_generate_location);
        
        StartCoroutine(GameManager.GM.InputManager.Main_Scene_Fade_In());

        yield return null;
        
    }
    
    
}
