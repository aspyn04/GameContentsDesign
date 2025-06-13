using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    [SerializeField] private RewardPopupController rewardPopup;

    private List<string> shownNPCIDs = new List<string>();

    [Header("Data")]
    public List<NPCData> npcDataList;
    public NPCDataManager npcDataManager;
    public TartManager tartManager;
    public DialogUI dialogUI;

    [Header("NPC Appear Object")]
    [SerializeField] private GameObject npcObject;
    [SerializeField] private float animateDuration = 0.5f;
    [SerializeField] private float startY = -300f;
    [SerializeField] private float endY = -76f;

    [Header("Tart Appear Object")]
    [SerializeField] private GameObject tartResultObject; // Tart �̹��� ������ GameObject
    [SerializeField] private RawImage tartResultImage;     // ���� �̹��� ��������

    void Start()
    {

        if (tartResultObject != null)
            tartResultObject.SetActive(false);

        if (dialogUI == null) Debug.LogError("DialogUI ���� X");
        
        if (npcDataManager != null)
            npcDataList = npcDataManager.NPCDataList;
        else
            Debug.LogError("NPCDataManager ���� X");

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
                Debug.LogWarning("�ش� ������ �ش��ϴ� NPC ����");
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

        // 1����
        if (day == 1)
        {
            targetNPCIDs = new List<string> { "2011005", "2011018", "2011016" };
        }

        // 2-4����
        else if (day >= 2 && day <= 4)
        {
            targetNPCIDs = new List<string> { "2011005", "2011018", "2011016", "2011003", "2011009", "2011020" };
        }

        // 5����
        else if (day == 5)
        {
            targetNPCIDs = new List<string> { "2011012", "2011003", "2011016", "2011005", "2011020", "2011026", "2011018", "2011009" };
        }

        // 6����
        else if (day == 6)
        {
            targetNPCIDs = new List<string> { "2011021", "2001001", "2011018", "2011003", "2011020", "2011005", "2011016", "2011026" };
        }

        // 7����
        else if (day == 7)
        {
            targetNPCIDs = new List<string> { "2011009", "2011012", "2011018", "2011005", "2011026", "2011021", "2011003", "2011016" };
        }

        // 8����
        else if (day == 8)
        {
            targetNPCIDs = new List<string> { "2011005", "2011016", "2011021", "2011009", "2011024", "2011004", "2011012", "2011003" };
        }

        // 9���� 
        else if (day == 9)
        {
            targetNPCIDs = new List<string> { "2011026", "2011018", "2011003", "2011020", "2011005", "2011016", "2011004", "2011015" };
        }

        // 10����
        else if (day == 9)
        {
            targetNPCIDs = new List<string> { "2011012", "2001001", "2011015", "2011026", "2011018", "2011020", "2011009", "2011003" };
        }

        // 11����
        else if (day == 11)
        {
            targetNPCIDs = new List<string> { "2011016", "2011003", "2011005", "2011021", "2011012", "2011024", "2011007", "2011002" };
        }

        // 12����
        else if (day == 12)
        {
            targetNPCIDs = new List<string> { "2001001", "2011020", "2011015", "2011026", "2011018", "2011006", "2011009", "2011004" };
        }

        // 13����
        else if (day == 13)
        {
            targetNPCIDs = new List<string> { "2001004", "2011005", "2011018", "2011016", "2011003", "2011009", "2011020" };
        }       
        
        // 14����
        else if (day == 14)
        {
            targetNPCIDs = new List<string> { "2011005", "2011016", "2011003", "2011021", "2011024", "2011002", "2011007", "2011009" };
        }
                
        // 15����
        else if (day == 15)
        {
            targetNPCIDs = new List<string> { "2001001", "2011018", "2011020", "2011012", "2011004", "2011015", "2011006", "2011014" };
        }
                
        // 16����
        else if (day == 16)
        {
            targetNPCIDs = new List<string> { "2011026", "2011008", "2011011", "2011025", "2011002", "2011007", "2011009", "2011016" };
        }
                
        // 17����
        else if (day == 17)
        {
            targetNPCIDs = new List<string> { "2011018", "2011005", "2011020", "2011016", "2011007", "2011008", "2001002", "2011017", "2011002", "2011025" };
        }  
        
        // 18����
        else if (day == 18)
        {
            targetNPCIDs = new List<string> { "2011016", "2011009", "2011021", "2011026", "2011004", "2011006", "2011015", "2001004", "2011011", "2011019" };
        }
                
        // 19����
        else if (day == 19)
        {
            targetNPCIDs = new List<string> { "2001001", "2011005", "2011018", "2011003", "2011020", "2011012", "2011024", "2011002", "2011017", "2011014" };
        }
                
        // 20����
        else if (day == 20)
        {
            targetNPCIDs = new List<string> { "2011005", "2011012", "2011024", "2011016", "2011004", "2011020", "2011019", "2001001", "2011011", "2011003" };
        }
                        
        // 21����
        else if (day == 21)
        {
            targetNPCIDs = new List<string> { "2011002", "2011010", "2011009", "2011026", "2011025", "2001004", "2011008", "2011015", "2001003", "2011021" };
        }
                        
        // 22����
        else if (day == 22)
        {
            targetNPCIDs = new List<string> { "2011014", "2001002", "2011017", "2011018", "2011006", "2011022", "2011007", "2011016", "2011012", "2011005" };
        }
                        
        // 23����
        else if (day == 23)
        {
            targetNPCIDs = new List<string> { "2011005", "2011018", "2011012", "2001004", "2011013", "2011026", "2011009", "2011001", "2011011", "2011024" };
        }
                        
        // 24����
        else if (day == 24)
        {
            targetNPCIDs = new List<string> { "2011002", "2001001", "2011015", "2011014", "2011010", "2011025", "2011006", "2011005", "2011023", "2011017" };
        }
                                
        // 25����
        else if (day == 25)
        {
            targetNPCIDs = new List<string> { "2011020", "2011016", "2011019", "2011022", "2001003", "2011012", "2011004", "2011009", "2011010", "2011021" };
        }
                                
        // 26����
        else if (day == 26)
        {
            targetNPCIDs = new List<string> { "2011008", "2011024", "2011013", "2011026", "2011003", "2011007", "2011016", "2001001", "2011018", "2011001" };
        }
                                
        // 27����
        else if (day == 27)
        {
            targetNPCIDs = new List<string> { "2011002", "2011020", "2011005", "2011003", "2011014", "2011008", "2011011", "2011022", "2011025", "2011017" };
        }
                                
        // 28����
        else if (day == 28)
        {
            targetNPCIDs = new List<string> { "2011006", "2001002", "2011001", "2011019", "2011007", "2011015", "2011021", "2011023", "2011004", "2001003" };
        }
                                
        // 29����
        else if (day == 29)
        {
            targetNPCIDs = new List<string> { "2011018", "2011009", "2011013", "2011024", "2011016", "2011010", "2011002", "2011006", "2001004", "2011026" };
        }
                                        
        // 30����
        else
        { 
            targetNPCIDs = new List<string> { "2011005", "2011008", "2011007", "2011012", "2011011", "2011014", "2011020", "2011021", "2001002", "2011017" };
        }

        var availableNPCs = targetNPCIDs.Except(shownNPCIDs).ToList();


        string selectedNPCID = availableNPCs[Random.Range(0, availableNPCs.Count)];
        shownNPCIDs.Add(selectedNPCID);

        return npcDataList.Find(npc => npc.npcID == selectedNPCID);
    }

    private IEnumerator HandleNPC(NPCData npc)
    {

        // 1) NPC �̹��� ���� �� Ȱ��ȭ
        dialogUI.SetNPCImage(npc.npcID);
        npcObject.SetActive(true);

        // 2) ���� �ִϸ��̼�
        yield return AnimateNPCEntrance(npcObject);
        yield return new WaitForSeconds(0.5f);

        // 3) �λ�/�ֹ� ���
        yield return dialogUI.Show(npc.greetingDialogue);
        yield return dialogUI.Show(npc.orderDialogue);

        // 4) Ÿ��Ʈ ���� ��ư Ŭ�� ���
        yield return dialogUI.WaitForMakeTartClick();
        dialogUI.HideDialogPanel();

        tartManager.StartTartMaking(npc.recipeID);

        // ���� �Ϸ� ���
        yield return new WaitUntil(() => tartManager.IsProductionDone);

        // 5) ���� ���� üũ
        bool success = tartManager.CheckTartResult();
        string resultText = success ? npc.satisfiedDialogue : npc.unsatisfiedDialogue;

        Debug.Log($"[NPCManager] ���� �Ϸ� �÷���: {tartManager.IsProductionDone}, ���� ����: {success}");

        // 6) Ÿ��Ʈ �̹��� ǥ��
        string tartID = success ? npc.recipeID.ToString() : "4001010";
        Texture tartTexture = Resources.Load<Texture>($"Images/Tart/Tart/{tartID}");

        if (tartTexture != null)
        {
            tartResultImage.texture = tartTexture;
            tartResultObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"[NPCManager] Ÿ��Ʈ �̹��� �ε� ����: {tartID}");
        }

        // 7) ��� ���
        yield return dialogUI.Show(resultText);
        dialogUI.HideDialogPanel();

        int cheese = success ? 100 : 20;
        int star = success ? 2 : 0;

        // 2. �ڿ� �ݿ�
        GoodsManager.Instance.AddCheese(cheese);
        if (star > 0) GoodsManager.Instance.AddStar(star);

        // 3. ��Ȯ�� ��ġ ����
        rewardPopup.ShowReward(cheese, star);


        // 10) NPC ���� & Ÿ��Ʈ �̹��� �����
        yield return FadeImage(tartResultImage, 1f, 0f, 0.24f);
        yield return AnimateNPCExit(npcObject);
        yield return new WaitForSeconds(0.5f);

        npcObject.SetActive(false);
        tartResultObject.SetActive(false);
        SetImageAlpha(tartResultImage, 1f);

        // 11) ���
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator FadeImage(RawImage image, float fromAlpha, float toAlpha, float duration)
    {
        if (image == null) yield break;

        Color color = image.color;
        float timer = 0f;

        while (timer < duration)
        {
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, timer / duration);
            image.color = new Color(color.r, color.g, color.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        image.color = new Color(color.r, color.g, color.b, toAlpha);
    }

    private void SetImageAlpha(RawImage image, float alpha)
    {
        if (image != null)
        {
            Color c = image.color;
            image.color = new Color(c.r, c.g, c.b, alpha);
        }
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