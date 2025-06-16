using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookManager : MonoBehaviour
{
    [Header("������ �гε� (�� 3��)")]
    [SerializeField] private List<GameObject> pagePanels;

    [Header("UI ��ư")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button closeButton;

    [Header("å ��ü �г� (��Ʈ)")]
    [SerializeField] private GameObject recipeBookPanel;

    private int currentPageIndex = 0;
    private bool isOpen = false;

    void Start()
    {
        prevButton.onClick.AddListener(OnClickPrev);
        nextButton.onClick.AddListener(OnClickNext);
        closeButton.onClick.AddListener(CloseBook);
        recipeBookPanel.SetActive(false);
        UpdatePage();
    }

    void Update()
    {
        if (!isOpen) return;

        // �������� �� �ð� ���� ����
        if (Time.timeScale != 0f)
            Time.timeScale = 0f;

    }

    public void OpenBook()
    {
        currentPageIndex = 0;
        isOpen = true;
        recipeBookPanel.SetActive(true);

        Time.timeScale = 0f;
        BGMManager_Game.Instance?.PauseMusic();

        UpdatePage();

        Debug.Log("=== [����] å ���� - �ر� ���� ���� ���� ===");

        foreach (var panel in pagePanels)
        {
            var controller = panel.GetComponent<RecipeEntryButton>();
            if (controller != null)
            {
                Debug.Log($"[����] ������ '{panel.name}'�� ��ư ���� ������Ʈ ȣ��");
                controller.UpdateAllEntries();
            }
            else
            {
                Debug.LogWarning($"[����] ������ '{panel.name}'�� RecipeEntryButton ����");
            }
        }

        Debug.Log("=== [����] å ���� - �ر� ���� ���� �� ===");
    }

    private void CloseBook()
    {
        isOpen = false;
        recipeBookPanel.SetActive(false);

        Time.timeScale = 1f;
        BGMManager_Game.Instance?.ResumeMusic();
    }

    private void OnClickPrev()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePage();
        }
    }

    private void OnClickNext()
    {
        if (currentPageIndex < pagePanels.Count - 1)
        {
            currentPageIndex++;
            UpdatePage();
        }
    }

    private void UpdatePage()
    {
        // ��� ������ ���� ���� �������� �ѱ�
        for (int i = 0; i < pagePanels.Count; i++)
        {
            pagePanels[i].SetActive(i == currentPageIndex);
        }

        // ��ư ���� ����
        prevButton.interactable = currentPageIndex > 0;
        nextButton.interactable = currentPageIndex < pagePanels.Count - 1;
    }
}
