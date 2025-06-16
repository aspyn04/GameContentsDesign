using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SettingManager : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;

    private bool isSettingOpen = false;

    void Start()
    {
        if (settingPanel != null)
            settingPanel.SetActive(false);
    }

    void Update()
    {
        if (!isSettingOpen) return;

        // ESC 또는 마우스 클릭 시 설정 닫기
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
        {
            CloseSetting();
        }
    }

    // Settings 버튼 눌렀을 때 외부에서 호출
    public void OnClickSetting()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(true);
            isSettingOpen = true;
            Time.timeScale = 0f; // 시간 정지
            BGMManager_Game.Instance.PauseMusic();   // 정지
        }
    }

    // ESC 또는 클릭으로 닫을 때
    private void CloseSetting()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
            isSettingOpen = false;
            Time.timeScale = 1f; // 시간 재개
            BGMManager_Game.Instance.ResumeMusic();  // 재개

        }
    }
}