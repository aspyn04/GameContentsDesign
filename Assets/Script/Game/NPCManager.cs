using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public List<NPCData> npcDataList;
    public TartManager tartManager;
    public DialogUI dialogUI;

    public void StartGuestLoop()
    {
        StartCoroutine(GuestLoopRoutine());
    }

    IEnumerator GuestLoopRoutine()
    {
        while (!TimeManager.Instance.IsDayEnded())
        {
            NPCData guest = GetRandomGuest();
            yield return StartCoroutine(HandleGuest(guest));
            yield return new WaitForSeconds(1f);
        }
    }

    NPCData GetRandomGuest()
    {
        return npcDataList[Random.Range(0, npcDataList.Count)];
    }

    IEnumerator HandleGuest(NPCData guest)
    {
        yield return dialogUI.Show(guest.greetingDialogue);
        yield return dialogUI.Show(guest.orderDialogue);
        yield return dialogUI.WaitForMakeTartClick();

        tartManager.StartTartMaking(guest);

        yield return new WaitUntil(() => tartManager.IsTartComplete);

        bool success = tartManager.CheckTartResult();
        string result = success ? guest.satisfiedDialogue : guest.unsatisfiedDialogue;

        yield return dialogUI.Show(result);
    }
}