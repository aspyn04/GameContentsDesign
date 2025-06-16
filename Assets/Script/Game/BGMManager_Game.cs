using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class DayBGMEntry
{
    public int day;
    public AudioClip clip;
}

public class BGMManager_Game : MonoBehaviour
{
    public static BGMManager_Game Instance;

    [System.Serializable]
    public class BGMRange
    {
        public int minDay;                // 시작 일차 (포함)
        public int maxDay;                // 끝 일차 (포함)
        public AudioClip bgmClip;         // 해당 구간에서 재생할 BGM
    }

    [Header("일차 구간별 BGM 설정")]
    [SerializeField] private List<BGMRange> bgmRanges = new List<BGMRange>();

    [Header("볼륨 설정")]
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

    private void OnValidate()
    {
        if (audioSource != null)
            audioSource.volume = masterVolume;
    }

    /// <summary>
    /// 현재 일차에 맞는 BGM을 찾아 재생
    /// </summary>
    public void PlayBGMByDay(int currentDay)
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }

        AudioClip clip = GetBGMForDay(currentDay);
        if (clip == null)
        {
            Debug.LogWarning($"[BGMManager] {currentDay}일차에 해당하는 BGM이 없습니다.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(SwitchMusic(clip));
    }


    private AudioClip GetBGMForDay(int day)
    {
        foreach (var range in bgmRanges)
        {
            if (day >= range.minDay && day <= range.maxDay)
                return range.bgmClip;
        }
        return null;
    }

    private IEnumerator SwitchMusic(AudioClip newClip)
    {
        if (audioSource.isPlaying)
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

            audioSource.Stop();
        }

        audioSource.clip = newClip;
        audioSource.volume = masterVolume;
        audioSource.Play();
        Debug.Log($"[BGMManager] {newClip.name} 재생 시작");
    }

    public void PauseMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            Debug.Log("BGM 일시정지");
        }
    }

    public void ResumeMusic()
    {
        if (!audioSource.isPlaying && audioSource.clip != null)
        {
            audioSource.UnPause();
            Debug.Log("BGM 재개");
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        if (audioSource != null)
            audioSource.volume = masterVolume;
    }

    public float GetMasterVolume() => masterVolume;
}