using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Thứ tự thực thi script này sẽ được ưu tiên hơn các script khác 
// (giá trị càng thấp càng được ưu tiên)
[DefaultExecutionOrder(-100)]

public class GameManager : MonoBehaviour // Quản lý các trạng thái và thông số tổng thể của game.
{
    // Singleton instance của GameManager
    public static GameManager Instance { get; private set; }
    //Singleton là một design pattern (mẫu thiết kế) trong lập trình phần mềm.
    //Nó đảm bảo rằng chỉ có một instance (thể hiện duy nhất) của một lớp tồn tại trong toàn bộ vòng đời của ứng dụng,
    //và cung cấp một điểm truy cập toàn cục (global access point) đến instance đó.
    //Singleton giúp dễ dàng truy cập GameManager từ bất kỳ đâu mà không cần phải truyền tham chiếu,
    //đồng thời ngăn chặn việc vô tình tạo ra nhiều GameManager gây ra lỗi logic.
    //Singleton cũng giúp dễ dàng theo dõi và quản lý các trạng thái của game.

    // Các component trong game

    // Mảng các Ghost trong game
    [SerializeField] private Ghost[] ghosts;

    // Pacman
    [SerializeField] private Pacman pacman;

    // Parent object chứa tất cả các Pellet
    [SerializeField] private Transform pellets;

    // Text hiển thị khi game over
    [SerializeField] private Text gameOverText;

    // Text hiển thị điểm số
    [SerializeField] private Text scoreText;

    // Text hiển thị số mạng còn lại
    [SerializeField] private Text livesText;


    // Text hiển thị HighScore
    [SerializeField] private Text highScoreText;

    private List<int> highScores = new List<int>();

    // Tham chiếu đến UI Text hiển thị HighScore
    public int highScore { get; private set; }



    // Điểm số và số mạng hiện tại

    // Điểm
    public int score { get; private set; } = 0;

    // Mạng
    public int lives { get; private set; } = 3;

    // Hệ số nhân điểm khi ăn Ghost liên tiếp
    private int ghostMultiplier = 1;

    // Các biến điều chỉnh độ khó của game

    // Tốc độ Pacman
    private float pacmanSpeed = 1.0f;

    // Tốc độ Ghost
    private float ghostSpeed = 1.0f;

    // Thời gian Ghost hoảng sợ sau khi ăn Power Pellet
    private float frightenedDuration = 7.0f;



    private void Awake()
    {
        // Đảm bảo chỉ có một instance(thực thể) của GameManager tồn tại

        // Nếu đã có instance
        if (Instance != null)
        {
            // Hủy object này 
            DestroyImmediate(gameObject);
        }
        // Nếu chưa có instance
        else
        {
            // Gán instance cho biến static GameManager
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        // Xóa instance khi object bị hủy
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        LoadHighScore();//Tải điểm số cao nhất
        // Bắt đầu game mới khi scene được load
        NewGame();
    }

    private void Update()
    {
        // Khởi động lại game khi game over và người chơi nhấn phím bất kỳ
        if (lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    // Bắt đầu game mới
    public void NewGame()
    {
        // Đặt lại điểm số
        SetScore(0);

        // Đặt lại số mạng
        SetLives(3);

        // Áp dụng độ khó
        ApplyDifficulty();

        // Bắt đầu vòng chơi mới
        NewRound();
    }

    // Bắt đầu vòng chơi mới
    private void NewRound()
    {
        // Ẩn text game over
        gameOverText.enabled = false;

        // Kích hoạt lại tất cả các Pellet
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);//Kích hoạt lại Pellet
        }

        // Đặt lại trạng thái của Pacman và Ghost
        ResetState();
    }

    // Đặt lại trạng thái của Pacman và Ghost
    private void ResetState()
    {
        // Đặt lại trạng thái của từng Ghost
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();//
        }

        // Đặt lại trạng thái của Pacman
        pacman.ResetState();
    }

