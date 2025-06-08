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

        selectedHistory.Clear();
        buttonState.Clear();

        // 1. ingredientButtons�� ��ġ ���� ����
        ShuffleButtonPositions();

        // 2. ��ư ���� �ʱ�ȭ �� Ŭ�� ������ ���
        foreach (var btn in ingredientButtons)
        {
            buttonState[btn] = false;

            Image img = btn.GetComponent<Image>();
            if (img != null) img.color = defaultColor;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnIngredientClicked(btn));
        }

        // 3. Next ��ư �ʱ�ȭ
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);
        nextButton.interactable = false;

        // 4. UI �г� ǥ��
        if (panelObject != null)
            panelObject.SetActive(true);
    }

    private void OnIngredientClicked(Button btn)
    {
        if (!isInitialized) return;

        bool isOn = !buttonState[btn];
        buttonState[btn] = isOn;

        // ���� ����
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

        Debug.Log($"TartCrust: ���õ� ���� = [{GetSequence(selectedHistory)}], " +
                  $"���� ���� = [{GetSequence(correctOrder)}], ��� = {success}");

        if (panelObject != null)
            panelObject.SetActive(false);

        tartManagerRef?.OnCrustComplete(success);
    }

    private string GetSequence(List<Button> list)
    {
        List<string> names = new List<string>();
        foreach (var b in list)
            names.Add(b.gameObject.name);
        return string.Join(" �� ", names);
    }

    /// <summary>
    /// ingredientButtons�� ��ġ�� �����մϴ�.
    /// </summary>
    private void ShuffleButtonPositions()
    {
        // a, b ��ġ ��ȯ
        Button aButton = ingredientButtons.Find(b => b.name == "a");
        Button bButton = ingredientButtons.Find(b => b.name == "b");

        if (aButton != null && bButton != null)
        {
            Vector3 tempPos = aButton.transform.localPosition;
            aButton.transform.localPosition = bButton.transform.localPosition;
            bButton.transform.localPosition = tempPos;
        }

        // c, d, e, f ����
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
