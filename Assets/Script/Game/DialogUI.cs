using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    public GameObject panel;
    public Text dialogText;
    public Button nextButton;
    public Button makeTartButton;
    private bool clicked;

    public IEnumerator Show(string text)
    {
        panel.SetActive(true);
        dialogText.text = text;
        clicked = false;
        nextButton.gameObject.SetActive(true);
        makeTartButton.gameObject.SetActive(false);

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() => clicked = true);

        yield return new WaitUntil(() => clicked);
    }

    public IEnumerator WaitForMakeTartClick()
    {
        clicked = false;
        nextButton.gameObject.SetActive(false);
        makeTartButton.gameObject.SetActive(true);

        makeTartButton.onClick.RemoveAllListeners();
        makeTartButton.onClick.AddListener(() => clicked = true);

        yield return new WaitUntil(() => clicked);

        panel.SetActive(false);
    }
}