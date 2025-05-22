using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public List<NPCData> npcDataList;
    public NPCDataManager npcDataManager; //������ ���� ���������
    public TartManager tartManager;
    public DialogUI dialogUI;

    void Start()
    {
        if (dialogUI == null)
            Debug.LogError("DialogUI ���� �� ��");

        if (npcDataManager != null)
        {
            npcDataList = npcDataManager.NPCDataList;
            Debug.Log($"NPC ������ ����: {npcDataList.Count}");
        }
        else
        {
            Debug.LogError("NPCDataManager ���� �� ��");
        }
    }

    public void StartNPCLoop()
    {
        Debug.Log("NPC ���� ����");
        StartCoroutine(NPCLoopRoutine());
    }

    IEnumerator NPCLoopRoutine()
    {
        while (!TimeManager.Instance.IsDayEnded())
        {
            NPCData npc = GetRandomNPC();

            if (npc != null)
            {
                yield return StartCoroutine(HandleNPC(npc));
                yield return new WaitForSeconds(1f);
            }
            else
            {
                Debug.LogWarning("�ش� ������ �ش��ϴ� NPC ����");
                yield return new WaitForSeconds(2f);
            }
        }
    }

    NPCData GetRandomNPC()
    {
        int currentDay = TimeManager.Instance.currentDay;

        int minID = currentDay <= 10 ? 2011001 : 2011009;
        int maxID = currentDay <= 10 ? 2011008 : 2011026;

        List<NPCData> candidates = npcDataList.FindAll(npc =>
        {
            string cleanedID = npc.npcID.Trim().Replace("\r", "").Replace("\n", "");

            if (int.TryParse(cleanedID, out int id))
            {
                return id >= minID && id <= maxID;
            }
            else
            {
                Debug.LogWarning($"�Ľ� ����: '{npc.npcID}' �� '{cleanedID}'");
                return false;
            }
        });

        Debug.Log($"���� {currentDay} �ĺ� NPC ��: {candidates.Count}");

        if (candidates.Count == 0)
        {
            return null;
        }

        return candidates[Random.Range(0, candidates.Count)];
    }
    IEnumerator HandleNPC(NPCData npc)
    {
        dialogUI.SetNPCImage(npc.npcID);

        yield return dialogUI.Show(npc.greetingDialogue);    // ù ���
        yield return dialogUI.Show(npc.orderDialogue);       // �ֹ� ���
        yield return dialogUI.WaitForMakeTartClick();        // Ÿ��Ʈ ��ư Ŭ�� ���

        tartManager.StartTartMaking(npc);
        yield return new WaitUntil(() => tartManager.IsTartComplete);

        bool success = tartManager.CheckTartResult();
        string result = success ? npc.satisfiedDialogue : npc.unsatisfiedDialogue;

        yield return dialogUI.Show(result); // ��� ���
    }


}
