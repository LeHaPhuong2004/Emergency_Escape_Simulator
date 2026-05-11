using UnityEngine;

public class TriggerBreak : MonoBehaviour
{
    public BreakWall bw;
    public bool isEnter = false;

    void Update()
    {
        if (!isEnter)
        {
            isEnter = true;

           
        }
    }
}