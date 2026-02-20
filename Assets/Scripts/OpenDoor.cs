using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(transform.eulerAngles + Vector3.up * openAngle);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    void Update()
    {
        if (isOpen)
            transform.rotation = Quaternion.Slerp(transform.rotation, openRot, Time.deltaTime * openSpeed);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, closedRot, Time.deltaTime * openSpeed);
    }
}
