using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RecipeEntry
{
    public int unlockDay;
    public Button button;
    public Image buttonImage;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    public Sprite detailSprite;
}

public class RecipeEntryButton : MonoBehaviour
{
    [Header("���� ��ư ����Ʈ")]
    [SerializeField] private List<RecipeEntry> entryList;

    [Header("��ư ���� �θ� (�����/���̱� ��)")]
    [SerializeField] private GameObject buttonGroupRoot;

    [Header("���� �� ���� �г�")]
    [SerializeField] private GameObject detailPanel;

    [Header("�� �г� ���� �̹���")]
    [SerializeField] private Image detailImage;

    [Header("�� �г� �ݱ� ��ư")]
    [SerializeField] private Button detailCloseButton;

    private RecipeEntry currentEntry = null;

    void Start()
    {
        foreach (var entry in entryList)
        {
            RecipeEntry capturedEntry = entry;

            if (entry.button != null)
            {
                entry.button.onClick.RemoveAllListeners(); // �ߺ� ����
                entry.button.onClick.AddListener(() => OnClickEntry(capturedEntry));
            }

            else
            {
                Debug.LogWarning("��ư�� ������� �ʾҽ��ϴ� (entry.button == null)");
            }
        }

        detailCloseButton?.onClick.AddListener(CloseDetailPanel);
        if (detailPanel != null)
            detailPanel.SetActive(false);
    }

    void OnEnable()
    {
        UpdateAllEntries(); // OnEnable���� ���¸� ����
    }

    void Update()
    {
        if (detailPanel != null && detailPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseDetailPanel();
        }
    }

    public void UpdateAllEntries()
    {
        int currentDay = TimeManager.Instance != null ? TimeManager.Instance.currentDay : 1;
        Debug.Log($"[����] ���� ����: {currentDay}");

        foreach (var entry in entryList)
        {
            if (entry.button == null || entry.buttonImage == null)
            {
                Debug.LogWarning($"[����] ��ư �Ǵ� �̹��� ���� �� �� �� {entry.unlockDay}");
                continue;
            }

            bool unlocked = currentDay >= entry.unlockDay;
            Debug.Log($"[����] {entry.button.name} / �ر���: {entry.unlockDay}, �رݵ�: {unlocked}");

            entry.button.interactable = unlocked;
            entry.buttonImage.sprite = unlocked ? entry.unlockedSprite : entry.lockedSprite;
        }
    }

    private void OnClickEntry(RecipeEntry entry)
    {
        if (!entry.button.interactable || detailImage == null || entry.detailSprite == null)
        {
            Debug.LogWarning("Ŭ�� �Ұ��ϰų� detailImage/detailSprite ����");
            return;
        }

        currentEntry = entry;

        buttonGroupRoot?.SetActive(false);
        detailPanel?.SetActive(true);

        detailImage.sprite = entry.detailSprite;
    }

    private void CloseDetailPanel()
    {
        detailPanel?.SetActive(false);
        buttonGroupRoot?.SetActive(true);
        currentEntry = null;
    }
}
