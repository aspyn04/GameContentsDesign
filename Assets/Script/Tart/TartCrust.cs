using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TartCrust : MonoBehaviour
{
    [Header("Correct order (a �� b �� c �� d �� e �� f)")]
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

    // ��ư�� on/off ���¸� ����
    private Dictionary<Button, bool> buttonState = new Dictionary<Button, bool>();

    // ����ڰ� �� ������� ���� ����Ʈ
    private List<Button> selectedHistory = new List<Button>();

    /// <summary>
    /// Init�� ȣ��Ǹ� ingredientButtons ��ġ�� ���� ���� ��,
    /// Ŭ�� �����ʿ� �ʱ� ���¸� �����մϴ�.
    /// </summary>
    public void Init(List<string> unused, TartManager manager)
    {
        tartManagerRef = manager;
        isInitialized = true;

        selectedHistory.Clear();
        buttonState.Clear();

        // 1) ingredientButtons�� �ִ� ��� ��ư�� ���� ��ġ(anchoredPosition)�� ����
        List<Vector2> originalPositions = new List<Vector2>();
        foreach (var btn in ingredientButtons)
        {
            RectTransform rt = btn.GetComponent<RectTransform>();
            originalPositions.Add(rt.anchoredPosition);
        }

        // 2) ��ġ ����Ʈ�� ���´�
        ShuffleList(originalPositions);

        // 3) ���� ��ġ�� ingredientButtons�� ������� ���Ҵ�
        for (int i = 0; i < ingredientButtons.Count; i++)
        {
            RectTransform rt = ingredientButtons[i].GetComponent<RectTransform>();
            rt.anchoredPosition = originalPositions[i];
        }

        // 4) �� ��ư ���� �ʱ�ȭ �� Ŭ�� ������ ����
        foreach (var btn in ingredientButtons)
        {
            buttonState[btn] = false;
            Image img = btn.GetComponent<Image>();
            if (img != null)
                img.color = defaultColor;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnIngredientClicked(btn));
        }

        // 5) Next ��ư �ʱ�ȭ
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);
        nextButton.interactable = false;

        // 6) �г� Ȱ��ȭ
        if (panelObject != null)
            panelObject.SetActive(true);
    }

    /// <summary>
    /// ����Ʈ ��Ҹ� �������� ���� ��ƿ��Ƽ �Լ�
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

        // �ð��� ��� ǥ��: �� ����
        Image img = btn.GetComponent<Image>();
        if (img != null)
            img.color = isOn ? selectedColor : defaultColor;

        if (isOn)
            selectedHistory.Add(btn);
        else
            selectedHistory.Remove(btn);

        // correctOrder.Count ���� �̻� ������ Next Ȱ��ȭ
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
