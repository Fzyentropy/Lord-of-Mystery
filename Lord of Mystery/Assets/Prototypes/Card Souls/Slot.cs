using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{

    public int x;
    public int y;

    public bool isAble = true;
    public SpriteRenderer spr;
    public SpriteRenderer edgeSpr;
    public Color normalColor = new Color(0.4f,0.4f,0.4f);
    public Color mouseOverColor = new Color(0.6F, 0.6F, 0.6F);
    

    private void Start()
    {
        if (spr == null)
            spr = GetComponent<SpriteRenderer>();
        if (edgeSpr == null)
            edgeSpr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        CheckIfNextToPlayer();
        Highlight();
    }
    
    
    // 判断是否在 player 相邻
    public void CheckIfNextToPlayer()
    {
        if (CardSouls_Manager.playerX == x)
        {
            if (CardSouls_Manager.playerY == y + 1 || CardSouls_Manager.playerY == y - 1)
            {
                isAble = true;
            }
            else
            {
                isAble = false;
            }
        }
        else if (CardSouls_Manager.playerX == x + 1 || CardSouls_Manager.playerX == x - 1) 
        {
            if (CardSouls_Manager.playerY == y)
            {
                isAble = true;
            }
            else
            {
                isAble = false;
            }
        }
        else
        {
            isAble = false;
        }
    }
    

    // 高亮方法
    public void Highlight()
    {
        if (isAble && !CardSouls_Manager.CardSoulsManager.isPlayerDoingAction)
        {
            edgeSpr.color = Color.white;
        }
        else
        {
            edgeSpr.color = Color.clear;
        }
    }
    
    // 点击方法

    private void OnMouseOver()
    {
        if (isAble && !CardSouls_Manager.CardSoulsManager.isPlayerDoingAction)
        {
            spr.color = mouseOverColor;
        }
    }

    private void OnMouseExit()
    {
        spr.color = normalColor;
    }

    private void OnMouseDown()
    {
        if (isAble && !CardSouls_Manager.CardSoulsManager.isPlayerDoingAction)
        {
            StartCoroutine(CardSouls_Manager.CardSoulsManager.MoveToSlot(this));
        }
    }
}
