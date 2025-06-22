using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private StoryDialogueManager dialogueManager;
    [SerializeField] private StoryPanelUI panelUI;
    [Header("ȿ���� Ŭ��")]
    [SerializeField] private AudioClip finalEffectSound;

    private AudioSource bgmSource;   // ���丮�� BGM
    private AudioSource sfxSource;   // ȿ����

    void Start()
    {
        // ���丮 BGM��
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.ignoreListenerPause = true; // Time.timeScale = 0f ����

        // ȿ������
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.ignoreListenerPause = true;
    }

    public bool HasStoryForDay(int day)
    {
        return dialogueManager.HasStoryForDay(day);
    }

    public IEnumerator PlayStory(int day)
    {
        Debug.Log($"[StoryManager] PlayStory ���� (day {day})");

        if (!dialogueManager.HasStoryForDay(day))
        {
            Debug.LogWarning($"[StoryManager] Day {day} ���丮 ����, �ߴ�");
            yield break;
        }

        var lines = dialogueManager.GetStoryForDay(day);
        var bgmClip = dialogueManager.GetBgmForDay(day);

        //  BGM ���
        if (bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.volume = 1f;
            bgmSource.Play();
        }

        //  ���丮 ���
        yield return panelUI.PlayStoryWithLastSfx(lines, day, sfxSource, finalEffectSound);

        //  BGM ���̵�ƿ� �� ���� ����
        if (bgmSource != null && bgmSource.clip != null)
        {
            float duration = 0.5f;
            float startVolume = bgmSource.volume;

            for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
            {
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
                yield return null;
            }

            bgmSource.volume = 0f;
            bgmSource.Stop();
            bgmSource.clip = null;
        }

        Debug.Log($"[StoryManager] Day {day} ���丮 ����");
    }
}
