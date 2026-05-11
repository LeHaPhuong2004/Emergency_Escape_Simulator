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

    private UseFireEx extinguisher;
    

    void Start()
    {
        playerCol = GetComponent<Collider>();
    }

    void Update()
    {
       
        // ================= DEBUG Q INPUT =================
        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("Q PRESSED");

            Debug.Log("heldObject = " + heldObject);
            Debug.Log("extinguisher = " + extinguisher);
        }

        // ================= INTERACT =================
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (TryOpenDoor()) return;

            
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (heldObject == null)
                TryPickUp();
            else
                DropObject();
        }

        // ================= THROW =================
        if (Input.GetMouseButtonDown(0) && heldObject != null)
        {
            ThrowObject();
        }

        // ================= FIRE =================
        if (extinguisher && heldObject)
        {
            Debug.Log("CALL SPRAY()");
            extinguisher.Spray(Input.GetKey(KeyCode.Q));
        }
    }

    void TryPickUp()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, pickUpRange))
        {
            Debug.Log("NO HIT");
            return;
        }

        if (
            !hit.collider.CompareTag("Pickup") &&
            !hit.collider.CompareTag("fire_ex") &&
            !hit.collider.CompareTag("towel") &&
            !hit.collider.CompareTag("wet_towel")
        )
        {
            Debug.Log("HIT BUT NOT PICKUP: " + hit.collider.name);
            return;
        }

        heldObject = hit.collider.gameObject;
        Debug.Log("PICKED: " + heldObject.name);
        if (hit.collider.CompareTag("fire_ex"))
        {
            itemName.text = "Fire Extinguisher";
        }
        else if (hit.collider.CompareTag("towel"))
        {
            itemName.text = "Towel";
        }
        //else if (hit.collider.CompareTag("wet_towel"))
        //{
        //    itemName.text = "Wet towel";
        //}

        heldRb = heldObject.GetComponent<Rigidbody>();
        heldCol = heldObject.GetComponent<Collider>();



        if (heldRb == null || heldCol == null)
        {
            Debug.Log("MISSING RB OR COL");
            return;
        }

        heldRb.useGravity = false;
        heldRb.isKinematic = true;

        if (playerCol != null)
            Physics.IgnoreCollision(heldCol, playerCol, true);

        heldObject.transform.SetParent(holdPoint);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

        // ===== DEBUG EXTINGUISHER =====
        extinguisher = heldObject.GetComponent<UseFireEx>();

        Debug.Log("UseFireEx FOUND = " + extinguisher);
    }

    void DropObject()
    {
        ReleaseObject();
        heldObject = null;
        itemName.text = "";
    }

    void ThrowObject()
    {
        GameObject obj = heldObject;
        Rigidbody rb = heldRb;

        ReleaseObject();

        if (rb != null)
        {
            rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
        }

        heldObject = null;
    }

    void ReleaseObject()
    {
        itemName.text = "";
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

        extinguisher = null;
        heldObject = null;
        heldRb = null;
        heldCol = null;
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

        door.ToggleDoor();
        Debug.Log("Opened Door: " + hit.collider.name);
        return true;
    }

    void MakeWetTowel()
    {
        itemName.text = "Wet Towel";
    }
}