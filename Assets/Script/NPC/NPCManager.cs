using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// NPC 등장(애니메이션) → 클릭 → 대사 → 타르트 제작 호출 → 결과 대사 흐름을 관리합니다.
/// </summary>
public class NPCManager : MonoBehaviour
{
    private List<string> shownNPCIDs = new List<string>();

    [Header("데이터 참조")]
    public List<NPCData> npcDataList;
    public NPCDataManager npcDataManager;
    public TartManager tartManager;
    public DialogUI dialogUI;

    [Header("NPC 등장 오브젝트")]
    [SerializeField] private GameObject npcObject;
    [SerializeField] private float animateDuration = 0.5f;
    [SerializeField] private float startY = -300f;
    [SerializeField] private float endY = -76f;

    void Start()
    {
        if (dialogUI == null) Debug.LogError("DialogUI 연결 안 됨");
        if (npcDataManager != null)
            npcDataList = npcDataManager.NPCDataList;
        else
            Debug.LogError("NPCDataManager 연결 안 됨");

        if (npcObject != null)
            npcObject.SetActive(false);
    }

    public void StartNPCLoop()
    {
        StartCoroutine(NPCLoopRoutine());
    }

    private IEnumerator NPCLoopRoutine()
    {
        while (!TimeManager.Instance.IsDayEnded())
        {
            NPCData npc = GetRandomNPC();
            if (npc != null)
            {
                NPCSoundManager.Instance?.PlaySpawn();
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

    public void HideNPCUI()
    {
        if (npcObject != null)
            npcObject.SetActive(false);

        dialogUI?.HideDialogPanel();
    }

    private NPCData GetRandomNPC()
    {
        int day = TimeManager.Instance.currentDay;

        List<string> targetNPCIDs;

        if (day == 1)
        {
            targetNPCIDs = new List<string> { "2001001", "2011005", "2011018", "2011016" };
        }

        else if (day >= 2 && day <= 4)
        {
            targetNPCIDs = new List<string> { "2001001", "2011005", "2011018", "2011016", "2011003", "2011009", "2011020" };
        }

        else if (day >= 5 && day <= 7)
        {
            targetNPCIDs = new List<string> { "2001001", "2011005", "2011018", "2011016", "2011003", "2011009", "2011020", "2011012", "2011021", "2011026" };
        }
        else if (day >= 8 && day <= 10)
        {
            targetNPCIDs = new List<string> { "2001001", "2011005", "2011018", "2011016", "2011003", "2011009", "2011020", "2011012", "2011021", "2011026", "2011004", "2011015", "2011024" };
        }
        else if (day >= 11 && day <= 12)
        {
            targetNPCIDs = new List<string> { "2001001", "2011005", "2011018", "2011016", "2011003", "2011009", "2011020", "2011012", "2011021", "2011026", "2011004", "2011015", "2011024", "2011002", "2011006", "2011007" };
        }

        else if (day == 13)
        {
            targetNPCIDs = new List<string> { "2001004", "2011005", "2011018", "2011016", "2011003", "2011009", "2011020" };
        }

        else if (day >= 15 && day <= 16)
        {
            targetNPCIDs = new List<string> { "2001001", "2011005", "2011018", "2011016", "2011003", "2011009", "2011020" };
        }

        else if (day >= 17 && day <= 19)
        {
            targetNPCIDs = new List<string> { "2001001", "2011005", "2011018", "2011016", "2011003", "2011009", "2011020" };
        }

        else if (day >= 20 && day <= 22)
        {
            targetNPCIDs = new List<string> { "2001001", "2011005", "2011018", "2011016", "2011003", "2011009", "2011020" };
        }

        else if (day >= 23 && day <= 30)
        {
            targetNPCIDs = new List<string> { "2001001", "2011005", "2011018", "2011016", "2011003", "2011009", "2011020" };
        }

        else
        {
            int minID = day <= 10 ? 2011001 : 2011009;
            int maxID = day <= 10 ? 2011008 : 2011026;

            targetNPCIDs = npcDataList.FindAll(npc =>
            {
                if (int.TryParse(npc.npcID.Trim(), out int id))
                    return id >= minID && id <= maxID;
                return false;
            }).Select(npc => npc.npcID).ToList();
        }

        // 중복 방지: 이미 등장한 NPC 제외
        var availableNPCs = targetNPCIDs.Except(shownNPCIDs).ToList();

        if (availableNPCs.Count == 0)
        {
            Debug.LogWarning("모든 가능한 NPC가 이미 등장했습니다.");
            return null;
        }

        string selectedNPCID = availableNPCs[Random.Range(0, availableNPCs.Count)];
        shownNPCIDs.Add(selectedNPCID);

        return npcDataList.Find(npc => npc.npcID == selectedNPCID);
    }

    private IEnumerator HandleNPC(NPCData npc)
    {
        // 1) NPC 이미지 세팅 및 활성화
        dialogUI.SetNPCImage(npc.npcID);
        npcObject.SetActive(true);

        // 2) 등장 애니메이션
        yield return AnimateNPCEntrance(npcObject);
        yield return new WaitForSeconds(0.5f);

        // 3) 인사/주문 대사
        yield return dialogUI.Show(npc.greetingDialogue);
        yield return dialogUI.Show(npc.orderDialogue);

        // 4) 타르트 제작 버튼 클릭 대기
        yield return dialogUI.WaitForMakeTartClick();

        dialogUI.HideDialogPanel();

        tartManager.StartTartMaking(npc.recipeID);

        // 토핑 단계 완료(성공/실패 상관없이)까지 대기
        yield return new WaitUntil(() => tartManager.IsProductionDone);

        // 최종 성공 여부
        bool success = tartManager.CheckTartResult();
        string resultText = success ? npc.satisfiedDialogue : npc.unsatisfiedDialogue;

        Debug.Log($"[NPCManager] 제작 완료 플래그: {tartManager.IsProductionDone}, 성공 여부: {success}");

        yield return dialogUI.Show(resultText);
        dialogUI.HideDialogPanel();
        yield return AnimateNPCExit(npcObject);
        yield return new WaitForSeconds(0.5f);
        npcObject.SetActive(false);

        // 다음 NPC 전에 플래그 초기화
        // (필요 시 TartManager 내부에서도 StartTartMaking에서 초기화됨)
        yield return new WaitForSeconds(1f);
    }


    private IEnumerator AnimateNPCEntrance(GameObject obj)
    {
        var rect = obj.GetComponent<RectTransform>();
        if (rect == null) yield break;

        Vector2 from = new Vector2(rect.anchoredPosition.x, startY);
        Vector2 to = new Vector2(rect.anchoredPosition.x, endY);
        float t = 0f;

        rect.anchoredPosition = from;
        while (t < animateDuration)
        {
            t += Time.deltaTime;
            float f = Mathf.Clamp01(t / animateDuration);
            f = Mathf.Sin(f * Mathf.PI * 0.5f);  // ease-out
            rect.anchoredPosition = Vector2.Lerp(from, to, f);
            yield return null;
        }
        rect.anchoredPosition = to;
    }

    private IEnumerator AnimateNPCExit(GameObject obj)
    {
        var rect = obj.GetComponent<RectTransform>();
        if (rect == null) yield break;

        Vector2 from = new Vector2(rect.anchoredPosition.x, endY);
        Vector2 to = new Vector2(rect.anchoredPosition.x, startY);
        float t = 0f;

        rect.anchoredPosition = from;
        while (t < animateDuration)
        {
            t += Time.deltaTime;
            float f = Mathf.Clamp01(t / animateDuration);
            f = Mathf.Sin(f * Mathf.PI * 0.5f);  // ease-out
            rect.anchoredPosition = Vector2.Lerp(from, to, f);
            yield return null;
        }
        rect.anchoredPosition = to;
    }

    public void OnDayEnd()
    {
        shownNPCIDs.Clear();
    }

}