using UnityEngine;

public class SmokeEffect : MonoBehaviour
{
    public float oxygenReduce = 10f; 

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            PlayerStatus status = other.GetComponent<PlayerStatus>();
            if (status != null)
            {
                //status.ReduceOxygen(oxygenDrainRate * Time.deltaTime);
            }
        }
    }
}