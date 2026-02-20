using UnityEngine;
using UnityEngine.UI;
public class PlayerStatus : MonoBehaviour
{
    public float maxHealth = 200f;
    public float currentHealth;

    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
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

    void Die()
    {
        Debug.Log("Player chết!");
        // sau này bạn có thể làm game over, respawn, reload scene...
    }
}
