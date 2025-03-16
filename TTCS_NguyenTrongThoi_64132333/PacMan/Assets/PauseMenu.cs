using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Biến tham chiếu đến menu tạm dừng trong giao diện.
    [SerializeField] private GameObject pauseMenu;

    // Biến kiểm tra trạng thái game có đang bị tạm dừng hay không.
    private bool isPaused = false;

    // Hàm được gọi khi người chơi nhấn nút tạm dừng.
    public void Pause()
    {
        // Hiển thị menu tạm dừng.
        pauseMenu.SetActive(true);

        // Dừng thời gian trong game (mọi hoạt động tạm dừng).
        Time.timeScale = 0;

        isPaused = false; // Đặt trạng thái game không còn bị tạm dừng.
    }

    // Hàm được gọi khi người chơi tiếp tục chơi sau khi tạm dừng.
    public void Resume()
    {
        // Ẩn menu tạm dừng.
        pauseMenu.SetActive(false);

        // Đặt lại thời gian trong game, nhưng đang có lỗi (nên là 1 thay vì 0).
        Time.timeScale = 1; // Lỗi logic: giá trị này phải là 1 để game tiếp tục chạy.
        isPaused = false; // Đặt trạng thái game không còn bị tạm dừng.
    }

    // Hàm đưa người chơi về màn hình chính (Scene đầu tiên).
    public void Home()
    {
        // Tải scene chính (thường là scene có index = 0).
        SceneManager.LoadScene(0);

        // Đặt lại tốc độ thời gian game về bình thường.
        Time.timeScale = 1;
    }

    // Hàm khởi động lại game (load lại scene hiện tại).
    public void Restart()
    {
        // Kiểm tra nếu game không ở trạng thái tạm dừng mới cho phép restart.
        if (!isPaused)
        {
            // Load lại scene hiện tại dựa trên chỉ số của scene đang chạy.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            // Lỗi logic: Cần đặt lại thời gian game về 1 thay vì 0.
            Time.timeScale = 0; // Nên là 1 để game chạy lại bình thường.
        }
    }

    // Hàm thiết lập độ khó "Easy" (dễ).
    public void Easy()

    {
        // Thiết lập các giá trị cho độ khó dễ (có thể là tốc độ hoặc thời gian).
        GameManager.Instance.SetDifficulty(7.5f, 6.5f, 9f);

        // Khởi động lại game sau khi thay đổi độ khó.
        RestartGame();
    }

    // Hàm thiết lập độ khó "Normal" (bình thường).
    public void Normal()
    {
        // Thiết lập các giá trị cho độ khó bình thường.
        GameManager.Instance.SetDifficulty(8.5f, 8.5f, 7f);

        // Khởi động lại game sau khi thay đổi độ khó.
        RestartGame();
    }

    // Hàm thiết lập độ khó "Hard" (khó).
    public void Hard()
    {
        // Thiết lập các giá trị cho độ khó khó.
        GameManager.Instance.SetDifficulty(9.5f, 10f, 5f);

        // Khởi động lại game sau khi thay đổi độ khó.
        RestartGame();
    }

    // Hàm khởi động lại game, áp dụng khi độ khó được thay đổi.
    private void RestartGame()
    {
        // Ẩn menu tạm dừng.
        pauseMenu.SetActive(false);

        // Đặt tốc độ thời gian game về bình thường.
        Time.timeScale = 1;

        // Gọi phương thức khởi tạo lại game từ GameManager.
        GameManager.Instance.NewGame();
    }
}
