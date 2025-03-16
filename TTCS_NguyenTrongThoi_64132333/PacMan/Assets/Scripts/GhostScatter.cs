using UnityEngine;

public class GhostScatter : GhostBehavior
{
    // Phương thức được gọi khi đối tượng bị vô hiệu hóa
    private void OnDisable()
    {
        // Kích hoạt hành vi "chase" (đuổi theo mục tiêu) của con ma
        ghost.chase.Enable();
    }

    // Phương thức được gọi khi ghost va chạm với một collider 2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Lấy thành phần Node từ đối tượng va chạm
        Node node = other.GetComponent<Node>();

        // Nếu đối tượng là một Node, hành vi scatter đang hoạt động và ghost không bị "frightened" (sợ hãi)
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            // Chọn một hướng ngẫu nhiên từ các hướng khả dụng tại Node
            int index = Random.Range(0, node.availableDirections.Count);

            // Nếu có hơn một hướng khả dụng và hướng ngẫu nhiên được chọn
            // trùng với hướng ngược lại hướng hiện tại, chuyển sang hướng khác
            if (node.availableDirections.Count > 1 && node.availableDirections[index] == -ghost.movement.direction)
            {
                index++; // Chọn hướng tiếp theo trong danh sách

                // Nếu vượt quá số lượng hướng khả dụng, quay lại hướng đầu tiên
                if (index >= node.availableDirections.Count)
                {
                    index = 0;
                }
            }

            // Cập nhật hướng di chuyển cho con ma
            ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }
}
