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
        tartResultObject.SetActive(false);

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

        // 2����
        else if (day == 2)
        {
            targetNPCIDs = new List<string> { "2011005", "2011018", "2011016", "2011003", "2011009", "2011020" };
        }
                
        else if (day == 3)
        {
            targetNPCIDs = new List<string> {  "2001001", "2011005", "2011018", "2011016", "2011003", "2011009", "2011020", "2011012", "2011021", "2011026" };
        }        
        
        else if (day == 4)
        {
            targetNPCIDs = new List<string> { "2001001", "2011005", "2011018", "2011016", "2011003", "2011009", "2011020", "2011012", "2011021", "2011026" };
        }


        // 5����
        else if (day == 5)
        {
            targetNPCIDs = new List<string> { "2001001", "2011005", "2011018", "2011016", "2011003", "2011009", "2011020", "2011012", "2011021", "2011026" };
        }

        // 6����
        else if (day == 6)
        {
            targetNPCIDs = new List<string> { "2001001", "2011005", "2011012", "2011021", "2011004", "2011009", "2011020", "2011015", "2011003", "2011016" };
        }

        // 7����
        else if (day == 7)
        {
            targetNPCIDs = new List<string> { "2011006", "2011007", "2011002", "2011018", "2011024", "2011026", "2011012", "2011003", "2011005", "2011009" };
        }

        // 8����
        else if (day == 8)
        {
            targetNPCIDs = new List<string> { "2011015", "2001001", "2011021", "2011004", "2011016", "2011002", "2011006", "2011020", "2011007", "2011018" };
        }

        // 9���� 
        else if (day == 9)
        {
            targetNPCIDs = new List<string> { "2001004", "2011005", "2011003", "2011012", "2011006", "2011024", "2011016", "2011026", "2011007", "2011018" };
        }

        // 10����
        else if (day == 9)
        {
            targetNPCIDs = new List<string> { "2011008", "2011014", "2011011", "2011025", "2011002", "2011004", "2011021", "2001004", "2011020", "2011009" };
        }

        // 11����
        else if (day == 11)
        {
            targetNPCIDs = new List<string> { "2011017", "2011008", "2011015", "2011024", "2011006", "2011011", "2011007", "2011014", "2011025", "2011013" };
        }

        // 12����
        else if (day == 12)
        {
            targetNPCIDs = new List<string> { "2011001", "2011023", "2001002", "2011017", "2011015", "2011008", "2011002", "2011014", "2011019", "2011009" };
        }

        // 13����
        else if (day == 13)
        {
            targetNPCIDs = new List<string> { "2011013", "2011023", "2011001", "2001002", "2011003", "2011010", "2011004", "2011016", "2011025", "2011006" };
        }       
        
        // 14����
        else if (day == 14)
        {
            targetNPCIDs = new List<string> { "2011010", "2011022", "2001003", "2011001", "2011013", "2001004", "2011023", "2011017", "2011015", "2011019" };
        }
                
        // 15����
        else if (day == 15)
        {
            targetNPCIDs = new List<string> { "2011007", "2001002", "2011014", "2011008", "2011020", "2011022", "2001003", "2011010", "2011023", "2011013" };
        }
                
        // 16����
        else if (day == 16)
        {
            targetNPCIDs = new List<string> { "2011001", "2011019", "2011017", "2011015", "2011002", "2011006", "2001002", "2011010", "2011022", "2011008" };
        }
                
        // 17����
        else if (day == 17)
        {
            targetNPCIDs = new List<string> { "2001004", "2011011", "2011025", "2011003", "2011016", "2011001", "2011019", "2001003", "2011009", "2011013" };
        }  
        
        // 18����
        else 
        {
            targetNPCIDs = new List<string> { "2011023", "2011017", "2011014", "2011011", "2011002", "2011007", "2011022", "2001002", "2011015", "2011006" };
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
        NPCSoundManager.Instance?.PlayReactionSound(npc.type, success);

        yield return dialogUI.Show(resultText);

        dialogUI.HideDialogPanel();

        int cheese = success ? 100 : 20;
        int star = success ? 2 : 0;

        // 2. �ڿ� �ݿ�
        GoodsManager.Instance.AddCheese(cheese);
        if (star > 0) GoodsManager.Instance.AddStar(star);

        // 3. ��Ȯ�� ��ġ ����
        rewardPopup.ShowReward(cheese, star);
        NPCSoundManager.Instance?.PlayCheeseRewardSound();


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