using UnityEngine;

// Đảm bảo GameObject chứa script này phải có thành phần SpriteRenderer.
[RequireComponent(typeof(SpriteRenderer))]
public class GhostEyes : MonoBehaviour
{
    // Các sprite đại diện cho hướng nhìn của mắt Ghost.
    public Sprite up;    // Sprite cho hướng lên.
    public Sprite down;  // Sprite cho hướng xuống.
    public Sprite left;  // Sprite cho hướng trái.
    public Sprite right; // Sprite cho hướng phải.

    // Thành phần SpriteRenderer để thay đổi hình ảnh mắt của Ghost.
    private SpriteRenderer spriteRenderer;

    // Thành phần Movement từ GameObject cha, dùng để lấy hướng di chuyển.
    private Movement movement;

    // Awake được gọi khi script khởi tạo. Dùng để lấy các tham chiếu cần thiết.
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Gắn SpriteRenderer từ GameObject hiện tại.
        movement = GetComponentInParent<Movement>();    // Gắn Movement từ GameObject cha.
    }

    // Update được gọi mỗi khung hình. 
    // Dùng để cập nhật sprite của mắt dựa trên hướng di chuyển.
    private void Update()
    {
        // Kiểm tra hướng di chuyển và thay đổi sprite của mắt tương ứng.
        if (movement.direction == Vector2.up)
        {
            spriteRenderer.sprite = up; // Nếu di chuyển lên, dùng sprite "up".
        }
        else if (movement.direction == Vector2.down)
        {
            spriteRenderer.sprite = down; // Nếu di chuyển xuống, dùng sprite "down".
        }
        else if (movement.direction == Vector2.left)
        {
            spriteRenderer.sprite = left; // Nếu di chuyển trái, dùng sprite "left".
        }
        else if (movement.direction == Vector2.right)
        {
            spriteRenderer.sprite = right; // Nếu di chuyển phải, dùng sprite "right".
        }
    }
}
