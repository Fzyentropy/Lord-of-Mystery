using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_BuildCards_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("msg", 2f);
        GameManager.GM.Generate_Card_Location("Loen");
    }

    void msg()
    {
        GameManager.GM.Generate_Message("Born");
    }
    
}
