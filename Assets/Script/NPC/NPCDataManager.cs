using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Resources/CSV/NPCData.csv �� �о�鿩 NPCDataList�� ����ϴ�.
/// (�÷� ����: npcID, npcName, orderedTart, greetingDialogue, orderDialogue, satisfiedDialogue, unsatisfiedDialogue)
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
            Debug.LogError("NPCDataManager: csvFile�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            // SplitCsvLine�� ����Ͽ� ū����ǥ ó���� �ʵ带 ������ �Ľ�
            List<string> values = SplitCsvLine(line);
            if (values.Count < 6)
            {
                Debug.LogWarning($"NPCDataManager: {i + 1}��° �� �ʵ� ���� ����({values.Count}, �ּ� 6�� �ʿ�)");
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

            // 2) orderedTart(���ڿ�) �� recipeID(int) �Ľ�
            if (!int.TryParse(values[1].Trim(), out int rid))
            {
                Debug.LogWarning($"NPCDataManager: recipeID �Ľ� ����: '{values[1].Trim()}', 0 ���");
                rid = 0;
            }
            data.recipeID = rid;

            NPCDataList.Add(data);
        }

        Debug.Log($"NPCDataManager: {NPCDataList.Count}�� NPC ������ �ε� �Ϸ�");
    }

    /// <summary>
    /// �� ���� �޾Ƽ� ��ū����ǥ�� ������ ���� ���� ��ǥ���� �����ϸ鼭 �ʵ� ����Ʈ�� �и��մϴ�.
    /// ��ȯ�� �� �ʵ��� �յ� ū����ǥ�� ���ŵ˴ϴ�.
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
                // ū����ǥ ���ڰ� �������� �� �� ������, �̴� �ʵ� �� �ϳ��� ū����ǥ��
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++; // ���� ū����ǥ�� �ǳʶ�
                }
                else
                {
                    // ū����ǥ ���: inQuotes ���� ����
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                // inQuotes==false ���¿��� ���� ��ǥ�� �ʵ� ������
                fields.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        // ������ �ʵ� �߰�
        fields.Add(current.ToString());

        // �� �ʵ忡�� ���ʵ尡 ū����ǥ�� ���ο� �ִٸ顱 �յ� ū����ǥ�� ����
        for (int i = 0; i < fields.Count; i++)
        {
            string f = fields[i];
            if (f.Length >= 2 && f[0] == '"' && f[f.Length - 1] == '"')
            {
                // �յ� ū����ǥ�� �����ϰ�, ������ �� ��¥�� ū����ǥ("")�� �ϳ��� �ٲ�
                f = f.Substring(1, f.Length - 2).Replace("\"\"", "\"");
            }
            fields[i] = f;
        }

        return fields;
    }
}