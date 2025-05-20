using UnityEngine;
using UnityEngine.UI;

public class TartOven : MonoBehaviour
{
    public Button strongBtn, mediumBtn, weakBtn, nextButton;
    private string selected;
    private string correct;
    private TartManager tartManager;

    public void StartOven(string setting, TartManager manager)
    {
        gameObject.SetActive(true);
        correct = setting;
        tartManager = manager;
        selected = "";

        strongBtn.onClick.AddListener(() => selected = "강");
        mediumBtn.onClick.AddListener(() => selected = "중간");
        weakBtn.onClick.AddListener(() => selected = "약");

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() => OnComplete());
    }

    void OnComplete()
    {
        bool success = selected == correct;
        tartManager.SetOvenResult(success);
        gameObject.SetActive(false);
    }
}