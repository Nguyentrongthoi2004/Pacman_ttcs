using UnityEngine;

// Class này kế thừa từ Pellet và định nghĩa hành vi của Power Pellet trong game Pacman
public class PowerPellet : Pellet
{
    // Thời gian hiệu lực của Power Pellet (tính bằng giây)
    public float duration = 8f;

    // Ghi đè hàm Eat() từ class Pellet
    protected override void Eat()
    {
        // Gọi hàm PowerPelletEaten() của GameManager để xử lý logic khi Pacman ăn Power Pellet
        GameManager.Instance.PowerPelletEaten(this);
    }
}