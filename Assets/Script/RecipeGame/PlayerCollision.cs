using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Vector3 startPosition;

    [Header("Player Lives UI")]
    public GameObject[] lives;
    public float invincibleDuration = 0.2f;

    [Header("Audio")]
    public AudioClip hitSound;
    public AudioClip eatSound;
    private AudioSource audioSource;

    private int lifeCount;
    private bool isInvincible;
    private float invincibleTimer;

    private TimeSlider timer;
    private Collider2D playerCollider;

    void Awake()
    {
        // 씬 시작 시점에 월드 좌표로 초기 위치를 저장
        startPosition = transform.position;
    }

    void Start()
    {
        lifeCount = lives.Length;
        audioSource = GetComponent<AudioSource>();
        timer = FindObjectOfType<TimeSlider>();
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
            // 실패 처리
            timer?.FinishFail();
            playerCollider.enabled = false;
        }
    }

    public void ResetPlayer()
    {
        // 저장해둔 초기 위치로 정확히 복귀
        transform.position = startPosition;

        // 라이프 초기화
        lifeCount = lives.Length;
        for (int i = 0; i < lives.Length; i++)
            lives[i].SetActive(true);

        // 무적 상태 해제
        isInvincible = false;
        invincibleTimer = 0f;

        // 콜라이더 재활성화
        playerCollider.enabled = true;
    }
}
