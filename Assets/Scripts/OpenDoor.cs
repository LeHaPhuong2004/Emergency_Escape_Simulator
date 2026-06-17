using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [Header("Door")]
    public float openAngle = 90f;
    public float openSpeed = 2f;

    [Header("Explosion")]
    public bool isExplosive = false;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public int damage = 80;

    public GameObject explosionEffect;

    [Header("Camera Shake")]
    public Shake cameraShake;

    public bool isOpen = false;
    private bool exploded = false;

    private Quaternion closedRot;
    private Quaternion openRot;
    public bool isHot = false;
    public bool isLocked = false;
    public bool needCrowbar;
    void Start()
    {
        closedRot = transform.rotation;

        openRot = Quaternion.Euler(
            transform.eulerAngles + Vector3.up * openAngle
        );
    }

    public void ToggleDoor()
    {
        //neu cua la cua no thì ch no
        if (isExplosive && !exploded)
        {
            ExplodeDoor();
            return;
        }

        //cua thuong
        isOpen = !isOpen;
    }

    void Update()
    {
        if (exploded) return;

        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                openRot,
                Time.deltaTime * openSpeed
            );
        }
        else
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                closedRot,
                Time.deltaTime * openSpeed
            );
        }
    }

    void ExplodeDoor()
    {
        exploded = true;

        Debug.Log("DOOR EXPLODED"); 
        AudioManager.instance.PlaySFX(AudioManager.instance.explosionClip);
        //bat particle no
        if (explosionEffect != null)
        {
            GameObject fx = Instantiate(
                explosionEffect,
                transform.position + Vector3.up * 1.5f,
                Quaternion.identity
            );

            Destroy(fx, 5f);
        }

        //bat am thanh no
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(
                AudioManager.instance.explosionClip
            );
        }

        if (cameraShake != null)
        {
            cameraShake.TriggerShake();
        }

        //gay st len nguoi choi
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            explosionRadius
        );

        foreach (Collider hit in hits)
        {
            //day nguoi choi
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(
                    explosionForce,
                    transform.position,
                    explosionRadius
                );
            }

            //gay st len nguoi choi khi no
            PlayerStatus player = hit.GetComponent<PlayerStatus>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
            //pha cua khi no
        Destroy(gameObject);
    }
}