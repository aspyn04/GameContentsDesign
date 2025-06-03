using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Resources/CSV/NPCData.csv 를 읽어들여 NPCDataList를 만듭니다.
/// (컬럼 순서: npcID, npcName, orderedTart, greetingDialogue, orderDialogue, satisfiedDialogue, unsatisfiedDialogue)
/// </summary>
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

            // SplitCsvLine을 사용하여 큰따옴표 처리된 필드를 포함해 파싱
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
                greetingDialogue = values[2].Trim(),

                // 4) orderDialogue
                orderDialogue = values[3].Trim(),

                // 5) satisfiedDialogue
                satisfiedDialogue = values[4].Trim(),

                // 6) unsatisfiedDialogue
                unsatisfiedDialogue = values[5].Trim()
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

    /// <summary>
    /// 한 줄을 받아서 “큰따옴표로 감싸진 구역 안의 쉼표”를 무시하면서 필드 리스트로 분리합니다.
    /// 반환된 각 필드의 앞뒤 큰따옴표는 제거됩니다.
    /// </summary>
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
                // 큰따옴표 문자가 연속으로 두 개 나오면, 이는 필드 내 하나의 큰따옴표임
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++; // 다음 큰따옴표는 건너뜀
                }
                else
                {
                    // 큰따옴표 토글: inQuotes 상태 변경
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                // inQuotes==false 상태에서 만난 쉼표는 필드 구분자
                fields.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        // 마지막 필드 추가
        fields.Add(current.ToString());

        // 각 필드에서 “필드가 큰따옴표로 감싸여 있다면” 앞뒤 큰따옴표를 제거
        for (int i = 0; i < fields.Count; i++)
        {
            string f = fields[i];
            if (f.Length >= 2 && f[0] == '"' && f[f.Length - 1] == '"')
            {
                // 앞뒤 큰따옴표를 제거하고, 내부의 두 개짜리 큰따옴표("")를 하나로 바꿈
                f = f.Substring(1, f.Length - 2).Replace("\"\"", "\"");
            }
            fields[i] = f;
        }

        return fields;
    }
}