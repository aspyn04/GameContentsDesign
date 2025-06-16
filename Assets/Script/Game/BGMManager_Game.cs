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
        public int minDay;                // ���� ���� (����)
        public int maxDay;                // �� ���� (����)
        public AudioClip bgmClip;         // �ش� �������� ����� BGM
    }

    [Header("���� ������ BGM ����")]
    [SerializeField] private List<BGMRange> bgmRanges = new List<BGMRange>();

    [Header("���� ����")]
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

    private void OnValidate()
    {
        if (audioSource != null)
            audioSource.volume = masterVolume;
    }

    /// <summary>
    /// ���� ������ �´� BGM�� ã�� ���
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
            Debug.LogWarning($"[BGMManager] {currentDay}������ �ش��ϴ� BGM�� �����ϴ�.");
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
        Debug.Log($"[BGMManager] {newClip.name} ��� ����");
    }

    public void PauseMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            Debug.Log("BGM �Ͻ�����");
        }
    }

    public void ResumeMusic()
    {
        if (!audioSource.isPlaying && audioSource.clip != null)
        {
            audioSource.UnPause();
            Debug.Log("BGM �簳");
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