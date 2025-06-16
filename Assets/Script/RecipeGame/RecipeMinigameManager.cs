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

    public IEnumerator PlayMiniGame()
    {
        int day = TimeManager.Instance.currentDay;
        Debug.Log($"[RecipeMinigameManager] PlayMiniGame for currentDay = {day}");

        foreach (var entry in panelEntries)
        {
            if (entry.panelObject != null)
                entry.panelObject.SetActive(false);
        }

        var entryToday = panelEntries.Find(e => e.day == day);
        if (entryToday == null || entryToday.panelObject == null)
        {
            Debug.LogError($"No panel found for day {day}");
            yield break;
        }

        entryToday.panelObject.SetActive(true);

        var manager = entryToday.panelObject.GetComponentInChildren<MinigameManager>();
        if (manager == null)
        {
            Debug.LogError("MinigameManager not found");
            yield break;
        }

        bool finished = false;
        manager.SetCompletionCallback(() => finished = true);
        manager.StartGame(day);

        while (!finished)
            yield return null;

        entryToday.panelObject.SetActive(false);
    }
    public bool HasMiniGameForDay(int day)
    {
        return panelEntries.Exists(e => e.day == day && e.panelObject != null);
    }

}
