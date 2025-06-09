// TimeSlider.cs
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeSlider : MonoBehaviour
{
    [Header("Game Root (Container)")]
    public GameObject gameRoot;

    [Header("Game Timer")]
    public Slider timeSlider;
    public Image fillImage;
    public float gameDuration = 50f;

    [Header("Lives")]
    public GameObject life1, life2, life3;

    [Header("Ingredients")]
    public GameObject ingredient01, ingredient02, ingredient03;

    [Header("Result Panels")]
    public GameObject clearPanel;
    public Button clearNextButton;
    public GameObject failPanel;
    public Button retryButton;
    private PlayerDrag playerDrag;

    private float timeLeft;
    private bool gameEnded;
    private bool has1, has2, has3;
    private Action onComplete;

    private float blinkTimer;
    private const float blinkInterval = 0.3f;

    void Start()
    {
        ResetGame();
        clearNextButton.onClick.AddListener(OnClearNext);
        retryButton.onClick.AddListener(OnRetry);
        playerDrag = FindObjectOfType<PlayerDrag>();

    }

    void Update()
    {
        if (gameEnded) return;

        timeLeft -= Time.deltaTime;
        timeSlider.value = Mathf.Clamp(timeLeft, 0f, gameDuration);

        // Color transition
        float t = 1f - (timeLeft / gameDuration);
        if (t < 0.5f)
            fillImage.color = Color.Lerp(Color.green, Color.yellow, t * 2f);
        else
            fillImage.color = Color.Lerp(Color.yellow, Color.red, (t - 0.5f) * 2f);

        // Blink when low
        if (timeLeft <= 10f)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkInterval)
            {
                blinkTimer = 0f;
                fillImage.enabled = !fillImage.enabled;
            }
        }
        else fillImage.enabled = true;

        // Fail conditions
        if (timeLeft <= 0f || (!life1.activeSelf && !life2.activeSelf && !life3.activeSelf))
            FinishFail();
    }

    public void ResetGame()
    {
        gameEnded = false;
        timeLeft = gameDuration;
        blinkTimer = 0f;
        has1 = has2 = has3 = false;

        // show root
        if (gameRoot != null) gameRoot.SetActive(true);

        // reset timer visuals
        timeSlider.maxValue = gameDuration;
        timeSlider.value = timeLeft;
        fillImage.enabled = true;
        fillImage.color = Color.green;

        // reset lives & ingredients
        life1.SetActive(true);
        life2.SetActive(true);
        life3.SetActive(true);
        ingredient01.SetActive(true);
        ingredient02.SetActive(true);
        ingredient03.SetActive(true);

        clearPanel.SetActive(false);
        failPanel.SetActive(false);
    }

    public void CollectIngredient(int idx)
    {
        if (gameEnded) return;
        switch (idx)
        {
            case 1: has1 = true; ingredient01.SetActive(false); break;
            case 2: has2 = true; ingredient02.SetActive(false); break;
            case 3: has3 = true; ingredient03.SetActive(false); break;
        }
    }

    public void ReachGoal()
    {
        if (gameEnded) return;
        Finish(has1 && has2 && has3);
    }

    public void FinishFail()
    {
        if (gameEnded) return;
        Finish(false);
    }

    public void Finish(bool success)
    {
        gameEnded = true;
        if (gameRoot != null) gameRoot.SetActive(false);

        // 드래그 비활성화
        if (playerDrag != null)
            playerDrag.enabled = false;

        if (success) clearPanel.SetActive(true);
        else failPanel.SetActive(true);
    }

    private void OnClearNext()
    {
        clearPanel.SetActive(false);
        onComplete?.Invoke();
    }

    // TimeSlider.cs (OnRetry 부분 수정)
    private void OnRetry()
    {
        if (!failPanel.activeSelf) return;

        failPanel.SetActive(false);

        // 게임 UI 복원
        ResetGame();

        // 플레이어 상태 초기화
        var player = FindObjectOfType<PlayerCollision>();
        if (player != null)
            player.ResetPlayer();

        // 드래그 스크립트 리셋 및 활성화
        var drag = FindObjectOfType<PlayerDrag>();
        if (drag != null)
        {
            drag.ResetDrag();      // 클릭 플래그 끈다
            drag.enabled = true;   // 스크립트 활성화
        }
    }


    public void SetCompletionCallback(Action callback)
    {
        onComplete = callback;
    }
}
