using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Death_Bar : MonoBehaviour
{

    public SpriteMask death_bar_mask;
    public TMP_Text death_number_text;
    public float empty_position_y;
    public float full_position_y;

    
    void Update()
    {
        UpdateDeathBar();
        UpdateDeathText();
    }

    void UpdateDeathBar()
    {
        death_bar_mask.transform.localPosition =
            new Vector3(
                death_bar_mask.transform.localPosition.x,
                empty_position_y + (full_position_y - empty_position_y) * GameManager.GM.ResourceManager.Death_UI_Amount/GameManager.GM.ResourceManager.Max_Death_UI_Amount,
                death_bar_mask.transform.localPosition.z
            );
    }

    void UpdateDeathText()
    {
        death_number_text.text = 
            GameManager.GM.ResourceManager.Death_UI_Amount + "/" + GameManager.GM.ResourceManager.Max_Death_UI_Amount;
    }
}