    // Kết thúc game
    private void GameOver()
    {
        // Hiển thị text game over
        gameOverText.enabled = true;

        // Vô hiệu hóa tất cả Ghost
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);//Vô hiệu hóa Ghost
        }

        // Vô hiệu hóa Pacman
        pacman.gameObject.SetActive(false);

        CheckHighScore();//Kiểm tra điểm số cao nhất
    }

    // Cập nhật số mạng và hiển thị trên UI
    private void SetLives(int lives)
    {
        this.lives = lives;

        // Hiển thị số mạng còn lại
        livesText.text = "x" + lives.ToString();
    }

    // Cập nhật điểm số và hiển thị trên UI
    private void SetScore(int score)
    {
        this.score = score;

        // Hiển thị điểm số với 2 chữ số
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    // Xử lý khi Pacman bị ăn
    public void PacmanEaten()
    {
        // Chạy hoạt ảnh chết của Pacman
        pacman.DeathSequence();

        // Giảm số mạng
        SetLives(lives - 1);

        // Nếu còn mạng
        if (lives > 0)
        {
            // Đặt lại trạng thái sau 3 giây
            Invoke(nameof(ResetState), 3f);
        }
        // Nếu hết mạng
        else
        {
            // Kết thúc game
            GameOver();
        }
    }

    // Xử lý khi Ghost bị ăn
    public void GhostEaten(Ghost ghost) //Khi ăn Ghost , sẽ cộng điểm và tăng hệ số nhân điểm của Ghost
    {
        // Tính điểm dựa trên hệ số nhân
        int points = ghost.points * ghostMultiplier; //Lấy điểm mặc định của Ghost và nhân với hệ số nhân của ghost 
                                                     // Ví dụ : Ghost 1 = 200 điểm , Ghost 2 = 400 điểm , Ghost 3 = 600 điểm , Ghost 4 = 800 điểm   
        // Cộng điểm
        SetScore(score + points);

        // Tăng hệ số nhân
        ghostMultiplier++;
    }

    // Xử lý khi Pellet bị ăn
    public void PelletEaten(Pellet pellet)//Khi ăn Pellet , sẽ cộng điểm và kiểm tra xem còn Pellet nào không
    {
        // Vô hiệu hóa Pellet
        pellet.gameObject.SetActive(false);

        // Cộng điểm
        SetScore(score + pellet.points);

        // Kiểm tra xem còn Pellet nào không
        if (!HasRemainingPellets())
        {
            // Vô hiệu hóa Pacman
            pacman.gameObject.SetActive(false);// Là game sẽ kết thúc và không thể di chuyển Pacman nữa

            // Bắt đầu vòng chơi mới sau 3 giây
            Invoke(nameof(NewRound), 3f); //Sau 3 giây sẽ bắt đầu vòng chơi mới 
        }
    }

    // Xử lý khi Power Pellet bị ăn
    public void PowerPelletEaten(PowerPellet pellet)
    {
        // Kích hoạt trạng thái hoảng sợ cho tất cả Ghost
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].frightened.Enable(frightenedDuration);//Khi ăn Power Pellet , Ghost sẽ hoảng sợ trong 7 giây
        }

        // Xử lý như Pellet thường
        PelletEaten(pellet);

        // Hủy bỏ việc đặt lại hệ số nhân (nếu có)
        CancelInvoke(nameof(ResetGhostMultiplier));

        // Đặt lại hệ số nhân sau khi hết thời gian hoảng sợ
        Invoke(nameof(ResetGhostMultiplier), frightenedDuration);//Sau 7 giây sẽ đặt lại hệ số nhân về 1
    }

    // Kiểm tra xem còn Pellet nào trên màn hình không
    private bool HasRemainingPellets() //Tìm kiếm tuyến tính , Độ phức tạp là O(n), n là số lượng pellet
    {
        // Duyệt qua tất cả các Pellet
        foreach (Transform pellet in pellets)
        {
            //Kiểm tra xem pellet còn active không
            if (pellet.gameObject.activeSelf)
            {
                //Nếu còn pellet active thì trả về true
                return true;//Qua đó xác định được game vẫn còn đang chơi
            }
        }

        // Nếu không còn Pellet nào, trả về false
        return false;//Xác định được game đã kết thúc
    }

    // Đặt lại hệ số nhân điểm Ghost
    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1; 
    }

    // Thiết lập độ khó của game
    public void SetDifficulty(float pacmanSpeed, float ghostSpeed, float frightenedDuration)
    {
        this.pacmanSpeed = pacmanSpeed;//Tốc độ của Pacman
        this.ghostSpeed = ghostSpeed;//Tốc độ của Ghost
        this.frightenedDuration = frightenedDuration;//Thời gian Ghost hoảng sợ
    }

    // Áp dụng độ khó cho Pacman và Ghost
    private void ApplyDifficulty()
    {
        pacman.speed = pacmanSpeed;//Gán tốc độ của Pacman

        // Gán tốc độ của từng Ghost
        foreach (Ghost ghost in ghosts)
        {
           
            if (ghost.enabled)//Nếu Ghost đang được kích hoạt
            {
                ghost.speed = ghostSpeed; //Gán tốc độ của Ghost
            }
            
        }
    }
    // Tải HighScores từ PlayerPrefs
    private void LoadHighScore()
    {
        highScores.Clear(); // Xóa danh sách điểm cao hiện tại
        string scoresString = PlayerPrefs.GetString("HighScores", "0,0,0,0,0"); // Tải chuỗi điểm cao, mặc định là "0,0,0,0,0"
        highScores = scoresString.Split(',').Select(int.Parse).ToList(); // Chuyển chuỗi thành danh sách số nguyên
        UpdateHighScoreText(); // Cập nhật UI
    }

    // Kiểm tra và lưu HighScore
    private void CheckHighScore()
    {
        if (score > highScores.Min()) // Nếu điểm hiện tại cao hơn điểm thấp nhất trong danh sách
        {
            highScores.Remove(highScores.Min()); // Xóa điểm thấp nhất
            highScores.Add(score); // Thêm điểm hiện tại
            highScores = highScores.OrderByDescending(x => x).ToList(); // Sắp xếp lại danh sách theo thứ tự giảm dần
            SaveHighScores(); // Lưu danh sách điểm cao vào PlayerPrefs
            UpdateHighScoreText(); // Cập nhật UI
        }
    }

    // Lưu HighScores vào PlayerPrefs
    private void SaveHighScores()
    {
        string scoresString = string.Join(",", highScores.Select(x => x.ToString()).ToArray()); // Chuyển danh sách số nguyên thành chuỗi
        PlayerPrefs.SetString("HighScores", scoresString); // Lưu chuỗi điểm cao vào PlayerPrefs
    }

    // Cập nhật UI Text để hiển thị HighScores
    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Scores:\n";
        for (int i = 0; i < 5; i++) // Lặp qua 5 vị trí
        {
            if (i < highScores.Count)
            {
                highScoreText.text += (i + 1) + ". " + highScores[i].ToString() + "\n";//Hiển thị điểm số cao nhất
            }
            else
            {
                highScoreText.text += (i + 1) + ". 0\n"; // Hiển thị 0 nếu không đủ 5 điểm
            }
        }
    }
}