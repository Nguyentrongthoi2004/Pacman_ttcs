using UnityEngine;

// Yêu cầu component Rigidbody2D phải được gắn vào object sử dụng script này
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    // Tốc độ di chuyển của đối tượng
    public float speed = 8f;
    // Hệ số nhân tốc độ (có thể dùng để tăng/giảm tốc độ tạm thời)
    public float speedMultiplier = 1f;
    // Hướng di chuyển ban đầu
    public Vector2 initialDirection;
    // Layer của các vật cản mà đối tượng sẽ va chạm
    public LayerMask obstacleLayer;

    // Rigidbody2D của đối tượng, chỉ cho phép đọc từ các script khác
    public Rigidbody2D rb { get; private set; }
    // Hướng di chuyển hiện tại của đối tượng
    public Vector2 direction { get; private set; }
    // Hướng di chuyển tiếp theo (nếu hướng hiện tại bị chặn)
    public Vector2 nextDirection { get; private set; }
    // Vị trí ban đầu của đối tượng
    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        // Lấy component Rigidbody2D của đối tượng và gán vào biến rb
        rb = GetComponent<Rigidbody2D>();
        // Lưu vị trí ban đầu của đối tượng
        startingPosition = transform.position;
    }

    private void Start()
    {
        // Khởi tạo trạng thái ban đầu của đối tượng khi game bắt đầu
        ResetState();
    }

    // Hàm để đặt lại trạng thái của đối tượng về ban đầu
    public void ResetState()
    {
        // Đặt lại hệ số nhân tốc độ về 1
        speedMultiplier = 1f;
        // Đặt lại hướng di chuyển hiện tại về hướng ban đầu
        direction = initialDirection;
        // Xóa hướng di chuyển tiếp theo
        nextDirection = Vector2.zero;
        // Đặt lại vị trí của đối tượng về vị trí ban đầu
        transform.position = startingPosition;
        // Cho phép đối tượng bị ảnh hưởng bởi vật lý
        rb.bodyType = RigidbodyType2D.Dynamic;
        // Kích hoạt script này
        enabled = true;
    }

    private void Update()
    {
        // Kiểm tra xem có hướng di chuyển tiếp theo không
        if (nextDirection != Vector2.zero)
        {
            // Nếu có, thử di chuyển theo hướng đó
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        // Lấy vị trí hiện tại của đối tượng
        Vector2 position = rb.position;
        // Tính toán vector di chuyển dựa trên tốc độ, hướng di chuyển và thời gian
        Vector2 translation = speed * speedMultiplier * Time.fixedDeltaTime * direction;
        // Di chuyển đối tượng đến vị trí mới bằng cách sử dụng MovePosition của Rigidbody2D
        rb.MovePosition(position + translation);
    }

    // Hàm để thiết lập hướng di chuyển của đối tượng
    public void SetDirection(Vector2 direction, bool forced = false)
    {
        // Kiểm tra xem có vật cản theo hướng di chuyển mới không, 
        // forced = true để buộc di chuyển ngay cả khi có vật cản
        if (forced || !Occupied(direction))
        {
            // Nếu không có vật cản hoặc forced = true, đặt hướng di chuyển mới
            this.direction = direction;
            // Xóa hướng di chuyển tiếp theo
            nextDirection = Vector2.zero;
        }
        else
        {
            // Nếu có vật cản, lưu hướng di chuyển mới vào nextDirection để di chuyển sau
            nextDirection = direction;
        }
    }

    // Hàm kiểm tra xem có vật cản theo hướng di chuyển hay không
    public bool Occupied(Vector2 direction)
    {
        // Sử dụng BoxCast để kiểm tra va chạm với vật cản.
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position, // Vị trí bắt đầu của BoxCast
            Vector2.one * 0.75f, // Kích thước của BoxCast
            0f, // Góc xoay của BoxCast
            direction, // Hướng của BoxCast
            1.5f, // Khoảng cách của BoxCast
            obstacleLayer); // Layer của vật cản cần kiểm tra

        // Trả về true nếu có va chạm với vật cản, false nếu không có
        return hit.collider != null;
    }
}