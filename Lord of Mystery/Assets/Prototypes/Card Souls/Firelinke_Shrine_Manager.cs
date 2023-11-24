using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Firelinke_Shrine_Manager : MonoBehaviour
{
    public static Firelinke_Shrine_Manager FSM;
    public GameObject BlackScreen;
    public bool isFading = false;

    public AudioSource fireLinkShrineMusic;
    
    
    void Start()
    {
        FSM = this;
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        isFading = true;
        
        BlackScreen.SetActive(true);

        Image black = BlackScreen.GetComponent<Image>();
        
        float elapsedTime = 0.05f;
        float elapsedStep = 0.03f;

        while (black.color.a > 0.05)
        {
            black.color = new Color(0, 0, 0, black.color.a - elapsedStep);
            yield return new WaitForSeconds(elapsedTime);
        }
        black.color = Color.clear;
        
        // yield return new WaitForSeconds(1f);
        fireLinkShrineMusic.Play();

        isFading = false;
    }

    public IEnumerator FadeOut(string sceneToLoad)
    {
        isFading = true;
        
        float volumeStep = 0.03f;
        float screenStep = 0.07f;
        float timeStep = 0.05f;
        Image black = BlackScreen.GetComponent<Image>();

        while (fireLinkShrineMusic.volume > 0.05 || black.color.a < 0.95)
        {
            fireLinkShrineMusic.volume -= volumeStep;
            black.color = new Color(0, 0, 0, black.color.a + screenStep);
            if (fireLinkShrineMusic.volume <= 0.05)
            {
                fireLinkShrineMusic.volume = 0;
            }

            if (black.color.a >= 0.95)
            {
                black.color = Color.black;
            }

            yield return new WaitForSeconds(timeStep);
        }

        yield return new WaitForSeconds(2f);
        
        LoadScene(sceneToLoad);
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
