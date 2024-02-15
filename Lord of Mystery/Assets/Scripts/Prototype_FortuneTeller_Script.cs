using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_FortuneTeller_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GM.Generate_Message("Fortune_Teller_In_Loen");
        GameManager.GM.Generate_Card_Location("The_Fortune_Teller", new Vector3(0,0,0));
    }
    
    
}
