using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
// using UnityEngine.UIElements;
using UnityEngine.UI;

public class Knowledge_Panel_Feature : MonoBehaviour
{

    public Knowledge _Knowledge;

    public TMP_Text knowledge_label;
    public TMP_Text knowledge_content;
    public Image knowledge_image;

    
    public GameObject physicalEnergyIconPrefab;
    public GameObject spiritualityInfusedMaterialIconPrefab;
    
    private void Start()
    {
        Check_If_Knowledge_Set_Well();
        Set_Knowledge();
    }
    

    void Check_If_Knowledge_Set_Well()
    {
        if (_Knowledge == null)
        {
            throw new NotImplementedException("Knowledge 为空");
        }
    }

    void Set_Knowledge()
    {
        knowledge_label.text = _Knowledge.Label;
        knowledge_image.sprite = Resources.Load<Sprite>("Image/" + _Knowledge.Image);
        knowledge_content.text = _Knowledge.Knowledge_Content;
        
        if (GameManager.currentLanguage == GameManager.Language.English)        // 设置语言
        {
            knowledge_label.font = GameManager.Font_English;
            knowledge_content.font = GameManager.Font_English;
            // card_label.fontSize = 8;
        }
        else if (GameManager.currentLanguage == GameManager.Language.Chinese)
        {
            knowledge_label.font = GameManager.Font_Chinese;
            knowledge_content.font = GameManager.Font_Chinese;
            
            knowledge_label.enableAutoSizing = true;
            knowledge_label.fontSizeMax = 25f;
            knowledge_label.fontSizeMin = 15f;

            knowledge_content.fontSize = 15f;
            knowledge_content.characterSpacing = -2;
            knowledge_content.lineSpacing = -20;
        }
        
        ReplaceKeywordWithIcon(knowledge_content,"P_E_0", physicalEnergyIconPrefab);
        ReplaceKeywordWithIcon(knowledge_content,"S_I_M", spiritualityInfusedMaterialIconPrefab);
    }
    
    
    void ReplaceKeywordWithIcon(TMP_Text knowledgeText, string keyword, GameObject iconPrefab)
    {
        int index = knowledgeText.text.IndexOf(keyword);
        while (index != -1)
        {
            // Replace keyword with space
            string whitespace = new string(' ', keyword.Length);
            knowledgeText.text = knowledgeText.text.Substring(0, index) + whitespace + knowledgeText.text.Substring(index + keyword.Length);
            knowledgeText.ForceMeshUpdate();

            // Get the position for the icon
            TMP_TextInfo textInfo = knowledgeText.textInfo;
            if (index < textInfo.characterCount)
            {
                var charInfo = textInfo.characterInfo[index];
                Vector3 charMidBaseLine = (charInfo.bottomLeft + charInfo.bottomRight) / 2;
                Vector3 iconPosition = knowledgeText.transform.TransformPoint(charMidBaseLine);

                // Instantiate the icon prefab
                GameObject icon = Instantiate(iconPrefab, iconPosition, Quaternion.identity, knowledgeText.transform);
                RectTransform iconRT = icon.GetComponent<RectTransform>();
                iconRT.anchoredPosition3D = new Vector3(iconRT.anchoredPosition3D.x, iconRT.anchoredPosition3D.y, 0);
                // iconRT.sizeDelta = new Vector2(30, 50); // Set size according to your needs
            }

            // Look for the next occurrence
            index = knowledgeText.text.IndexOf(keyword, index + whitespace.Length);
        }
    }
    

    



}
