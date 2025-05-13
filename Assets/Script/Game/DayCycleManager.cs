using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    [SerializeField] private GameObject endOfDayPanel;
    private void Start()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDayEnded += HandleDayEnded;
        }
        else
        {
            Debug.LogError("TimeManager.Instance가 null입니다. 이벤트 구독 실패.");
        }
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDayEnded -= HandleDayEnded;
        }
    }

    void HandleDayEnded()
    {
        endOfDayPanel.SetActive(true);
        Time.timeScale = 0f; // 일시정지
        // 하루가 끝날 때의 처리
        //ResourceManager.Instance.ProcessDailyIncome();

        //TimeManager.Instance.SaveGame();
    }


    public void ProceedToNextDay()
    {
        Time.timeScale = 1f;
        TimeManager.Instance.currentDay++;
        endOfDayPanel.SetActive(false);

        FindObjectOfType<TimeUI>()?.ForceUpdateUI();

        if (TimeManager.Instance.currentDay > 30)
        {
            Debug.Log("게임 종료!");
        }
    }
}

