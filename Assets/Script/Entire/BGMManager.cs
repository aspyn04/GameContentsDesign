using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("재생할 배경 음악 클립")]
    [SerializeField] private AudioClip titleBGM;

    [Header("페이드 아웃 시간 (초)")]
    [SerializeField] private float fadeOutDuration = 2f;

    private AudioSource audioSource;

    void Awake()
    {
        // 싱글턴 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSource 직접 생성
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
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
        audioSource.volume = 1f;
        audioSource.Play();
        Debug.Log("배경음악 재생 시작");
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
}