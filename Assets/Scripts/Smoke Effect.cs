using UnityEngine;

public class SmokeEffect : MonoBehaviour
{
    public float oxygenReduce = 10f;
    public float healthReduceWhenNoBreath = 5f;
   
   
    void OnTriggerStay(Collider other)
    {
        bool isCrouch = Input.GetKey(KeyCode.LeftShift);
        if (other.CompareTag("Player"))
        {
           
            PlayerStatus status = other.GetComponent<PlayerStatus>();
            if (status != null)
            {
                if (isCrouch)
                {
                    return;
                }
                 if(status.currentBreath > 0) { status.ReduceBreath(oxygenReduce * Time.deltaTime);  }  
                   
                    
                else
                {
                    status.TakeDamage(healthReduceWhenNoBreath * Time.deltaTime);
                }
              
            }
        }
    }
}