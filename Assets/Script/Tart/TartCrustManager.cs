using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TartCrust : MonoBehaviour
{
    public List<Button> ingredientButtons;
    public Button nextButton;
    private List<string> correctOrder;
    private List<string> userOrder;
    private TartManager tartManager;

    public void StartCrust(List<string> order, TartManager manager)
    {
        gameObject.SetActive(true);
        correctOrder = order;
        tartManager = manager;
        userOrder = new List<string>();

        foreach (var btn in ingredientButtons)
        {
            btn.onClick.RemoveAllListeners();
            string name = btn.name;
            btn.onClick.AddListener(() => userOrder.Add(name));
        }

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() => OnComplete());
    }

    void OnComplete()
    {
        bool success = userOrder.Count == correctOrder.Count;
        for (int i = 0; i < userOrder.Count && success; i++)
            success &= (userOrder[i] == correctOrder[i]);

        tartManager.SetCrustResult(success);
        gameObject.SetActive(false);
    }
}