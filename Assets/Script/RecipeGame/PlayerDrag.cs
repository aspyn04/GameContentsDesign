// PlayerDrag.cs
using UnityEngine;

public class PlayerDrag : MonoBehaviour
{
    private Camera cam;
    private Rigidbody2D rb;
    private bool isDragging = false;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void FixedUpdate()
    {
        if (!isDragging) return;
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        rb.MovePosition(mouseWorldPos);
    }

    // �ܺο��� �巡�� ���¸� ������ �����ϱ� ���� �޼���
    public void ResetDrag()
    {
        isDragging = false;
    }
}
