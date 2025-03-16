using UnityEngine;

// Thiết lập thứ tự thực thi của script, ưu tiên chạy sớm hơn các script khác.
[DefaultExecutionOrder(-10)]

// Bảo đảm GameObject chứa script này phải có thành phần Movement.
[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour // Lớp Ghost tất cả quản lý hành vi của Ghost trong game.
{
    // Thuộc tính công khai, cho phép đọc từ bên ngoài nhưng chỉ được gán giá trị nội bộ.
    public Movement movement { get; private set; } // Tham chiếu đến thành phần Movement.
    public GhostHome home { get; private set; } // Tham chiếu đến thành phần GhostHome.
    public GhostScatter scatter { get; private set; } // Tham chiếu đến thành phần GhostScatter.
    public GhostChase chase { get; private set; } // Tham chiếu đến thành phần GhostChase.
    public GhostFrightened frightened { get; private set; } // Tham chiếu đến thành phần GhostFrightened.

    // Hành vi khởi tạo ban đầu của Ghost.
    public GhostBehavior initialBehavior;

    // Mục tiêu mà Ghost sẽ hướng đến (Pacman hoặc vị trí cụ thể).
    public Transform target;

    // Điểm thưởng khi Ghost bị ăn bởi Pacman.
    public int points = 200;

    // Thuộc tính truy cập và chỉnh sửa tốc độ của Ghost thông qua thành phần Movement.
    public float speed
    {
        get => movement.speed; // Lấy tốc độ từ thành phần Movement.
        set => movement.speed = value; // Gán tốc độ vào thành phần Movement.
    }

    // Awake được gọi khi script khởi tạo. Dùng để lấy tham chiếu đến các thành phần khác.
    private void Awake()
    {
        movement = GetComponent<Movement>(); // Gắn thành phần Movement.
        home = GetComponent<GhostHome>(); // Gắn thành phần GhostHome.
        scatter = GetComponent<GhostScatter>(); // Gắn thành phần GhostScatter.
        chase = GetComponent<GhostChase>(); // Gắn thành phần GhostChase.
        frightened = GetComponent<GhostFrightened>(); // Gắn thành phần GhostFrightened.
    }

    // Start được gọi khi script bắt đầu chạy. Thiết lập trạng thái ban đầu của Ghost.
    private void Start()
    {
        ResetState(); // Đặt lại trạng thái của Ghost về mặc định.
    }

    // Phương thức để đặt lại trạng thái của Ghost.
    public void ResetState()
    {
        gameObject.SetActive(true); // Kích hoạt Ghost (trong trường hợp bị vô hiệu trước đó).
        movement.ResetState(); // Đặt lại trạng thái di chuyển của Ghost.

        frightened.Disable(); // Vô hiệu hóa trạng thái sợ hãi.
        chase.Disable(); // Vô hiệu hóa trạng thái đuổi bắt.
        scatter.Enable(); // Bật trạng thái Scatter (di chuyển ngẫu nhiên).

        if (home != initialBehavior) // Nếu hành vi khởi tạo không phải là Home...
        {
            home.Disable(); // Vô hiệu hóa Home.
        }

        if (initialBehavior != null) // Nếu hành vi khởi tạo không null...
        {
            initialBehavior.Enable(); // Bật hành vi khởi tạo.
        }
    }

    // Đặt vị trí của Ghost trên bản đồ.
    public void SetPosition(Vector3 position)
    {
        position.z = transform.position.z; // Giữ nguyên giá trị trục Z để tránh lỗi về chiều sâu.
        transform.position = position; // Gán vị trí mới cho Ghost.
    }

    // Hàm xử lý khi Ghost va chạm với đối tượng khác (Pacman).
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman")) // Nếu va chạm với Pacman...
        {
            if (frightened.enabled) // Nếu Ghost đang ở trạng thái sợ hãi...
            {
                GameManager.Instance.GhostEaten(this); // Gọi hàm xử lý Ghost bị ăn trong GameManager.
            }
            else // Nếu không ở trạng thái sợ hãi...
            {
                GameManager.Instance.PacmanEaten(); // Gọi hàm xử lý Pacman bị ăn trong GameManager.
            }
        }
    }
}
