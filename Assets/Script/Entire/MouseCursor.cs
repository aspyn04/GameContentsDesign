using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public static MouseCursor Instance { get; private set; }

    [Header("기본 커서")]
    public Texture2D defaultCursor;
    public Vector2 defaultHotspot = Vector2.zero; // 커서의 클릭 지점 (중앙은 0.5, 0.5)

    [Header("커스텀 커서 (선택 사항)")]
    public Texture2D customCursor;
    public Vector2 customHotspot = Vector2.zero;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 새로운 인스턴스 파괴
        }
    }

    void Start()
    {
        SetDefaultCursor(); // 게임 시작 시 기본 커서로 설정
    }

    // 기본 커서로 설정하는 함수
    public void SetDefaultCursor()
    {
        if (defaultCursor != null)
        {
            Cursor.SetCursor(defaultCursor, defaultHotspot, CursorMode.Auto);
        }
        else
        {
            Debug.LogWarning("Default Cursor is not assigned in CursorManager.");
        }
    }

    // 커스텀 커서로 설정하는 함수
    public void SetCustomCursor(Texture2D cursorTexture = null, Vector2 hotspot = default(Vector2))
    {
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
        }
        else if (customCursor != null) // 커스텀 커서 텍스처가 따로 지정되지 않았다면, 미리 할당된 customCursor 사용
        {
            Cursor.SetCursor(customCursor, customHotspot, CursorMode.Auto);
        }
        else
        {
            Debug.LogWarning("No custom cursor texture provided or assigned in CursorManager.");
        }
    }

    // 커서를 보이거나 숨기는 함수
    public void SetCursorVisibility(bool isVisible)
    {
        Cursor.visible = isVisible;
    }

    // 커서 잠금 모드를 설정하는 함수
    public void SetCursorLockMode(CursorLockMode lockMode)
    {
        Cursor.lockState = lockMode;
    }

    // 마우스 포인터가 UI 위에 있는지 확인 (옵션)
    // using UnityEngine.EventSystems; 를 추가해야 합니다.
    // public bool IsPointerOverUIObject()
    // {
    //     return EventSystem.current.IsPointerOverGameObject();
    // }
}