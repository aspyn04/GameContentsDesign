using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TartCrustManager : MonoBehaviour
{
    [Header("버튼 프리팹 (a ~ f)")]
    [SerializeField] private GameObject prefabA;
    [SerializeField] private GameObject prefabB;
    [SerializeField] private GameObject prefabC;
    [SerializeField] private GameObject prefabD;
    [SerializeField] private GameObject prefabE;
    [SerializeField] private GameObject prefabF;

    [Header("버튼이 배치될 슬롯 위치 (Empty Object 6개)")]
    [SerializeField] private List<Transform> slotPositions;

    [Header("UI 패널 오브젝트")]
    [SerializeField] private GameObject tartCrustUI;
    [SerializeField] private GameObject tartOvenUI;

    [Header("버튼 색상")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;

    [Header("테스트 모드")]
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
                Debug.LogError("프리팹이 존재하지 않음: " + name);
                continue;
            }

            Transform slot = slotPositions[i];
            GameObject buttonGO = Instantiate(prefab, slot);

            if (!buttonGO.TryGetComponent(out Button btn) || !buttonGO.TryGetComponent(out Image img))
            {
                Debug.LogError("프리팹 '" + name + "'에 Button 또는 Image 컴포넌트가 없습니다.");
                continue;
            }

            TMP_Text label = buttonGO.GetComponentInChildren<TMP_Text>();
            if (label != null)
            {
                label.text = name;
            }
            else
            {
                Debug.LogWarning("버튼 '" + name + "'에 TMP_Text가 없습니다.");
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
            Debug.Log("현재까지 순서 맞음: " + GetSequence(selectedHistory));

            if (selectedHistory.Count == correctOrder.Count)
            {
                Debug.Log("크러스트 정답 완료");
                if (tartCrustUI != null) tartCrustUI.SetActive(false);
                if (tartOvenUI != null) tartOvenUI.SetActive(true);
            }
        }
        else
        {
            Debug.Log("순서 틀림: " + GetSequence(selectedHistory));
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
