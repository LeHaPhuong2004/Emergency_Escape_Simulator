using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager instance;

    public enum Phase
    {
        Light,
        Medium,
        Heavy
    }

    public Phase currentPhase;

    public float gameTime;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!IntroLock.introFinished) return;
        gameTime += Time.deltaTime;

        if (gameTime < 60)
        {
            currentPhase = Phase.Light;
        }
        else if (gameTime < 120)
        {
            currentPhase = Phase.Medium;
        }
        else
        {
            currentPhase = Phase.Heavy;
        }
    }

    public float GetPhaseMultiplier()
    {
        switch (currentPhase)
        {
            case Phase.Light:
                return 1f;

            case Phase.Medium:
                return 1.5f;

            case Phase.Heavy:
                return 2.2f;
        }

        return 1f;
    }
}