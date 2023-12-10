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
        GameManager.GM.Generate_Card_Body_Part("Physical_Body");
    }
    
    
}
