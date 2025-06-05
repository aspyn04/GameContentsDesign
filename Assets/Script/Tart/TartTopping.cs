using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TartTopping : MonoBehaviour
{
    [Header("���ο� Toggle ���")]
    [SerializeField] private List<Toggle> toppingToggles;

    [Header("���� ��ư")]
    [SerializeField] private Button nextButton;

    [Header("���� UI �г�")]
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

        Debug.Log($"TartTopping: ���õ� ����=[{string.Join(", ", toppingToggles.Where(t => t.isOn).Select(t => t.gameObject.name))}], " +
                  $"�ʿ�=[{string.Join(", ", requiredIngredients)}], ���={success}");

        if (panelObject != null)
            panelObject.SetActive(false);

        tartManagerRef?.OnToppingComplete(success);
    }
}
