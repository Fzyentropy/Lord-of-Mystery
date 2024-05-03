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
        
        ReplaceKeywordWithIcon(knowledge_content,"Physical_Energy", physicalEnergyIconPrefab);
        ReplaceKeywordWithIcon(knowledge_content,"Spirituality_Infused_Material", spiritualityInfusedMaterialIconPrefab);
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
    

    /*void InstantiateIcons(string keyword, GameObject iconPrefab)
    {
        // Update the TMP text to ensure we have the latest info.
        knowledge_content.ForceMeshUpdate();
        TMP_TextInfo textInfo = knowledge_content.textInfo;

        int index = knowledge_content.text.IndexOf(keyword);
        while (index != -1)
        {
            
            // Calculate the position to place the icon.
            int charIndex = textInfo.characterInfo[index].index;
            Vector3 iconPosition = CalculateIconPosition(textInfo, charIndex, keyword.Length);

            // Instantiate the icon prefab at the calculated position
            Instantiate(iconPrefab, iconPosition, Quaternion.identity, knowledge_content.transform);

            // Move to the next occurrence
            index = knowledge_content.text.IndexOf(keyword, index + keyword.Length);
        }
    }*/
    
    void InstantiateIcons(string keyword, GameObject iconPrefab)
    {
        knowledge_content.ForceMeshUpdate();
        TMP_TextInfo textInfo = knowledge_content.textInfo;

        int index = knowledge_content.text.IndexOf(keyword);
        while (index != -1)
        {
            if (index < textInfo.characterCount)
            {
                // Calculate the icon's position based on the midpoint of the keyword
                TMP_CharacterInfo charInfo = textInfo.characterInfo[index];
                Vector3 charMidBaseLine = (charInfo.bottomLeft + charInfo.topRight) / 2;
                charMidBaseLine = knowledge_content.transform.TransformPoint(charMidBaseLine);

                Vector2 anchoredPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    knowledge_content.rectTransform, 
                    charMidBaseLine, 
                    knowledge_content.canvas.worldCamera, 
                    out anchoredPosition);

                // Instantiate the icon prefab at the calculated position
                GameObject icon = Instantiate(iconPrefab, knowledge_content.transform);
                // Transform iconRT = icon.GetComponent<Transform>();
                // iconRT.anchoredPosition = anchoredPosition;
                // iconRT.localRotation = Quaternion.identity;
                // iconRT.localScale = Vector3.one; // Adjust scale as needed
            }

            // Move to the next occurrence
            index = knowledge_content.text.IndexOf(keyword, index + keyword.Length);
        }
    }


    Vector3 CalculateIconPosition(TMP_TextInfo textInfo, int charIndex, int keywordLength)
    {
        
        
        // This example uses the midpoint of the keyword for icon placement.
        int midPoint = charIndex + keywordLength / 2;
        if (midPoint >= textInfo.characterCount)
            midPoint = textInfo.characterCount - 1;

        var charInfo = textInfo.characterInfo[midPoint];
        Vector3 charMidBaseLine = (charInfo.bottomLeft + charInfo.bottomRight) / 2;
        Vector3 iconPosition = knowledge_content.transform.TransformPoint(charMidBaseLine);

        // Optionally adjust the Y-position or other aspects of the icon placement
        iconPosition.y += charInfo.ascender / 2;  // Adjust vertically to center the icon with the text line
        return iconPosition;
    }


    
    



}
