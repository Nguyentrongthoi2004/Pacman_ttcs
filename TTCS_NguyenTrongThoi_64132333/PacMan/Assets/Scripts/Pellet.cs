using UnityEngine;

[RequireComponent(typeof(Collider2D))]
// Đảm bảo GameObject này luôn có một thành phần `Collider2D` để xử lý va chạm.
public class Pellet : MonoBehaviour
{
    public int points = 10;
    // Giá trị điểm số khi Pacman ăn viên pellet này.

    protected virtual void Eat()
    {
        // Phương thức thực thi khi viên pellet được ăn.
        // Gọi hàm xử lý trong GameManager, truyền vào đối tượng pellet hiện tại.
        GameManager.Instance.PelletEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D other)// Hàm được gọi khi có va chạm với Collider2D khác.
    {
        // Phương thức được gọi khi có một đối tượng khác va chạm với pellet.

        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            // Kiểm tra xem đối tượng va chạm có nằm trong layer "Pacman" hay không.
            Eat();
            // Nếu đúng, gọi phương thức `Eat()` để xử lý.
        }
    }
}
