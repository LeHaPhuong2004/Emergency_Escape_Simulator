using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    // ================= CAMERA =================
    [Header("Camera")]
    public float mouseSensitivity = 2.5f;
    private float verticalRotation;
    private Transform cameraHolder;

    // ================= MOVEMENT =================
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;

    public float acceleration = 14f;
    public float deceleration = 20f;
    public float airControl = 0.4f;

    private float moveX;
    private float moveZ;

    // ================= JUMP =================
    [Header("Jump")]
    public float jumpForce = 7f;
    public LayerMask groundLayer;

    // ================= PRIVATE =================
    private Rigidbody rb;
    private bool isGrounded;
    private float rayDistance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        cameraHolder = transform.Find("CameraHolder");

        CapsuleCollider col = GetComponent<CapsuleCollider>();
        rayDistance = (col.height * 0.5f) + 0.2f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // INPUT SMOOTH
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            Jump();

        GroundCheck();
    }

    void FixedUpdate()
    {
        Move();
        ExtraGravity();
    }

    void LateUpdate()
    {
        RotateCamera();
    }

    // ================= MOVE =================
    void Move()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        Vector3 dir = (transform.right * moveX + transform.forward * moveZ).normalized;
        Vector3 targetVelocity = dir * speed;

        Vector3 velocity = rb.linearVelocity;

        float control = isGrounded ? acceleration : acceleration * airControl;
        float slowDown = isGrounded ? deceleration : 0;

        if (dir.magnitude > 0)
        {
            velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, control * Time.fixedDeltaTime);
            velocity.z = Mathf.Lerp(velocity.z, targetVelocity.z, control * Time.fixedDeltaTime);
        }
        else
        {
            velocity.x = Mathf.Lerp(velocity.x, 0, slowDown * Time.fixedDeltaTime);
            velocity.z = Mathf.Lerp(velocity.z, 0, slowDown * Time.fixedDeltaTime);
        }

        rb.linearVelocity = velocity;
    }

    // ================= CAMERA =================
    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, mouseX, 0f));

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -85f, 85f);
        cameraHolder.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    // ================= JUMP =================
    void Jump()
    {
        Vector3 vel = rb.linearVelocity;
        vel.y = 0;
        rb.linearVelocity = vel;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // ================= GROUND CHECK =================
    void GroundCheck()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        isGrounded = Physics.Raycast(origin, Vector3.down, rayDistance, groundLayer);
    }

    // ================= EXTRA GRAVITY =================
    void ExtraGravity()
    {
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * 1.8f * Time.fixedDeltaTime;
        }
    }
}