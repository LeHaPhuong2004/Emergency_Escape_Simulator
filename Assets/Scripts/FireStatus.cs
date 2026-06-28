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
            fireSlider.maxValue = fireHP;
            fireSlider.value = fireHP;
        }
    }

    public void TakeDamage(float dmg)
    {
       
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
       
        Destroy(gameObject);
    }
}