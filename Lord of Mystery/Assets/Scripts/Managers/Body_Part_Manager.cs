using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Body_Part_Manager : MonoBehaviour
{

    // 当前 board 上各 body part 的数量 
    public int Body_Part_Physical_Body;
    public int Body_Part_Spirit;
    public int Body_Part_Psyche;

    private void Start()
    {
        Initialize_Body_Part_Count();
    }

    /////////////////////////       设置函数

    public void Initialize_Body_Part_Count()
    {
        Body_Part_Physical_Body = 0;
        Body_Part_Spirit = 0;
        Body_Part_Psyche = 0;
    }

    /////////////////////////       设置函数  结束



    /////////////////////////       Body Part 操作函数

    public void Generate_Body_Part(string bodyPartString, Vector3 from, Vector3 To) // 生成 1个 对应类型的 body part，并运动
    {
        GameObject generatedBodyPart = null;

        if (bodyPartString == "Physical_Body")
        {
            Body_Part_Physical_Body++;

            generatedBodyPart = GameManager.GM.Generate_Card_Body_Part("Physical_Body");
        }

        if (bodyPartString == "Spirit")
        {
            Body_Part_Spirit++;

            generatedBodyPart = GameManager.GM.Generate_Card_Body_Part("Spirit");
        }

        if (bodyPartString == "Psyche")
        {
            Body_Part_Psyche++;

            generatedBodyPart = GameManager.GM.Generate_Card_Body_Part("Psyche");
        }

        if (generatedBodyPart != null)
            Animate_Body_Part_Movement(generatedBodyPart, from, To);
    }

    public void Destroy_Body_Part(GameObject bodyPartGameObject)
    {
        if (bodyPartGameObject.GetComponent<Card_Body_Part_Feature>()._CardBodyPart.Id == "Physical_Body" && Body_Part_Physical_Body > 0)
        {
            Destroy(bodyPartGameObject);
            Body_Part_Physical_Body--;
        }
        if (bodyPartGameObject.GetComponent<Card_Body_Part_Feature>()._CardBodyPart.Id == "Spirit" && Body_Part_Spirit > 0)
        {
            Destroy(bodyPartGameObject);
            Body_Part_Spirit--;
        }
        if (bodyPartGameObject.GetComponent<Card_Body_Part_Feature>()._CardBodyPart.Id == "Psyche" && Body_Part_Psyche > 0)
        {
            Destroy(bodyPartGameObject);
            Body_Part_Psyche--;
        }
        
            // TODO 销毁 body part 的炫酷动画

    }


    private void Animate_Body_Part_Movement(GameObject bodyPartToMove, Vector3 from, Vector3 To)    // body part 移动动画
    {
        float duration = 0.3f;      // 在这调整移动的时长

        if (bodyPartToMove.transform.position != from)      // 如果该物体不在 from 的位置，则移动到 from
        {
            bodyPartToMove.transform.position = from;
        }

        bodyPartToMove.transform.DOMove(To, duration);      // 使用 DO Tween 移动
        
    }
    
    
    
    
    
    
    
    
    
    
    
}