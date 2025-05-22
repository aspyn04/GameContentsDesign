using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TartCrustManager : MonoBehaviour
{
    [Header("��ư ������ (a ~ f)")]
    [SerializeField] private GameObject prefabA;
    [SerializeField] private GameObject prefabB;
    [SerializeField] private GameObject prefabC;
    [SerializeField] private GameObject prefabD;
    [SerializeField] private GameObject prefabE;
    [SerializeField] private GameObject prefabF;

    [Header("��ư�� ��ġ�� ���� ��ġ (Empty Object 6��)")]
    [SerializeField] private List<Transform> slotPositions;

    [Header("UI �г� ������Ʈ")]
    [SerializeField] private GameObject tartCrustUI;
    [SerializeField] private GameObject tartOvenUI;

    [Header("��ư ����")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;

    [Header("�׽�Ʈ ���")]
    [SerializeField] private bool testMode = false;

    private Dictionary<string, GameObject> prefabMap;
    private Dictionary<Button, string> buttonToName = new();
    private Dictionary<Button, Image> buttonToImage = new();
    private Dictionary<Button, bool> buttonState = new();

    private List<Button> correctOrder = new();
    private List<Button> selectedHistory = new();

    private void Start()
    {
        if (testMode)
        {
            List<string> testOrder = new List<string> { "a", "b", "c", "d", "e", "f" };
            Init(testOrder);
        }
    }

    public void Init(List<string> crustOrder)
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

        Shuffle(slotPositions);
        SpawnButtons(crustOrder);

        if (tartCrustUI != null) tartCrustUI.SetActive(true);
        if (tartOvenUI != null) tartOvenUI.SetActive(false);
    }

    void SpawnButtons(List<string> crustOrder)
    {
        correctOrder.Clear();
        buttonToName.Clear();
        buttonToImage.Clear();
        buttonState.Clear();
        selectedHistory.Clear();

        for (int i = 0; i < crustOrder.Count; i++)
        {
            string name = crustOrder[i];

            if (!prefabMap.TryGetValue(name, out GameObject prefab))
            {
                Debug.LogError("�������� �������� ����: " + name);
                continue;
            }

            Transform slot = slotPositions[i];
            GameObject buttonGO = Instantiate(prefab, slot);

            if (!buttonGO.TryGetComponent(out Button btn) || !buttonGO.TryGetComponent(out Image img))
            {
                Debug.LogError("������ '" + name + "'�� Button �Ǵ� Image ������Ʈ�� �����ϴ�.");
                continue;
            }

            TMP_Text label = buttonGO.GetComponentInChildren<TMP_Text>();
            if (label != null)
            {
                label.text = name;
            }
            else
            {
                Debug.LogWarning("��ư '" + name + "'�� TMP_Text�� �����ϴ�.");
            }

            buttonToName[btn] = name;
            buttonToImage[btn] = img;
            buttonState[btn] = false;

            Button captured = btn;
            captured.onClick.AddListener(() => OnClick(captured));

            correctOrder.Add(captured);
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
                Debug.Log("ũ����Ʈ ���� �Ϸ�");
                if (tartCrustUI != null) tartCrustUI.SetActive(false);
                if (tartOvenUI != null) tartOvenUI.SetActive(true);
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
        List<string> names = new();
        foreach (var b in list)
        {
            if (buttonToName.TryGetValue(b, out string name))
                names.Add(name);
        }
        return string.Join(" -> ", names);
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
