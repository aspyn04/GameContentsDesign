using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TartCrust : MonoBehaviour
{
    [Header("Correct order (a → b → c → d → e → f)")]
    [SerializeField] private List<Button> correctOrder = new List<Button>();

    [Header("All ingredient buttons (any order)")]
    [SerializeField] private List<Button> ingredientButtons = new List<Button>();

    [Header("Next-stage button")]
    [SerializeField] private Button nextButton;

    [Header("Panel containing all ingredient buttons")]
    [SerializeField] private GameObject panelObject;

    [Header("Default color for unselected buttons")]
    [SerializeField] private Color defaultColor = Color.white;

    [Header("Color for selected buttons")]
    [SerializeField] private Color selectedColor = Color.yellow;

    private TartManager tartManagerRef;
    private bool isInitialized = false;

    // 버튼별 on/off 상태를 저장
    private Dictionary<Button, bool> buttonState = new Dictionary<Button, bool>();

    // 사용자가 켠 순서대로 담은 리스트
    private List<Button> selectedHistory = new List<Button>();

    /// <summary>
    /// Init이 호출되면 ingredientButtons 위치를 서로 섞은 뒤,
    /// 클릭 리스너와 초기 상태를 설정합니다.
    /// </summary>
    public void Init(List<string> unused, TartManager manager)
    {
        tartManagerRef = manager;
        isInitialized = true;

        selectedHistory.Clear();
        buttonState.Clear();

        // 1) ingredientButtons에 있는 모든 버튼의 원래 위치(anchoredPosition)를 수집
        List<Vector2> originalPositions = new List<Vector2>();
        foreach (var btn in ingredientButtons)
        {
            RectTransform rt = btn.GetComponent<RectTransform>();
            originalPositions.Add(rt.anchoredPosition);
        }

        // 2) 위치 리스트를 섞는다
        ShuffleList(originalPositions);

        // 3) 섞인 위치를 ingredientButtons에 순서대로 재할당
        for (int i = 0; i < ingredientButtons.Count; i++)
        {
            RectTransform rt = ingredientButtons[i].GetComponent<RectTransform>();
            rt.anchoredPosition = originalPositions[i];
        }

        // 4) 각 버튼 상태 초기화 및 클릭 리스너 연결
        foreach (var btn in ingredientButtons)
        {
            buttonState[btn] = false;
            Image img = btn.GetComponent<Image>();
            if (img != null)
                img.color = defaultColor;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnIngredientClicked(btn));
        }

        // 5) Next 버튼 초기화
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);
        nextButton.interactable = false;

        // 6) 패널 활성화
        if (panelObject != null)
            panelObject.SetActive(true);
    }

    /// <summary>
    /// 리스트 요소를 무작위로 섞는 유틸리티 함수
    /// </summary>
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    private void OnIngredientClicked(Button btn)
    {
        if (!isInitialized) return;

        bool isOn = !buttonState[btn];
        buttonState[btn] = isOn;

        // 시각적 토글 표시: 색 변경
        Image img = btn.GetComponent<Image>();
        if (img != null)
            img.color = isOn ? selectedColor : defaultColor;

        if (isOn)
            selectedHistory.Add(btn);
        else
            selectedHistory.Remove(btn);

        // correctOrder.Count 개수 이상 켜지면 Next 활성화
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

        Debug.Log(
            "[TartCrust] Selected = [" +
            GetSequence(selectedHistory) +
            "], Correct = [" +
            GetSequence(correctOrder) +
            "], Result = " +
            success
        );

        if (panelObject != null)
            panelObject.SetActive(false);

        tartManagerRef?.OnCrustComplete(success);
    }

    private string GetSequence(List<Button> list)
    {
        List<string> names = new List<string>();
        foreach (var b in list)
            names.Add(b.gameObject.name);
        return string.Join(" -> ", names);
    }
}
