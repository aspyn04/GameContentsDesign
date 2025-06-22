using System;
using System.Collections;
using System.Collections.Generic;
// TimeManager.cs
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public int currentDay = 1;
    public float currentTimeInMinutes = 480f;
    public float gameSpeed = 2f;
    public event Action OnDayEnded;

    private bool dayEnded = false;
    private bool pauseForMiniGame = false;    // ← 미니게임 중 멈추기

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private bool blockDayEnd = false;
    public void BlockDayEnd(bool block) => blockDayEnd = block;

    void Update()
    {
        if (dayEnded || pauseForMiniGame) return;

        currentTimeInMinutes += Time.deltaTime * gameSpeed;

        if (currentTimeInMinutes >= 1080f && !blockDayEnd)
            EndDay();
    }

    private void EndDay()
    {
        if (dayEnded) return;
        dayEnded = true;
        OnDayEnded?.Invoke();
    }

    public void ResetDay()
    {
        currentTimeInMinutes = 480f;
        dayEnded = false;
    }

    public bool IsDayEnded() => dayEnded;

    public string GetCurrentTimeString()
    {
        int h = (int)(currentTimeInMinutes / 60);
        int m = (int)(currentTimeInMinutes % 60);
        return $"{h:D2}:{m:D2}";
    }

    public void SkipToEndOfDay() => currentTimeInMinutes = 1080f;

    // ================================================
    // 미니게임 진입 전후로 호출할 메서드들
    // ================================================
    public void PauseForMiniGame() => pauseForMiniGame = true;
    public void ResumeAfterMiniGame() => pauseForMiniGame = false;
}
