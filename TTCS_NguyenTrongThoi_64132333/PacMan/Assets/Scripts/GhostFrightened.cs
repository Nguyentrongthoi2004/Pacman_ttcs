using UnityEngine;

// Lớp GhostFrightened kế thừa từ GhostBehavior, điều khiển trạng thái "sợ hãi" của Ghost.
public class GhostFrightened : GhostBehavior
{
    // Các thành phần SpriteRenderer để hiển thị các trạng thái khác nhau.
    public SpriteRenderer body;   // Sprite chính của thân Ghost.
    public SpriteRenderer eyes;   // Sprite của mắt Ghost.
    public SpriteRenderer blue;   // Sprite hiển thị khi Ghost ở trạng thái sợ hãi (màu xanh).
    public SpriteRenderer white;  // Sprite hiển thị khi Ghost sắp hết trạng thái sợ hãi (nhấp nháy trắng).

    private bool eaten; // Biến cờ xác định Ghost đã bị ăn hay chưa.

    // Ghi đè phương thức Enable để kích hoạt trạng thái sợ hãi.
    public override void Enable(float duration)
    {
        base.Enable(duration); // Gọi phương thức Enable của lớp cha.

        // Thay đổi hiển thị của Ghost trong trạng thái sợ hãi.
        body.enabled = false;  // Ẩn thân chính.
        eyes.enabled = false;  // Ẩn mắt.
        blue.enabled = true;   // Hiển thị trạng thái sợ hãi (màu xanh).
        white.enabled = false; // Ẩn trạng thái nhấp nháy (màu trắng).

        // Kích hoạt trạng thái nhấp nháy sau nửa thời gian sợ hãi.
        Invoke(nameof(Flash), duration / 2f);
    }

    // Ghi đè phương thức Disable để tắt trạng thái sợ hãi.
    public override void Disable()
    {
        base.Disable(); // Gọi phương thức Disable của lớp cha.

        // Phục hồi hiển thị ban đầu của Ghost.
        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    // Xử lý khi Ghost bị ăn bởi Pacman.
    private void Eaten()
    {
        eaten = true; // Đánh dấu Ghost đã bị ăn.

        // Đưa Ghost về vị trí bên trong "nhà" (home).
        ghost.SetPosition(ghost.home.inside.position);

        // Kích hoạt trạng thái "nhà" (home) để phục hồi Ghost.
        ghost.home.Enable(duration);

        // Cập nhật hiển thị để chỉ hiển thị mắt Ghost.
        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    // Kích hoạt trạng thái nhấp nháy (màu trắng) khi gần hết thời gian sợ hãi.
    private void Flash()
    {
        if (!eaten) // Nếu Ghost chưa bị ăn...
        {
            blue.enabled = false;  // Tắt hiển thị màu xanh.
            white.enabled = true; // Hiển thị trạng thái nhấp nháy (màu trắng).
            white.GetComponent<AnimatedSprite>().Restart(); // Bắt đầu hiệu ứng nhấp nháy.
        }
    }

    // Khi script được kích hoạt, thiết lập ban đầu cho trạng thái sợ hãi.
    private void OnEnable()
    {
        blue.GetComponent<AnimatedSprite>().Restart(); // Khởi động hiệu ứng sprite nhấp nháy màu xanh.
        ghost.movement.speedMultiplier = 0.5f;        // Giảm tốc độ di chuyển của Ghost.
        eaten = false;                               // Đặt cờ "eaten" về false.
    }

    // Khi script bị vô hiệu hóa, khôi phục các thuộc tính mặc định.
    private void OnDisable()
    {
        ghost.movement.speedMultiplier = 1f; // Phục hồi tốc độ di chuyển bình thường.
        eaten = false;                      // Đặt cờ "eaten" về false.
    }

    // Xử lý va chạm với các nút (Node) trong bản đồ.
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>(); // Lấy Node mà Ghost va chạm.

        if (node != null && enabled) // Nếu va chạm với một Node và trạng thái sợ hãi đang bật...
        {
            Vector2 direction = Vector2.zero; // Hướng di chuyển tối ưu.
            float maxDistance = float.MinValue; // Khoảng cách xa nhất từ Pacman.

            // Tìm hướng di chuyển để xa Pacman nhất.
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // Tính toán khoảng cách từ vị trí mới đến Pacman.
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude; // Dùng bình phương khoảng cách để tối ưu hiệu suất.

                // Cập nhật hướng di chuyển nếu khoảng cách lớn hơn.
                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            // Cập nhật hướng di chuyển mới cho Ghost.
            ghost.movement.SetDirection(direction);
        }
    }

    // Xử lý va chạm với Pacman.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman")) // Nếu va chạm với Pacman...
        {
            if (enabled)
            { // Nếu trạng thái sợ hãi đang bật...
                Eaten(); // Gọi phương thức xử lý Ghost bị ăn.
            }
        }
    }
}
