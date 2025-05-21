using System.Collections;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    [SerializeField] private GameObject endOfDayPanel;
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private NPCManager npcManager;

    void Start()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayEnded += HandleDayEnded;

        StartCoroutine(StartDayRoutine());
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayEnded -= HandleDayEnded;
    }

    IEnumerator StartDayRoutine()
    {
        Debug.Log("StartDayRoutine 진입");

        while (TimeManager.Instance == null || TimeManager.Instance.currentDay == 0)
            yield return null;

        int day = TimeManager.Instance.currentDay;
        Debug.Log("현재 Day: " + day);

        if (cutsceneManager == null)
        {
            Debug.LogError("cutsceneManager가 null입니다.");
        }
        else
        {
            Debug.Log("cutsceneManager 연결 확인됨");

            if (cutsceneManager.HasCutsceneForDay(day))
            {
                Debug.Log("컷씬 조건 만족 - 재생 시도");
                Time.timeScale = 0f;
                yield return cutsceneManager.PlayCutscene(day);
                Time.timeScale = 1f;
            }
            else
            {
                Debug.Log("컷씬 없음");
            }
        }

        npcManager.StartGuestLoop();
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
            Debug.Log("게임 종료!");
        else
            StartCoroutine(StartDayRoutine());
    }
}
