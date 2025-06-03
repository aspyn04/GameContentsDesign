using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TitleButtonController : MonoBehaviour
{

    [SerializeField] private GameObject creditPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject startButtonImageScript; // 버튼 GameObject 전체

    private bool isCreditOpen = false;

    void Start()
    {
        BGMManager.Instance?.PlayMusic();

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
        startButtonImageScript.SetActive(false);
        startButton.interactable = false; // 버튼 비활성화
        UISoundManager.Instance?.PlayClick();
        BGMManager.Instance?.StopMusic();
        SceneFadeOut.Instance?.FadeToScene(1); 
    }

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
