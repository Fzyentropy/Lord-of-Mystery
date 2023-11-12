using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_BuildCards_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("msg", 2f, 3);
    }

    void msg()
    {
        GameManager.GM.Generate_Message("Born");
    }
    
}
