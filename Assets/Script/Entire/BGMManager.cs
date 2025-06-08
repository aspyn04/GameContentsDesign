using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("����� ��� ���� Ŭ��")]
    [SerializeField] private AudioClip titleBGM;

    [Header("������ ���� (0 = ����, 1 = �ִ�)")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;

    [Header("���̵� �ƿ� �ð� (��)")]
    [SerializeField] private float fadeOutDuration = 2f;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = masterVolume;
    }

    /// <summary>
    /// �ν����Ϳ��� �����̴��� ���� ���� �� ��� �ݿ�
    /// </summary>
    private void OnValidate()
    {
        if (audioSource != null)
            audioSource.volume = masterVolume;
    }

    public void PlayMusic()
    {
        if (titleBGM == null)
        {
            Debug.LogError("titleBGM�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        audioSource.clip = titleBGM;
        audioSource.mute = false;
        audioSource.volume = masterVolume;
        audioSource.Play();
        Debug.Log("������� ��� ����, ����=" + masterVolume);
    }

    public void StopMusic()
    {
        if (audioSource == null || !audioSource.isPlaying)
        {
            Debug.Log("������ ��� ���� �ƴմϴ�.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(FadeOutAndMute());
    }

    private IEnumerator FadeOutAndMute()
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeOutDuration);
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.mute = true;
        Debug.Log("���̵� �ƿ� �Ϸ� �� ���Ұ� ó��");
    }

    /// <summary>
    /// ��Ÿ�ӿ� ������ �����ϰ� ��� �����մϴ�.
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        if (audioSource != null && audioSource.isPlaying)
            audioSource.volume = masterVolume;
    }
    /// <summary>
    /// BGM �Ͻ�����
    /// </summary>
    public void PauseMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
            Debug.Log("������� �Ͻ�����");
        }
    }

    /// <summary>
    /// BGM �簳
    /// </summary>
    public void ResumeMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.UnPause();
            Debug.Log("������� �簳");
        }
    }

    public float GetMasterVolume() => masterVolume;
}