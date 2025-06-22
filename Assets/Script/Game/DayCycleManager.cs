using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCycleManager : MonoBehaviour
{
    [SerializeField] private RecipeMinigameManager miniGameManager;
    [SerializeField] private CutSceneManager cutsceneManager;
    [SerializeField] private StoryManager storyManager;
    [SerializeField] private NPCManager npcManager;
    [SerializeField] private EndingManager endingManager;
    [SerializeField] private GameObject endOfDayPanel;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private GameObject gameUIRoot;

    private float savedVolume;

    private void Awake()
    {
        gameUIRoot.SetActive(false); 
    }
    private void Start()
    {
        int day = TimeManager.Instance.currentDay;

        savedVolume = BGMManager_Game.Instance?.GetMasterVolume() ?? 1f;
        BGMManager_Game.Instance.PlayBGMByDay(day);
        endOfDayPanel.SetActive(false);
        fadeImage.gameObject.SetActive(false);
        TimeManager.Instance.OnDayEnded += HandleDayEnded;
        StartCoroutine(StartDayRoutine());
    }

    private IEnumerator StartDayRoutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        while (TimeManager.Instance == null || TimeManager.Instance.currentDay == 0)
            yield return null;

        int day = TimeManager.Instance.currentDay;
        gameUIRoot.SetActive(true);

        if (miniGameManager != null && miniGameManager.HasMiniGameForDay(day))
        {
            gameUIRoot.SetActive(false);
            TimeManager.Instance.PauseForMiniGame();
            BGMManager_Game.Instance?.PauseMusic();

            yield return miniGameManager.PlayMiniGame();

            TimeManager.Instance.ResumeAfterMiniGame();
            BGMManager_Game.Instance.PlayBGMByDay(day);
            gameUIRoot.SetActive(true);
        }

        if (cutsceneManager != null && cutsceneManager.HasCutsceneForDay(day))
        {
            Debug.Log("컷씬 재생");
            TimeManager.Instance.PauseForMiniGame();
            BGMManager_Game.Instance?.PauseMusic();
            Time.timeScale = 0f;

            yield return cutsceneManager.PlayCutscene(day);

            Time.timeScale = 1f;
            TimeManager.Instance.ResumeAfterMiniGame();
            BGMManager_Game.Instance.PlayBGMByDay(day);
        }

        if (storyManager != null && storyManager.HasStoryForDay(day))
        {
            Debug.Log($"[StartDayRoutine] Story 시작 (day {day})");

            TimeManager.Instance.PauseForMiniGame();
            TimeManager.Instance.BlockDayEnd(true); // 
            BGMManager_Game.Instance?.PauseMusic();
            Time.timeScale = 0f;

            yield return StartCoroutine(storyManager.PlayStory(day));

            Time.timeScale = 1f;
            TimeManager.Instance.ResumeAfterMiniGame();   
            TimeManager.Instance.BlockDayEnd(false);    

            BGMManager_Game.Instance.PlayBGMByDay(day);

            Debug.Log($"[StartDayRoutine] Story 종료 (day {day})");
        }

        gameUIRoot.SetActive(true);
        npcManager.StartNPCLoop();
    }

    private void HandleDayEnded() => StartCoroutine(EndOfDaySequence());

    private IEnumerator EndOfDaySequence()
    {
        fadeImage.gameObject.SetActive(true);
        float startVol = BGMManager_Game.Instance?.GetMasterVolume() ?? 0f;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, t);
            BGMManager_Game.Instance?.SetMasterVolume(Mathf.Lerp(startVol, 0f, t));
            yield return null;
        }
        Time.timeScale = 0f;
        endOfDayPanel.SetActive(true);
        if (UISoundManager.Instance != null)
        {
            UISoundManager.Instance.PlayEnd();
        }
        else
        {
            Debug.Log("이준노 바보");
        }
        npcManager.tartManager?.HideAllPanels();   // 타르트 제작 UI 닫기
        npcManager.HideNPCUI();
    }

    public void ProceedToNextDay() => StartCoroutine(ProceedToNextDaySequence());

    private IEnumerator ProceedToNextDaySequence()
    {
        GoodsManager.Instance.ResetDaily();
        TimeManager.Instance.currentDay++;
        TimeManager.Instance.ResetDay();
        endOfDayPanel.SetActive(false);
        fadeImage.gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, 1f - t);
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
        Time.timeScale = 1f;


        npcManager.OnDayEnd(); // ← 여기에 추가

        BGMManager_Game.Instance?.SetMasterVolume(savedVolume);

        if (TimeManager.Instance.currentDay >= 18)
            endingManager.Ending();
        else
            StartCoroutine(StartDayRoutine());
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayEnded -= HandleDayEnded;
    }
}
