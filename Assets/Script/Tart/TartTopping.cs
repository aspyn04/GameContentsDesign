using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� �ܰ�: ��� �ܰ� ����� ����, 
/// ���ο� Toggle(���� ���ID ���)�� Ȯ���� �� ����� TartManager�� ����.
/// </summary>
/// <summary>
/// ������ Ȯ�Ρ� �ܰ� �Ŵ���.
/// ��� �ܰ� ���� ���ο� ��� ���¸� ���Ͽ� ���� ����� TartManager�� �����մϴ�.
/// </summary>
public class TartTopping : MonoBehaviour
{
    [Header("���� Ȯ�ο� Toggle ��� (��� ���ð� ���� ��� ���)")]
    [SerializeField] private List<Toggle> toppingToggles;

    [Header("���� ��ư")]
    [SerializeField] private Button nextButton;

    [Header("��ü ���� Ȯ�� UI �г�")]
    [SerializeField] private GameObject panelObject;

    private List<string> requiredIngredients;
    private bool crustWasSuccessful;
    private TartManager tartManagerRef;
    private bool isInitialized = false;

    /// <summary>
    /// TartManager���� ȣ���Ͽ� ���� �ܰ� �ʱ�ȭ.
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

            Debug.Log($"TartTopping: ���õ� ����=[{string.Join(", ", selectedList)}], " +
                      $"�ʿ�=[{string.Join(", ", requiredIngredients)}], ���={success}");
        }

        if (panelObject != null)
            panelObject.SetActive(false);

        tartManagerRef?.OnToppingComplete(success);
    }
}