using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public List<NPCData> npcDataList;
    public TartManager tartManager;
    public DialogUI dialogUI;

    public void StartNPCLoop()
    {
        StartCoroutine(NPCLoopRoutine());
    }

    IEnumerator NPCLoopRoutine()
    {
        while (!TimeManager.Instance.IsDayEnded())
        {
            NPCData npc = GetRandomNPC();
            yield return StartCoroutine(HandleNPC(npc));
            yield return new WaitForSeconds(1f);
        }
    }

    // 일차별 가져올 NPC 구분 (id로)

    NPCData GetRandomNPC()
    {
        return npcDataList[Random.Range(0, npcDataList.Count)];
    }

    IEnumerator HandleNPC(NPCData npc)
    {
        yield return dialogUI.Show(npc.greetingDialogue);
        yield return dialogUI.Show(npc.orderDialogue);
        yield return dialogUI.WaitForMakeTartClick();

        tartManager.StartTartMaking(npc);

        yield return new WaitUntil(() => tartManager.IsTartComplete);

        bool success = tartManager.CheckTartResult();
        string result = success ? npc.satisfiedDialogue : npc.unsatisfiedDialogue;

        yield return dialogUI.Show(result);
    }
}