using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private AudioSource audioSource;

    [SerializeField] private AudioClip titleBGM;

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
    }

    public void PlayTitleMusic()
    {
        audioSource.clip = titleBGM;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}