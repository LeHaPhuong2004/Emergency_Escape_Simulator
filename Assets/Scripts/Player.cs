using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
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

    private PlayerStatus status;
    private CapsuleCollider col;
    private float crouchHeight = 1f;
    private float normalHeight;

    [Header("Detection")]
    public float groundDamping = 5f;
    public LayerMask groundLayer;
    public bool isGrounded;

    private Rigidbody rb;
    private float moveX, moveZ;

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

       //chay
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
        //mana
        if (isSprinting)
        {
            status.ReduceMana(manaPerSecond * Time.deltaTime);

            if (status.currentMana <= 0)
            {
                isSprinting = false;
            }
        }
        else
        {
            status.currentMana += 12f * Time.deltaTime;
            status.currentMana = Mathf.Clamp(status.currentMana, 0, status.maxMana);
            status.manaSlider.value = status.currentMana;
        }
        bool isCrouching = Input.GetKey(KeyCode.LeftShift);
        float targetHeigh = isCrouching ? crouchHeight : normalHeight;
        col.height = Mathf.Lerp(col.height, targetHeigh, crouchSmooth*Time.deltaTime);
        col.center = new Vector3(0, col.height / 2f, 0);

        // cui nguoi
        float targetY = Input.GetKey(KeyCode.LeftShift) ? crouchY : normalY;
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

        float currentSpeed = moveSpeed;

        // sprint
        if (isSprinting && moveZ > 0)
        {
            currentSpeed *= sprintMultiplier;
        }

        // crouch
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= crouchSpeedMultiplier;
            isSprinting = false;
        }

        Vector3 targetVelocity = moveDir.normalized * currentSpeed;

        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.back) > 0.5f)
            {
                isSprinting = false;
                break;
            }
        }
    }
    void Jump()
    {
    
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}