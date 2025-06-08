using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance;

    [Header("Ŭ�� ����")]
    [SerializeField] private AudioClip clickSound;
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
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// ��ư Ŭ�� �� UI �̺�Ʈ �� ȣ��
    /// </summary>
    public void PlayClick()
    {
        if (clickSound == null) return;
        audioSource.PlayOneShot(clickSound, uiVolume);
    }

    /// <summary>
    /// ��Ÿ�ӿ� UI ������ ������ ��
    /// </summary>
    public void SetUIVolume(float volume)
    {
        uiVolume = Mathf.Clamp01(volume);
    }

    public float GetUIVolume() => uiVolume;
}
