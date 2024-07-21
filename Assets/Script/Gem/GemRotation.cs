using UnityEngine;

public class GemRotation : MonoBehaviour
{
    public float rotationSpeed = 100f; // 회전 속도를 설정할 수 있습니다.
    private bool isDragging = false;
    private Vector3 lastMousePosition;

    void Update()
    {
        // 마우스 왼쪽 버튼이 눌렸을 때 드래그 시작
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        // 마우스 왼쪽 버튼이 떼졌을 때 드래그 중지
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // 드래그 중일 때 오브젝트 회전
        if (isDragging)
        {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;
            float rotationX = deltaMousePosition.y * rotationSpeed * Time.deltaTime;
            float rotationY = -deltaMousePosition.x * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.right, rotationX, Space.World);
            transform.Rotate(Vector3.up, rotationY, Space.World);

            lastMousePosition = Input.mousePosition;
        }
    }
}