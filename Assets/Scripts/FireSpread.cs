using UnityEngine;

public class FireSpread : MonoBehaviour
{
    public GameObject firePrefab;
    public BoxCollider fireArea;
    public string fireTag = "fire"; // Tag để tính dame
    public LayerMask groundLayer;   // Chỉ định Layer nào là mặt đất/tường (tránh bắn vào mái nhà)

    public float spreadDelay = 4f;
    public float spreadInterval = 6f;
    public float spreadChance = 0.5f;
    public int maxFireCount = 40;

    public static int currentFireCount = 0;

    void Start()
    {
        currentFireCount++;
        // Kiểm tra nếu chưa có Tag "Fire" trong Project, bạn phải tạo nó trước trong Editor
        InvokeRepeating(nameof(SpreadFire), spreadDelay, spreadInterval);
    }

    void SpreadFire()
    {
        if (currentFireCount >= maxFireCount) return;
        if (Random.value > spreadChance) return;

        // Lấy vị trí ngẫu nhiên trong vùng Area
        Vector3 randomPos = GetRandomPointInBounds(fireArea.bounds);

        // Bắn Raycast từ TRÊN CAO xuống nhưng chỉ va chạm với groundLayer
        // Điều này giúp lửa xuyên qua mái nhà (nếu mái nhà không thuộc groundLayer)
        if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, fireArea.bounds.size.y + 5f, groundLayer))
        {
            // 1. Sinh lửa tại điểm va chạm
            GameObject newFire = Instantiate(firePrefab, hit.point, Quaternion.identity);

            // 2. Gán Tag và Layer bằng code để đảm bảo luôn đúng
            newFire.tag = fireTag;

            // 3. (Tùy chọn) Xoay lửa theo bề mặt (nếu cháy trên tường nghiêng)
            newFire.transform.up = hit.normal;

            currentFireCount++;
        }
    }

    Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        // Lấy điểm bắt đầu từ phía trên cùng của BoxCollider
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