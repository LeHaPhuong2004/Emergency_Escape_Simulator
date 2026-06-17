using UnityEngine;

public class SmokeEffect : MonoBehaviour
{
    public float oxygenReduce = 10f;
    public float healthReduceWhenNoBreath = 5f;

    void OnTriggerStay(Collider other)
    {
        if (!IntroLock.introFinished) return;
        if (!other.CompareTag("Player")) return;

        PlayerStatus status = other.GetComponent<PlayerStatus>();
        Player player = other.GetComponent<Player>();

        if (status == null || player == null) return;

        bool isCrouch = player.isCrouching;

        float crouchMultiplier = isCrouch ? 0.2f : 1f;

        float phaseMultiplier = PhaseManager.instance.GetPhaseMultiplier();

        float exposureMultiplier = status.GetExposureMultiplier();

        Interaction interaction =
    other.GetComponent<Interaction>();

        float maskMultiplier = 1f;

        if (interaction != null &&
            interaction.wearingWetMask)
        {
            maskMultiplier = 0.3f;
        }

        float oxygenDamage =
            oxygenReduce *
            crouchMultiplier *
            phaseMultiplier *
            exposureMultiplier *
            maskMultiplier *
            Time.deltaTime;

        float healthDamage =
            healthReduceWhenNoBreath *
            crouchMultiplier *
            phaseMultiplier *
            exposureMultiplier *
            maskMultiplier *
            Time.deltaTime;

        if (status.currentBreath > 0)
        {
            status.ReduceBreath(oxygenDamage);
        }
        else
        {
            status.TakeDamage(healthDamage);
        }
    }
}