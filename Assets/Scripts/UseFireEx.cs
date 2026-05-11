using UnityEngine;

public class UseFireEx : MonoBehaviour
{
    public ParticleSystem spray;
    public Transform output;
    public Collider sprayHitbox; // Cái BoxCollider ở mũi vòi
    public float damagePerSecond = 50f;

    private bool isCurrentlySpraying = false; // Biến kiểm soát trạng thái nội bộ

    void Start()
    {
        // Quan trọng: Tắt hitbox ngay khi game bắt đầu
        if (sprayHitbox != null)
            sprayHitbox.enabled = false;

        if (spray != null)
            spray.Stop();
    }

    public void Spray(bool isSpraying)
    {
        if (!spray) return;

        // Lưu trạng thái để dùng trong OnTriggerStay
        isCurrentlySpraying = isSpraying;

        spray.transform.position = output.position;
        spray.transform.rotation = output.rotation;

        if (isSpraying)
        {
            if (!spray.isPlaying) spray.Play();
            if (sprayHitbox != null) sprayHitbox.enabled = true; // Chỉ bật khi ấn Q
        }
        else
        {
            if (spray.isPlaying) spray.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            if (sprayHitbox != null) sprayHitbox.enabled = false; // Tắt ngay khi nhả Q
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Thêm một lớp bảo vệ: Chỉ trừ máu nếu đang thực sự xịt
        if (!isCurrentlySpraying) return;

        FireHealth fire = other.GetComponent<FireHealth>();
        if (fire != null)
        {
            fire.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}