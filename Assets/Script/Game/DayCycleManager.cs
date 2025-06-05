// DayCycleManager.cs
using System.Collections;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    [SerializeField] private GameObject endOfDayPanel;
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private NPCManager npcManager;
    [SerializeField] private EndingManager endingManager;
    [SerializeField] private GameObject settingsPanel; // Settings �г� ����

    void Start()
    {
        BGMManager.Instance?.PlayMusic();

        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayEnded += HandleDayEnded;

        // ���� �� Settings �г��� ���α�
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        StartCoroutine(StartDayRoutine());
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDayEnded -= HandleDayEnded;
    }

    IEnumerator StartDayRoutine()
    {
        while (TimeManager.Instance == null || TimeManager.Instance.currentDay == 0)
            yield return null;

        int day = TimeManager.Instance.currentDay;

        if (cutsceneManager != null && cutsceneManager.HasCutsceneForDay(day))
        {
            Time.timeScale = 0f;
            yield return cutsceneManager.PlayCutscene(day);
            Time.timeScale = 1f;
        }

        npcManager.StartNPCLoop();
    }

    void HandleDayEnded()
    {
        endOfDayPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ProceedToNextDay()
    {
        Time.timeScale = 1f;
        TimeManager.Instance.currentDay++;
        TimeManager.Instance.ResetDay();
        endOfDayPanel.SetActive(false);

        if (TimeManager.Instance.currentDay > 30)
            endingManager.Ending();
        else
            StartCoroutine(StartDayRoutine());
    }

    // Settings ��ư�� ������ �� ȣ��� �޼���
    public void OpenSettings()
    {
        if (settingsPanel == null) return;

        // ���� �ð��� ���߰� Settings �г� Ȱ��ȭ
        Time.timeScale = 0f;
        settingsPanel.SetActive(true);
    }

    // Settings �гο��� �ݱ�(�Ǵ� ���) ���� �� ȣ��
    public void CloseSettings()
    {
        if (settingsPanel == null) return;

        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
