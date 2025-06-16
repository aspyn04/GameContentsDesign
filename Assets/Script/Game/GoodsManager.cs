using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoodsManager : MonoBehaviour
{
    public static GoodsManager Instance;

    [Header("���� �ڿ�")]
    public int totalCheese = 0;
    public int totalStar = 0;

    [Header("���� �ڿ�")]
    public int dailyCheese = 0;
    public int dailyStar = 0;

    [Header("UI �ؽ�Ʈ ����")]
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
    /// ġ�� �߰� (���� + ����), UI ����ȭ
    /// </summary>
    public void AddCheese(int amount)
    {
        totalCheese += amount;
        dailyCheese += amount;
        RefreshUI();
    }

    /// <summary>
    /// ������ �߰� (���� + ����), UI ����ȭ
    /// </summary>
    public void AddStar(int amount)
    {
        totalStar += amount;
        dailyStar += amount;
        RefreshUI();
    }

    /// <summary>
    /// ���� �ڿ��� �ʱ�ȭ (���� �� ȣ��)
    /// </summary>
    public void ResetDaily()
    {
        dailyCheese = 0;
        dailyStar = 0;
        RefreshUI();
    }

    /// <summary>
    /// ���������� �ڿ� ��� �ʱ�ȭ (���� ���� ��)
    /// </summary>
    public void ResetAll()
    {
        totalCheese = totalStar = 0;
        ResetDaily();
    }

    /// <summary>
    /// UI �ؽ�Ʈ�� ���� ��ġ �ݿ�
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
