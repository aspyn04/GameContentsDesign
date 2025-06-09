using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioClip bgmClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgmClip;
        audioSource.Play();
    }
}

