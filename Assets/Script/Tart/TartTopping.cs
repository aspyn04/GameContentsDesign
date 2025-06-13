using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TartTopping : MonoBehaviour
{
    [Header("���ο� ��ư�� (�̸��� ��� ID ���ڿ��� ����)")]
    [SerializeField] private List<Button> toppingButtons = new List<Button>();

    [Header("�߾ӿ� ������ ��� �̹����� (��ư �̸��� �̸� ��ġ�ؾ� ��)")]
    [SerializeField] private List<GameObject> toppingImageObjects = new List<GameObject>();

    [Header("���� �ܰ� ��ư")]
    [SerializeField] private Button nextButton;

    [Header("���� UI �г�")]
    [SerializeField] private GameObject panelObject;

    [Header("��ư �⺻ ����")]
    [SerializeField] private Color defaultColor = Color.white;

    [Header("��ư ���� ����")]
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

        // �̹��� ���� (�̸� ��������)
        foreach (var imgObj in toppingImageObjects)
        {
            string key = imgObj.name.ToLower(); // ex: "choco"
            if (!toppingImageMap.ContainsKey(key))
                toppingImageMap.Add(key, imgObj);

            imgObj.SetActive(false); // ���� �� ���α�
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

        // ��� ����
        bool isOn = !buttonState[btn];
        buttonState[btn] = isOn;

        // ���� ����
        var img = btn.GetComponent<Image>();
        if (img != null)
            img.color = isOn ? selectedColor : defaultColor;

        // �߾� �̹��� on/off
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

        Debug.Log($"TartTopping: ����=[{string.Join(", ", selected)}], " +
                  $"�ʿ�=[{string.Join(", ", requiredIngredients)}], ����={success}");

        if (panelObject != null)
            panelObject.SetActive(false);

        tartManagerRef?.OnToppingComplete(success);
    }
}
