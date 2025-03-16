using UnityEngine;

// Yêu cầu component Collider2D phải được gắn vào object sử dụng script này
[RequireComponent(typeof(Collider2D))]
public class Passage : MonoBehaviour
{
    // Transform của điểm đến khi đi qua passage
    public Transform connection;

    // Hàm được gọi khi collider của Passage va chạm với collider khác
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Lấy vị trí của điểm đến (connection)
        Vector3 position = connection.position;
        // Giữ nguyên giá trị z của đối tượng di chuyển qua passage
        position.z = other.transform.position.z;
        // Di chuyển đối tượng đến vị trí của điểm đến
        other.transform.position = position;
    }
}