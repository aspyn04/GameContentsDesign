using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class SceneFadeIn : MonoBehaviour
{
    public static SceneFadeIn Instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(FadeAndLoad(sceneIndex));
    }

    IEnumerator FadeAndLoad(int sceneIndex)
    {
        yield return StartCoroutine(Fade(1, 0)); // Fade out
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}