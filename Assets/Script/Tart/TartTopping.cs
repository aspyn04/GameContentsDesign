using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 토핑 단계: 재료 단계 결과에 따라, 
/// 토핑용 Toggle(같은 재료ID 토글)을 확인한 뒤 결과를 TartManager에 전달.
/// </summary>
/// <summary>
/// “토핑 확인” 단계 매니저.
/// 재료 단계 성공 여부와 토글 상태를 비교하여 최종 결과를 TartManager에 전달합니다.
/// </summary>
public class TartTopping : MonoBehaviour
{
    [Header("토핑 확인용 Toggle 목록 (재료 선택과 동일 토글 사용)")]
    [SerializeField] private List<Toggle> toppingToggles;

    [Header("다음 버튼")]
    [SerializeField] private Button nextButton;

    [Header("전체 토핑 확인 UI 패널")]
    [SerializeField] private GameObject panelObject;

    private List<string> requiredIngredients;
    private bool crustWasSuccessful;
    private TartManager tartManagerRef;
    private bool isInitialized = false;

    /// <summary>
    /// TartManager에서 호출하여 토핑 단계 초기화.
    /// </summary>
    public void Init(List<string> recipeIngredients, bool crustSuccess, TartManager manager)
    {
        requiredIngredients = new List<string>(recipeIngredients);
        crustWasSuccessful = crustSuccess;
        tartManagerRef = manager;
        isInitialized = true;

        if (panelObject != null)
            panelObject.SetActive(true);

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);
    }

    private void OnNextClicked()
    {
        if (!isInitialized) return;

        bool success = false;

        if (!crustWasSuccessful)
        {
            success = false;
        }
        else
        {
            List<string> selectedList = new List<string>();
            foreach (var tog in toppingToggles)
            {
                if (tog.isOn)
                    selectedList.Add(tog.gameObject.name);
            }

            if (selectedList.Count == requiredIngredients.Count)
            {
                success = new HashSet<string>(selectedList).SetEquals(requiredIngredients);
            }
            else
            {
                success = false;
            }

            Debug.Log($"TartTopping: 선택된 토핑=[{string.Join(", ", selectedList)}], " +
                      $"필요=[{string.Join(", ", requiredIngredients)}], 결과={success}");
        }

        if (panelObject != null)
            panelObject.SetActive(false);

        tartManagerRef?.OnToppingComplete(success);
    }
}