using UnityEngine;

public class MenuScript : MonoBehaviour
{

    GameManager gameManager;

    public void StartGameButton()
    {
        gameManager.gamePaused = false;
        gameManager.gameStarted = false;
    }

    public void ContinueGameButton()
    {
        gameManager.gamePaused = false;
    }

    public void ResetGameButton()
    {
        gameManager.RestartGameViaButton();
    }

    public void OptionsButton()
    {
        Debug.Log("Optioning XD");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (gameObject.tag == "StartMenu")
        {
            gameManager.startMenu = gameObject;
        }
        if (gameObject.tag == "PauseMenu")
        {
            gameManager.pauseMenu = gameObject;
        }
        if (gameObject.tag == "GameOverMenu")
        {
            gameManager.gameOverMenu = gameObject;
        }
    }

    void Update()
    {
        
    }
}
