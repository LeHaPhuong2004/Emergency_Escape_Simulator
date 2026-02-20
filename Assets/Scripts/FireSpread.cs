using UnityEngine;

public class FireSpread : MonoBehaviour
{
    public GameObject firePrefab;
    public BoxCollider fireArea;

    public float spreadDelay = 4f;
    public float spreadInterval = 6f;   // thời gian lan tiếp
    public float spreadChance = 0.5f;
    public int maxFireCount = 40;

    private static int currentFireCount = 0;

    void Start()
    {
        currentFireCount++;
        InvokeRepeating(nameof(SpreadFire), spreadDelay, spreadInterval);
    }

    void SpreadFire()
    {
        if (currentFireCount >= maxFireCount) return;
        if (Random.value > spreadChance) return;

        Vector3 randomPos = GetRandomPointInBounds(fireArea.bounds);

        // bắn ray xuống để tìm mặt đất
        if (Physics.Raycast(randomPos + Vector3.up * 3f, Vector3.down, out RaycastHit hit, 10f))
        {
            Instantiate(firePrefab, hit.point, Quaternion.identity);
        }
    }

    Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.max.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    void OnDestroy()
    {
        currentFireCount--;
    }
}