using HutongGames.PlayMaker.Actions;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BreakWall : MonoBehaviour
{
    public Rigidbody[] pieces;

    bool hasBroken = false;

    public float strength = 50f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBroken)
        {
            hasBroken = true;

            foreach (var rb in pieces)
            {
                rb.isKinematic = false;

                rb.AddExplosionForce(
                    strength,
                    transform.position,
                    5f
                );
            }

            //Invoke(nameof(DestroyAll), 3f);
        }
    }

    void DestroyAll()
    {
        Destroy(transform.parent.gameObject);
    }
}