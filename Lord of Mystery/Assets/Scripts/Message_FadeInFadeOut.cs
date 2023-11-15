using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Message_FadeInFadeOut : MonoBehaviour
{
    public float fadeInDuration = 1.0f;
    public float fadeOutDuration = 1.0f;
    public float moveDistance = 50.0f;

    private Vector3 originalPosition;
    private CanvasGroup canvasGroup;
    
    private bool ableToClick = false;  // 防止点击一下 panel 生成后直接消失
 
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        originalPosition = transform.localPosition;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        // Move the object to the right
        Vector3 startPosition = originalPosition + Vector3.right * moveDistance;
        transform.localPosition = startPosition;
        float elapsedTime = 0;

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

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ableToClick) // Left mouse button
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
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
}
