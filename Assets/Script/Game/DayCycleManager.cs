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
            Debug.LogError("TimeManager.Instance�� null�Դϴ�. �̺�Ʈ ���� ����.");
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
        Time.timeScale = 0f; // �Ͻ�����
        // �Ϸ簡 ���� ���� ó��
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
            Debug.Log("���� ����!");
        }
    }
}

