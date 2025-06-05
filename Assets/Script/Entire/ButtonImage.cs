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

    [Header("사운드 클립")]
    [Tooltip("클릭 시 재생할 오디오 클립")]
    public AudioClip clickSound;

    private Image targetImage;
    private AudioSource audioSource;
    private bool isPointerOver = false;

    void Awake()
    {
        targetImage = GetComponent<Image>();
        if (defaultSprite != null)
            targetImage.sprite = defaultSprite;

        // AudioSource가 없으면 새로 추가
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

        // 클릭 사운드 재생
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetImage.sprite = isPointerOver ? hoverSprite : defaultSprite;
    }
}
