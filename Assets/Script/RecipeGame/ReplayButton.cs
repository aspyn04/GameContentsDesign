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
        // 효과음 재생
        audioSource.PlayOneShot(clickSound);

        // 시간 정지 해제
        Time.timeScale = 1f;

        // 효과음 끊기지 않도록 0.2초 후 씬 로드
        Invoke(nameof(LoadLastScene), 0.2f);
    }

    void LoadLastScene()
    {
        string lastScene = PlayerPrefs.GetString("LastScene", "Stage_01");
        SceneManager.LoadScene(lastScene);
    }
}



