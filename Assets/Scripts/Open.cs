using UnityEngine;

public class Open : MonoBehaviour
{
    public enum OpenType { Rotate, Slide }

    [Header("Type")]
    public OpenType openType;

    [Header("Settings")]
    public float speed = 3f;
    public bool isOpen = false;

    [Header("Rotate")]
    public Vector3 rotationAmount; // ví dụ: (0, 90, 0)

    [Header("Slide")]
    public Vector3 moveDirection; // ví dụ: (1, 0, 0)
    public float moveDistance = 0.5f;

    private Vector3 startPos;
    private Quaternion startRot;

    private Vector3 targetPos;
    private Quaternion targetRot;

    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;

        targetPos = startPos + moveDirection.normalized * moveDistance;
        targetRot = startRot * Quaternion.Euler(rotationAmount);
    }

    void Update()
    {
        if (openType == OpenType.Rotate)
        {
            Quaternion target = isOpen ? targetRot : startRot;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, target, speed * Time.deltaTime);
        }
        else if (openType == OpenType.Slide)
        {
            Vector3 target = isOpen ? targetPos : startPos;
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, speed * Time.deltaTime);
        }
    }

    public void Toggle()
    {
        isOpen = !isOpen;
    }
}
