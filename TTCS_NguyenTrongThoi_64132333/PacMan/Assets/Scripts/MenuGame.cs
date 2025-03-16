using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện để quản lý các scene.
using UnityEngine.UI; // Thư viện để làm việc với UI.

public class MenuGame : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    // Biến `scoreText` để hiển thị điểm số trong UI, có thể gắn trực tiếp từ Unity Editor.

    // Phương thức bắt đầu trò chơi, chuyển đến Scene chính của game.
    public void StartGame()
    {
        Debug.Log("Starting Game...");
        // Hiển thị thông báo trong Console khi bắt đầu game.

        SceneManager.LoadScene(1);
        // Tải Scene có index 1, thường là Scene chính của game.

        Time.timeScale = 0;
        // Dừng thời gian game ngay khi chuyển Scene (nếu cần dừng trước khi tiếp tục).
    }

    // Phương thức quay lại menu chính, chuyển đến Scene menu chính.
    public void BackToMenu()
    {
        Debug.Log("Returning to Main Menu...");
        // Hiển thị thông báo trong Console khi quay lại menu chính.

        SceneManager.LoadScene(0);
        // Tải Scene có index 0, thường là Scene menu chính.
    }

    // Phương thức hiển thị điểm số hiện tại.
    public void ShowScore()
    {
        if (scoreText != null)
        {
            // Kiểm tra nếu `scoreText` đã được gán trong Unity Editor.

            int currentScore = GameManager.Instance.score;
            // Lấy điểm số hiện tại từ GameManager.

            scoreText.text = "Score: " + currentScore.ToString();
            // Cập nhật text để hiển thị điểm số.

            Debug.Log("Displaying Score: " + currentScore);
            // Hiển thị thông báo trong Console kèm theo điểm số.
        }
        else
        {
            Debug.LogWarning("Score Text UI is not assigned in the Inspector!");
            // Cảnh báo nếu `scoreText` chưa được gán.
        }
    }

    // Phương thức thoát trò chơi.
    public void ExitGame()
    {
        Debug.Log("Exit button clicked!");
        // Hiển thị thông báo khi nhấn nút thoát.

        Application.Quit();
        // Thoát trò chơi (chỉ hoạt động khi build thành ứng dụng, không hoạt động trong Unity Editor).

#if UNITY_EDITOR
        Debug.Log("Game is quitting in Editor mode!");
        // Debug bổ sung để kiểm tra chức năng thoát trong Unity Editor.
#endif
    }
}
