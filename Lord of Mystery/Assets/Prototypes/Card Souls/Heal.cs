using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public SpriteRenderer spr;
    public Color normalColor = new Color(0.4f,0.4f,0.4f);
    public Color mouseOverColor = new Color(0.6F, 0.6F, 0.6F);


    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.color = normalColor;
    }

    private void OnMouseEnter()
    {
        spr.color = mouseOverColor;
    }

    private void OnMouseExit()
    {
        spr.color = normalColor;
    }

    private void OnMouseDown()
    {
        if (!CardSouls_Manager.CardSoulsManager.isPlayerDoingAction)
        {
            StartCoroutine(CardSouls_Manager.CardSoulsManager.Heal());
        }
    }
}
