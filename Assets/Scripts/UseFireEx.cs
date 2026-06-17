using UnityEngine;

public class UseFireEx : MonoBehaviour
{
    public ParticleSystem spray;
    public Transform output;
    public Collider sprayHitbox; // boxcollider mui voi
    public float damagePerSecond = 50f;

    private bool isCurrentlySpraying = false; //bien kiem soat trang thai noi bo
    void Start()
    {
        //tam tat hitbox
        if (sprayHitbox != null)
            sprayHitbox.enabled = false;

        if (spray != null)
            spray.Stop();
    }

    public void Spray(bool isSpraying)
    {
        if (!spray) return;

        //luu trang thai 
        isCurrentlySpraying = isSpraying;

        spray.transform.position = output.position;
        spray.transform.rotation = output.rotation;

        if (isSpraying)
        {
            if (!spray.isPlaying) spray.Play();
            if (sprayHitbox != null) sprayHitbox.enabled = true; //chi bat khi an Q
        }
        else
        {
            if (spray.isPlaying) spray.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            if (sprayHitbox != null) sprayHitbox.enabled = false; // tat ngay khi khong can Q
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //chi tru mau khi xit
        if (!isCurrentlySpraying) return;

        FireHealth fire = other.GetComponent<FireHealth>();
        if (fire != null)
        {
            fire.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}