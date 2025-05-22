using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public List<NPCData> npcDataList;
    public NPCDataManager npcDataManager; //데이터 직접 가져오기용
    public TartManager tartManager;
    public DialogUI dialogUI;

    void Start()
    {
        if (dialogUI == null)
            Debug.LogError("DialogUI 연결 안 됨");

        if (npcDataManager != null)
        {
            npcDataList = npcDataManager.NPCDataList;
            Debug.Log($"NPC 데이터 개수: {npcDataList.Count}");
        }
        else
        {
            Debug.LogError("NPCDataManager 연결 안 됨");
        }
    }

    public void StartNPCLoop()
    {
        Debug.Log("NPC 루프 시작");
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
                Debug.LogWarning("해당 일차에 해당하는 NPC 없음");
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
                Debug.LogWarning($"파싱 실패: '{npc.npcID}' → '{cleanedID}'");
                return false;
            }
        });

        Debug.Log($"일차 {currentDay} 후보 NPC 수: {candidates.Count}");

        if (candidates.Count == 0)
        {
            return null;
        }

        return candidates[Random.Range(0, candidates.Count)];
    }
    IEnumerator HandleNPC(NPCData npc)
    {
        dialogUI.SetNPCImage(npc.npcID);

        yield return dialogUI.Show(npc.greetingDialogue);    // 첫 대사
        yield return dialogUI.Show(npc.orderDialogue);       // 주문 대사
        yield return dialogUI.WaitForMakeTartClick();        // 타르트 버튼 클릭 대기

        tartManager.StartTartMaking(npc);
        yield return new WaitUntil(() => tartManager.IsTartComplete);

        bool success = tartManager.CheckTartResult();
        string result = success ? npc.satisfiedDialogue : npc.unsatisfiedDialogue;

        yield return dialogUI.Show(result); // 결과 대사
    }


}
