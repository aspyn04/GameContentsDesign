using UnityEngine;
using UnityEngine.UI;
using System;

public class MinigameManager : MonoBehaviour
{
    [Header("This Panel's Day")]
    [SerializeField] private int panelDay;

    [Header("Game Root")]
    public GameObject gameRoot;

    [Header("Timer UI")]
    public Slider timeSlider;
    public Image fillImage;
    public float gameDuration = 50f;

    [Header("Lives")]
    public GameObject life1, life2, life3;

    [Header("Ingredients")]
    public GameObject ingredient01, ingredient02, ingredient03;

    [Header("슬라이더 색상")]
    public Color firstColor = Color.green;
    public Color secondColor = Color.yellow;
    public Color thirdColor = Color.red;

    [Header("Result Panels")]
    public GameObject clearPanel;
    public Button clearNextButton;
    public GameObject failPanel;
    public Button retryButton;

    private PlayerDrag playerDrag;
    private float timeLeft;
    private bool gameEnded;
    private bool has1, has2, has3;
    private float blinkTimer;
    private const float blinkInterval = 0.3f;

    private Action onComplete;
    private int currentDay = -1;

    void Start()
    {
        clearNextButton.onClick.AddListener(OnClearNext);
        retryButton.onClick.AddListener(OnRetry);
        playerDrag = FindObjectOfType<PlayerDrag>();
    }

    void Update()
    {
        if (gameEnded) return;

        timeLeft -= Time.deltaTime;
        timeSlider.value = Mathf.Clamp(timeLeft, 0f, gameDuration);

        float t = 1f - (timeLeft / gameDuration);
        fillImage.color = t < 0.5f
            ? Color.Lerp(firstColor, secondColor, t * 2f)
            : Color.Lerp(secondColor, thirdColor, (t - 0.5f) * 2f);


        if (timeLeft <= 10f)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkInterval)
            {
                blinkTimer = 0f;
                fillImage.enabled = !fillImage.enabled;
            }
        }
        else
        {
            fillImage.enabled = true;
        }

        if (timeLeft <= 0f || (!life1.activeSelf && !life2.activeSelf && !life3.activeSelf))
            FinishFail();
    }

    public void StartGame(int day)
    {
        if (day != panelDay)
        {
            Debug.LogWarning($"[MinigameManager] Mismatched day! Expected: {panelDay}, got: {day}");
            return;
        }

        currentDay = day;
        Debug.Log($"[MinigameManager] StartGame called for day: {currentDay}");
        ResetGame();
    }

    private void ResetGame()
    {
        Debug.Log($"[MinigameManager] ResetGame - using currentDay: {currentDay}");

        gameEnded = false;
        timeLeft = gameDuration;
        blinkTimer = 0f;
        has1 = has2 = has3 = false;

       //모든 패널 비활성화 후 currentDay 패널만 활성화
        foreach (var entry in RecipeMinigameManager.Instance.panelEntries)
        {
            if (entry.panelObject != null)
                entry.panelObject.SetActive(entry.day == currentDay);
        }

        if (gameRoot != null) gameRoot.SetActive(true);

        timeSlider.maxValue = gameDuration;
        timeSlider.value = timeLeft;
        fillImage.enabled = true;
        fillImage.color = Color.green;

        life1.SetActive(true);
        life2.SetActive(true);
        life3.SetActive(true);
        ingredient01.SetActive(true);
        ingredient02.SetActive(true);
        ingredient03.SetActive(true);

        clearPanel.SetActive(false);
        failPanel.SetActive(false);
        retryButton?.gameObject.SetActive(true);
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

        if (playerDrag != null)
            playerDrag.enabled = false;

        if (success)
            clearPanel.SetActive(true);
        else
            failPanel.SetActive(true);
    }

    private void OnClearNext()
    {
        clearPanel.SetActive(false);
        onComplete?.Invoke();
    }

    private void OnRetry()
    {
        Debug.Log($"[MinigameManager] OnRetry clicked on panelDay: {panelDay}");
        failPanel.SetActive(false);
        ResetGame();

        var player = FindObjectOfType<PlayerCollision>();
        if (player != null)
            player.ResetPlayer();

        var drag = FindObjectOfType<PlayerDrag>();
        if (drag != null)
        {
            drag.ResetDrag();
            drag.enabled = true;
        }
    }

    public void SetCompletionCallback(Action callback)
    {
        onComplete = callback;
    }
}
