using System.Collections;
using UnityEngine;

public class NPCSoundManager : MonoBehaviour
{
    public static NPCSoundManager Instance;

    [Header("NPC 등장 사운드")]
    [SerializeField] private AudioClip npcSpawnSound;
    [Range(0f, 1f)][SerializeField] private float npcSpawnVolume = 0.6f;

    [Header("슬라임 만족 / 불만족 사운드")]
    [SerializeField] private AudioClip slimeSatisfied;
    [Range(0f, 1f)][SerializeField] private float slimeSatisfiedVolume = 0.6f;
    [SerializeField] private AudioClip slimeUnsatisfied;
    [Range(0f, 1f)][SerializeField] private float slimeUnsatisfiedVolume = 0.6f;

    [Header("털덩이 만족 / 불만족 사운드")]
    [SerializeField] private AudioClip furballSatisfied;
    [Range(0f, 1f)][SerializeField] private float furballSatisfiedVolume = 0.6f;
    [SerializeField] private AudioClip furballUnsatisfied;
    [Range(0f, 1f)][SerializeField] private float furballUnsatisfiedVolume = 0.6f;

    [Header("치즈 보상 사운드")]
    [SerializeField] private AudioClip cheeseRewardSound;
    [Range(0f, 1f)][SerializeField] private float cheeseRewardVolume = 0.6f;

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

    public void PlaySpawn()
    {
        if (npcSpawnSound != null)
            audioSource.PlayOneShot(npcSpawnSound, npcSpawnVolume);
    }

    public void PlayReactionSound(string npcType, bool isSatisfied)
    {
        AudioClip clipToPlay = null;
        float volume = 0.6f;

        switch (npcType.ToLower())
        {
            case "slime":
                clipToPlay = isSatisfied ? slimeSatisfied : slimeUnsatisfied;
                volume = isSatisfied ? slimeSatisfiedVolume : slimeUnsatisfiedVolume;
                break;
            case "furball":
                clipToPlay = isSatisfied ? furballSatisfied : furballUnsatisfied;
                volume = isSatisfied ? furballSatisfiedVolume : furballUnsatisfiedVolume;
                break;
        }

        if (clipToPlay != null)
            audioSource.PlayOneShot(clipToPlay, volume);
    }

    public void PlayCheeseRewardSound()
    {
        if (cheeseRewardSound != null)
            audioSource.PlayOneShot(cheeseRewardSound, cheeseRewardVolume);
    }

    // 예: 개별 볼륨 접근용 getter (원할 경우)
    public float GetSlimeSatisfiedVolume() => slimeSatisfiedVolume;
    public void SetSlimeSatisfiedVolume(float volume) => slimeSatisfiedVolume = Mathf.Clamp01(volume);
}
