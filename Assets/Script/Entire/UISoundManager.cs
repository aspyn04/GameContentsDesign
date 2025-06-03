using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance;
    [SerializeField] private AudioClip npcSpawnSound;
    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.4f;
        audioSource.playOnAwake = false;
    }

    public void PlayClick()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
    public void PlayNPCSpawn()
    {
        if (npcSpawnSound != null)
            audioSource.PlayOneShot(npcSpawnSound);
    }
}
