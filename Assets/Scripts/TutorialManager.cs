using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;

    [Header("References")]
    public Interaction playerInteraction;
    public OpenDoor tutorialDoor;

    private int step;

    private bool w;
    private bool a;
    private bool s;
    private bool d;

    void Start()
    {
        ShowStep();
    }

    void Update()
    {
        switch (step)
        {
            case 0:
                CheckMovement();
                break;

            case 1:
                CheckRun();
                break;

            case 2:
                CheckCrouch();
                break;

            case 3:
                CheckJump();
                break;

            case 4:
                CheckPickup();
                break;

            case 5:
                CheckSpray();
                break;

            case 6:
                CheckThrow();
                break;

            case 7:
                CheckDoor();
                break;

            case 8:
                CheckWearMask();
                break;
        }
    }

    void CheckMovement()
    {
        if (Input.GetKeyDown(KeyCode.W)) w = true;
        if (Input.GetKeyDown(KeyCode.A)) a = true;
        if (Input.GetKeyDown(KeyCode.S)) s = true;
        if (Input.GetKeyDown(KeyCode.D)) d = true;

        if (w && a && s && d)
            NextStep();
    }

    void CheckRun()
    {
        if (Input.GetKeyDown(KeyCode.W))
            NextStep();
    }

    void CheckCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            NextStep();
    }

    void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            NextStep();
    }

    void CheckPickup()
    {
        if (playerInteraction == null)
            return;

        if (playerInteraction.GetComponentInChildren<UseFireEx>() != null)
            NextStep();
    }

    void CheckSpray()
    {
        if (playerInteraction != null &&
            playerInteraction.IsHoldingExtinguisher() &&
            Input.GetKey(KeyCode.Q))
        {
            NextStep();
        }
    }

    void CheckThrow()
    {
        if (!playerInteraction.IsHoldingObject())
        {
            NextStep();
        }
    }

    void CheckDoor()
    {
        if (tutorialDoor == null)
            return;

        if (tutorialDoor.isOpen)
            NextStep();
    }

    void CheckWearMask()
    {
        if (playerInteraction != null &&
            playerInteraction.wearingWetMask)
        {
            NextStep();
        }
    }

    void NextStep()
    {
        step++;
        ShowStep();
    }

    void ShowStep()
    {
        switch (step)
        {
            case 0:
                tutorialText.text = "Press WASD to Move";
                break;

            case 1:
                tutorialText.text = "Double Press W to Run";
                break;

            case 2:
                tutorialText.text = "Press Left Shift to Crouch";
                break;

            case 3:
                tutorialText.text = "Press Space to Jump";
                break;

            case 4:
                tutorialText.text = "Press F to Pick Up Fire Extinguisher";
                break;

            case 5:
                tutorialText.text = "Hold Q to Extinguish Fire";
                break;

            case 6:
                tutorialText.text = "Press Left Mouse Button to Throw Item";
                break;

            case 7:
                tutorialText.text = "Press E to Open the Door and Go to the Bathroom";
                break;

            case 8:
                tutorialText.text = "Press E to Wet the Towel and Press E Again to Wear It";
                break;

            case 9:
                tutorialText.text = "Tutorial Complete!";
                break;
        }
    }
}