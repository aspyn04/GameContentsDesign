using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TartOven : MonoBehaviour
{
    [Header("오븐 시작 버튼")]
    [SerializeField] private Button startOvenButton;

    [Header("다음 단계 버튼")]
    [SerializeField] private Button nextButton;

    [Header("오븐 UI 패널 (이미지는 계속 보여야 함)")]
    [SerializeField] private GameObject ovenPanel;

    [Header("굽기 시간 (초)")]
    [SerializeField] private float bakeDuration = 3f;

    [Header("굽기 중에 재생할 루프 오디오 (옵션)")]
    [SerializeField] private AudioClip bakingLoopClip;

    [Header("굽기 완료 시 재생할 오디오")]
    [SerializeField] private AudioClip finishBakeClip;

    private TartManager tartManagerRef;
    private AudioSource audioSource;

    // Next 버튼이 활성된 이후 눌린 횟수 추적
    private int nextClickCount = 0;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// TartManager에서 호출하여 오븐 단계를 시작합니다.
    /// ovenPanel은 반드시 활성 상태여야 하며, 버튼들만 숨기거나 비활성화합니다.
    /// </summary>
    public void Init(TartManager manager)
    {
        tartManagerRef = manager;
        nextClickCount = 0;

        // 오븐 패널 자체는 활성화된 채로 두고, 버튼들만 초기 상태로 설정
        if (ovenPanel != null)
            ovenPanel.SetActive(true);

        startOvenButton.gameObject.SetActive(true);
        startOvenButton.interactable = true;

        startOvenButton.onClick.RemoveAllListeners();
        startOvenButton.onClick.AddListener(OnStartOvenClicked);

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);
    }

    private void OnStartOvenClicked()
    {
        // 오븐 시작 버튼을 누른 뒤에는 더 이상 누르지 못하게 disable
        startOvenButton.interactable = false;

        // 굽기 사운드 루프 재생
        if (bakingLoopClip != null)
        {
            audioSource.clip = bakingLoopClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        StartCoroutine(BakeCoroutine());
    }

    private IEnumerator BakeCoroutine()
    {
        yield return new WaitForSeconds(bakeDuration);

        // 굽기 사운드 중단
        if (audioSource.isPlaying)
            audioSource.Stop();

        // 굽기 완료 사운드 재생
        if (finishBakeClip != null)
            audioSource.PlayOneShot(finishBakeClip);
            startOvenButton.interactable = true;

        // 1) "다음" 버튼만 보이게 활성화
        nextButton.interactable = true;

        Debug.Log($"TartOven: {bakeDuration}초 경과, 다음 단계 버튼 활성화됨");
    }

    public void OnNextClicked()
    {
        // 다단계 실패 처리: 두 번 이상 누르면 실패
        nextClickCount++;

        if (nextClickCount >= 2)
        {
            Debug.Log("TartOven: Next 버튼이 두 번 눌려서 실패 처리");

            // 오븐 버튼, 다음 버튼 둘 다 비활성화(숨기기)
            startOvenButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);

            // 패널 이미지는 그대로 남겨둡니다 (이미지가 요구사항)
            tartManagerRef?.OnOvenComplete(false);
        }
        else
        {
            // 첫 번째 클릭 → 성공으로 간주
            Debug.Log("TartOven: Next 버튼 첫 클릭, 오븐 단계 성공 처리");

            // 오븐 버튼, 다음 버튼 둘 다 비활성화(숨기기)
            startOvenButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);

            // 패널 이미지는 그대로 남겨둡니다
            tartManagerRef?.OnOvenComplete(true);
        }
    }
}
