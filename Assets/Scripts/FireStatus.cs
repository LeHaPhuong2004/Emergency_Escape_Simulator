using UnityEngine;
using UnityEngine.UI;

public class FireHealth : MonoBehaviour
{
    public float fireHP = 200f;

    public Slider fireSlider;

    void Start()
    {
        if (fireSlider != null)
        {
            fireSlider.maxValue = fireHP; // Tự động khớp với 200
            fireSlider.value = fireHP;
        }
    }

    public void TakeDamage(float dmg)
    {
        Debug.Log("Lửa đang nhận sát thương: " + dmg + " | Máu còn lại: " + fireHP);
        fireHP -= dmg;
        fireHP = Mathf.Clamp(fireHP, 0, 200);

        if (fireSlider != null)
            fireSlider.value = fireHP;

        if (fireHP <= 0)
        {
            Extinguish();
        }
    }

    void Extinguish()
    {
        Debug.Log("Fire extinguished!");
        Destroy(gameObject);
    }
}