using UnityEngine;

public class IngredientTrigger : MonoBehaviour
{
    [Tooltip("1, 2, 3 중 해당 재료의 인덱스를 설정")]
    public int ingredientIndex;
    public AudioClip collectSound;
    [Range(0f, 1f)] public float soundVolume = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // TimeSlider에 재료 수집 알림
        TimeSlider timer = FindObjectOfType<TimeSlider>();
        if (timer != null)
            timer.CollectIngredient(ingredientIndex);

        // 사운드 재생
        AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position, soundVolume);

        // 트리거 비활성화
        gameObject.SetActive(false);
    }
}