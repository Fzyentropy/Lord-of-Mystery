using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lord_Of_Mystery_Card_Gallery : MonoBehaviour
{
    
    float xcor = 0f;
    float ycor = 0f;
    float x_interval = 8f;
    float y_interval = -10f;
    int counter = 0;
    
    void Start()
    {
        StartCoroutine(start_generate());
    }

    IEnumerator start_generate()
    {
        yield return new WaitForSeconds(1f);
        Generate_All_Card_Location();
        Generate_All_Body_Part();
        Generate_All_Knowledge();
    }

    void Generate_All_Card_Location()
    {
        
        foreach (var card_location in GameManager.GM.CardLoader.Location_Card_List)
        {
            GameManager.GM.Generate_Card_Location(card_location.Id, new Vector3(xcor, ycor, 0));

            counter++;
            
            if (counter + 1 > 10)
            {
                xcor = 0f;
                ycor += y_interval;
                counter = 0;
            }
            else
            {
                xcor += x_interval;
            }
        }
    }
    
    void Generate_All_Body_Part()
    {
        
        foreach (var body_part in GameManager.GM.CardLoader.Body_Part_Card_List)
        {
            GameManager.GM.BodyPartManager.Generate_Body_Part_To_Board(body_part.Id, new Vector3(xcor, ycor, 0),new Vector3(xcor, ycor, 0));

            counter++;
            
            if (counter + 1 > 10)
            {
                xcor = 0f;
                ycor += y_interval;
                counter = 0;
            }
            else
            {
                xcor += x_interval;
            }
        }
    }
    
    void Generate_All_Knowledge()
    {
        
        foreach (var knowledge in GameManager.GM.CardLoader.Knowledge_List)
        {
            GameManager.GM.Generate_Knowledge_Card(knowledge.Id, new Vector3(xcor, ycor, 0));

            counter++;
            
            if (counter + 1 > 10)
            {
                xcor = 0f;
                ycor += y_interval;
                counter = 0;
            }
            else
            {
                xcor += x_interval;
            }
        }
    }

    
    
}
