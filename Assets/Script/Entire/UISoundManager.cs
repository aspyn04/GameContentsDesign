using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance;

    [Header("클릭 사운드")]
    [SerializeField] private AudioClip clickSound;

    [Header("종료 사운드")]
    [SerializeField] private AudioClip endSound;

    [Range(0f, 1f)]
    [SerializeField] private float uiVolume = 0.4f;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// 버튼 클릭 등 UI 이벤트 시 호출
    /// </summary>
    public void PlayClick()
    {
        if (clickSound == null) return;
        audioSource.PlayOneShot(clickSound, uiVolume);
    }

    /// <summary>
    /// UI 종료 효과음 등 재생
    /// </summary>
    public void PlayEnd()
    {
        if (endSound == null) return;
        audioSource.PlayOneShot(endSound, uiVolume);
    }

    /// <summary>
    /// 런타임에 UI 볼륨을 조절할 때
    /// </summary>
    public void SetUIVolume(float volume)
    {
        uiVolume = Mathf.Clamp01(volume);
    }

    public float GetUIVolume() => uiVolume;
}
