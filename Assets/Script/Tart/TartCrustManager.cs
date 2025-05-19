using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TartCrustManager : MonoBehaviour
{
    [Header("��ư ������ (a ~ f)")]
    [SerializeField] private GameObject prefabA;
    [SerializeField] private GameObject prefabB;
    [SerializeField] private GameObject prefabC;
    [SerializeField] private GameObject prefabD;
    [SerializeField] private GameObject prefabE;
    [SerializeField] private GameObject prefabF;

    [Header("���� ��ġ (�� ������Ʈ 6��)")]
    [SerializeField] private List<Transform> slotPositions;

    [Header("UI ��ȯ ������Ʈ")]
    [SerializeField] private GameObject TartCrust;
    [SerializeField] private GameObject TartOven;

    [Header("��ư ����")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;

    // ���� ������
    private Dictionary<string, GameObject> prefabMap;
    private Dictionary<Button, string> buttonToName = new Dictionary<Button, string>();
    private Dictionary<Button, Image> buttonToImage = new Dictionary<Button, Image>();
    private Dictionary<Button, bool> buttonState = new Dictionary<Button, bool>();

    private List<Button> correctOrder = new List<Button>();     // a ~ f ��ư ����
    private List<Button> selectedHistory = new List<Button>();  // ������ ���� ����

    void Start()
    {
        prefabMap = new Dictionary<string, GameObject>
        {
            { "a", prefabA },
            { "b", prefabB },
            { "c", prefabC },
            { "d", prefabD },
            { "e", prefabE },
            { "f", prefabF }
        };

        SpawnButtons();
        TartCrust?.SetActive(true);
        TartOven?.SetActive(false);
    }

    void SpawnButtons()
    {
        List<string> ingredientNames = new List<string> { "a", "b", "c", "d", "e", "f" };
        Shuffle(slotPositions);

        correctOrder.Clear();
        buttonToName.Clear();
        buttonToImage.Clear();
        buttonState.Clear();
        selectedHistory.Clear();

        foreach (string name in ingredientNames)
        {
            GameObject prefab = prefabMap[name];
            Transform slot = slotPositions[ingredientNames.IndexOf(name)];
            GameObject buttonGO = Instantiate(prefab, slot);

            Button btn = buttonGO.GetComponent<Button>();
            Image img = buttonGO.GetComponent<Image>();

            buttonToName[btn] = name;
            buttonToImage[btn] = img;
            buttonState[btn] = false;

            Button captured = btn;
            captured.onClick.AddListener(() => OnClick(captured));
        }

        // ������ a ~ f ������� ��ư ��ü�� ����
        foreach (string name in ingredientNames)
        {
            foreach (var pair in buttonToName)
            {
                if (pair.Value == name)
                {
                    correctOrder.Add(pair.Key);
                    break;
                }
            }
        }
    }

    void OnClick(Button btn)
    {
        bool isOn = !buttonState[btn];
        buttonState[btn] = isOn;
        buttonToImage[btn].color = isOn ? selectedColor : defaultColor;

        if (isOn)
            selectedHistory.Add(btn);
        else
            selectedHistory.Remove(btn);

        if (IsCorrectOrder())
        {
            Debug.Log("������� ���� ����: " + GetSequence(selectedHistory));
            if (selectedHistory.Count == correctOrder.Count)
            {
                Debug.Log("���� ����!");
                TartCrust?.SetActive(false);
                TartOven?.SetActive(true);
            }
        }
        else
        {
            Debug.Log("���� Ʋ��: " + GetSequence(selectedHistory));
        }
    }

    bool IsCorrectOrder()
    {
        for (int i = 0; i < selectedHistory.Count; i++)
        {
            if (selectedHistory[i] != correctOrder[i])
                return false;
        }
        return true;
    }

    string GetSequence(List<Button> list)
    {
        List<string> names = new List<string>();
        foreach (var b in list)
            names.Add(buttonToName[b]);
        return string.Join(" �� ", names);
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
