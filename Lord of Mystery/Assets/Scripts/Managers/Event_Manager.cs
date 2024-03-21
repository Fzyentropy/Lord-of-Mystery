using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Event_Manager : MonoBehaviour
{

    ////////////////////////////////////      Event 记录参数

    public bool Event_Private_Soiree = false;           // Private Soiree 是否被触发过





    
    
    
    
    
    
    
    
    ////////////////////////////////////       Event 触发逻辑
    
    private void Start()
    {
        StartCoroutine(Trigger_Event_Based_On_Condition());
    }

    IEnumerator Trigger_Event_Based_On_Condition()
    {
        while (true)
        {
            // 当 Fund 数量大于 10 的时候，触发 Private Soiree
            if(GameManager.GM.ResourceManager.Fund >= 6   && !Event_Private_Soiree)  { Trigger_Private_Soiree(); }

















            yield return null;
        }
    }
    


    ////////////////////////////////////       Event 具体执行


    public void Trigger_Private_Soiree()
    {
        Event_Private_Soiree = true;

        GameManager.GM.Generate_Card_Location("Private_Soiree",
            new Vector3(
                Random.Range(0,30),
                Random.Range(0,-15),
                1));

    }


    
    
    
    
    
    
    
    
    
    


}
