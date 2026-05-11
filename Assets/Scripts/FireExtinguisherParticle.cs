using UnityEngine;

public class FireExtinguisherParticle : MonoBehaviour
{
    private Rigidbody rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Cấu hình Rigidbody để tối ưu cho việc nhận va chạm Trigger
        rb.isKinematic = true;
        rb.useGravity = false;

        // Chế độ này giúp nhận diện va chạm với các vật Kinematic khác (như vòi xịt) tốt hơn
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    void Update()
    {
        // Ép Rigidbody không bao giờ rơi vào trạng thái "ngủ" (Sleep)
        // Đây là lý do tại sao trước đó bạn phải bước vào mới nhận va chạm
        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }
    }
}