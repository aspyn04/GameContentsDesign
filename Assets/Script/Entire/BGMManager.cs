using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("����� ��� ���� Ŭ��")]
    [SerializeField] private AudioClip titleBGM;

    [Header("���̵� �ƿ� �ð� (��)")]
    [SerializeField] private float fadeOutDuration = 2f;

    private AudioSource audioSource;

    void Awake()
    {
        // �̱��� ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSource ���� ����
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
    }

    public void PlayMusic()
    {
        if (titleBGM == null)
        {
            Debug.LogError("titleBGM�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        audioSource.clip = titleBGM;
        audioSource.mute = false;
        audioSource.volume = 1f;
        audioSource.Play();
        Debug.Log("������� ��� ����");
    }

    public void StopMusic()
    {
        if (audioSource == null || !audioSource.isPlaying)
        {
            Debug.Log("������ ��� ���� �ƴմϴ�.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(FadeOutAndMute());
    }

    private IEnumerator FadeOutAndMute()
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

        audioSource.volume = 0f;
        audioSource.mute = true;
        Debug.Log("���̵� �ƿ� �Ϸ� �� ���Ұ� ó��");
    }
}