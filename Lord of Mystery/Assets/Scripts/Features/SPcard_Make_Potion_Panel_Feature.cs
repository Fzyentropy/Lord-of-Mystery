using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPcard_Make_Potion_Panel_Feature : MonoBehaviour
{
    
    // 生成此 panel 的 Make Potion 卡
    public GameObject attached_make_potion_card;
    public SPcard_Make_Potion_Feature attached_make_potion_card_feature;
    
    
    
    
    
    public void Set_Attached_Make_Potion_Card(GameObject attachedCard)     // 设置生成此 panel 的卡牌指代，顺便设置 feature 指代，实例化 Panel 时从外部设置
    {                                                                   // 只能从外部设置，因为从 panel 自身无法查找到生成 panel 的卡是哪张
        attached_make_potion_card = attachedCard;
        attached_make_potion_card_feature = attached_make_potion_card.GetComponent<SPcard_Make_Potion_Feature>();
    }
    
    
}
