// DayObjectActivator.cs
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TimeManager.currentDay 값을 보고
/// day 값이 같은 오브젝트만 활성화하는 간단한 관리자
/// </summary>
public class Interior : MonoBehaviour
{
    [System.Serializable]
    public class DayObjectEntry
    {
        [Tooltip("몇 일차에 켤 것인지")]
        public int day;

        [Tooltip("활성화할 대상 오브젝트")]
        public GameObject target;
    }

    [Header("일차별 오브젝트 매핑(Inspector에서 설정)")]
    public List<DayObjectEntry> dayObjects = new List<DayObjectEntry>();

    void Awake()
    {
        // 시작할 때 전부 꺼둠 (원하면 생략 가능)
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

            // 필요할 때만 상태 변경 -- 불필요한 SetActive 호출 방지
            if (entry.target.activeSelf != shouldBeActive)
                entry.target.SetActive(shouldBeActive);
        }
    }
}
