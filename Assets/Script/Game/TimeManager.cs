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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        currentTimeInMinutes += Time.deltaTime * gameSpeed;

        if (currentTimeInMinutes >= 1080f) 
        {
            EndDay();
        }
    }
    void EndDay()
    {
        OnDayEnded?.Invoke();
        currentTimeInMinutes = 480f;
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

}
