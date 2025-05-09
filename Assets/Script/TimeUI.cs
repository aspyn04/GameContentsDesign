using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text dayText;

    private int lastDisplayedMinute = -1;

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
        timeText.text = $"Time {TimeManager.Instance.GetCurrentTimeString()}";
        dayText.text = $"Day {TimeManager.Instance.currentDay}";
    }
}