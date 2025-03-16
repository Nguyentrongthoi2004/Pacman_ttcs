using System.Collections;
using UnityEngine;

// Lớp GhostHome kế thừa từ GhostBehavior, quản lý hành vi của Ghost khi ở trong "nhà".
public class GhostHome : GhostBehavior
{
    public Transform inside;  // Điểm bên trong nhà, nơi Ghost được đặt lại sau khi bị ăn.
    public Transform outside; // Điểm bên ngoài nhà, nơi Ghost bắt đầu di chuyển khi thoát ra.

    // Khi script được kích hoạt, dừng tất cả các Coroutine hiện tại.
    private void OnEnable()
    {
        StopAllCoroutines(); // Đảm bảo không có hoạt động chuyển tiếp nào đang chạy.
    }

    // Khi script bị vô hiệu hóa, bắt đầu quá trình thoát ra khỏi nhà.
    private void OnDisable()
    {
        // Kiểm tra xem GameObject vẫn đang hoạt động để tránh lỗi khi đối tượng bị hủy.
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ExitTransition()); // Bắt đầu quá trình chuyển đổi ra khỏi nhà.
        }
    }

    // Xử lý va chạm với tường bên trong nhà.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu Ghost đang trong nhà và va chạm với "Obstacle" (tường)...
        if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            // Đảo ngược hướng di chuyển để tạo hiệu ứng "nảy" trong nhà.
            ghost.movement.SetDirection(-ghost.movement.direction);
        }
    }

    // Coroutine xử lý quá trình chuyển đổi ra khỏi nhà.
    private IEnumerator ExitTransition()
    {
        // Tắt di chuyển khi thực hiện chuyển đổi vị trí thủ công.
        ghost.movement.SetDirection(Vector2.up, true); // Hướng di chuyển lên.
        ghost.movement.rb.bodyType = RigidbodyType2D.Kinematic; // Đặt Rigidbody ở chế độ Kinematic.
        ghost.movement.enabled = false; // Vô hiệu hóa logic di chuyển tự động.

        Vector3 position = transform.position; // Lấy vị trí hiện tại của Ghost.

        float duration = 0.5f; // Thời gian cho mỗi giai đoạn chuyển đổi.
        float elapsed = 0f;   // Bộ đếm thời gian đã trôi qua.

        // Chuyển đổi vị trí Ghost từ vị trí hiện tại đến vị trí bên trong nhà.
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration)); // Lerp để di chuyển mượt.
            elapsed += Time.deltaTime; // Tăng thời gian đã trôi qua.
            yield return null; // Chờ đến khung hình tiếp theo.
        }

        elapsed = 0f; // Đặt lại thời gian để thực hiện giai đoạn tiếp theo.

        // Chuyển đổi vị trí Ghost từ bên trong nhà đến bên ngoài nhà.
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration)); // Lerp đến vị trí ngoài.
            elapsed += Time.deltaTime; // Tăng thời gian đã trôi qua.
            yield return null; // Chờ đến khung hình tiếp theo.
        }

        // Sau khi thoát ra, chọn hướng ngẫu nhiên (trái hoặc phải) và kích hoạt lại di chuyển.
        ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true); // Hướng ngẫu nhiên.
        ghost.movement.rb.bodyType = RigidbodyType2D.Dynamic; // Đặt Rigidbody về chế độ Dynamic.
        ghost.movement.enabled = true; // Kích hoạt logic di chuyển tự động.
    }
}
