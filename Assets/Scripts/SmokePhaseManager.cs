using UnityEngine;

public class SmokePhaseController : MonoBehaviour
{
    public GameObject lightSmoke;
    public GameObject mediumSmoke;
    public GameObject heavySmoke;

    void Update()
    {
        var phase =
            PhaseManager.instance.currentPhase;

        lightSmoke.SetActive(
            phase == PhaseManager.Phase.Light);

        mediumSmoke.SetActive(
            phase == PhaseManager.Phase.Medium);

        heavySmoke.SetActive(
            phase == PhaseManager.Phase.Heavy);
    }
}