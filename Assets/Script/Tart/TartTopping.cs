using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TartTopping : MonoBehaviour
{

    [Header("토핑용 버튼들 (이름을 재료 ID 문자열로 설정)")]
    [SerializeField] private List<Button> toppingButtons = new List<Button>();

    [Header("다음 단계 버튼")]
    [SerializeField] private Button nextButton;

    [Header("토핑 UI 패널")]
    [SerializeField] private GameObject panelObject;

    [Header("버튼 기본 색상")]
    [SerializeField] private Color defaultColor = Color.white;

    [Header("버튼 선택 색상")]
    [SerializeField] private Color selectedColor = Color.green;

    private TartManager tartManagerRef;
    private List<string> requiredIngredients;
    private Dictionary<Button, bool> buttonState = new Dictionary<Button, bool>();
    private bool isInitialized = false;

    /// <summary>
    /// recipeIngredients: CSV에서 읽어온, 이 타르트가 필요로 하는 재료 ID 문자열 목록
    /// </summary>
    public void Init(List<string> recipeIngredients, TartManager manager)
    {
        requiredIngredients = new List<string>(recipeIngredients);
        tartManagerRef = manager;
        isInitialized = true;

        buttonState.Clear();
        foreach (var btn in toppingButtons)
        {
            var colorScript = btn.GetComponent<ButtonImangeColor>();
            if (colorScript != null)
            {
                colorScript.InitializeButton();      
                colorScript.ResetButtonState();     
            }

            buttonState[btn] = false;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnToppingButtonClicked(btn));
        }

        nextButton.interactable = true;
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);

        if (panelObject != null)
            panelObject.SetActive(true);
    }


    private void OnToppingButtonClicked(Button btn)
    {
        if (!isInitialized) return;

        // 토글 상태 변경
        bool isOn = !buttonState[btn];
        buttonState[btn] = isOn;

        // 색상 표시
        var img = btn.GetComponent<Image>();
        if (img != null)
            img.color = isOn ? selectedColor : defaultColor;
    }

    private void OnNextClicked()
    {
        if (!isInitialized) return;

        var selected = buttonState
            .Where(kvp => kvp.Value)
            .Select(kvp => kvp.Key.gameObject.name)
            .ToList();

        bool success = selected.Count == requiredIngredients.Count
                       && !selected.Except(requiredIngredients).Any();

        Debug.Log($"TartTopping: 선택=[{string.Join(", ", selected)}], " +
                  $"필요=[{string.Join(", ", requiredIngredients)}], 성공={success}");

        if (panelObject != null)
            panelObject.SetActive(false);

        tartManagerRef?.OnToppingComplete(success);
    }

}