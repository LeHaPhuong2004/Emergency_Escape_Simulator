using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float jumpForce = 7f;

    [Header("Sprint")]
    public float sprintMultiplier = 1.8f;
    public float doubleTapTime = 0.3f;
    private float lastWPressTime = -1f;
    private bool isSprinting = false;
    public float manaPerSecond = 12f;

    [Header("Crouch")]
    public float crouchSpeedMultiplier = 0.5f;
    public Transform cameraHolder;
    public float crouchY = 0.2f;
    public float normalY = 1.6f;
    public float crouchSmooth = 10f;

    [Header("Ground")]
    public float groundDamping = 5f;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("State (IMPORTANT)")]
    public bool isCrouching;   // <<< thêm cái này

    private PlayerStatus status;
    private CapsuleCollider col;
    private Rigidbody rb;

    private float moveX, moveZ;
    private float crouchHeight = 1f;
    private float normalHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        status = GetComponent<PlayerStatus>();

        rb.freezeRotation = true;

        col = GetComponent<CapsuleCollider>();
        normalHeight = col.height;
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);

        if (isGrounded)
            rb.linearDamping = groundDamping;
        else
            rb.linearDamping = 0;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // ===== CROUCH STATE =====
        isCrouching = Input.GetKey(KeyCode.LeftShift);

        // ===== SPRINT =====
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time - lastWPressTime <= doubleTapTime && status.currentMana > 0)
            {
                isSprinting = true;
            }
            lastWPressTime = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            isSprinting = false;
        }

        // mana
        if (isSprinting)
        {
            status.ReduceMana(manaPerSecond * Time.deltaTime);

            if (status.currentMana <= 0)
                isSprinting = false;
        }
        else
        {
            status.currentMana += 12f * Time.deltaTime;
            status.currentMana = Mathf.Clamp(status.currentMana, 0, status.maxMana);
            status.manaSlider.value = status.currentMana;
        }

        // ===== COLLIDER CROUCH =====
        float targetHeight = isCrouching ? crouchHeight : normalHeight;
        col.height = Mathf.Lerp(col.height, targetHeight, crouchSmooth * Time.deltaTime);
        col.center = new Vector3(0, col.height / 2f, 0);

        // ===== CAMERA CROUCH =====
        float targetY = isCrouching ? crouchY : normalY;
        Vector3 camPos = cameraHolder.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetY, crouchSmooth * Time.deltaTime);
        cameraHolder.localPosition = camPos;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 moveDir = transform.forward * moveZ + transform.right * moveX;

        float speed = moveSpeed;

        if (isSprinting && moveZ > 0)
            speed *= sprintMultiplier;

        if (isCrouching)
        {
            speed *= crouchSpeedMultiplier;
            isSprinting = false;
        }

        Vector3 vel = moveDir.normalized * speed;

        rb.linearVelocity = new Vector3(vel.x, rb.linearVelocity.y, vel.z);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}