using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public Sprite pressedSprite;

    [Header("���� Ŭ��")]
    [Tooltip("Ŭ�� �� ����� ����� Ŭ��")]
    public AudioClip clickSound;

    private Image targetImage;
    private AudioSource audioSource;
    private bool isPointerOver = false;

    void Awake()
    {
        targetImage = GetComponent<Image>();
        if (defaultSprite != null)
            targetImage.sprite = defaultSprite;

        // AudioSource�� ������ ���� �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        targetImage.sprite = hoverSprite != null ? hoverSprite : defaultSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        targetImage.sprite = defaultSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetImage.sprite = pressedSprite != null ? pressedSprite : hoverSprite;

        // Ŭ�� ���� ���
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetImage.sprite = isPointerOver ? hoverSprite : defaultSprite;
    }
}
