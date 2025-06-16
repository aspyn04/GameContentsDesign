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

        // ESC �Ǵ� ���콺 Ŭ�� �� ���� �ݱ�
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
        {
            CloseSetting();
        }
    }

    // Settings ��ư ������ �� �ܺο��� ȣ��
    public void OnClickSetting()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(true);
            isSettingOpen = true;
            Time.timeScale = 0f; // �ð� ����
            BGMManager_Game.Instance.PauseMusic();   // ����
        }
    }

    // ESC �Ǵ� Ŭ������ ���� ��
    private void CloseSetting()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
            isSettingOpen = false;
            Time.timeScale = 1f; // �ð� �簳
            BGMManager_Game.Instance.ResumeMusic();  // �簳

        }
    }
}