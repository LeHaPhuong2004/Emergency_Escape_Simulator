using UnityEngine;
using UnityEngine.UI;
public class PlayerStatus : MonoBehaviour
{
    public PlayerMovement playerMovement; 
    public CameraFollow camerafollow;
    private bool isDead = false;
    public Transform cameraHolder; 
    public float maxHealth, maxMana, maxBreath = 200f;
    public float currentHealth, currentMana, currentBreath;
    public Slider healthSlider;
    public Slider manaSlider;
    public Slider breathSlider;
    public Canvas canvasInGame;
    public Canvas canvasGameOver;
    void Start()
    {
        Time.timeScale = 1f;
        currentHealth = maxHealth;
        currentBreath = maxBreath;
        currentMana = maxMana;
        manaSlider.maxValue = maxMana;
        breathSlider.maxValue = maxBreath;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        manaSlider.value = currentMana;
        breathSlider.value = currentBreath;
        canvasGameOver.gameObject.SetActive(false);
       
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ReduceBreath(float point)
    {
        currentBreath -= point;
        currentBreath = Mathf.Clamp(currentBreath, 0, maxBreath);
        breathSlider.value = currentBreath;
    }

    public void ReduceMana(float point)
    {
        currentMana -= point;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        manaSlider.value = currentMana;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // 1. Tắt UI game
        canvasInGame.gameObject.SetActive(false);

        // 2. Bật UI Game Over
        canvasGameOver.gameObject.SetActive(true);

        // 3. Tắt điều khiển player
        if (playerMovement != null && camerafollow != null)
        {
            playerMovement.enabled = false;
            camerafollow.enabled = false;
        }

        // 4. Dừng vật lý (tránh trượt)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // 5. Mở chuột để bấm UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

  
    }


}
