using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundManager : MonoBehaviour
{
    public static NPCSoundManager Instance;

    [Header("NPC ���� ����")]
    [SerializeField] private AudioClip npcSpawnSound;
    [Range(0f, 1f)]
    [SerializeField] private float npcVolume = 0.6f;

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
    /// NPC�� ������ �� ȣ��
    /// </summary>
    public void PlaySpawn()
    {
        if (npcSpawnSound == null) return;
        audioSource.PlayOneShot(npcSpawnSound, npcVolume);
    }

    /// <summary>
    /// ��Ÿ�ӿ� NPC ���� ������ ������ ��
    /// </summary>
    public void SetNPCVolume(float volume)
    {
        npcVolume = Mathf.Clamp01(volume);
    }

    public float GetNPCVolume() => npcVolume;
}