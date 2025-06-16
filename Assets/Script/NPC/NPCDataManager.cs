using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NPCDataManager : MonoBehaviour
{
    [Header("Resources/CSV/NPCData.csv")]
    [SerializeField] private TextAsset csvFile;

    public List<NPCData> NPCDataList = new List<NPCData>();

    void Start()
    {
        LoadNPCDataFromCSV();
    }

    private void LoadNPCDataFromCSV()
    {
        if (csvFile == null)
        {
            Debug.LogError("NPCDataManager: csvFile이 할당되지 않았습니다.");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            List<string> values = SplitCsvLine(line);
            if (values.Count < 6)
            {
                Debug.LogWarning($"NPCDataManager: {i + 1}번째 줄 필드 개수 부족({values.Count}, 최소 6개 필요)");
                continue;
            }

            NPCData data = new NPCData
            {
                // 1) npcID
                npcID = values[0].Trim(),

                // 3) greetingDialogue
                type = values[2].Trim(),

                // 3) greetingDialogue
                greetingDialogue = values[3].Trim(),

                // 4) orderDialogue
                orderDialogue = values[4].Trim(),

                // 5) satisfiedDialogue
                satisfiedDialogue = values[5].Trim(),

                // 6) unsatisfiedDialogue
                unsatisfiedDialogue = values[6].Trim()
            };

            // 2) orderedTart(문자열) → recipeID(int) 파싱
            if (!int.TryParse(values[1].Trim(), out int rid))
            {
                Debug.LogWarning($"NPCDataManager: recipeID 파싱 실패: '{values[1].Trim()}', 0 사용");
                rid = 0;
            }
            data.recipeID = rid;

            NPCDataList.Add(data);
        }

        Debug.Log($"NPCDataManager: {NPCDataList.Count}개 NPC 데이터 로드 완료");
    }

    private List<string> SplitCsvLine(string line)
    {
        List<string> fields = new List<string>();
        StringBuilder current = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++; 
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        fields.Add(current.ToString());

        for (int i = 0; i < fields.Count; i++)
        {
            string f = fields[i];
            if (f.Length >= 2 && f[0] == '"' && f[f.Length - 1] == '"')
            {
                f = f.Substring(1, f.Length - 2).Replace("\"\"", "\"");
            }
            fields[i] = f;
        }

        return fields;
    }
}