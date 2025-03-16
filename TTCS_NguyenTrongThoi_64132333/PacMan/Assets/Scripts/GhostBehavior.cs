using UnityEngine;

// Yêu cầu component Ghost phải được gắn vào object sử dụng script này
[RequireComponent(typeof(Ghost))]
public abstract class GhostBehavior : MonoBehaviour//Quản lý hành vi của Ghost
{
    // Component Ghost của đối tượng, chỉ cho phép đọc từ các script khác
    public Ghost ghost { get; private set; }
    // Thời gian hiệu lực của hành vi (tính bằng giây)
    public float duration;

    private void Awake()
    {
        // Lấy component Ghost của đối tượng và gán vào biến ghost
        ghost = GetComponent<Ghost>();
    }

    // Hàm kích hoạt hành vi với thời gian hiệu lực mặc định
    public void Enable()
    {
        Enable(duration);//Gọi hàm Enable với thời gian hiệu lực mặc định
    }

    // Hàm kích hoạt hành vi với thời gian hiệu lực được chỉ định
    public virtual void Enable(float duration)
    {
        // Kích hoạt script này
        enabled = true;

        // Hủy bỏ lời gọi hàm Disable() trước đó (nếu có)
        CancelInvoke();
        // Gọi hàm Disable() sau một khoảng thời gian bằng duration
        Invoke(nameof(Disable), duration);
    }

    // Hàm vô hiệu hóa hành vi
    public virtual void Disable()
    {
        // Vô hiệu hóa script này
        enabled = false;

        // Hủy bỏ lời gọi hàm Disable() trước đó (nếu có)
        CancelInvoke();
    }
}