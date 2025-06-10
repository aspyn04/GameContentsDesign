using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DayCycleManager : MonoBehaviour
{
    [SerializeField] private RecipeMinigameManager miniGameManager;
    [SerializeField] private CutSceneManager cutsceneManager;
    [SerializeField] private NPCManager npcManager;
    [SerializeField] private EndingManager endingManager;
    [SerializeField] private GameObject endOfDayPanel;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private GameObject gameUIRoot;

    private float savedVolume;

    private void Start()
    {
        savedVolume = BGMManager.Instance?.GetMasterVolume() ?? 1f;
        BGMManager.Instance?.PlayMusic();
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

        if (miniGameManager != null && miniGameManager.HasMiniGameForDay(day))
        {
            gameUIRoot.SetActive(false);
            TimeManager.Instance.PauseForMiniGame();
            BGMManager.Instance?.PauseMusic();

            yield return miniGameManager.PlayMiniGame(day);

            TimeManager.Instance.ResumeAfterMiniGame();
            BGMManager.Instance?.PlayMusic();
            gameUIRoot.SetActive(true);
        }

        if (cutsceneManager != null && cutsceneManager.HasCutsceneForDay(day))
        {
            Debug.Log("ÄÆ¾À Àç»ý");
            TimeManager.Instance.PauseForMiniGame();
            BGMManager.Instance?.PauseMusic();
            Time.timeScale = 0f;

            yield return cutsceneManager.PlayCutscene(day);

            Time.timeScale = 1f;
            TimeManager.Instance.ResumeAfterMiniGame();
            BGMManager.Instance?.PlayMusic();
        }


        gameUIRoot.SetActive(true);
        npcManager.StartNPCLoop();
    }

    private void HandleDayEnded() => StartCoroutine(EndOfDaySequence());

    private IEnumerator EndOfDaySequence()
    {
        fadeImage.gameObject.SetActive(true);
        float startVol = BGMManager.Instance?.GetMasterVolume() ?? 0f;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, t);
            BGMManager.Instance?.SetMasterVolume(Mathf.Lerp(startVol, 0f, t));
            yield return null;
        }
        Time.timeScale = 0f;
        endOfDayPanel.SetActive(true);
        npcManager.tartManager?.HideAllPanels();   // Å¸¸£Æ® Á¦ÀÛ UI ´Ý±â
        npcManager.HideNPCUI();
    }

    public void ProceedToNextDay() => StartCoroutine(ProceedToNextDaySequence());

    private IEnumerator ProceedToNextDaySequence()
    {
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


        npcManager.OnDayEnd(); // ¡ç ¿©±â¿¡ Ãß°¡

        BGMManager.Instance?.SetMasterVolume(savedVolume);
        BGMManager.Instance?.PlayMusic();

        if (TimeManager.Instance.currentDay > 30)
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
