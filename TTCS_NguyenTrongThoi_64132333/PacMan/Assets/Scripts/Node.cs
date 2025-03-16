using System.Collections.Generic;
using UnityEngine;

// Class này định nghĩa một Node trong game, thường được sử dụng trong các game di chuyển theo lưới như Pacman
public class Node : MonoBehaviour
{
    // Layer của các vật cản (ví dụ: tường)
    public LayerMask obstacleLayer; // LayerMask là một cấu trúc dữ liệu trong Unity dùng để lưu trữ thông tin về layer.
    // Danh sách các hướng di chuyển có thể từ Node này
    public readonly List<Vector2> availableDirections = new(); // readonly chỉ cho phép gán giá trị một lần, sau đó không thể thay đổi giá trị đó.

    private void Start()
    {
        // Xóa danh sách các hướng di chuyển có thể (để chắc chắn rằng danh sách rỗng trước khi kiểm tra)
        availableDirections.Clear();

        // Kiểm tra các hướng di chuyển có thể bằng cách sử dụng BoxCast để xem có va chạm với vật cản không
        // Nếu không va chạm, hướng di chuyển đó sẽ được thêm vào danh sách availableDirections
        CheckAvailableDirection(Vector2.up);    // Kiểm tra hướng lên
        CheckAvailableDirection(Vector2.down);  // Kiểm tra hướng xuống
        CheckAvailableDirection(Vector2.left);  // Kiểm tra hướng trái
        CheckAvailableDirection(Vector2.right); // Kiểm tra hướng phải
    }

    // Hàm kiểm tra xem có thể di chuyển theo hướng direction hay không
    private void CheckAvailableDirection(Vector2 direction)
    {
        // Sử dụng BoxCast để kiểm tra va chạm với vật cản theo hướng direction
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position,     // Vị trí bắt đầu của BoxCast (vị trí của Node)
            Vector2.one * 0.5f,   // Kích thước của BoxCast
            0f,                     // Góc xoay của BoxCast
            direction,             // Hướng của BoxCast
            1f,                     // Khoảng cách của BoxCast
            obstacleLayer);        // Layer của vật cản cần kiểm tra

        // Nếu không có va chạm (hit.collider == null), tức là không có vật cản theo hướng đó
        if (hit.collider == null)
        {
            // Thêm hướng di chuyển direction vào danh sách availableDirections
            availableDirections.Add(direction);
        }
    }
}