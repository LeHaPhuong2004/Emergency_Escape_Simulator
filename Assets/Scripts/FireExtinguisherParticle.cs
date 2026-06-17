using UnityEngine;

public class FireExtinguisherParticle : MonoBehaviour
{
    private Rigidbody rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

       //bat kinematic va tat dung vat ly de lua khong bi roi
        rb.isKinematic = true;
        rb.useGravity = false;

        //che do giup nhan dien va cham voi vat the kinematic
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    void Update()
    {
        //cho rb vao trang thai ngu, chỉ khi buoc vao moi trigger.
        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }
    }
}