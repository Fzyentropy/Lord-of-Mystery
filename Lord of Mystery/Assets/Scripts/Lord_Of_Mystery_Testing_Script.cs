using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lord_Of_Mystery_Testing_Script : MonoBehaviour
{

    public GameObject make_potion_card;
    
    void Start()
    {
        StartCoroutine(Start_Test());
    }

    IEnumerator Start_Test()
    {

        yield return new WaitForSeconds(1f);

        Instantiate(make_potion_card, new Vector3(3, -3, 1), Quaternion.identity);
        
        GameManager.GM.ResourceManager.Add_Physical_Energy(10, Vector3.zero);
        GameManager.GM.ResourceManager.Add_Spirituality_Infused_Material(5, Vector3.zero);
        
        GameManager.GM.ResourceManager.Add_Spiritual_Energy(5, Vector3.zero);
        
        GameManager.GM.ResourceManager.Draw_A_Knowledge_By_Name("Potion_Formula_Level_9_Apprentice_Of_The_Whisper", Vector3.zero, Vector3.one);
        
        GameManager.GM.ResourceManager.Draw_A_Knowledge_By_Name("Knowledge_Potion", Vector3.zero, Vector3.zero);
        GameManager.GM.ResourceManager.Draw_A_Knowledge_By_Name("Knowledge_Three_Kingdoms", Vector3.zero, Vector3.zero);

        for (int i = 0; i < 10; i++)
        {
            GameManager.GM.ResourceManager.Draw_A_Knowledge_With_Rarity_Involved(Vector3.zero, Vector3.one);
        }



    }


}
