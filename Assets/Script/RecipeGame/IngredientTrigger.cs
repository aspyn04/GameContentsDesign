using UnityEngine;

public class IngredientTrigger : MonoBehaviour
{
    [Tooltip("1, 2, 3 �� �ش� ����� �ε����� ����")]
    public int ingredientIndex;
    public AudioClip collectSound;
    [Range(0f, 1f)] public float soundVolume = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // TimeSlider�� ��� ���� �˸�
        TimeSlider timer = FindObjectOfType<TimeSlider>();
        if (timer != null)
            timer.CollectIngredient(ingredientIndex);

        // ���� ���
        AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position, soundVolume);

        // Ʈ���� ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
}