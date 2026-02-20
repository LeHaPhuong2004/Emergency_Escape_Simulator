using UnityEngine;


public class OutlineSelected : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 3f;

    private Transform currentHover;
    private Outline currentOutline;
    private OpenDoor currentDoor;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Reset old hover
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
            currentHover = null;
            currentDoor = null;
        }

        // Raycast
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            Transform target = hit.transform;

            if (!target.CompareTag("SelectedObject"))
                return;

            currentHover = target;

            // Outline (KHÔNG add runtime)
            Outline outline = target.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = true;
                outline.OutlineColor = Color.cyan;
                outline.OutlineWidth = 6f;
                currentOutline = outline;
            }

            // Door
            OpenDoor door = target.GetComponent<OpenDoor>();
            if (door != null)
            {
                currentDoor = door;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    door.ToggleDoor();
                }
            }
        }
    }
}
