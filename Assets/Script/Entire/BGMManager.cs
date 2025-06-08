using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("재생할 배경 음악 클립")]
    [SerializeField] private AudioClip titleBGM;

    [Header("마스터 볼륨 (0 = 무음, 1 = 최대)")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;

    [Header("페이드 아웃 시간 (초)")]
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
    /// 인스펙터에서 슬라이더로 볼륨 조정 시 즉시 반영
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
            Debug.LogError("titleBGM이 할당되지 않았습니다.");
            return;
        }

        audioSource.clip = titleBGM;
        audioSource.mute = false;
        audioSource.volume = masterVolume;
        audioSource.Play();
        Debug.Log("배경음악 재생 시작, 볼륨=" + masterVolume);
    }

    public void StopMusic()
    {
        if (audioSource == null || !audioSource.isPlaying)
        {
            Debug.Log("음악이 재생 중이 아닙니다.");
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
        Debug.Log("페이드 아웃 완료 및 음소거 처리");
    }

    /// <summary>
    /// 런타임에 볼륨을 변경하고 즉시 적용합니다.
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        if (audioSource != null && audioSource.isPlaying)
            audioSource.volume = masterVolume;
    }
    /// <summary>
    /// BGM 일시정지
    /// </summary>
    public void PauseMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
            Debug.Log("배경음악 일시정지");
        }
    }

    /// <summary>
    /// BGM 재개
    /// </summary>
    public void ResumeMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.UnPause();
            Debug.Log("배경음악 재개");
        }
    }

    public float GetMasterVolume() => masterVolume;
}