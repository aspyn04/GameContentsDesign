// DayCycleManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DayCycleManager : MonoBehaviour
{
    [Header("Day Cycle ����")]
    [SerializeField] private GameObject endOfDayPanel;
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private NPCManager npcManager;
    [SerializeField] private EndingManager endingManager;

    [Header("���̵�� Image (���� ȭ��)")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private float savedVolume;

    private void Start()
    {
        // BGM ��� �� ���� ���
        BGMManager.Instance?.PlayMusic();
        savedVolume = BGMManager.Instance?.GetMasterVolume() ?? 1f;

        // ������ ���� EndOfDayPanel�� fadeImage �� �� ���Ӵϴ�
        endOfDayPanel.SetActive(false);

        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayEnded += HandleDayEnded;

        StartCoroutine(StartDayRoutine());
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayEnded -= HandleDayEnded;
    }

    private IEnumerator StartDayRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (TimeManager.Instance == null || TimeManager.Instance.currentDay == 0)
            yield return null;

        int day = TimeManager.Instance.currentDay;
        if (cutsceneManager != null && cutsceneManager.HasCutsceneForDay(day))
        {
            Time.timeScale = 0f;
            yield return cutsceneManager.PlayCutscene(day);
            Time.timeScale = 1f;
        }

        npcManager.StartNPCLoop();
    }

    private void HandleDayEnded()
    {
        ResetAllUI();
        StartCoroutine(EndOfDaySequence());
    }

    private IEnumerator EndOfDaySequence()
    {

        //2) fadeImage �Ѱ� ���� 0���� ����
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0f);
        }

        var bgm = BGMManager.Instance;
        float startVol = bgm != null ? bgm.GetMasterVolume() : 0f;
        float elapsed = 0f;

        //3) ���̵�ƿ� (01) + BGM ���� ���̵�
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            fadeImage.color = new Color(0, 0, 0, t);
            if (bgm != null)
                bgm.SetMasterVolume(Mathf.Lerp(startVol, 0f, t));

            yield return null;
        }

        // 4) Ȯ���� ���� ����
        fadeImage.color = new Color(0, 0, 0, 1f);
        if (bgm != null)
            bgm.SetMasterVolume(0f);

        //  5) �ð� ���� & �г� ǥ��
        Time.timeScale = 0f;
        endOfDayPanel.SetActive(true);

        // 6) fadeImage�� ������ ���� �����Ƿ�, �г� ���̰� �ٷ� ����
        yield return null; // �� ������ ���

    }

    /// <summary>EndOfDayPanel�� �������� ��ư���� ȣ��</summary>
    public void ProceedToNextDay()
    {
        endOfDayPanel.SetActive(false);
        TimeManager.Instance.currentDay++;
        TimeManager.Instance.ResetDay();

        // 4) UI �ʱ�ȭ
        ResetAllUI();
        StartCoroutine(ProceedToNextDaySequence());
    }

    private IEnumerator ProceedToNextDaySequence()
    {
        //  1) fadeImage �ٽ� �Ѱ� ���̵��� (1��0)
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 1f);
        }

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, 1f - t);
            yield return null;
        }

        //  2) fadeImage ���� ���� �� ���α�
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0f);
            fadeImage.gameObject.SetActive(false);
        }

        //  3) �ð� �簳, ��¥ ����, �ʱ�ȭ
        Time.timeScale = 1f;


        // 5) BGM ���� + ���
        BGMManager.Instance?.SetMasterVolume(savedVolume);
        BGMManager.Instance?.PlayMusic();

        // 6) ���� �� ����
        if (TimeManager.Instance.currentDay > 30)
            endingManager.Ending();
        else
            StartCoroutine(StartDayRoutine());
    }

    private void ResetAllUI()
    {
        // ��ȭâ �����
        var dialog = FindObjectOfType<DialogUI>();
        if (dialog != null)
        {
            dialog.HideDialogPanel();
            dialog.HideNPCImage();
            dialog.HideTartTable();
        }

        // ���� UI �����
        var crust = FindObjectOfType<TartCrust>();
        if (crust != null) crust.gameObject.SetActive(false);
        var oven = FindObjectOfType<TartOven>();
        if (oven != null) oven.gameObject.SetActive(false);
        var topping = FindObjectOfType<TartTopping>();
        if (topping != null) topping.gameObject.SetActive(false);
    }
}