using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TartTopping : MonoBehaviour
{
    [Header("토핑용 버튼들 (이름을 재료 ID 문자열로 설정)")]
    [SerializeField] private List<Button> toppingButtons = new List<Button>();

    [Header("중앙에 보여줄 재료 이미지들 (버튼 이름과 이름 일치해야 함)")]
    [SerializeField] private List<GameObject> toppingImageObjects = new List<GameObject>();

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
    private Dictionary<string, GameObject> toppingImageMap = new Dictionary<string, GameObject>();
    private bool isInitialized = false;

    public void Init(List<string> recipeIngredients, TartManager manager)
    {
        requiredIngredients = new List<string>(recipeIngredients);
        tartManagerRef = manager;
        isInitialized = true;

        buttonState.Clear();
        toppingImageMap.Clear();

        // 이미지 매핑 (이름 기준으로)
        foreach (var imgObj in toppingImageObjects)
        {
            string key = imgObj.name.ToLower(); // ex: "choco"
            if (!toppingImageMap.ContainsKey(key))
                toppingImageMap.Add(key, imgObj);

            imgObj.SetActive(false); // 시작 시 꺼두기
        }

        foreach (var btn in toppingButtons)
        {
            string id = btn.gameObject.name.ToLower();

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

        string id = btn.gameObject.name.ToLower();

        // 토글 상태
        bool isOn = !buttonState[btn];
        buttonState[btn] = isOn;

        // 색상 변경
        var img = btn.GetComponent<Image>();
        if (img != null)
            img.color = isOn ? selectedColor : defaultColor;

        // 중앙 이미지 on/off
        if (toppingImageMap.TryGetValue(id, out GameObject visualObj))
        {
            visualObj.SetActive(isOn);
        }
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
