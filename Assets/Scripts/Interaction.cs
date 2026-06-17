using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float pickUpRange = 3f;
    public Transform holdPoint;
    public float throwForce = 12f;
    public TextMeshProUGUI itemName;

    private GameObject heldObject;
    private Rigidbody heldRb;
    private Collider heldCol;
    private Collider playerCol;

    public Transform faceMaskPoint;
    private UseFireEx extinguisher;

    public bool wearingWetMask = false;

    [Header("Door UI")]
    public GameObject handModel;        
    public TextMeshProUGUI interactText;
    public TextMeshProUGUI interactText2;
    [Header("Animator")]
    public Animator handAnimator;

    private bool isCheckingDoor;

    float coughTimer;
    public GameObject HeldObject => heldObject;
    void Start()
    {
        if (interactText2 != null)
            interactText2.gameObject.SetActive(false);
        playerCol = GetComponent<Collider>();

        if (handModel != null)
            handModel.SetActive(false);

        if (interactText != null)
            interactText.gameObject.SetActive(false);

        if (handAnimator != null)
            handAnimator.SetBool("IsChecking", false);

        if (itemName != null)
            itemName.text = "";
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Q))
        {
            if (extinguisher != null && heldObject != null)
                extinguisher.Spray(true);
        }
        else
        {
            if (extinguisher != null)
                extinguisher.Spray(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (TryOpenDoor()) return;
            if (TryWashTowel()) return;
            WearTowelAsMask();
        }

      
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (heldObject == null)
                TryPickUp();
            else
                DropObject();
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (TryCheckDoor())
            {
                isCheckingDoor = true;
                return;
            }

            if (heldObject != null)
                ThrowObject();
        }

        if (Input.GetMouseButton(0))
        {
            HandleDoorCheckHold();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopDoorCheckHold();
        }

        ShowDoorHover();
        HandleCough();
        if (!Input.GetMouseButton(0))
        {
            StopDoorCheckHold();
        }
    }
    bool TryCheckDoor()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, pickUpRange))
            return false;

        OpenDoor door = hit.collider.GetComponent<OpenDoor>();
        if (door == null)
            return false;

        if (door.isHot)
            interactText.text = "The door is too hot...";
        else
            interactText.text = "Seems safe...";

        CancelInvoke(nameof(ResetDoorText));
        Invoke(nameof(ResetDoorText), 1.5f);

        return true;
    }

    void HandleDoorCheckHold()
    {
        if (!Input.GetMouseButton(0))
            return;

        if (handModel != null)
            handModel.SetActive(true);

        if (handAnimator != null)
            handAnimator.SetBool("IsChecking", true);

        if (interactText != null)
            interactText.gameObject.SetActive(true);
    }

    void StopDoorCheckHold()
    {
        if (handModel != null)
            handModel.SetActive(false);

        if (handAnimator != null)
            handAnimator.SetBool("IsChecking", false);

        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    void ResetDoorText()
    {
        if (interactText != null)
            interactText.text = "Left Click to Check Door";
    }

    void ShowDoorHover()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.GetComponent<OpenDoor>() != null)
            {
                if (interactText != null)
                {
                    interactText.gameObject.SetActive(true);

                    if (!Input.GetMouseButton(0))
                        interactText.text = "Left Click to Check Door";
                }
                return;
            }
        }

        if (!isCheckingDoor && interactText != null)
            interactText.gameObject.SetActive(false);
    }

    void TryPickUp()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, pickUpRange))
            return;

        heldObject = hit.collider.gameObject;

        heldRb = heldObject.GetComponent<Rigidbody>();
        heldCol = heldObject.GetComponent<Collider>();

        if (heldRb == null || heldCol == null)
            return;

        heldRb.useGravity = false;
        heldRb.isKinematic = true;

        if (playerCol != null)
            Physics.IgnoreCollision(heldCol, playerCol, true);

        heldObject.transform.SetParent(holdPoint);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

        extinguisher = heldObject.GetComponent<UseFireEx>();

        if (itemName != null)
            itemName.text = heldObject.name;
    }

    void ThrowObject()
    {
        if (heldObject == null) return;

        Rigidbody rb = heldRb;

        ReleaseObject();

        if (rb != null)
            rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
    }


    bool TryWashTowel()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, pickUpRange))
            return false;

        if (!hit.collider.CompareTag("sink"))
            return false;

        if (heldObject == null)
            return false;

        if (!heldObject.CompareTag("towel"))
            return false;

        heldObject.tag = "wet_towel";

        if (itemName != null)
            itemName.text = "Wet Towel";

        return true;
    }

    void WearTowelAsMask()
    {
        if (heldObject == null) return;
        if (!heldObject.CompareTag("wet_towel")) return;

        heldObject.transform.SetParent(faceMaskPoint);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

        if (heldRb != null)
        {
            heldRb.isKinematic = true;
            heldRb.useGravity = false;
        }

        if (heldCol != null)
            heldCol.enabled = false;

        heldObject = null;
        heldRb = null;
        heldCol = null;

        wearingWetMask = true;

        if (itemName != null)
            itemName.text = "Mask On";

    }

    void DropObject()
    {
        ReleaseObject();
    }

    void ReleaseObject()
    {
        if (heldObject == null) return;

        if (heldCol != null)
        {
            heldCol.enabled = true;

            if (playerCol != null)
                Physics.IgnoreCollision(heldCol, playerCol, false);
        }

        if (heldRb != null)
        {
            heldRb.useGravity = true;
            heldRb.isKinematic = false;
        }

        heldObject.transform.SetParent(null);

        heldObject = null;
        heldRb = null;
        heldCol = null;
        extinguisher = null;

        if (itemName != null)
            itemName.text = "";
    }
    bool TryOpenDoor()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, pickUpRange))
            return false;

        OpenDoor door = hit.collider.GetComponent<OpenDoor>();
        if (door == null)
            return false;

        if (door.isLocked)
        {
            interactText2.gameObject.SetActive(true);
            interactText2.text = "Door Locked";

            CancelInvoke(nameof(HideInteractText2));
            Invoke(nameof(HideInteractText2), 1.5f);

            return true;
        }

        if (door.needCrowbar && !HasCrowbar())
        {
            interactText2.gameObject.SetActive(true);
            interactText2.text = "Need Crowbar";

            CancelInvoke(nameof(HideInteractText2));
            Invoke(nameof(HideInteractText2), 1.5f);

            return true;
        }

        door.ToggleDoor();
        return true;
    }
    void HideInteractText2()
    {
        if (interactText2 != null)
            interactText2.gameObject.SetActive(false);
    }

    void HandleCough() { }

    public bool HasCrowbar()
    {
        return heldObject != null && heldObject.CompareTag("crowbar");
    }
    public bool IsHoldingObject()
    {
        return heldObject != null;
    }

    public bool IsHoldingExtinguisher()
    {
        return extinguisher != null;
    }

}