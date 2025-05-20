using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TartTopping : MonoBehaviour
{
    public List<Toggle> toppingToggles;
    public Button nextButton;
    private List<string> correctToppings;
    private TartManager tartManager;

    public void StartTopping(List<string> toppings, TartManager manager)
    {
        gameObject.SetActive(true);
        correctToppings = toppings;
        tartManager = manager;

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() => OnComplete());
    }

    void OnComplete()
    {
        List<string> selected = new List<string>();
        foreach (var toggle in toppingToggles)
        {
            if (toggle.isOn)
                selected.Add(toggle.name);
        }

        bool success = selected.Count == correctToppings.Count && !selected.Except(correctToppings).Any();
        tartManager.SetToppingResult(success);
        gameObject.SetActive(false);
    }
}