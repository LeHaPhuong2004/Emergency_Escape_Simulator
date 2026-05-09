using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float pickUpRange = 3f;
    public Transform holdPoint;
    public float throwForce = 12f;

    private GameObject heldObject;
    private Rigidbody heldRb;
    private Collider heldCol;
    private Collider playerCol;

    void Start()
    {
        playerCol = GetComponent<Collider>();
    }

    void Update()
    {
  
        //Debug.DrawRay(Camera.main.transform.position,
        //              Camera.main.transform.forward * pickUpRange,
        //              Color.red);

        if (Input.GetKeyDown(KeyCode.E))
        {
 
            if (TryOpenDoor()) return;

            if (heldObject == null)
                TryPickUp();
            else
                DropObject();
        }

        if (Input.GetMouseButtonDown(0) && heldObject != null)
        {
            ThrowObject();
        }
    }

    void TryPickUp()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("Pickup"))
            {
                heldObject = hit.collider.gameObject;
                heldRb = heldObject.GetComponent<Rigidbody>();
                heldCol = heldObject.GetComponent<Collider>();

                heldRb.useGravity = false;
                heldRb.isKinematic = true;

                if (playerCol != null)
                    Physics.IgnoreCollision(heldCol, playerCol, true);

       
                heldCol.enabled = false;

                heldObject.transform.SetParent(holdPoint);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void DropObject()
    {
        ReleaseObject();
    }

    void ThrowObject()
    {
        ReleaseObject();

        heldRb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

        heldObject = null;
    }

    void ReleaseObject()
    {
        heldCol.enabled = true;

        if (playerCol != null)
            Physics.IgnoreCollision(heldCol, playerCol, false);

        heldRb.useGravity = true;
        heldRb.isKinematic = false;

        heldObject.transform.SetParent(null);
    }

    bool TryOpenDoor()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            OpenDoor door = hit.collider.GetComponent<OpenDoor>();
            if (door != null)
            {
                door.ToggleDoor();
                Debug.Log("Opened Door");
                return true;
            }
        }

        return false;
    }
}