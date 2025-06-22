using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private StoryDialogueManager dialogueManager;
    [SerializeField] private StoryPanelUI panelUI;
    [Header("효과음 클립")]
    [SerializeField] private AudioClip finalEffectSound;

    private AudioSource bgmSource;   // 스토리용 BGM
    private AudioSource sfxSource;   // 효과음

    void Start()
    {
        // 스토리 BGM용
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.ignoreListenerPause = true; // Time.timeScale = 0f 대응

        // 효과음용
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
        Debug.Log($"[StoryManager] PlayStory 시작 (day {day})");

        if (!dialogueManager.HasStoryForDay(day))
        {
            Debug.LogWarning($"[StoryManager] Day {day} 스토리 없음, 중단");
            yield break;
        }

        var lines = dialogueManager.GetStoryForDay(day);
        var bgmClip = dialogueManager.GetBgmForDay(day);

        //  BGM 재생
        if (bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.volume = 1f;
            bgmSource.Play();
        }

        //  스토리 출력
        yield return panelUI.PlayStoryWithLastSfx(lines, day, sfxSource, finalEffectSound);

        //  BGM 페이드아웃 및 완전 종료
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

        Debug.Log($"[StoryManager] Day {day} 스토리 종료");
    }
}
