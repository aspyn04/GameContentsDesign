using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TartTopping : MonoBehaviour
{
    [Header("토핑용 Toggle 목록")]
    [SerializeField] private List<Toggle> toppingToggles;

    [Header("다음 버튼")]
    [SerializeField] private Button nextButton;

    [Header("토핑 UI 패널")]
    [SerializeField] private GameObject panelObject;

    private List<string> requiredIngredients;
    private bool priorSuccess;
    private TartManager tartManagerRef;
    private bool isInitialized = false;
    
    public void Init(List<string> recipeIngredients, bool crustWasSuccessful, TartManager manager)
    {
        requiredIngredients = new List<string>(recipeIngredients);
        priorSuccess = crustWasSuccessful;
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

        if (!priorSuccess)
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

            success = (selectedList.Count == requiredIngredients.Count)
                      && !selectedList.Except(requiredIngredients).Any();
        }

        Debug.Log($"TartTopping: 선택된 토핑=[{string.Join(", ", toppingToggles.Where(t => t.isOn).Select(t => t.gameObject.name))}], " +
                  $"필요=[{string.Join(", ", requiredIngredients)}], 결과={success}");

        if (panelObject != null)
            panelObject.SetActive(false);

        tartManagerRef?.OnToppingComplete(success);
    }
}
