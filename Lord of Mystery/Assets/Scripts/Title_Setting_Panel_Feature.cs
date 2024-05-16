using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Title_Setting_Panel_Feature : MonoBehaviour
{

    public TMP_Text panel_label;
    public TMP_Text panel_description;
    
    
    void Start()
    {
        if (GameManager.currentLanguage == GameManager.Language.Chinese)
        {
            Set_Label("设置");
            Set_Description("调整变量，修改命运之线，为即将到来的冒险设定舞台。每一个选择都改变着你的命运之织，为你迎接未知的谜团做好准备。");
        }
        else if (GameManager.currentLanguage == GameManager.Language.English)
        {

            Set_Label("Setting");
            Set_Description("Adjust the variables, tweak the threads of fate, and set the stage for the adventures to come. Every choice alters the tapestry of your destiny, preparing you for the mysteries that await.");
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void Set_Label(string label)
    {
        panel_label.text = label;
        
        if (GameManager.currentLanguage == GameManager.Language.English)        // 设置语言
        {
            panel_label.font = GameManager.Font_English;
            // card_label.fontSize = 8;
        }
        else if (GameManager.currentLanguage == GameManager.Language.Chinese)
        {
            panel_label.font = GameManager.Font_Chinese;
            panel_label.fontSize = 10;
        }
    }

    public void Set_Description(string description)
    {
        panel_description.text = description;
        
        if (GameManager.currentLanguage == GameManager.Language.English)        // 设置语言
        {
            panel_label.font = GameManager.Font_English;
            // card_label.fontSize = 8;
        }
        else if (GameManager.currentLanguage == GameManager.Language.Chinese)
        {
            panel_description.font = GameManager.Font_Chinese;
            panel_description.fontSize = 7;
            panel_description.characterSpacing = -2;
            panel_description.lineSpacing = -20;
        }
        
    }
}
