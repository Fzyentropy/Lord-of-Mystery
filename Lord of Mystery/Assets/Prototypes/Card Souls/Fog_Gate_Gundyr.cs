using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog_Gate_Gundyr : MonoBehaviour
{

    public SpriteRenderer spr;
    public Color normalColor = new Color(0.4f,0.4f,0.4f);
    public Color mouseOverColor = new Color(0.6F, 0.6F, 0.6F);
    

    private void OnMouseOver()
    {
        if (!Firelinke_Shrine_Manager.FSM.isFading)
            spr.color = mouseOverColor;
    }

    private void OnMouseExit()
    {
        spr.color = normalColor;
    }

    private void OnMouseDown()
    {
        if (!Firelinke_Shrine_Manager.FSM.isFading)
            StartCoroutine(Firelinke_Shrine_Manager.FSM.FadeOut("Prototype_CardSouls_Gundyr"));
    }
}
