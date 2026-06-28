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

    [Header("Ground & Slope")]
    public float groundDamping = 2f; 
    public LayerMask groundLayer;
    public bool isGrounded;
    public float maxSlopeAngle = 45f;
    private RaycastHit slopeHit;

    [Header("State (IMPORTANT)")]
    public bool isCrouching;

    private PlayerStatus status;
    private CapsuleCollider col;
    private Rigidbody rb;

    private float moveX, moveZ;
    private float crouchHeight = 1f;
    private float normalHeight;

    [Header("Footstep")]
    public float footstepDelay = 0.5f;
    private float footstepTimer;

    void Start()
    {
        footstepTimer = footstepDelay;
        rb = GetComponent<Rigidbody>();
        status = GetComponent<PlayerStatus>();

        //khoa rb de nhan vat khong bi tac dong khi va chạm
        rb.freezeRotation = true;

        col = GetComponent<CapsuleCollider>();
        normalHeight = col.height;
    }

    void Update()
    {
        HandleFootstep();

        //lay input tu nguoi choi
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");

        // ban tia raycst xuong dat de kiem tra mat dat va luu vao slopehit
        float rayLength = (col.height * 0.5f) + 0.2f;

        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            out slopeHit,
            rayLength,
            groundLayer
        );

        //quan li luc cản
        if (isGrounded)
            rb.linearDamping = groundDamping;
        else
            rb.linearDamping = 0;

       //nhay
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        //cui nguoi
        isCrouching = Input.GetKey(KeyCode.LeftShift);

        //chay nhanh
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

        // quan ly mana khi chay nhanh
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

        //collider keo xuong theo camera khi cui
        float targetHeight = isCrouching ? crouchHeight : normalHeight;
        col.height = Mathf.Lerp(col.height, targetHeight, crouchSmooth * Time.deltaTime);
        col.center = new Vector3(0, col.height / 2f, 0);

       //camera keo xuong
        float targetY = isCrouching ? crouchY : normalY;
        Vector3 camPos = cameraHolder.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetY, crouchSmooth * Time.deltaTime);
        cameraHolder.localPosition = camPos;
    }

    void HandleFootstep()
    {
        bool isMoving =
    isGrounded &&
    (
        Input.GetAxisRaw("Horizontal") != 0 ||
        Input.GetAxisRaw("Vertical") != 0
    );

        if (isMoving)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.footStepClip);
                footstepTimer = footstepDelay;
            }
        }
        else
        {
            footstepTimer = 0;
        }
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

        Vector3 finalMoveDir = moveDir.normalized;

        if (isGrounded)
        {
            float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);

            if (slopeAngle < maxSlopeAngle && slopeAngle > 0)
            {
                finalMoveDir = Vector3.ProjectOnPlane(
                    finalMoveDir,
                    slopeHit.normal
                ).normalized;
            }

            Vector3 vel = finalMoveDir * speed;

            // Giữ nguyên velocity Y để không làm khựng cú nhảy
            rb.linearVelocity = new Vector3(
                vel.x,
                rb.linearVelocity.y,
                vel.z
            );
        }
        else
        {
            Vector3 vel = finalMoveDir * speed;

            rb.linearVelocity = new Vector3(
                vel.x,
                rb.linearVelocity.y,
                vel.z
            );
        }
    }

    void Jump()
    {
        //reset van toc y chi khi nhay xuong de co cu nhay chuan xac

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        }

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}