using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
    public TMP_Text Amt_Fund;
    public TMP_Text Amt_PhysicalStrength;
    public TMP_Text Amt_Spirit;
    public TMP_Text Amt_Soul;
    public TMP_Text Amt_SpiritualInfusedMaterial;
    public TMP_Text Amt_Knowledge;
    
    public TMP_Text Amt_Belief;
    public TMP_Text Amt_Madness;
    public TMP_Text Amt_Putrefaction;
    public TMP_Text Amt_Godhood;
    

    // Update is called once per frame
    void Update()
    {
        Amt_Fund.text = GameManager.Amount_Fund.ToString();
        Amt_PhysicalStrength.text = GameManager.Amount_PhysicalStrength.ToString();
        Amt_Spirit.text = GameManager.Amount_Spirit.ToString();
        Amt_Soul.text = GameManager.Amount_Soul.ToString();
        Amt_SpiritualInfusedMaterial.text = GameManager.Amount_SpiritualityInfusedMaterial.ToString();
        Amt_Knowledge.text = GameManager.Amount_Knowledge.ToString();
        
        Amt_Belief.text = GameManager.Amount_Belief.ToString();
        Amt_Madness.text = GameManager.Amount_Madness.ToString();
        Amt_Putrefaction.text = GameManager.Amount_Putrefaction.ToString();
        Amt_Godhood.text = GameManager.Amount_Godhood.ToString();
    }
}
