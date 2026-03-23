using UnityEngine;

public class FireSpread : MonoBehaviour
{
    public GameObject firePrefab;
    public BoxCollider fireArea;
    public string fireTag = "fire"; 
    public LayerMask groundLayer;   

    public float spreadDelay = 4f;
    public float spreadInterval = 6f;
    public float spreadChance = 0.5f;
    public int maxFireCount = 40;

    public static int currentFireCount = 0;

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
        if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, fireArea.bounds.size.y + 5f, groundLayer))
        {        
            GameObject newFire = Instantiate(firePrefab, hit.point, Quaternion.identity);

            newFire.tag = fireTag;
            newFire.transform.up = hit.normal;

            currentFireCount++;
        }
    }

    Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.max.y + 1f,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    void OnDestroy()
    {
        currentFireCount--;
    }
}