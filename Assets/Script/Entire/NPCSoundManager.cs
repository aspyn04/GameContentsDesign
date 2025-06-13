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

    [Header("������ ���� / �Ҹ��� ����")]
    [SerializeField] private AudioClip slimeSatisfied;
    [SerializeField] private AudioClip slimeUnsatisfied;

    [Header("�е��� ���� / �Ҹ��� ����")]
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
    /// NPC�� ������ �� ȣ��
    /// </summary>
    public void PlaySpawn()
    {
        if (npcSpawnSound == null) return;
        audioSource.PlayOneShot(npcSpawnSound, npcVolume);
    }

    /// <summary>
    /// NPC Ÿ�԰� ���� ���ο� ���� ���� ���
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
    /// ��Ÿ�ӿ� NPC ���� ������ ������ ��
    /// </summary>
    public void SetNPCVolume(float volume)
    {
        npcVolume = Mathf.Clamp01(volume);
    }

    public float GetNPCVolume() => npcVolume;
}
