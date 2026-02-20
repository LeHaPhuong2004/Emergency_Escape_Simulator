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
        // debug tia ray
        Debug.DrawRay(Camera.main.transform.position,
                      Camera.main.transform.forward * pickUpRange,
                      Color.red);

        if (Input.GetKeyDown(KeyCode.E))
        {
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

                // TẮT PHYSICS khi cầm
                heldRb.useGravity = false;
                heldRb.isKinematic = true;

                // tránh đẩy player
                if (playerCol != null)
                    Physics.IgnoreCollision(heldCol, playerCol, true);

                // (ổn định nhất) tắt collider khi đang cầm
                heldCol.enabled = false;

                // gắn vào điểm giữ
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

        // thêm lực ném
        heldRb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

        heldObject = null;
    }

    void ReleaseObject()
    {
        // bật lại collider
        heldCol.enabled = true;

        // bật lại va chạm với player
        if (playerCol != null)
            Physics.IgnoreCollision(heldCol, playerCol, false);

        // bật lại physics
        heldRb.useGravity = true;
        heldRb.isKinematic = false;

        heldObject.transform.SetParent(null);
    }
}