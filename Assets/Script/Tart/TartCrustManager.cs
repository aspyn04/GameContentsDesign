using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TartCrustManager : MonoBehaviour
{
    [Header("버튼 프리팹 (a ~ f)")]
    [SerializeField] private GameObject prefabA;
    [SerializeField] private GameObject prefabB;
    [SerializeField] private GameObject prefabC;
    [SerializeField] private GameObject prefabD;
    [SerializeField] private GameObject prefabE;
    [SerializeField] private GameObject prefabF;

    [Header("슬롯 위치 (빈 오브젝트 6개)")]
    [SerializeField] private List<Transform> slotPositions;

    [Header("UI 전환 오브젝트")]
    [SerializeField] private GameObject TartCrust;
    [SerializeField] private GameObject TartOven;

    [Header("버튼 색상")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;

    // 내부 데이터
    private Dictionary<string, GameObject> prefabMap;
    private Dictionary<Button, string> buttonToName = new Dictionary<Button, string>();
    private Dictionary<Button, Image> buttonToImage = new Dictionary<Button, Image>();
    private Dictionary<Button, bool> buttonState = new Dictionary<Button, bool>();

    private List<Button> correctOrder = new List<Button>();     // a ~ f 버튼 순서
    private List<Button> selectedHistory = new List<Button>();  // 유저가 누른 순서

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

        // 정답은 a ~ f 순서대로 버튼 객체로 저장
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
            Debug.Log("현재까지 순서 맞음: " + GetSequence(selectedHistory));
            if (selectedHistory.Count == correctOrder.Count)
            {
                Debug.Log("정답 성공!");
                TartCrust?.SetActive(false);
                TartOven?.SetActive(true);
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
        List<string> names = new List<string>();
        foreach (var b in list)
            names.Add(buttonToName[b]);
        return string.Join(" → ", names);
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
