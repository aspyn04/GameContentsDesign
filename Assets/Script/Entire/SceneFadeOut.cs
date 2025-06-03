using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFadeOut : MonoBehaviour
{
    public static SceneFadeOut Instance;

    [SerializeField] private Image fadeImage;             // 검은색 이미지 (Canvas 안에 있음)
    [SerializeField] private float fadeDuration = 1f;     // 페이드 시간

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);  // 시작 시 검은 화면 보여줌
            fadeImage.color = new Color(0, 0, 0, 1);
            StartCoroutine(Fade(1f, 0f, () => fadeImage.gameObject.SetActive(false))); // 검은 화면 → 투명 (페이드 인)
        }
    }

    public void FadeToScene(int sceneIndex)
    {
        fadeImage.gameObject.SetActive(true); // 씬 전환 시 검은 화면 보이게
        StartCoroutine(Fade(0f, 1f, () => SceneManager.LoadScene(sceneIndex))); // 투명 → 검은 화면 후 씬 전환
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, System.Action onComplete = null)
    {
        float elapsed = 0f;
        Color originalColor = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, endAlpha);
        onComplete?.Invoke(); // 끝나고 나서 콜백 실행
    }
}
