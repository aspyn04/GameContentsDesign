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
        int day = TimeManager.Instance.currentDay;

        if (cutsceneManager.HasCutsceneForDay(day))
        {
            Time.timeScale = 0f;
            yield return cutsceneManager.PlayCutscene(day);
            Time.timeScale = 1f;
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
