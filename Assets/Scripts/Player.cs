using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Detection")]
    public float groundDamping = 5f;
    public LayerMask groundLayer;
    public bool isGrounded;

    private Rigidbody rb;
    private float moveX, moveZ;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");

        // Kiểm tra mặt đất
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);

        // Cập nhật linearDamping thay cho drag
        if (isGrounded)
            rb.linearDamping = groundDamping;
        else
            rb.linearDamping = 0;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        // Tính toán hướng dựa trên hướng xoay của Player đã được Script Camera cập nhật
        Vector3 moveDir = transform.forward * moveZ + transform.right * moveX;

        // Gán vận tốc trực tiếp để triệt tiêu độ trễ của lực (AddForce)
        Vector3 targetVelocity = moveDir.normalized * moveSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    void Jump()
    {
        // THAY THẾ: rb.velocity -> rb.linearVelocity
        // Reset vận tốc Y về 0 để cú nhảy luôn có độ cao bằng nhau
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}