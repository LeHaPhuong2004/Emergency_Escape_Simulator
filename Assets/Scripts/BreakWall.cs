using UnityEngine;

public class BreakWall : MonoBehaviour
{
    public Rigidbody[] pieces;

    bool hasBroken = false;

    public float strength = 50f;
    public float radius = 5f;

    public Shake cameraShake;
    public GameObject explosionEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBroken) return;

        //nhan dien nguoi choi
        if (other.CompareTag("Player"))
        {
            Break();
            return;
        }

        // chi can player co rb la break
        if (other.attachedRigidbody != null)
        {
            Break();
        }
    }

    void Break()
    {
        hasBroken = true;

        // rung cam
        if (cameraShake != null)
        {
            cameraShake.TriggerIntroExplosion();
        }

        //hieu ung no
        if (explosionEffect != null)
        {
            GameObject fx = Instantiate(
                explosionEffect,
                transform.position + Vector3.up,
                Quaternion.identity
            );

            Destroy(fx, 5f);
        }

       //pha tuong
        foreach (var rb in pieces)
        {
            rb.isKinematic = false;

            rb.AddExplosionForce(
                strength,
                transform.position,
                radius
            );
        }

        // Invoke(nameof(DestroyAll), 3f);
    }

}