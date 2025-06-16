using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Player Lives UI")]
    public GameObject[] lives;
    public float invincibleDuration = 0.2f;

    [Header("Audio")]
    public AudioClip hitSound;
    public AudioClip eatSound;
    private AudioSource audioSource;

    [Header("Spawn Point")]
    public Transform startPoint; // 각 스테이지에서 Inspector에서 지정
    private Vector3 initialStartPosition; // 실행 시점에 저장해두는 위치

    private int lifeCount;
    private bool isInvincible;
    private float invincibleTimer;

    private MinigameManager timer;
    private Collider2D playerCollider;

    void Start()
    {
        if (startPoint != null)
        {
            initialStartPosition = startPoint.position; // 최초 위치 저장
        }
        else
        {
            Debug.LogWarning("[PlayerCollision] startPoint not assigned.");
            initialStartPosition = transform.position; // fallback
        }

        lifeCount = lives.Length;
        audioSource = GetComponent<AudioSource>();
        timer = FindObjectOfType<MinigameManager>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
                isInvincible = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInvincible) return;

        if (collision.collider.CompareTag("Wall"))
        {
            Debug.Log("벽에 부딪힘");
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        if (lifeCount <= 0) return;

        audioSource.PlayOneShot(hitSound);
        lifeCount--;
        lives[lifeCount].SetActive(false);

        isInvincible = true;
        invincibleTimer = invincibleDuration;

        if (lifeCount <= 0)
        {
            timer?.FinishFail();
            playerCollider.enabled = false;
        }
    }

    public void ResetPlayer()
    {
        // 초기 저장 위치로 정확하게 복귀
        transform.position = initialStartPosition;

        lifeCount = lives.Length;
        foreach (var life in lives)
            life.SetActive(true);

        isInvincible = false;
        invincibleTimer = 0f;
        playerCollider.enabled = true;
    }
}