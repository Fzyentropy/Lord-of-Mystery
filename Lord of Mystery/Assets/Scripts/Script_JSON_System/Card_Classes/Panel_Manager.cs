using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Manager : MonoBehaviour
{

    // 所有不同卡牌/功能的对应 Panel prefab
    public GameObject Panel_Card_Automatic;
    public GameObject Panel_Card_Location;
    public GameObject Panel_Sequence;
    public GameObject Panel_Function;
    
    // 不同卡牌/功能对应的 Panel 指代
    private GameObject automatic_card_panel;
    private GameObject location_card_panel;
    private GameObject sequence_card_panel;
    private GameObject function_panel;

    private bool isPanelOpen = false;
    private GameObject certain_panel;






    public void Open_Panel()
    {
        
        // 待补全各种卡牌、功能类型判定 TODO 

        /*if (gameObject.GetComponent<Card_Automatic_Feature>() != null)
        {
            
        }
        
        else 
        if (gameObject.GetComponent<Card_Action_Feature>() != null)
        {
            
        }
        
        else */
        
        if (gameObject.GetComponent<Card_Location_Feature>() != null)
        {
            Card_Location_Feature certain_location_card = gameObject.GetComponent<Card_Location_Feature>();
            // certain_panel = Instantiate(Panel_Card_Location)
            
            // certain_panel.text = 
            
            // 如何设置各个不同 panel？  每个不同 panel和对应卡牌是软绑定的。
        }
        
        
    }
    
    
    
    
    




}
