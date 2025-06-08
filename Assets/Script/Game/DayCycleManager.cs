// DayCycleManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DayCycleManager : MonoBehaviour
{
    [Header("Day Cycle 참조")]
    [SerializeField] private GameObject endOfDayPanel;
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private NPCManager npcManager;
    [SerializeField] private EndingManager endingManager;

    [Header("페이드용 Image (검은 화면)")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private float savedVolume;

    private void Start()
    {
        // BGM 재생 및 볼륨 기억
        BGMManager.Instance?.PlayMusic();
        savedVolume = BGMManager.Instance?.GetMasterVolume() ?? 1f;

        // 시작할 때엔 EndOfDayPanel과 fadeImage 둘 다 꺼둡니다
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

        //2) fadeImage 켜고 알파 0으로 시작
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0f);
        }

        var bgm = BGMManager.Instance;
        float startVol = bgm != null ? bgm.GetMasterVolume() : 0f;
        float elapsed = 0f;

        //3) 페이드아웃 (01) + BGM 볼륨 페이드
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            fadeImage.color = new Color(0, 0, 0, t);
            if (bgm != null)
                bgm.SetMasterVolume(Mathf.Lerp(startVol, 0f, t));

            yield return null;
        }

        // 4) 확실히 최종 세팅
        fadeImage.color = new Color(0, 0, 0, 1f);
        if (bgm != null)
            bgm.SetMasterVolume(0f);

        //  5) 시간 정지 & 패널 표시
        Time.timeScale = 0f;
        endOfDayPanel.SetActive(true);

        // 6) fadeImage는 여전히 켜져 있으므로, 패널 보이게 바로 끄기
        yield return null; // 한 프레임 대기

    }

    /// <summary>EndOfDayPanel의 “다음” 버튼에서 호출</summary>
    public void ProceedToNextDay()
    {
        endOfDayPanel.SetActive(false);
        TimeManager.Instance.currentDay++;
        TimeManager.Instance.ResetDay();

        // 4) UI 초기화
        ResetAllUI();
        StartCoroutine(ProceedToNextDaySequence());
    }

    private IEnumerator ProceedToNextDaySequence()
    {
        //  1) fadeImage 다시 켜고 페이드인 (1→0)
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

        //  2) fadeImage 완전 투명 뒤 꺼두기
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0f);
            fadeImage.gameObject.SetActive(false);
        }

        //  3) 시간 재개, 날짜 증가, 초기화
        Time.timeScale = 1f;


        // 5) BGM 복원 + 재생
        BGMManager.Instance?.SetMasterVolume(savedVolume);
        BGMManager.Instance?.PlayMusic();

        // 6) 다음 날 시작
        if (TimeManager.Instance.currentDay > 30)
            endingManager.Ending();
        else
            StartCoroutine(StartDayRoutine());
    }

    private void ResetAllUI()
    {
        // 대화창 숨기기
        var dialog = FindObjectOfType<DialogUI>();
        if (dialog != null)
        {
            dialog.HideDialogPanel();
            dialog.HideNPCImage();
            dialog.HideTartTable();
        }

        // 제작 UI 숨기기
        var crust = FindObjectOfType<TartCrust>();
        if (crust != null) crust.gameObject.SetActive(false);
        var oven = FindObjectOfType<TartOven>();
        if (oven != null) oven.gameObject.SetActive(false);
        var topping = FindObjectOfType<TartTopping>();
        if (topping != null) topping.gameObject.SetActive(false);
    }
}