using UnityEngine;

// [RequireComponent(typeof(SpriteRenderer))] 
// Bảo đảm GameObject chứa thành phần này cũng phải có một SpriteRenderer. 
// Nếu chưa có, Unity sẽ tự động thêm khi script được gắn vào GameObject.
[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour//quản lý hoạt ảnh
{
    // Một mảng các Sprite sẽ được sử dụng cho hoạt ảnh.
    public Sprite[] sprites = new Sprite[0];

    // Thời gian giữa mỗi frame hoạt ảnh.
    public float animationTime = 0.25f;

    // Quy định hoạt ảnh có lặp lại hay không.
    public bool loop = true;

    // Tham chiếu đến SpriteRenderer để hiển thị các sprite.
    private SpriteRenderer spriteRenderer; //SpriteRenderer là một component (thành phần) trong Unity dùng để hiển thị Sprite (hình ảnh 2D) trên màn hình.

    // Lưu trữ chỉ số frame hiện tại của hoạt ảnh.
    private int animationFrame;

    // Awake được gọi khi script được khởi tạo. 
    // Ở đây, lấy tham chiếu đến SpriteRenderer của GameObject.
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // OnEnable được gọi khi GameObject hoặc script được kích hoạt.
    // Bật SpriteRenderer để hiển thị sprite.
    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    // OnDisable được gọi khi GameObject hoặc script bị vô hiệu hóa.
    // Vô hiệu hóa SpriteRenderer để ngừng hiển thị sprite.
    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    // Start được gọi khi script bắt đầu chạy.
    // Lên lịch gọi hàm Advance đều đặn với khoảng thời gian animationTime.
    private void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
        //Cú pháp InvokeRepeating(string methodName, float time, float repeatRate);
    }

    // Hàm Advance để chuyển sang frame tiếp theo của hoạt ảnh.
    private void Advance()
    {
        // Nếu SpriteRenderer không được bật, không thực hiện gì.
        if (!spriteRenderer.enabled)
        {
            return;
        }

        // Tăng chỉ số frame.
        animationFrame++;

        // Nếu frame vượt quá số lượng sprite và chế độ lặp được bật, quay lại frame đầu tiên.
        if (animationFrame >= sprites.Length && loop)
        {
            animationFrame = 0;
        }

        // Nếu frame nằm trong khoảng hợp lệ, cập nhật sprite hiển thị.
        if (animationFrame >= 0 && animationFrame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[animationFrame];
        }
    }

    // Hàm Restart để bắt đầu lại hoạt ảnh từ đầu.
    public void Restart()
    {
        // Đặt animationFrame về -1 để Advance() sẽ chuyển đến frame đầu tiên.
        animationFrame = -1;

        // Gọi Advance() để cập nhật frame ngay lập tức.
        Advance();
    }
}
