using System.Collections.Generic;
using UnityEngine;

public class StoryDialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class StoryLine
    {
        public int imageIndex;
        public string speaker;
        public string dialogue;
    }

    [System.Serializable]
    public class DayCSVEntry
    {
        public int day;
        public TextAsset csvFile;
        public AudioClip bgm;
    }

    [Header("������ ���丮 CSV ����")]
    [SerializeField] private List<DayCSVEntry> dayCsvEntries;

    private Dictionary<int, List<StoryLine>> storyByDay = new();
    private Dictionary<int, AudioClip> bgmByDay = new();

    void Awake()
    {
        foreach (var entry in dayCsvEntries)
        {
            if (entry.csvFile != null)
            {
                Debug.Log($"[StoryCSV] Try load Day {entry.day} : {entry.csvFile.name}");
                ParseCSV(entry.day, entry.csvFile);

                if (storyByDay.TryGetValue(entry.day, out var list))
                    Debug.Log($"[StoryCSV] Day {entry.day} -> ���� {list.Count}�� �ε�");
            }
            else
            {
                Debug.LogWarning($"[StoryCSV] Day {entry.day} CSV�� ��� �ֽ��ϴ�!");
            }

            if (entry.bgm != null)
            {
                bgmByDay[entry.day] = entry.bgm;
                Debug.Log($"[StoryCSV] Day {entry.day} BGM ��ϵ� : {entry.bgm.name}");
            }
        }

        if (storyByDay.ContainsKey(1))
            Debug.Log($"[StoryCSV] >>> Day 1 ���丮 Ȯ���� ({storyByDay[1].Count} lines)");
        else
            Debug.LogWarning("[StoryCSV] >>> Day 1 ���丮 ����!");
    }

    void ParseCSV(int day, TextAsset csv)
    {
        if (csv == null)
        {
            Debug.LogWarning($"[StoryCSV] Day {day} ������ null �Դϴ�.");
            return;
        }

        var list = new List<StoryLine>();
        var lines = csv.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            var raw = lines[i].Replace("\r", "").Trim();
            if (string.IsNullOrEmpty(raw)) continue;

            string[] parts = raw.Contains("\t") ? raw.Split('\t') : raw.Split(',');

            if (parts.Length < 3)
            {
                Debug.LogWarning($"[StoryCSV] Day {day} ���� {i + 1} Į�� ���� : {raw}");
                continue;
            }

            if (!int.TryParse(parts[0].Trim(), out int imgIdx))
            {
                Debug.LogWarning($"[StoryCSV] Day {day} ���� {i + 1} �̹��� �ε��� �Ľ� ����");
                continue;
            }

            var line = new StoryLine
            {
                imageIndex = imgIdx,
                speaker = parts[1].Trim().Trim('"'),
                dialogue = parts[2].Trim().Trim('"')
            };

            list.Add(line);
        }

        storyByDay[day] = list;
        Debug.Log($"[StoryCSV] Day {day} �ε� �Ϸ� �� ���μ� = {list.Count}");
    }

    public List<StoryLine> GetStoryForDay(int day)
    {
        return storyByDay.TryGetValue(day, out var list) ? list : new List<StoryLine>();
    }

    public bool HasStoryForDay(int day)
    {
        return storyByDay.ContainsKey(day);
    }

    public AudioClip GetBgmForDay(int day)
    {
        return bgmByDay.TryGetValue(day, out var clip) ? clip : null;
    }
}
