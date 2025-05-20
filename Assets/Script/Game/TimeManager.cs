using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public int currentDay = 1;
    public float currentTimeInMinutes = 480f;
    public float gameSpeed = 2f;

    public event Action OnDayEnded;

    private bool dayEnded = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (dayEnded) return; // �Ϸ簡 �������� �ð� ����

        currentTimeInMinutes += Time.deltaTime * gameSpeed;

        if (currentTimeInMinutes >= 1080f)
        {
            EndDay();
        }
    }

    void EndDay()
    {
        if (dayEnded) return; // �ߺ� ����

        dayEnded = true;
        OnDayEnded?.Invoke();
        Debug.Log("�Ϸ簡 �������ϴ�.");
    }

    public void ResetDay()
    {
        currentTimeInMinutes = 480f; // ���� 8��
        dayEnded = false;
    }

    public string GetCurrentTimeString()
    {
        int hours = (int)(currentTimeInMinutes / 60);
        int minutes = (int)(currentTimeInMinutes % 60);
        return $"{hours:D2}:{minutes:D2}";
    }

    public void SkipToEndOfDay()
    {
        currentTimeInMinutes = 1080f; // 18:00
    }

    public bool IsDayEnded()
    {
        return dayEnded;
    }
}
