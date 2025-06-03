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

    private Image targetImage;
    private bool isPointerOver = false;

    void Awake()
    {
        targetImage = GetComponent<Image>();
        if (defaultSprite != null)
            targetImage.sprite = defaultSprite;
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
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetImage.sprite = isPointerOver ? hoverSprite : defaultSprite;
    }
}