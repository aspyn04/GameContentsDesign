using UnityEngine;
using TMPro;
using UnityEngine.UI; // 버튼 사용

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private Button nextDayButton; // 버튼 참조 추가
    [SerializeField] private DayCycleManager dayCycleManager; // DayCycleManager 참조 추가

    private int lastDisplayedMinute = -1;

    private void Start()
    {
        if (nextDayButton != null && dayCycleManager != null)
        {
            nextDayButton.onClick.AddListener(dayCycleManager.ProceedToNextDay);
        }
    }

    private void OnDestroy()
    {
        if (nextDayButton != null && dayCycleManager != null)
        {
            nextDayButton.onClick.RemoveListener(dayCycleManager.ProceedToNextDay);
        }
    }

    private void Update()
    {
        if (TimeManager.Instance == null) return;

        int currentMinute = (int)TimeManager.Instance.currentTimeInMinutes;

        if (currentMinute / 10 != lastDisplayedMinute / 10)
        {
            lastDisplayedMinute = currentMinute;
            UpdateTimeText();
        }
    }

    public void ForceUpdateUI()
    {
        UpdateTimeText();
        lastDisplayedMinute = (int)TimeManager.Instance.currentTimeInMinutes;
    }

    private void UpdateTimeText()
    {
        timeText.text = $"{TimeManager.Instance.GetCurrentTimeString()}";
        dayText.text = $"Day {TimeManager.Instance.currentDay}";
    }
}
