using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static int Number_SoulPiece;

    public static int Amount_Fund;
    public static int Amount_PhysicalStrength;
    public static int Amount_Spirit;
    public static int Amount_Soul;
    public static int Amount_SpiritualityInfusedMaterial;
    public static int Amount_Knowledge;
    
    public static int Amount_Belief;
    public static int Amount_Madness;
    public static int Amount_Putrefaction;
    public static int Amount_Godhood;

    
    

    /////////////////////////////////////////////////////////////////////////////////////////

    
    public static GameManager GM;
    public Card_Loader CardLoader;

    public GameObject message_Panel;
    public GameObject Card_Location_Prefab;


    private void Awake()
    {
        SetGameManager();
    }

    private void Start()
    {
        SetCardLoader();
    }
    
    
    /////////////////////////////////////////////////////////////////////////////////////////   Initial Set up
    
    void SetGameManager()
    {
        GM = this;
    }

    void SetCardLoader()
    {
        if (CardLoader == null)
        {
            CardLoader = GameObject.Find("Card_Loader").GetComponent<Card_Loader>();
        }
    }


    public void Generate_Card_Location(string id)
    {
        GameObject cardLocation = Instantiate(Card_Location_Prefab, new Vector3(15,-10,0), Quaternion.identity);
        cardLocation.GetComponent<Card_Location_Feature>()._cardLocation = CardLoader.Get_Card_Location_By_Id(id);
    }
    
    public void Generate_Message(string id)
    {
        GameObject messagePanel = Instantiate(message_Panel,GameObject.Find("Canvas").transform);
        messagePanel.GetComponent<Message_Feature>()._message = CardLoader.Get_Message_By_Id(id);
    }



























}