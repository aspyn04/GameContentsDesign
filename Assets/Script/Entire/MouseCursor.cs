using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public static MouseCursor Instance { get; private set; }

    [Header("�⺻ Ŀ��")]
    public Texture2D defaultCursor;
    public Vector2 defaultHotspot = Vector2.zero; // Ŀ���� Ŭ�� ���� (�߾��� 0.5, 0.5)

    [Header("Ŀ���� Ŀ�� (���� ����)")]
    public Texture2D customCursor;
    public Vector2 customHotspot = Vector2.zero;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ����Ǿ �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� ���ο� �ν��Ͻ� �ı�
        }
    }

    void Start()
    {
        SetDefaultCursor(); // ���� ���� �� �⺻ Ŀ���� ����
    }

    // �⺻ Ŀ���� �����ϴ� �Լ�
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

    // Ŀ���� Ŀ���� �����ϴ� �Լ�
    public void SetCustomCursor(Texture2D cursorTexture = null, Vector2 hotspot = default(Vector2))
    {
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
        }
        else if (customCursor != null) // Ŀ���� Ŀ�� �ؽ�ó�� ���� �������� �ʾҴٸ�, �̸� �Ҵ�� customCursor ���
        {
            Cursor.SetCursor(customCursor, customHotspot, CursorMode.Auto);
        }
        else
        {
            Debug.LogWarning("No custom cursor texture provided or assigned in CursorManager.");
        }
    }

    // Ŀ���� ���̰ų� ����� �Լ�
    public void SetCursorVisibility(bool isVisible)
    {
        Cursor.visible = isVisible;
    }

    // Ŀ�� ��� ��带 �����ϴ� �Լ�
    public void SetCursorLockMode(CursorLockMode lockMode)
    {
        Cursor.lockState = lockMode;
    }

    // ���콺 �����Ͱ� UI ���� �ִ��� Ȯ�� (�ɼ�)
    // using UnityEngine.EventSystems; �� �߰��ؾ� �մϴ�.
    // public bool IsPointerOverUIObject()
    // {
    //     return EventSystem.current.IsPointerOverGameObject();
    // }
}