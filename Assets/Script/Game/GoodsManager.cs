using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoodsManager : MonoBehaviour
{
    public static GoodsManager Instance;

    [Header("누적 자원")]
    public int totalCheese = 0;
    public int totalStar = 0;

    [Header("오늘 자원")]
    public int dailyCheese = 0;
    public int dailyStar = 0;

    [Header("UI 텍스트 참조")]
    [SerializeField] public TMP_Text totalCheeseText;
    [SerializeField] public TMP_Text totalStarText;
    [SerializeField] public TMP_Text dailyCheeseText;
    [SerializeField] public TMP_Text dailyStarText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        RefreshUI();
    }

    /// <summary>
    /// 치즈 추가 (누적 + 오늘), UI 동기화
    /// </summary>
    public void AddCheese(int amount)
    {
        totalCheese += amount;
        dailyCheese += amount;
        RefreshUI();
    }

    /// <summary>
    /// 별조각 추가 (누적 + 오늘), UI 동기화
    /// </summary>
    public void AddStar(int amount)
    {
        totalStar += amount;
        dailyStar += amount;
        RefreshUI();
    }

    /// <summary>
    /// 오늘 자원만 초기화 (다음 날 호출)
    /// </summary>
    public void ResetDaily()
    {
        dailyCheese = 0;
        dailyStar = 0;
        RefreshUI();
    }

    /// <summary>
    /// 누적·오늘 자원 모두 초기화 (게임 리셋 등)
    /// </summary>
    public void ResetAll()
    {
        totalCheese = totalStar = 0;
        ResetDaily();
    }

    /// <summary>
    /// UI 텍스트에 현재 수치 반영
    /// </summary>
    public void RefreshUI()
    {
        Debug.Log($"[GoodsManager] RefreshUI: totalCheese={totalCheese}, dailyCheese={dailyCheese}");

        if (totalCheeseText != null) totalCheeseText.text = totalCheese.ToString();
        if (totalStarText != null) totalStarText.text = totalStar.ToString();
        if (dailyCheeseText != null) dailyCheeseText.text = dailyCheese.ToString();
        if (dailyStarText != null) dailyStarText.text = dailyStar.ToString();
    }
}
