using UnityEngine;

// Class này kế thừa từ GhostBehavior và định nghĩa hành vi đuổi theo của Ghost
public class GhostChase : GhostBehavior
{
    // Hàm được gọi khi component này bị vô hiệu hóa
    private void OnDisable()
    {
        // Khi hành vi đuổi theo bị vô hiệu hóa, kích hoạt hành vi phân tán
        ghost.scatter.Enable(); // Kích hoạt chế độ scatter của Ghost
    }

    // Hàm được gọi khi collider của Ghost va chạm với collider khác
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Lấy component Node của đối tượng va chạm
        Node node = other.GetComponent<Node>();
        // Kiểm tra xem Ghost có đang ở trạng thái hoảng sợ hay không
        // Chỉ thực hiện logic đuổi theo khi Ghost không hoảng sợ và component này đang được kích hoạt
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            // Khởi tạo biến direction để lưu trữ hướng di chuyển tiếp theo
            Vector2 direction = Vector2.zero;
            // Khởi tạo biến minDistance với giá trị lớn nhất có thể để lưu trữ khoảng cách ngắn nhất
            float minDistance = float.MaxValue;
            // Duyệt qua tất cả các hướng di chuyển có thể tại Node hiện tại
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // Tính toán vị trí mới của Ghost nếu di chuyển theo hướng availableDirection
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);// Tính toán khoảng cách từ vị trí mới đến vị trí của Pacman (ghost.target)
                // Tính toán khoảng cách từ vị trí mới đến vị trí của Pacman (ghost.target)
                // Sử dụng sqrMagnitude để tối ưu hiệu năng (không cần tính căn bậc hai)
                float distance = (ghost.target.position - newPosition).sqrMagnitude;
                // Nếu khoảng cách mới nhỏ hơn khoảng cách hiện tại (minDistance)
                if (distance < minDistance)
                {
                    // Cập nhật hướng di chuyển (direction) và khoảng cách ngắn nhất (minDistance)
                    direction = availableDirection;
                    minDistance = distance;
                }
            }
            // Thiết lập hướng di chuyển mới cho Ghost bằng cách gọi hàm SetDirection của component Movement
            ghost.movement.SetDirection(direction);
        }
    }
}