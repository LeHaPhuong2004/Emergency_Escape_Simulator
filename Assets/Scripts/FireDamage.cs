using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public Transform fireCenter;

    public float maxDistance = 6f;

    public float maxDamage = 40f;

    void OnTriggerStay(Collider other)
    {
        if (!IntroLock.introFinished) return;
        if (other.CompareTag("Player"))
        {
            PlayerStatus health =
                other.GetComponent<PlayerStatus>();

            if (health != null)
            {
                float distance =
                    Vector3.Distance(
                        other.transform.position,
                        fireCenter.position
                    );

                float intensity =
                    1 - (distance / maxDistance);

                intensity = Mathf.Clamp01(intensity);

                intensity = Mathf.Pow(intensity, 4);

                float damage =
                    maxDamage *
                    intensity *
                    Time.deltaTime;

                health.TakeDamage(damage);
            }
        }
    }
}