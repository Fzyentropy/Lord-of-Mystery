using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class Message_FadeInFadeOut : MonoBehaviour,IPointerDownHandler
{
    public float fadeInDuration = 1.0f;
    public float fadeOutDuration = 1.0f;
    public float moveDistance = 50.0f;

    private Vector3 originalPosition;
    private CanvasGroup canvasGroup;
    
    private bool ableToClick = false;  // 防止点击一下 panel 生成后直接消失
    
    // Mis


    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        originalPosition = transform.localPosition;
        

        StartCoroutine(FadeIn());
        StartCoroutine(Close_If_Not_Click_In_XX_Sec());
    }
    
    

    public IEnumerator FadeIn()
    {
        
        // 音效
        GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Message_Showup);
        
        
        if (GameManager.GM.PanelManager.current_message != null)
        {
            Message_FadeInFadeOut msg = GameManager.GM.PanelManager.current_message;
            StartCoroutine(msg.FadeOut());
        }
        GameManager.GM.PanelManager.current_message = this;

        // Move the object to the right
        Vector3 startPosition = originalPosition + Vector3.right * moveDistance;
        transform.localPosition = startPosition;
        float elapsedTime = 0;

        // yield return new WaitUntil(() => GameManager.GM.PanelManager.current_message == null);

        while (elapsedTime < fadeInDuration)
        {
            float progress = elapsedTime / fadeInDuration;
            canvasGroup.alpha = progress;
            transform.localPosition = Vector3.Lerp(startPosition, originalPosition, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
        transform.localPosition = originalPosition;

        ableToClick = true;

    }

    

    public IEnumerator FadeOut()
    {
        
        // 音效
        GameManager.GM.AudioManager.Play_AudioSource(GameManager.GM.AudioManager.SFX_Message_Fadeout);
        
        
        GameManager.GM.PanelManager.current_message = null;
        
        Vector3 endPosition = originalPosition - Vector3.right * moveDistance;
        float elapsedTime = 0;

        while (elapsedTime < fadeOutDuration)
        {
            float progress = elapsedTime / fadeOutDuration;
            canvasGroup.alpha = 1 - progress;
            transform.localPosition = Vector3.Lerp(originalPosition, endPosition, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
        
    }


    public IEnumerator Close_If_Not_Click_In_XX_Sec()
    {
        float close_time = 20f;

        yield return new WaitForSeconds(close_time);
        
        StartCoroutine(FadeOut());
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(FadeOut());
    }
}
