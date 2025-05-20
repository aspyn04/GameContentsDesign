using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


[System.Serializable]

public class NPCData
{
    public string npcID;
    public string npcName;
    public string type;
    public string species;
    public string greetingDialogue;
    public string orderDialogue;
    public string orderedTart;
    public string satisfiedDialogue;
    public string unsatisfiedDialogue;
}

public class NPCDataManager : MonoBehaviour
{
    public TextAsset csvFile; // �ν����Ϳ��� �Ҵ�
    public List<NPCData> NPCDataList = new List<NPCData>();

    void Start()
    {
        LoadNPCDataFromCSV();
    }

    void LoadNPCDataFromCSV()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV ������ �����ϴ�!");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // ù ���� ����ϱ� �ǳʶ�
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] values = line.Split(',');

            if (values.Length < 8)
            {
                Debug.LogWarning("�����Ͱ� �����մϴ�: " + line);
                continue;
            }

            NPCData data = new NPCData()
            {
                npcID = values[0],
                npcName = values[1],
                type = values[2],
                species = values[3],
                greetingDialogue = values[4],
                orderDialogue = values[5],
                orderedTart = values[6],
                satisfiedDialogue = values[7],
                unsatisfiedDialogue = values[8]
            };

            NPCDataList.Add(data);
        }

        Debug.Log("�մ� ������ �ε� �Ϸ�: " + NPCDataList.Count + "��");
    }
}
