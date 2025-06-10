using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonImangeColor : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    public Graphic targetImage;
    public Button button;

    public Color normalColor = Color.white;
    public Color hoverColor = new Color(0.6f, 0.6f, 0.6f);
    public Color pressedColor = new Color(0.4f, 0.4f, 0.4f);
    public Color activeColor = new Color(0.2f, 0.2f, 0.2f);
    public Color disabledColor = Color.white;

    private int clickCount = 0;
    private bool isPointerOver = false;
    private bool isActive = false;

    private void OnEnable()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClicked);
            button.onClick.AddListener(OnButtonClicked);
            Debug.Log($"[OnEnable] {name} 이벤트 재연결 완료");
        }

        if (targetImage != null)
        {
            targetImage.color = normalColor;
        }

        isActive = false;
        clickCount = 0;
    }

    void Start()
    {
        ResetButtonState();

        if (button != null)
        {
            button.transition = Selectable.Transition.None;
            button.onClick.RemoveListener(OnButtonClicked); // ← 기존 리스너 제거

            button.onClick.AddListener(OnButtonClicked);
            Debug.Log($"[START] {name} 버튼 이벤트 연결됨");


        }

        if (targetImage != null)
        {
            targetImage.color = normalColor;
        }
        else
        {
        }
    }
    public void InitializeButton()
    {
        if (button == null) return;

        button.transition = Selectable.Transition.None;

        var btnImage = button.GetComponent<Image>();
        if (btnImage != null)
        {
            btnImage.sprite = null;
            btnImage.color = new Color(1f, 1f, 1f, 0f); // 투명
        }

        button.onClick.RemoveListener(OnButtonClicked); // 중복 제거
        button.onClick.AddListener(OnButtonClicked);    // 다시 연결

        ApplyColor(normalColor);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        if (!isActive) ApplyColor(hoverColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        if (!isActive) ApplyColor(normalColor);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isActive) ApplyColor(pressedColor);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isActive) ApplyColor(isPointerOver ? hoverColor : normalColor);
    }

    private void OnButtonClicked()
    {
        Debug.Log("버튼클릭");
        clickCount++;
        isActive = (clickCount % 2 == 1); 

        if (targetImage != null)
            targetImage.color = isActive ? activeColor : disabledColor;
    }

    public void ResetButtonState()
    {
        isActive = false;
        clickCount = 0;

        if (targetImage != null)
        {
            Debug.Log($"[{name}] Reset: image color = normalColor");
            targetImage.color = normalColor;
        }
        else
        {
            Debug.LogError($"[{name}] Reset: targetImage == null !!!");
        }

        if (button != null)
        {
            Debug.Log($"[{name}] Reset: interactable = true");
            button.interactable = false; 
            button.interactable = true;
        }
    }

    public void OnNextStepTriggered()
    {
        ResetButtonState();
    }

    private void ApplyColor(Color color)
    {
        if (targetImage != null)
            targetImage.color = color;
    }
}