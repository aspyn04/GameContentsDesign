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
        // �� ���� ������ ���� ��ǥ�� �ʱ� ��ġ�� ����
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
            Debug.Log("���� �ε���");
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
            // ���� ó��
            timer?.FinishFail();
            playerCollider.enabled = false;
        }
    }

    public void ResetPlayer()
    {
        // �����ص� �ʱ� ��ġ�� ��Ȯ�� ����
        transform.position = startPosition;

        // ������ �ʱ�ȭ
        lifeCount = lives.Length;
        for (int i = 0; i < lives.Length; i++)
            lives[i].SetActive(true);

        // ���� ���� ����
        isInvincible = false;
        invincibleTimer = 0f;

        // �ݶ��̴� ��Ȱ��ȭ
        playerCollider.enabled = true;
    }
}
