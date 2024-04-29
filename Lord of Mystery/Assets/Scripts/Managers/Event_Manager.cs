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
            if(GameManager.GM.Player_Owned_Card_Location_List.Count >= 6   && !Event_Private_Soiree)  { StartCoroutine(Trigger_Private_Soiree()); }

















            yield return null;
        }
    }
    


    ////////////////////////////////////       Event 具体执行

    IEnumerator Trigger_Private_Soiree()
    {
        Event_Private_Soiree = true;
        
        yield return new WaitForSeconds(7f);
        
        // 如果当前有 panel 打开，则关闭 panel
        if (GameManager.GM.PanelManager.current_panel != null)    // 如果已经有 panel 打开，则关闭 panel
        {
            GameManager.GM.PanelManager.Close_Current_Panel();      // 调用 panel manager 中的销毁 panel 方法
        }
        
        // 随机 random 一个位置
        Vector2 soiree_pos = new Vector3(Random.Range(10, 30), Random.Range(-5, -15));

        // 生成 Private Soiree
        GameManager.GM.Generate_Card_Location("Private_Soiree", 
            new Vector3(soiree_pos.x, soiree_pos.y, 0));
        
        // 移动 Camera
        GameManager.GM.InputManager.Move_Camera_To(soiree_pos);

    }


    
    
    
    
    
    
    
    
    
    


}
