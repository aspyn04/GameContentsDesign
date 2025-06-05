// DayCycleManager.cs
using System.Collections;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    [SerializeField] private GameObject endOfDayPanel;
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private NPCManager npcManager;
    [SerializeField] private EndingManager endingManager;
    [SerializeField] private GameObject settingsPanel; // Settings 패널 참조

    void Start()
    {
        BGMManager.Instance?.PlayMusic();

        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayEnded += HandleDayEnded;

        // 시작 시 Settings 패널은 꺼두기
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        StartCoroutine(StartDayRoutine());
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayEnded -= HandleDayEnded;
    }

    IEnumerator StartDayRoutine()
    {
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

    void HandleDayEnded()
    {
        endOfDayPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ProceedToNextDay()
    {
        Time.timeScale = 1f;
        TimeManager.Instance.currentDay++;
        TimeManager.Instance.ResetDay();
        endOfDayPanel.SetActive(false);

        if (TimeManager.Instance.currentDay > 30)
            endingManager.Ending();
        else
            StartCoroutine(StartDayRoutine());
    }

    // Settings 버튼을 눌렀을 때 호출될 메서드
    public void OpenSettings()
    {
        if (settingsPanel == null) return;

        // 게임 시간을 멈추고 Settings 패널 활성화
        Time.timeScale = 0f;
        settingsPanel.SetActive(true);
    }

    // Settings 패널에서 닫기(또는 취소) 누를 때 호출
    public void CloseSettings()
    {
        if (settingsPanel == null) return;

        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
