using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using random = UnityEngine.Random;
using Random = Unity.Mathematics.Random;

public class ImageButton : MonoBehaviour
{
    public Color co; 
    private void Start()
    {
        co = gameObject.GetComponent<SpriteRenderer>().color;
    }

    private void OnMouseOver()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = co;
    }


    private void OnMouseDown()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        Debug.Log("clicked!");

        int randomElement = random.Range(0, GameManager.GM.CardLoader.Body_Part_Card_List.Count - 1);
        GameManager.GM.Generate_Card_Body_Part(GameManager.GM.CardLoader.Body_Part_Card_List[randomElement].Id);
        // new Vector3(
        //     random.Range(-12,12),random.Range(-12,12),1));


    }
}
