using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int difficulity;
    public int score = 0;
    public int highScore = 0;

    public bool gameStarted = false;
    public bool gamePaused = true;
    public bool gameOver = false;

    public Camera camera;
    public Text scoreText;

    float shakeTime = 0;
    float shakeAmount = 1;
    float shakeDecrease = 1f;

    public GameObject pauseMenu;
    public GameObject startMenu;
    public GameObject gameOverMenu;


    public void AddScore(int rowCount)
    {
        score += 10 * (int)Mathf.Pow(2, rowCount - 1);
    }
    void HighScore()
    {
        if (gameOver && score > highScore)
        {
            highScore = score;
        }
    }

    public void GameStatus()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
        }
    }
    public void RestartGame()
    {
        DontDestroyOnLoad(gameObject);
        if (Input.GetKeyDown(KeyCode.Escape) && gameOver)
        {
            gamePaused = !gamePaused;
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
            score = 0;
            gameOver = false;
        }

    }
    public void RestartGameViaButton()
    {
        DontDestroyOnLoad(gameObject);

        gamePaused = false;
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        score = 0;
        gameOver = false;

    }

    public void ScreenShake(float shakeTime, float shakeAmount)
    {
        if (shakeTime * shakeAmount > this.shakeTime * this.shakeAmount)
        {
            this.shakeAmount = shakeAmount;
            this.shakeTime = shakeTime;
        }
    }
    private void Shaking()
    {
        if (shakeTime > 0)
        {
            camera.transform.localPosition = Random.insideUnitSphere * shakeAmount;
            shakeTime -= Time.deltaTime * shakeDecrease;
        }
        else
        {
            shakeTime = 0;
        }
    }
    void ResetCamera()
    {
        camera = FindAnyObjectByType<Camera>();
    }

    private void ManageGameMenu()
    {
        if (gameStarted)
        {
            startMenu.SetActive(true);
        }
        else
        {
            startMenu.SetActive(false);
        }

        if (gamePaused && !gameStarted && !gameOver)
        {
            pauseMenu.SetActive(true);
        } 
        else
        {
            pauseMenu.SetActive(false);
        }

        if (gameOver)
        {
            gameOverMenu.SetActive(true);
        }
        else
        {
            gameOverMenu.SetActive(false);
        }
    }

    private void ManageDifficulity()
    {
        difficulity = difficulity + (int)(score / 500);
        if (difficulity > 20)
        {
            difficulity = 20;
        }
    }
    

    void Start()
    {
        
        if (GameObject.FindGameObjectsWithTag("GameManager").Length > 1)
        {
            Destroy(gameObject);
        }
        ResetCamera();

        gameStarted = true;
        gamePaused = true;
    }

    void Update()
    {
        GameStatus();
        RestartGame();
        ResetCamera();
        Shaking();
        HighScore();
        ManageGameMenu();
        ManageDifficulity();
    }
}
