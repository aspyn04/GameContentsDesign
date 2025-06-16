using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookManager : MonoBehaviour
{
    [Header("페이지 패널들 (총 3개)")]
    [SerializeField] private List<GameObject> pagePanels;

    [Header("UI 버튼")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button closeButton;

    [Header("책 전체 패널 (루트)")]
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

        // 열려있을 때 시간 정지 유지
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

        Debug.Log("=== [도감] 책 열기 - 해금 상태 점검 시작 ===");

        foreach (var panel in pagePanels)
        {
            var controller = panel.GetComponent<RecipeEntryButton>();
            if (controller != null)
            {
                Debug.Log($"[도감] 페이지 '{panel.name}'의 버튼 상태 업데이트 호출");
                controller.UpdateAllEntries();
            }
            else
            {
                Debug.LogWarning($"[도감] 페이지 '{panel.name}'에 RecipeEntryButton 없음");
            }
        }

        Debug.Log("=== [도감] 책 열기 - 해금 상태 점검 끝 ===");
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
        // 모든 페이지 끄고 현재 페이지만 켜기
        for (int i = 0; i < pagePanels.Count; i++)
        {
            pagePanels[i].SetActive(i == currentPageIndex);
        }

        // 버튼 상태 갱신
        prevButton.interactable = currentPageIndex > 0;
        nextButton.interactable = currentPageIndex < pagePanels.Count - 1;
    }
}
