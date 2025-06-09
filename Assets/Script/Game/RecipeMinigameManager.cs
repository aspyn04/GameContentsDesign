using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PanelEntry
{
    public int day;
    public GameObject panelObject;
}

public class RecipeMinigameManager : MonoBehaviour
{
    public static RecipeMinigameManager Instance;
    [SerializeField]
    public List<PanelEntry> panelEntries = new List<PanelEntry>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 해당 day에 미니게임이 등록되어 있는지 확인
    /// </summary>
    public bool HasMiniGameForDay(int day)
    {
        return panelEntries.Exists(e => e.day == day && e.panelObject != null);
    }

    public IEnumerator PlayMiniGame(int day)
    {
        var entry = panelEntries.Find(e => e.day == day && e.panelObject != null);
        if (entry == null) yield break;

        var panel = entry.panelObject;
        var slider = panel.GetComponentInChildren<TimeSlider>();
        if (slider == null) yield break;

        bool finished = false;
        slider.SetCompletionCallback(() => finished = true);

        panel.SetActive(true);
        slider.ResetGame();

        while (!finished)
            yield return null;

        panel.SetActive(false);
    }
}
