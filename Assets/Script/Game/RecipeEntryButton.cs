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
    [Header("도감 버튼 리스트")]
    [SerializeField] private List<RecipeEntry> entryList;

    [Header("버튼 묶음 부모 (숨기기/보이기 용)")]
    [SerializeField] private GameObject buttonGroupRoot;

    [Header("공용 상세 정보 패널")]
    [SerializeField] private GameObject detailPanel;

    [Header("상세 패널 내부 이미지")]
    [SerializeField] private Image detailImage;

    [Header("상세 패널 닫기 버튼")]
    [SerializeField] private Button detailCloseButton;

    private RecipeEntry currentEntry = null;

    void Start()
    {
        foreach (var entry in entryList)
        {
            RecipeEntry capturedEntry = entry;

            if (entry.button != null)
            {
                entry.button.onClick.RemoveAllListeners(); // 중복 방지
                entry.button.onClick.AddListener(() => OnClickEntry(capturedEntry));
            }

            else
            {
                Debug.LogWarning("버튼이 연결되지 않았습니다 (entry.button == null)");
            }
        }

        detailCloseButton?.onClick.AddListener(CloseDetailPanel);
        if (detailPanel != null)
            detailPanel.SetActive(false);
    }

    void OnEnable()
    {
        UpdateAllEntries(); // OnEnable에선 상태만 갱신
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
        Debug.Log($"[도감] 현재 일차: {currentDay}");

        foreach (var entry in entryList)
        {
            if (entry.button == null || entry.buttonImage == null)
            {
                Debug.LogWarning($"[도감] 버튼 또는 이미지 연결 안 됨 → {entry.unlockDay}");
                continue;
            }

            bool unlocked = currentDay >= entry.unlockDay;
            Debug.Log($"[도감] {entry.button.name} / 해금일: {entry.unlockDay}, 해금됨: {unlocked}");

            entry.button.interactable = unlocked;
            entry.buttonImage.sprite = unlocked ? entry.unlockedSprite : entry.lockedSprite;
        }
    }

    private void OnClickEntry(RecipeEntry entry)
    {
        if (!entry.button.interactable || detailImage == null || entry.detailSprite == null)
        {
            Debug.LogWarning("클릭 불가하거나 detailImage/detailSprite 누락");
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
