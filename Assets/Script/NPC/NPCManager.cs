using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// NPC 등장(애니메이션) → 클릭 → 대사 → 타르트 제작 호출 → 결과 대사 흐름을 관리합니다.
/// </summary>
public class NPCManager : MonoBehaviour
{
    [Header("데이터 참조")]
    public List<NPCData> npcDataList;
    public NPCDataManager npcDataManager;
    public TartManager tartManager;
    public DialogUI dialogUI;

    [Header("NPC 등장 오브젝트")]
    [SerializeField] private GameObject npcObject;
    [SerializeField] private float animateDuration = 0.5f;
    [SerializeField] private float startY = -300f;   // 화면 하단 시작 Y
    [SerializeField] private float endY = -76f;      // 등장 후 Y 위치

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
                UISoundManager.Instance?.PlayNPCSpawn();
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

    private NPCData GetRandomNPC()
    {
        int currentDay = TimeManager.Instance.currentDay;
        int minID = currentDay <= 10 ? 2011001 : 2011009;
        int maxID = currentDay <= 10 ? 2011008 : 2011026;

        List<NPCData> candidates = npcDataList.FindAll(npc =>
        {
            string cleaned = npc.npcID.Trim().Replace("\r", "").Replace("\n", "");
            if (int.TryParse(cleaned, out int id))
                return id >= minID && id <= maxID;
            else
            {
                Debug.LogWarning($"NPC ID 파싱 실패: '{npc.npcID}'");
                return false;
            }
        });

        if (candidates.Count == 0) return null;
        return candidates[Random.Range(0, candidates.Count)];
    }

    /// <summary>
    /// NPC 등장 애니메이션 → 2초 대기 → 자동으로 대사 시작 흐름
    /// </summary>
    private IEnumerator HandleNPC(NPCData npc)
    {
        // 1) NPC 이미지 설정 및 활성화
        dialogUI.SetNPCImage(npc.npcID);
        GameObject imageObj = dialogUI.GetNPCImageObject();
        if (imageObj == null)
        {
            Debug.LogWarning("NPC 이미지 오브젝트가 없거나 활성화 실패");
            yield break;
        }

        npcObject.SetActive(true);

        // 2) 등장 애니메이션 (startY → endY)
        yield return StartCoroutine(AnimateNPCEntrance(npcObject));

        // 3) 애니메이션 끝난 뒤 2초 대기
        yield return new WaitForSeconds(0.5f);

        // 4) 인사 대사 자동으로 시작
        yield return dialogUI.Show(npc.greetingDialogue);
        yield return dialogUI.Show(npc.orderDialogue);

        // 5) “타르트 만들기” 버튼 클릭 대기
        yield return dialogUI.WaitForMakeTartClick();

        // 6) 타르트 제작 시작 (recipeID만 넘김)
        tartManager.StartTartMaking(npc.recipeID);
        yield return new WaitUntil(() => tartManager.IsTartComplete);

        // 7) 결과 대사
        bool success = tartManager.CheckTartResult();
        string result = success ? npc.satisfiedDialogue : npc.unsatisfiedDialogue;
        yield return dialogUI.Show(result);

        // 8) NPC 비활성화
        npcObject.SetActive(false);
    }

    private IEnumerator AnimateNPCEntrance(GameObject obj)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        if (rect == null) yield break;

        Vector2 startPos = new Vector2(rect.anchoredPosition.x, startY);
        Vector2 endPos = new Vector2(rect.anchoredPosition.x, endY);
        float elapsed = 0f;

        rect.anchoredPosition = startPos;
        while (elapsed < animateDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animateDuration);
            float ease = Mathf.Sin(t * Mathf.PI * 0.5f);  // ease-out
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, ease);
            yield return null;
        }
        rect.anchoredPosition = endPos;
    }
}