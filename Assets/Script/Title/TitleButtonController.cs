using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonController : MonoBehaviour
{
    [SerializeField] private GameObject creditPanel;
    private bool isCreditOpen = false;

    void Start()
    {
        BGMManager.Instance?.PlayTitleMusic();

        if (creditPanel != null)
            creditPanel.SetActive(false);
    }

    void Update()
    {
        if (!isCreditOpen) return;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
        {
            CloseCredit();
        }
    }

    public void OnClickStart()
    {
        UISoundManager.Instance?.PlayClick();
        BGMManager.Instance?.StopMusic();
        SceneManager.LoadScene(1);
    }

    public void OnClickSettings() { }

    public void OnClickExit() { Application.Quit(); }

    public void OnClickCredit()
    {
        if (creditPanel != null)
        {
            creditPanel.SetActive(true);
            isCreditOpen = true;
        }
    }

    private void CloseCredit()
    {
        if (creditPanel != null)
        {
            creditPanel.SetActive(false);
            isCreditOpen = false;
        }
    }
}
