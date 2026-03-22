using UnityEngine;

public class SmokeEffect : MonoBehaviour
{
    public float oxygenReduce = 10f; // Trừ 10 oxy mỗi giây

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Giả sử bạn có biến Oxygen trong PlayerStatus
            PlayerStatus status = other.GetComponent<PlayerStatus>();
            if (status != null)
            {
                //status.ReduceOxygen(oxygenDrainRate * Time.deltaTime);
            }
        }
    }
}