using UnityEngine;
using UnityEngine.UI;
public class PlayerStatus : MonoBehaviour
{
    public Player playerMovement; 
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

        canvasInGame.gameObject.SetActive(false);

        canvasGameOver.gameObject.SetActive(true);

        if (playerMovement != null && camerafollow != null)
        {
            playerMovement.enabled = false;
            camerafollow.enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }


}
