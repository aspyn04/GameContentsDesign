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

    // Next ��ư�� Ȱ���� ���� ���� Ƚ�� ����
    private int nextClickCount = 0;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// TartManager���� ȣ���Ͽ� ���� �ܰ踦 �����մϴ�.
    /// ovenPanel�� �ݵ�� Ȱ�� ���¿��� �ϸ�, ��ư�鸸 ����ų� ��Ȱ��ȭ�մϴ�.
    /// </summary>
    public void Init(TartManager manager)
    {
        tartManagerRef = manager;
        nextClickCount = 0;

        // ���� �г� ��ü�� Ȱ��ȭ�� ä�� �ΰ�, ��ư�鸸 �ʱ� ���·� ����
        if (ovenPanel != null)
            ovenPanel.SetActive(true);

        startOvenButton.gameObject.SetActive(true);
        startOvenButton.interactable = true;

        startOvenButton.onClick.RemoveAllListeners();
        startOvenButton.onClick.AddListener(OnStartOvenClicked);

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);
    }

    private void OnStartOvenClicked()
    {
        // ���� ���� ��ư�� ���� �ڿ��� �� �̻� ������ ���ϰ� disable
        startOvenButton.interactable = false;

        // ���� ���� ���� ���
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

        // ���� �Ϸ� ���� ���
        if (finishBakeClip != null)
            audioSource.PlayOneShot(finishBakeClip);
            startOvenButton.interactable = true;

        // 1) "����" ��ư�� ���̰� Ȱ��ȭ
        nextButton.interactable = true;

        Debug.Log($"TartOven: {bakeDuration}�� ���, ���� �ܰ� ��ư Ȱ��ȭ��");
    }

    public void OnNextClicked()
    {
        // �ٴܰ� ���� ó��: �� �� �̻� ������ ����
        nextClickCount++;

        if (nextClickCount >= 2)
        {
            Debug.Log("TartOven: Next ��ư�� �� �� ������ ���� ó��");

            // ���� ��ư, ���� ��ư �� �� ��Ȱ��ȭ(�����)
            startOvenButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);

            // �г� �̹����� �״�� ���ܵӴϴ� (�̹����� �䱸����)
            tartManagerRef?.OnOvenComplete(false);
        }
        else
        {
            // ù ��° Ŭ�� �� �������� ����
            Debug.Log("TartOven: Next ��ư ù Ŭ��, ���� �ܰ� ���� ó��");

            // ���� ��ư, ���� ��ư �� �� ��Ȱ��ȭ(�����)
            startOvenButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);

            // �г� �̹����� �״�� ���ܵӴϴ�
            tartManagerRef?.OnOvenComplete(true);
        }
    }
}
