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
    
    // 两条 Line
    public GameObject Line_Up;
    public GameObject Line_Down;

    // panel 指代
    public bool isPanelOpen = false;
    public GameObject current_panel;

    // message 指代
    public Message_FadeInFadeOut current_message;

    // 数值参数
    public Vector3 panel_original_scale = Vector3.one;





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
        panel_original_scale = panel_original_transform.lossyScale;
        
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
            current_panel.GetComponent<Card_Location_Panel_Feature>().Return_Body_Part();
            current_panel.GetComponent<Card_Location_Panel_Feature>().Play_Panel_Close_Audio();
        }

        if (current_panel.GetComponent<SPcard_Make_Potion_Panel_Feature>() != null)
        {
            current_panel.GetComponent<SPcard_Make_Potion_Panel_Feature>().Return_Absorbed_Resource();
            GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Panel_Close);
        }
        
        Destroy(current_panel);
        GameManager.GM.PanelManager.current_panel = null;
        GameManager.GM.PanelManager.isPanelOpen = false;        // 重新设置 panel 是否打开 为否
        
        GameManager.GM.InputManager.is_calling_exit_panel = false;      // 将 Exit Panel 打开 参数设置为 否

    }


    public void Draw_Sequence_Frame()
    {
        StartCoroutine(Sequence_Frame_Fade_In());
    }

    private IEnumerator Sequence_Frame_Fade_In()
    {
        SpriteRenderer[] all_frame_edge = GameObject.Find("Sequence_Frame").GetComponentsInChildren<SpriteRenderer>();
        
        ///// Fade in
        
        float timeInterval = 0.05f;  // 设置每步渐变的时间间隔
        float duration = 3f;
        float remainingTime = duration;
        
        // 先设置为白色透明
        foreach (var sprite in all_frame_edge)
        {
            sprite.color = new Color(1,1,1,0);
        }
        
        // 等待 6s，与 Level 10 一起出现
        yield return new WaitForSeconds(6f);

        // 渐变增加 a值
        while (remainingTime > 0)
        {
            // 等待 timeInterval时间长度 - 0.05 秒
            yield return new WaitForSeconds(timeInterval);

            foreach (var sprite in all_frame_edge)
            {
                sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,sprite.color.a + timeInterval/duration);
            }
            
            remainingTime -= timeInterval;
        }
        
        // 最终设置为白色
        foreach (var sprite in all_frame_edge)
        {
            sprite.color = Color.white;
        }
    }

    public void Expand_Line(float delay, float length)
    {
        StartCoroutine(Set_Line_Length(delay, length));
    }

    public IEnumerator Set_Line_Length(float delay, float length)
    {
        LineRenderer line_renderer_up = Line_Up.GetComponent<LineRenderer>();
        LineRenderer line_renderer_down = Line_Down.GetComponent<LineRenderer>();

        float totalTime = 2f;
        float interval = 0.05f;
        float Length_to_expand = length;
        float step = length * interval / totalTime;


        yield return new WaitForSeconds(delay);
        
        while (Length_to_expand > 0)
        {

            line_renderer_up.SetPosition(1, new Vector3(
                line_renderer_up.GetPosition(1).x + step,
                line_renderer_up.GetPosition(1).y, 
                line_renderer_up.GetPosition(1).z));
            
            line_renderer_down.SetPosition(1, new Vector3(
                line_renderer_down.GetPosition(1).x + step,
                line_renderer_down.GetPosition(1).y, 
                line_renderer_down.GetPosition(1).z));

            Length_to_expand -= step;
            yield return new WaitForSeconds(interval);
        }
        
        
    }
    
    
    




}
