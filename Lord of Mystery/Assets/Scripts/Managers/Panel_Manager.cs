using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

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

    // panel 指代
    public bool isPanelOpen = false;
    public GameObject current_panel;

    // message 指代
    





    public void Set_Panel_Reference_And_Scale_Up(GameObject panel)
    {
        current_panel = panel;
        StartCoroutine(Scale_Up_Panel(panel));
    }

    IEnumerator Scale_Up_Panel(GameObject panel)
    {
        float scaleDuration = 0.2f;     // Scale 的时长
        
        // 获取到 Game Object 的原始 transform 和 所有 spriteRenderer 与 TMP text 的数据
        Transform panel_original_transform = panel.transform;
        Vector3 panel_original_scale = panel_original_transform.lossyScale;
        
        SpriteRenderer[] original_spriterenderers = panel.GetComponentsInChildren<SpriteRenderer>();
        Dictionary<SpriteRenderer, Color> original_spriterenderer_color = new Dictionary<SpriteRenderer, Color>() { };
        foreach (var spriterenderer in original_spriterenderers)
        {
            original_spriterenderer_color.Add(spriterenderer,spriterenderer.color);
        }
        
        TMP_Text[] original_texts = panel.GetComponentsInChildren<TMP_Text>();
        Dictionary<TMP_Text, Color> original_text_color = new Dictionary<TMP_Text, Color>() { };
        foreach (var tmp_text in original_texts)
        {
            original_text_color.Add(tmp_text,tmp_text.color);
        }
        
        
        // 将 Game Object 的原始 transform 和 所有 spriteRenderer 与 TMP text 的数据，设置为开始 scale up 的值
        panel.transform.localScale = new Vector3(panel_original_transform.localScale.x * 0.6f, panel_original_transform.localScale.y * 0.6f,panel_original_transform.localScale.z);
        foreach (var spriterenderer in original_spriterenderers)
        {
            spriterenderer.color = Color.clear;
        }
        foreach (var tmp_text in original_texts)
        {
            tmp_text.color = Color.clear;
        }       
        
        
        // 真正将 panel Scale Up，透明度变化
        panel.transform.DOScale(panel_original_scale, scaleDuration);
        foreach (var spriterenderer in original_spriterenderers)
        {
            spriterenderer.DOColor(original_spriterenderer_color[spriterenderer], scaleDuration);
        }
        foreach (var tmp_text in original_texts)
        {
            tmp_text.DOColor(original_text_color[tmp_text], scaleDuration);
        }
        
        yield return null;
    }
    
    
    public void Close_Current_Panel()
    {

        if (current_panel.GetComponent<Card_Location_Panel_Feature>() != null)
        {
            current_panel.GetComponent<Card_Location_Panel_Feature>().Return_Resource();
        }
        
        Destroy(current_panel);
        
        // 在此处添加 任何处理 有关返还资源的
        
    }
    
    
    
    




}
