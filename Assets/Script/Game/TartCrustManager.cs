using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TartCrustManager : MonoBehaviour
{
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private Transform buttonParent; // ButtonSlots
    private List<Transform> slotTransforms = new List<Transform>();

    private List<string> allIngredients = new List<string> { "a", "b", "c", "d", "e", "f" };
    private List<string> targetSequence = new List<string>();
    private int currentStepIndex = 0;

    public System.Action OnDoughCompleted;

    void Start()
    {
        CacheSlotPositions();
        GenerateTargetSequence(6);
        SpawnButtonsInRandomSlots();
    }

    void CacheSlotPositions()
    {
        slotTransforms.Clear();
        foreach (Transform child in buttonParent)
        {
            slotTransforms.Add(child);
        }
    }

    void GenerateTargetSequence(int length)
    {
        var shuffled = new List<string>(allIngredients);
        Shuffle(shuffled);
        targetSequence = shuffled.GetRange(0, length);
        Debug.Log("���� ����: " + string.Join(", ", targetSequence));
    }

    void SpawnButtonsInRandomSlots()
    {
        List<string> shuffledIngredients = new List<string>(allIngredients);
        Shuffle(shuffledIngredients);
        Shuffle(slotTransforms); // ���� ������ �����ϰ�

        for (int i = 0; i < allIngredients.Count; i++)
        {
            var buttonGO = Instantiate(ingredientButtonPrefab, slotTransforms[i]);
            var text = buttonGO.GetComponentInChildren<Text>();
            if (text != null) text.text = shuffledIngredients[i];

            string captured = shuffledIngredients[i];
            buttonGO.GetComponent<Button>().onClick.AddListener(() => OnIngredientClicked(captured));
        }
    }

    void OnIngredientClicked(string clicked)
    {
        if (clicked == targetSequence[currentStepIndex])
        {
            currentStepIndex++;
            Debug.Log($"����: {clicked}");
            if (currentStepIndex >= targetSequence.Count)
            {
                Debug.Log("���� �Ϸ�!");
                OnDoughCompleted?.Invoke();
            }
        }
        else
        {
            Debug.Log($"Ʋ��: {clicked} (����: {targetSequence[currentStepIndex]})");
            // ���� ó�� ����
        }
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