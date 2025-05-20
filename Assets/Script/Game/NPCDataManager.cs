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
    public TextAsset csvFile; // 인스펙터에서 할당
    public List<NPCData> NPCDataList = new List<NPCData>();

    void Start()
    {
        LoadNPCDataFromCSV();
    }

    void LoadNPCDataFromCSV()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV 파일이 없습니다!");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더니까 건너뜀
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] values = line.Split(',');

            if (values.Length < 8)
            {
                Debug.LogWarning("데이터가 부족합니다: " + line);
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

        Debug.Log("손님 데이터 로드 완료: " + NPCDataList.Count + "명");
    }
}
