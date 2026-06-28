using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;

    [Header("References")]
    public Interaction playerInteraction;
    public OpenDoor tutorialDoor;
    public GameObject canvasDone;

    private int step;

    private bool w;
    private bool a;
    private bool s;
    private bool d;
    public bool isExtinguished;

    public FireHealth tutorialFire;

    public Player controller;
    public Interaction interaction;
    public CameraFollow cameraFollow;
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
                CheckDoorCheck();
                break;

            case 8:
                CheckDoor();
                break;

            case 9:
                CheckWearMask();
                break;
        }
    }
    void CheckDoorCheck()
    {
        if (playerInteraction != null &&
            playerInteraction.checkedDoor)
        {
            NextStep();
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
    private int runPressCount;
    void CheckRun()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            runPressCount++;

            if (runPressCount >= 2)
            {
                runPressCount = 0;
                NextStep();
            }
        }
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
        if (tutorialFire == null)
        {
            NextStep();
            return;
        }

        if (tutorialFire.fireHP <= 0)
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
                tutorialText.text =
                    T("Press WASD to Move",
                      "Nhấn WASD để di chuyển");
                break;

            case 1:
                tutorialText.text =
                    T("Double Press W to Run",
                      "Nhấn W hai lần để chạy");
                break;

            case 2:
                tutorialText.text =
                    T("Press Left Shift to Crouch",
                      "Nhấn Shift trái để cúi người");
                break;

            case 3:
                tutorialText.text =
                    T("Press Space to Jump",
                      "Nhấn Space để nhảy");
                break;

            case 4:
                tutorialText.text =
                    T("Press F to Pick Up Fire Extinguisher",
                      "Nhấn F để nhặt bình chữa cháy");
                break;

            case 5:
                tutorialText.text =
                    T("Hold Q to Extinguish Fire",
                      "Giữ Q để phun chữa cháy");
                break;

            case 6:
                tutorialText.text =
                    T("Press Left Mouse Button to Throw Item",
                      "Nhấn chuột trái để ném vật phẩm");
                break;

            case 7:
                tutorialText.text =
                    T("Hold Left Mouse Button to Check the Door",
                      "Giữ chuột trái để kiểm tra cửa");
                break;

            case 8:
                tutorialText.text =
                    T("Press E to Open the Door and Go to the Bathroom",
                      "Nhấn E để mở cửa và đi vào phòng tắm");
                break;

            case 9:
                tutorialText.text =
                    T("Press E to Wet the Towel and Press E Again to Wear It",
                      "Nhấn E để làm ướt khăn rồi nhấn E lần nữa để đeo");
                break;

            case 10:
                tutorialText.gameObject.SetActive(false);
                Invoke(nameof(ShowDonePanel), 1f); // hoặc 2f
                break;
        }
    }
    void ShowDonePanel()
    {
        controller.enabled = false;
        interaction.enabled = false;
        cameraFollow.enabled = false;

        canvasDone.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    string T(string en, string vi)
    {
        return LanguageManager.Instance.CurrentLanguage == 0
            ? en
            : vi;
    }
}