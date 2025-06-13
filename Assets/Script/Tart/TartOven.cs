using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TartOven : MonoBehaviour
{
    [Header("���� ���� ��ư")]
    [SerializeField] private Button startOvenButton;

    [Header("���� �ܰ� ��ư")]
    [SerializeField] private Button nextButton;

    [Header("���� UI �г� (�̹����� ��� ������ ��)")]
    [SerializeField] private GameObject ovenPanel;

    [Header("���� �ð� (��)")]
    [SerializeField] private float bakeDuration = 3f;

    [Header("���� �߿� ����� ���� ����� (�ɼ�)")]
    [SerializeField] private AudioClip bakingLoopClip;

    [Header("���� �Ϸ� �� ����� �����")]
    [SerializeField] private AudioClip finishBakeClip;

    [Header("���� �۵� �� ���� ������ �̹��� 1 (Image �Ǵ� RawImage)")]
    [SerializeField] private Graphic bakingEffectGraphic1;

    [Header("���� �۵� �� ȸ���� �̹��� 2")]
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
        // �̹���2�� �׻� ���̵�, ���� �۵� �߿��� ȸ��
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

        // rotatingImage2�� �׻� ���̹Ƿ� SetActive ���� ����

        // ��ư ������ ����
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
            Debug.Log("TartOven: �� �� �̻� ���� �� ���� ���� ���� (UI�� ����)");
        }

        // ������ �׻� �۵�
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

        Debug.Log($"TartOven: {bakeDuration}�� ���. startClickCount={startClickCount}, ���� ����={isFailed}");
    }

    private void OnNextClicked()
    {
        Debug.Log($"TartOven: ���� ��ư Ŭ����. ���� ��� �� {(isFailed ? "����" : "����")}");

        startOvenButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        if (ovenPanel != null)
            ovenPanel.SetActive(false);

        if (bakingEffectGraphic1 != null)
            SetGraphicAlpha(bakingEffectGraphic1, 0f);

        isBaking = false;

        tartManagerRef?.OnOvenComplete(!isFailed); // ���и� false, �����̸� true ����
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
