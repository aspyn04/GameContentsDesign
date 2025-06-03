using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// NPC ����(�ִϸ��̼�) �� Ŭ�� �� ��� �� Ÿ��Ʈ ���� ȣ�� �� ��� ��� �帧�� �����մϴ�.
/// </summary>
public class NPCManager : MonoBehaviour
{
    [Header("������ ����")]
    public List<NPCData> npcDataList;
    public NPCDataManager npcDataManager;
    public TartManager tartManager;
    public DialogUI dialogUI;

    [Header("NPC ���� ������Ʈ")]
    [SerializeField] private GameObject npcObject;
    [SerializeField] private float animateDuration = 0.5f;
    [SerializeField] private float startY = -300f;   // ȭ�� �ϴ� ���� Y
    [SerializeField] private float endY = -76f;      // ���� �� Y ��ġ

    void Start()
    {
        if (dialogUI == null) Debug.LogError("DialogUI ���� �� ��");
        if (npcDataManager != null)
            npcDataList = npcDataManager.NPCDataList;
        else
            Debug.LogError("NPCDataManager ���� �� ��");

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
                Debug.LogWarning("�ش� ������ �ش��ϴ� NPC ����");
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
                Debug.LogWarning($"NPC ID �Ľ� ����: '{npc.npcID}'");
                return false;
            }
        });

        if (candidates.Count == 0) return null;
        return candidates[Random.Range(0, candidates.Count)];
    }

    /// <summary>
    /// NPC ���� �ִϸ��̼� �� 2�� ��� �� �ڵ����� ��� ���� �帧
    /// </summary>
    private IEnumerator HandleNPC(NPCData npc)
    {
        // 1) NPC �̹��� ���� �� Ȱ��ȭ
        dialogUI.SetNPCImage(npc.npcID);
        GameObject imageObj = dialogUI.GetNPCImageObject();
        if (imageObj == null)
        {
            Debug.LogWarning("NPC �̹��� ������Ʈ�� ���ų� Ȱ��ȭ ����");
            yield break;
        }

        npcObject.SetActive(true);

        // 2) ���� �ִϸ��̼� (startY �� endY)
        yield return StartCoroutine(AnimateNPCEntrance(npcObject));

        // 3) �ִϸ��̼� ���� �� 2�� ���
        yield return new WaitForSeconds(0.5f);

        // 4) �λ� ��� �ڵ����� ����
        yield return dialogUI.Show(npc.greetingDialogue);
        yield return dialogUI.Show(npc.orderDialogue);

        // 5) ��Ÿ��Ʈ ����⡱ ��ư Ŭ�� ���
        yield return dialogUI.WaitForMakeTartClick();

        // 6) Ÿ��Ʈ ���� ���� (recipeID�� �ѱ�)
        tartManager.StartTartMaking(npc.recipeID);
        yield return new WaitUntil(() => tartManager.IsTartComplete);

        // 7) ��� ���
        bool success = tartManager.CheckTartResult();
        string result = success ? npc.satisfiedDialogue : npc.unsatisfiedDialogue;
        yield return dialogUI.Show(result);

        // 8) NPC ��Ȱ��ȭ
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