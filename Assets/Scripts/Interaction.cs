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
    public bool checkedDoor = false;
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

        //ShowDoorHover();
        HandleCough();
        if (!Input.GetMouseButton(0))
        {
            StopDoorCheckHold();
        }
    }
    string T(string en, string vi)
    {
        return LanguageManager.Instance.CurrentLanguage == 0 ? en : vi;
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
            interactText.text = T("The door is too hot...", "Cửa quá nóng...");
        else
            interactText.text = T("Seems safe...", "Có vẻ an toàn...");
        checkedDoor = true;
        CancelInvoke(nameof(HideDoorText));
        Invoke(nameof(HideDoorText), 1.5f);

        return true;
    }
    void HideDoorText()
    {
        if (interactText != null)
            interactText.gameObject.SetActive(false);
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
    string GetItemName(GameObject obj)
    {
        if (obj == null) return "";

        bool isEN = LanguageManager.Instance.CurrentLanguage == 0;

        switch (obj.tag)
        {
            case "fire_ex":
                return isEN ? "Fire Extinguisher" : "Bình chữa cháy";

            case "towel":
                return isEN ? "Towel" : "Khăn";

            case "wet_towel":
                return isEN ? "Wet Towel" : "Khăn ướt";

            case "crowbar":
                return isEN ? "Crowbar" : "Xà beng";
        }

        return obj.name;
    }
    void StopDoorCheckHold()
    {
        if (handModel != null)
            handModel.SetActive(false);

        if (handAnimator != null)
            handAnimator.SetBool("IsChecking", false);

        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
            ResetDoorText();     
        }
    }

    void ResetDoorText()
    {
        interactText.text = "";
        //T("Left Click to Check Door", "Chuột trái để kiểm tra cửa");
    }

    void ShowDoorHover()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.GetComponent<OpenDoor>() != null)
            {
                interactText.gameObject.SetActive(true);
                interactText.text = T("Left Click to Check Door",
                                      "Chuột trái để kiểm tra cửa");
                return;
            }
        }

        interactText.gameObject.SetActive(false);
    }

    bool TryPickUp()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, pickUpRange))
            return false;

        GameObject target = hit.collider.gameObject;

        if (!target.CompareTag("fire_ex") &&
            !target.CompareTag("towel") &&
            !target.CompareTag("crowbar") &&
            !target.CompareTag("wet_towel"))
        {
            return false;
        }

        heldObject = target;

        heldRb = heldObject.GetComponent<Rigidbody>();
        heldCol = heldObject.GetComponent<Collider>();

        if (heldRb == null || heldCol == null)
            return false;

        heldRb.useGravity = false;
        heldRb.isKinematic = true;

        if (playerCol != null)
            Physics.IgnoreCollision(heldCol, playerCol, true);

        heldObject.transform.SetParent(holdPoint);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

        extinguisher = heldObject.GetComponent<UseFireEx>();

        if (itemName != null)
            itemName.text = GetItemName(heldObject);

        return true;
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
            itemName.text = GetItemName(heldObject);

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

        wearingWetMask = true;

        // 👉 lấy tên TRƯỚC khi null
        if (itemName != null)
            itemName.text = GetItemName(heldObject);

        heldObject = null;
        heldRb = null;
        heldCol = null;
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
            interactText2.text = T("Door Locked", "Cửa bị khóa");

            CancelInvoke(nameof(HideInteractText2));
            Invoke(nameof(HideInteractText2), 1.5f);

            return true;
        }

        if (door.needCrowbar)
        {
            if (!HasCrowbar())
            {
                interactText2.gameObject.SetActive(true);
                interactText2.text = T("Need Crowbar", "Cần xà beng");

                CancelInvoke(nameof(HideInteractText2));
                Invoke(nameof(HideInteractText2), 1.5f);

                return true;
            }

            Animator crowbarAnimator = heldObject.GetComponent<Animator>();

            if (crowbarAnimator != null)
                crowbarAnimator.SetTrigger("UseCrowbar");

            door.ToggleDoor();
            return true;
        }

        // cửa thường
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