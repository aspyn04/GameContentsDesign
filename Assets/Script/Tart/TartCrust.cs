using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TartCrust : MonoBehaviour
{
    [Header("정답 순서대로 배열된 버튼 (예: a, b, c, d, …)")]
    [SerializeField] private List<Button> correctOrder = new List<Button>();

    [Header("모든 재료 버튼 (순서는 상관없음)")]
    [SerializeField] private List<Button> ingredientButtons = new List<Button>();

    [Header("다음 단계 버튼")]
    [SerializeField] private Button nextButton;

    [Header("재료 단계 전체 UI 패널")]
    [SerializeField] private GameObject panelObject;

    [Header("기본 색상 (비활성)")]
    [SerializeField] private Color defaultColor = Color.white;

    [Header("선택된 버튼 색상")]
    [SerializeField] private Color selectedColor = Color.yellow;

    private TartManager tartManagerRef;
    private bool isInitialized = false;

    private Dictionary<Button, bool> buttonState = new Dictionary<Button, bool>();
    private List<Button> selectedHistory = new List<Button>();

    public void Init(List<string> unused, TartManager manager)
    {
        tartManagerRef = manager;
        isInitialized = true;

        selectedHistory.Clear();
        buttonState.Clear();

        // 1. ingredientButtons의 위치 랜덤 섞기
        ShuffleButtonPositions();

        // 2. 버튼 색상 초기화 및 클릭 리스너 등록
        foreach (var btn in ingredientButtons)
        {
            buttonState[btn] = false;

            Image img = btn.GetComponent<Image>();
            if (img != null) img.color = defaultColor;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnIngredientClicked(btn));
        }

        // 3. Next 버튼 초기화
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);
        nextButton.interactable = false;

        // 4. UI 패널 표시
        if (panelObject != null)
            panelObject.SetActive(true);
    }

    private void OnIngredientClicked(Button btn)
    {
        if (!isInitialized) return;

        bool isOn = !buttonState[btn];
        buttonState[btn] = isOn;

        // 색상 변경
        Image img = btn.GetComponent<Image>();
        if (img != null) img.color = isOn ? selectedColor : defaultColor;

        if (isOn)
            selectedHistory.Add(btn);
        else
            selectedHistory.Remove(btn);

        nextButton.interactable = (selectedHistory.Count >= correctOrder.Count);
    }

    private void OnNextClicked()
    {
        if (!isInitialized) return;

        bool success = false;

        if (selectedHistory.Count == correctOrder.Count)
        {
            success = true;
            for (int i = 0; i < correctOrder.Count; i++)
            {
                if (selectedHistory[i] != correctOrder[i])
                {
                    success = false;
                    break;
                }
            }
        }

        Debug.Log($"TartCrust: 선택된 순서 = [{GetSequence(selectedHistory)}], " +
                  $"정답 순서 = [{GetSequence(correctOrder)}], 결과 = {success}");

        if (panelObject != null)
            panelObject.SetActive(false);

        tartManagerRef?.OnCrustComplete(success);
    }

    private string GetSequence(List<Button> list)
    {
        List<string> names = new List<string>();
        foreach (var b in list)
            names.Add(b.gameObject.name);
        return string.Join(" → ", names);
    }

    /// <summary>
    /// ingredientButtons의 위치를 셔플합니다.
    /// </summary>
    private void ShuffleButtonPositions()
    {
        // a, b 위치 교환
        Button aButton = ingredientButtons.Find(b => b.name == "a");
        Button bButton = ingredientButtons.Find(b => b.name == "b");

        if (aButton != null && bButton != null)
        {
            Vector3 tempPos = aButton.transform.localPosition;
            aButton.transform.localPosition = bButton.transform.localPosition;
            bButton.transform.localPosition = tempPos;
        }

        // c, d, e, f 섞기
        List<Button> groupButtons = ingredientButtons.FindAll(b =>
            b.name == "c" || b.name == "d" || b.name == "e" || b.name == "f");

        List<Vector3> positions = new List<Vector3>();
        foreach (var btn in groupButtons)
            positions.Add(btn.transform.localPosition);

        for (int i = 0; i < positions.Count; i++)
        {
            int rand = Random.Range(i, positions.Count);
            (positions[i], positions[rand]) = (positions[rand], positions[i]);
        }

        for (int i = 0; i < groupButtons.Count; i++)
            groupButtons[i].transform.localPosition = positions[i];
    }

}
