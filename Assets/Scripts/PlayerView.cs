using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public float mouseSensitivity = 120f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Camera xoay lên xuống
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Player xoay trái phải
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
