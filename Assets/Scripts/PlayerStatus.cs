using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public Player playerMovement;
    public CameraFollow camerafollow;

    private bool isDead = false;

    public Transform cameraHolder;

    [Header("Stats")]
    public float maxHealth = 200f;
    public float maxMana = 200f;
    public float maxBreath = 200f;

    public float currentHealth;
    public float currentMana;
    public float currentBreath;

    [Header("UI")]
    public Slider healthSlider;
    public Slider manaSlider;
    public Slider breathSlider;

    public Canvas canvasInGame;
    public Canvas canvasGameOver;

    [Header("Blood Overlay")]
    public Image bloodOverlay;

    public float indoorTime;
    [Header("Audio")]
    public float hurtSoundCooldown = 40f;

    private float hurtTimer;


    void Start()
    {
        Time.timeScale = 1f;

        currentHealth = maxHealth;
        currentMana = maxMana;
        currentBreath = maxBreath;

     
        healthSlider.maxValue = maxHealth;
        manaSlider.maxValue = maxMana;
        breathSlider.maxValue = maxBreath;

        healthSlider.value = currentHealth;
        manaSlider.value = currentMana;
        breathSlider.value = currentBreath;

       //tat canvas thua khi khoi dong
        canvasGameOver.gameObject.SetActive(false);

        //alpha blood khi bat dau
        if (bloodOverlay != null)
        {
            Color c = bloodOverlay.color;
            c.a = 0f;
            bloodOverlay.color = c;
        }
    }

    void Update()
    {
        indoorTime += Time.deltaTime;

        hurtTimer -= Time.deltaTime;

        UpdateBloodUI();
    }

    void UpdateBloodUI()
    {
        if (bloodOverlay == null) return;

        float healthPercent =
            currentHealth / maxHealth;

        // tang alpha hieu ung mau khi mau cang thap
        float targetAlpha =
            Mathf.Clamp01(1f - healthPercent) * 0.7f;

        Color c = bloodOverlay.color;

        c.a = Mathf.Lerp(
            c.a,
            targetAlpha,
            Time.deltaTime * 3f
        );

        bloodOverlay.color = c;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth
        );

        healthSlider.value = currentHealth;

        
        if (hurtTimer <= 0)
        {
            //AudioManager.instance.PlaySFX(
            //    AudioManager.instance.hurtClip
            //);

            hurtTimer = hurtSoundCooldown;
        }

    //Chet
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ReduceBreath(float point)
    {
        currentBreath -= point;

        currentBreath = Mathf.Clamp(
            currentBreath,
            0,
            maxBreath
        );

        breathSlider.value = currentBreath;
    }

   
    public void ReduceMana(float point)
    {
        currentMana -= point;

        currentMana = Mathf.Clamp(
            currentMana,
            0,
            maxMana
        );

        manaSlider.value = currentMana;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        canvasInGame.gameObject.SetActive(false);

        canvasGameOver.gameObject.SetActive(true);

        // tat player
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (camerafollow != null)
        {
            camerafollow.enabled = false;
        }

        // tat rb
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public float GetExposureMultiplier()
    {
        return 1 + (indoorTime / 180f);
    }
}