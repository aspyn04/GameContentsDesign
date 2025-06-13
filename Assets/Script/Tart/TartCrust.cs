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

    [Header("��Ŀ ���� (�Ʒ����� �� ����)")]
    [SerializeField] private List<RawImage> beakerSlots;

    [Header("��� �ؽ�ó (���� ��ġ)")]
    [SerializeField] private List<Texture> ingredientTextures;

    private List<int> selectedIndices = new List<int>();

    private TartManager tartManagerRef;
    private bool isInitialized = false;

    private Dictionary<Button, bool> buttonState = new Dictionary<Button, bool>();
    private List<Button> selectedHistory = new List<Button>();

    public void Init(List<string> unused, TartManager manager)
    {
        tartManagerRef = manager;
        isInitialized = true;

        selectedHistory.Clear();
        selectedIndices.Clear();
        buttonState.Clear();

        // ��ġ ����
        ShuffleButtonPositions();

        // ��ư �ʱ�ȭ
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

        // �г� ǥ��
        if (panelObject != null)
            panelObject.SetActive(true);

        UpdateBeakerVisual(); // �ʱ�ȭ �� ��Ŀ�� �ʱ�ȭ
    }

    private void OnIngredientClicked(Button btn)
    {
        if (!isInitialized) return;

        bool isOn = !buttonState[btn];
        buttonState[btn] = isOn;

        // ���� ����
        Image img = btn.GetComponent<Image>();
        if (img != null) img.color = isOn ? selectedColor : defaultColor;

        // ����/���� ó��
        if (isOn)
        {
            selectedHistory.Add(btn);
            int idx = ingredientButtons.IndexOf(btn);
            if (idx != -1) selectedIndices.Add(idx);
        }
        else
        {
            selectedHistory.Remove(btn);
            int idx = ingredientButtons.IndexOf(btn);
            if (idx != -1) selectedIndices.Remove(idx);
        }

        nextButton.interactable = (selectedHistory.Count >= correctOrder.Count);

        UpdateBeakerVisual(); // ��Ŀ �ð� ����
    }

    private void UpdateBeakerVisual()
    {
        for (int i = 0; i < beakerSlots.Count; i++)
        {
            if (i < selectedIndices.Count)
            {
                int ingredientIdx = selectedIndices[i];
                beakerSlots[i].texture = ingredientIdx < ingredientTextures.Count ? ingredientTextures[ingredientIdx] : null;
                beakerSlots[i].enabled = true;
            }
            else
            {
                beakerSlots[i].texture = null;
                beakerSlots[i].enabled = false;
            }
        }
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

    private void ShuffleButtonPositions()
    {
        List<Button> abGroup = ingredientButtons.FindAll(b => b.name == "a" || b.name == "b");
        if (abGroup.Count == 2)
        {
            Vector3 posA = abGroup[0].transform.localPosition;
            Vector3 posB = abGroup[1].transform.localPosition;

            if (Random.value < 0.5f)
            {
                abGroup[0].transform.localPosition = posB;
                abGroup[1].transform.localPosition = posA;
            }
        }

        List<Button> cdefGroup = ingredientButtons.FindAll(b =>
            b.name == "c" || b.name == "d" || b.name == "e" || b.name == "f");

        List<Vector3> cdefPositions = new List<Vector3>();
        foreach (var btn in cdefGroup)
            cdefPositions.Add(btn.transform.localPosition);

        for (int i = 0; i < cdefPositions.Count; i++)
        {
            int rand = Random.Range(i, cdefPositions.Count);
            (cdefPositions[i], cdefPositions[rand]) = (cdefPositions[rand], cdefPositions[i]);
        }

        for (int i = 0; i < cdefGroup.Count; i++)
            cdefGroup[i].transform.localPosition = cdefPositions[i];
    }
}
