// DayObjectActivator.cs
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TimeManager.currentDay ���� ����
/// day ���� ���� ������Ʈ�� Ȱ��ȭ�ϴ� ������ ������
/// </summary>
public class Interior : MonoBehaviour
{
    [System.Serializable]
    public class DayObjectEntry
    {
        [Tooltip("�� ������ �� ������")]
        public int day;

        [Tooltip("Ȱ��ȭ�� ��� ������Ʈ")]
        public GameObject target;
    }

    [Header("������ ������Ʈ ����(Inspector���� ����)")]
    public List<DayObjectEntry> dayObjects = new List<DayObjectEntry>();

    void Awake()
    {
        // ������ �� ���� ���� (���ϸ� ���� ����)
        foreach (var entry in dayObjects)
            if (entry.target != null)
                entry.target.SetActive(false);
    }

    void Update()
    {
        if (TimeManager.Instance == null) return;

        int today = TimeManager.Instance.currentDay;

        foreach (var entry in dayObjects)
        {
            if (entry.target == null) continue;

            bool shouldBeActive = (entry.day == today);

            // �ʿ��� ���� ���� ���� -- ���ʿ��� SetActive ȣ�� ����
            if (entry.target.activeSelf != shouldBeActive)
                entry.target.SetActive(shouldBeActive);
        }
    }
}
