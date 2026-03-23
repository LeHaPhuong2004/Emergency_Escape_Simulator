using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float mouseSensitivity = 70f;
    public Transform playerBody; // player xoay ngang
    float xRotation = 0f;
    float mouseX;
    float mouseY;
    float smoothMouseX;
    float smoothMouseY;
    public float smoothTime = 0.05f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        Look();
    }

    void Look()
    {
        // lấy input thô
        mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        // làm mượt chuột
        smoothMouseX = Mathf.Lerp(smoothMouseX, mouseX, 1f / smoothTime * Time.deltaTime);
        smoothMouseY = Mathf.Lerp(smoothMouseY, mouseY, 1f / smoothTime * Time.deltaTime);

        // xoay lên xuống
        xRotation -= smoothMouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // xoay player
        playerBody.Rotate(Vector3.up * smoothMouseX * Time.deltaTime);
    }
}
