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

    // 오븐 시작 버튼이 눌린 횟수를 추적
    private int startClickCount = 0;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// TartManager에서 오븐 단계를 시작할 때 호출
    /// </summary>
    public void Init(TartManager manager)
    {
        tartManagerRef = manager;
        startClickCount = 0;

        // 패널 보이기
        if (ovenPanel != null)
            ovenPanel.SetActive(true);

        // 버튼 상태 초기화
        startOvenButton.gameObject.SetActive(true);
        startOvenButton.interactable = true;
        nextButton.gameObject.SetActive(true);
        nextButton.interactable = false;

        // 리스너 설정
        startOvenButton.onClick.RemoveAllListeners();
        startOvenButton.onClick.AddListener(OnStartOvenClicked);

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextClicked);
    }

    private void OnStartOvenClicked()
    {
        startClickCount++;

        // 두 번 이상 누르면 실패 처리
        if (startClickCount >= 2)
        {
            Debug.Log("TartOven: 시작 버튼이 두 번 눌려 실패 처리");
            FailOven();
            return;
        }

        // 첫 클릭: 굽기 시작
        startOvenButton.interactable = false;

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

        // 완료 사운드 재생
        if (finishBakeClip != null)
            audioSource.PlayOneShot(finishBakeClip);

        // 다음 버튼 표시
        nextButton.interactable = true;

        Debug.Log($"TartOven: {bakeDuration}초 경과, 다음 단계 버튼 활성화됨");
    }

    private void OnNextClicked()
    {
        // 성공 처리
        Debug.Log("TartOven: 다음 버튼 클릭, 오븐 단계 성공 처리");
        CompleteOven();
    }

    private void FailOven()
    {
        // UI 정리
        startOvenButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        if (ovenPanel != null)
            ovenPanel.SetActive(false);

        // 실패 알림
        tartManagerRef?.OnOvenComplete(false);
    }

    private void CompleteOven()
    {
        // UI 정리
        startOvenButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        if (ovenPanel != null)
            ovenPanel.SetActive(false);

        // 성공 알림
        tartManagerRef?.OnOvenComplete(true);
    }
}