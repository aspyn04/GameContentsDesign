using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundManager : MonoBehaviour
{
    public static NPCSoundManager Instance;

    [Header("NPC 등장 사운드")]
    [SerializeField] private AudioClip npcSpawnSound;
    [Range(0f, 1f)]
    [SerializeField] private float npcVolume = 0.6f;

    [Header("슬라임 만족 / 불만족 사운드")]
    [SerializeField] private AudioClip slimeSatisfied;
    [SerializeField] private AudioClip slimeUnsatisfied;

    [Header("털덩이 만족 / 불만족 사운드")]
    [SerializeField] private AudioClip furballSatisfied;
    [SerializeField] private AudioClip furballUnsatisfied;

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
    /// NPC가 등장할 때 호출
    /// </summary>
    public void PlaySpawn()
    {
        if (npcSpawnSound == null) return;
        audioSource.PlayOneShot(npcSpawnSound, npcVolume);
    }

    /// <summary>
    /// NPC 타입과 만족 여부에 따라 사운드 재생
    /// </summary>
    public void PlayReactionSound(string npcType, bool isSatisfied)
    {
        AudioClip clipToPlay = null;

        switch (npcType.ToLower())
        {
            case "slime":
                clipToPlay = isSatisfied ? slimeSatisfied : slimeUnsatisfied;
                break;
            case "furball":
                clipToPlay = isSatisfied ? furballSatisfied : furballUnsatisfied;
                break;
        }

        if (clipToPlay != null)
            audioSource.PlayOneShot(clipToPlay, npcVolume);
    }

    /// <summary>
    /// 런타임에 NPC 사운드 볼륨을 조절할 때
    /// </summary>
    public void SetNPCVolume(float volume)
    {
        npcVolume = Mathf.Clamp01(volume);
    }

    public float GetNPCVolume() => npcVolume;
}
