using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TartCrust : MonoBehaviour
{
    [Header("���� ������� �迭�� ��ư (��: a, b, c, d, ��)")]
    [SerializeField] private List<Button> correctOrder = new List<Button>();

    [Header("��� ��� ��ư (������ �������)")]
    [SerializeField] private List<Button> ingredientButtons = new List<Button>();

    [Header("���� �ܰ� ��ư")]
    [SerializeField] private Button nextButton;

    [Header("��� �ܰ� ��ü UI �г�")]
    [SerializeField] private GameObject panelObject;

    [Header("�⺻ ���� (��Ȱ��)")]
    [SerializeField] private Color defaultColor = Color.white;

    [Header("���õ� ��ư ����")]
    [SerializeField] private Color selectedColor = Color.yellow;

    private TartManager tartManagerRef;
    private bool isInitialized = false;

    private Dictionary<Button, bool> buttonState = new Dictionary<Button, bool>();
    private List<Button> selectedHistory = new List<Button>();

    public void Init(List<string> unused, TartManager manager)
    {
        tartManagerRef = manager;
        isInitialized = true;

        // �����丮 �� ���� �ʱ�ȭ
        selectedHistory.Clear();
        buttonState.Clear();

        // �� ��ư�� Ŭ�� ������ �ޱ�, �� �ʱ�ȭ
        foreach (var btn in ingredientButtons)
        {
            buttonState[btn] = false;
            Image img = btn.GetComponent<Image>();
            if (img != null) img.color = defaultColor;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnIngredientClicked(btn));
        }

        // Next ��ư �ʱ�ȭ
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);
        nextButton.interactable = false;

        // �г� Ȱ��ȭ
        if (panelObject != null)
            panelObject.SetActive(true);
    }

    private void OnIngredientClicked(Button btn)
    {
        if (!isInitialized) return;

        bool isOn = !buttonState[btn];
        buttonState[btn] = isOn;

        // �ð��� ��� ǥ��
        Image img = btn.GetComponent<Image>();
        if (img != null) img.color = isOn ? selectedColor : defaultColor;

        if (isOn) selectedHistory.Add(btn);
        else selectedHistory.Remove(btn);

        // correctOrder.Count ���� �̻� ���õǸ� Next Ȱ��ȭ
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

        Debug.Log($"TartCrust: ���õ� ����=[{GetSequence(selectedHistory)}], " +
                  $"���� ����=[{GetSequence(correctOrder)}], ���={success}");

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
