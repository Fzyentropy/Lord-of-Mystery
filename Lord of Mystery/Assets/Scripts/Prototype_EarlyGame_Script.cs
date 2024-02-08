using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_EarlyGame_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GM.Generate_Card_Location("Location_Panel_Test_1");
        GameManager.GM.Generate_Card_Location("Location_Panel_Test_2");
        GameManager.GM.Generate_Card_Location("Location_Panel_Test_3");
        GameManager.GM.Generate_Card_Location("Location_Panel_Test_4");
        GameManager.GM.BodyPartManager.Generate_Body_Part_To_Board("Physical_Body", Vector3.zero, new Vector3(1,1,0));
        GameManager.GM.BodyPartManager.Generate_Body_Part_To_Board("Spirit", Vector3.zero, new Vector3(1,1,0));
    }
    
    
}
