using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFadeOut : MonoBehaviour
{
    public static SceneFadeOut Instance;

    [SerializeField] private Image fadeImage;             // ������ �̹��� (Canvas �ȿ� ����)
    [SerializeField] private float fadeDuration = 1f;     // ���̵� �ð�

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);  // ���� �� ���� ȭ�� ������
            fadeImage.color = new Color(0, 0, 0, 1);
            StartCoroutine(Fade(1f, 0f, () => fadeImage.gameObject.SetActive(false))); // ���� ȭ�� �� ���� (���̵� ��)
        }
    }

    public void FadeToScene(int sceneIndex)
    {
        fadeImage.gameObject.SetActive(true); // �� ��ȯ �� ���� ȭ�� ���̰�
        StartCoroutine(Fade(0f, 1f, () => SceneManager.LoadScene(sceneIndex))); // ���� �� ���� ȭ�� �� �� ��ȯ
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
        onComplete?.Invoke(); // ������ ���� �ݹ� ����
    }
}
