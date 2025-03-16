using UnityEngine;

// Yêu cầu component Movement phải được gắn vào object sử dụng script này
[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    // Chuỗi hoạt ảnh khi Pacman chết
    [SerializeField] private AnimatedSprite deathSequence;
    // Component SpriteRenderer của Pacman
    private SpriteRenderer spriteRenderer;
    // Component CircleCollider2D của Pacman
    private CircleCollider2D circleCollider;//CircleCollider2D là một component (thành phần) trong Unity dùng để xác định vùng va chạm hình tròn.
    // Component Movement của Pacman, chỉ cho phép đọc từ các script khác
    public Movement movement { get; private set; }

    // Thuộc tính Speed truy cập tốc độ từ component Movement
    public float speed
    {
        get => movement.speed; // Lấy giá trị speed từ movement
        set => movement.speed = value; // Gán giá trị speed cho movement
    }

    private void Awake()
    {
        // Lấy các component cần thiết
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        // Xử lý input từ bàn phím để di chuyển Pacman
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.SetDirection(Vector2.up); // Di chuyển lên
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.SetDirection(Vector2.down); // Di chuyển xuống
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.SetDirection(Vector2.left); // Di chuyển sang trái
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.SetDirection(Vector2.right); // Di chuyển sang phải
        }

        // Xoay Pacman theo hướng di chuyển
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);// Tính góc giữa hướng di chuyển và trục x
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);// Xoay Pacman
    }

    // Hàm đặt lại trạng thái của Pacman về ban đầu
    public void ResetState()
    {
        // Kích hoạt script, sprite renderer, collider và gameObject
        enabled = true;
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;
        gameObject.SetActive(true);
        // Vô hiệu hóa hoạt ảnh chết
        deathSequence.enabled = false;
        // Đặt lại trạng thái của component Movement
        movement.ResetState();
    }

    // Hàm thực hiện hoạt ảnh chết của Pacman
    public void DeathSequence()
    {
        // Vô hiệu hóa script, sprite renderer, collider và component Movement
        enabled = false;
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
        movement.enabled = false;
        // Kích hoạt và chạy lại hoạt ảnh chết
        deathSequence.enabled = true;
        deathSequence.Restart();
    }
}