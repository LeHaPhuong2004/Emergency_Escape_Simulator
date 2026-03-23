using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public float damagePerSecond = 5f; 

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus health = other.GetComponent<PlayerStatus>();
            if (health != null)
            {
                health.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
