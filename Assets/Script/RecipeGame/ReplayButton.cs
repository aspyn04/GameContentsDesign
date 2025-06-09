using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayButton : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        // ȿ���� ���
        audioSource.PlayOneShot(clickSound);

        // �ð� ���� ����
        Time.timeScale = 1f;

        // ȿ���� ������ �ʵ��� 0.2�� �� �� �ε�
        Invoke(nameof(LoadLastScene), 0.2f);
    }

    void LoadLastScene()
    {
        string lastScene = PlayerPrefs.GetString("LastScene", "Stage_01");
        SceneManager.LoadScene(lastScene);
    }
}



