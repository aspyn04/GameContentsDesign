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

    private TartManager tartManagerRef;
    private AudioSource audioSource;

    // ���� ���� ��ư�� ���� Ƚ���� ����
    private int startClickCount = 0;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// TartManager���� ���� �ܰ踦 ������ �� ȣ��
    /// </summary>
    public void Init(TartManager manager)
    {
        tartManagerRef = manager;
        startClickCount = 0;

        // �г� ���̱�
        if (ovenPanel != null)
            ovenPanel.SetActive(true);

        // ��ư ���� �ʱ�ȭ
        startOvenButton.gameObject.SetActive(true);
        startOvenButton.interactable = true;
        nextButton.gameObject.SetActive(true);
        nextButton.interactable = false;

        // ������ ����
        startOvenButton.onClick.RemoveAllListeners();
        startOvenButton.onClick.AddListener(OnStartOvenClicked);

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);
    }

    private void OnStartOvenClicked()
    {
        startClickCount++;

        // �� �� �̻� ������ ���� ó��
        if (startClickCount >= 2)
        {
            Debug.Log("TartOven: ���� ��ư�� �� �� ���� ���� ó��");
            FailOven();
            return;
        }

        // ù Ŭ��: ���� ����
        startOvenButton.interactable = false;

        if (bakingLoopClip != null)
        {
            audioSource.clip = bakingLoopClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        StartCoroutine(BakeCoroutine());
    }

    private IEnumerator BakeCoroutine()
    {
        yield return new WaitForSeconds(bakeDuration);

        // ���� ���� �ߴ�
        if (audioSource.isPlaying)
            audioSource.Stop();

        // �Ϸ� ���� ���
        if (finishBakeClip != null)
            audioSource.PlayOneShot(finishBakeClip);

        // ���� ��ư ǥ��
        nextButton.interactable = true;

        Debug.Log($"TartOven: {bakeDuration}�� ���, ���� �ܰ� ��ư Ȱ��ȭ��");
    }

    private void OnNextClicked()
    {
        // ���� ó��
        Debug.Log("TartOven: ���� ��ư Ŭ��, ���� �ܰ� ���� ó��");
        CompleteOven();
    }

    private void FailOven()
    {
        // UI ����
        startOvenButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        if (ovenPanel != null)
            ovenPanel.SetActive(false);

        // ���� �˸�
        tartManagerRef?.OnOvenComplete(false);
    }

    private void CompleteOven()
    {
        // UI ����
        startOvenButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        if (ovenPanel != null)
            ovenPanel.SetActive(false);

        // ���� �˸�
        tartManagerRef?.OnOvenComplete(true);
    }
}