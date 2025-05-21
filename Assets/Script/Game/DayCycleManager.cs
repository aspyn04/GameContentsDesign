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
        {
            TimeManager.Instance.OnDayEnded += HandleDayEnded;
        }
        StartCoroutine(StartDayRoutine());
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDayEnded -= HandleDayEnded;
        }
    }

    IEnumerator StartDayRoutine()
    {
        while (TimeManager.Instance == null || TimeManager.Instance.currentDay == 0)
        {
            yield return null;
        }

        int day = TimeManager.Instance.currentDay;

        if (cutsceneManager == null)
        {
            // ƒ∆æ¿ ∏≈¥œ¿˙ ø¨∞· æ» µ  
        }

        else
        {
            if (cutsceneManager.HasCutsceneForDay(day))
            {
                Time.timeScale = 0f;
                yield return cutsceneManager.PlayCutscene(day);
                Time.timeScale = 1f;
            }
            else
            {
                // ƒ∆æ¿ æ¯¿Ω
            }
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
        {
            //∞‘¿” ¡æ∑·
        }

        else
        {
            StartCoroutine(StartDayRoutine());
        }
    }
}
