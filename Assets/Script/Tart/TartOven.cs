using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TartOven : MonoBehaviour
{
    [Header("오븐 시작 버튼")]
    [SerializeField] private Button startOvenButton;

    [Header("다음 단계 버튼")]
    [SerializeField] private Button nextButton;

    [Header("오븐 UI 패널 (이미지는 계속 보여야 함)")]
    [SerializeField] private GameObject ovenPanel;

    [Header("굽기 시간 (초)")]
    [SerializeField] private float bakeDuration = 3f;

    [Header("굽기 중에 재생할 루프 오디오 (옵션)")]
    [SerializeField] private AudioClip bakingLoopClip;

    [Header("굽기 완료 시 재생할 오디오")]
    [SerializeField] private AudioClip finishBakeClip;

    [Header("오븐 작동 시 알파 조절할 이미지 1 (Image 또는 RawImage)")]
    [SerializeField] private Graphic bakingEffectGraphic1;

    [Header("오븐 작동 시 회전할 이미지 2")]
    [SerializeField] private GameObject rotatingImage2;

    private TartManager tartManagerRef;
    private AudioSource audioSource;
    private int startClickCount = 0;
    private bool isBaking = false;
    private bool isFailed = false;
    private Coroutine alphaCoroutine;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        // 이미지2는 항상 보이되, 오븐 작동 중에만 회전
        if (isBaking && rotatingImage2 != null)
        {
            rotatingImage2.transform.Rotate(Vector3.forward * -120 * Time.deltaTime);
        }
    }

    public void Init(TartManager manager)
    {
        tartManagerRef = manager;
        startClickCount = 0;
        isFailed = false;
        isBaking = false;

        if (ovenPanel != null)
            ovenPanel.SetActive(true);

        startOvenButton.gameObject.SetActive(true);
        startOvenButton.interactable = true;
        nextButton.gameObject.SetActive(true);
        nextButton.interactable = false;

        if (bakingEffectGraphic1 != null)
            SetGraphicAlpha(bakingEffectGraphic1, 0f);

        // rotatingImage2는 항상 보이므로 SetActive 하지 않음

        // 버튼 리스너 설정
        startOvenButton.onClick.RemoveAllListeners();
        startOvenButton.onClick.AddListener(OnStartOvenClicked);

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);
    }

    private void OnStartOvenClicked()
    {
        startClickCount++;

        if (startClickCount >= 2 && !isFailed)
        {
            isFailed = true;
            Debug.Log("TartOven: 두 번 이상 누름 → 실패 상태 저장 (UI는 유지)");
        }

        // 오븐은 항상 작동
        startOvenButton.interactable = false;

        if (bakingLoopClip != null)
        {
            audioSource.clip = bakingLoopClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        if (bakingEffectGraphic1 != null)
        {
            if (alphaCoroutine != null) StopCoroutine(alphaCoroutine);
            alphaCoroutine = StartCoroutine(FadeAlpha(bakingEffectGraphic1, 0f, 125f, 0.2f));
        }

        isBaking = true;
        StartCoroutine(BakeCoroutine());
    }

    private IEnumerator BakeCoroutine()
    {
        yield return new WaitForSeconds(bakeDuration);

        if (audioSource.isPlaying)
            audioSource.Stop();

        if (finishBakeClip != null)
            audioSource.PlayOneShot(finishBakeClip);

        startOvenButton.interactable = true;
        nextButton.interactable = true;

        if (bakingEffectGraphic1 != null)
        {
            if (alphaCoroutine != null) StopCoroutine(alphaCoroutine);
            alphaCoroutine = StartCoroutine(FadeAlpha(bakingEffectGraphic1, 125f, 0f, 0.5f));
        }

        isBaking = false;

        Debug.Log($"TartOven: {bakeDuration}초 경과. startClickCount={startClickCount}, 실패 여부={isFailed}");
    }

    private void OnNextClicked()
    {
        Debug.Log($"TartOven: 다음 버튼 클릭됨. 최종 결과 → {(isFailed ? "실패" : "성공")}");

        startOvenButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        if (ovenPanel != null)
            ovenPanel.SetActive(false);

        if (bakingEffectGraphic1 != null)
            SetGraphicAlpha(bakingEffectGraphic1, 0f);

        isBaking = false;

        tartManagerRef?.OnOvenComplete(!isFailed); // 실패면 false, 성공이면 true 전달
    }

    private IEnumerator FadeAlpha(Graphic graphic, float fromAlpha255, float toAlpha255, float duration)
    {
        Color color = graphic.color;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            float alpha = Mathf.Lerp(fromAlpha255, toAlpha255, t) / 255f;
            graphic.color = new Color(color.r, color.g, color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        graphic.color = new Color(color.r, color.g, color.b, toAlpha255 / 255f);
    }

    private void SetGraphicAlpha(Graphic graphic, float alpha255)
    {
        Color c = graphic.color;
        graphic.color = new Color(c.r, c.g, c.b, alpha255 / 255f);
    }
}
