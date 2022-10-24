using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    GS_PAUSEMENU, GS_GAME, GS_GAME_OVER, GS_LEVELCOMPLETED
}
public enum Hue
{
    RED, GREEN, BLUE
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameState currentGameState;
    public Canvas inGameCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas endGameCanvas;
    public Text batteriesText;
    public Text timerText;
    public Text enemiesDefeatedText;
    public Image[] lightsTab;
    public Image[] livesTab;
    public LightController lightController;
    public Text score;

    private float timer;
    private int lives = 3;
    private int enemiesDefeated;
    private int batteries;
    private bool hasBlueHue, hasRedHue, hasGreenHue;
   

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InGame();
        UpdateLivesTab();

        inGameCanvas.enabled = true;
        pauseMenuCanvas.enabled = false;
        levelCompletedCanvas.enabled = false;
        endGameCanvas.enabled = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentGameState == GameState.GS_PAUSEMENU)
                InGame();
            else if(currentGameState == GameState.GS_GAME)
                PauseMenu();
        }

        if(Input.GetKey(KeyCode.S) && currentGameState == GameState.GS_GAME_OVER)
        {
            ReloadLevel();
        }

        if (currentGameState == GameState.GS_GAME)
        {
            UpdateTimer();
        }
    }

    public void OnNextLevelButtonClicked()
    {
        SceneManager.LoadScene("Scenes/Level2");
    }

    private void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        pauseMenuCanvas.enabled = currentGameState == GameState.GS_PAUSEMENU;
        inGameCanvas.enabled = newGameState == GameState.GS_GAME;
        levelCompletedCanvas.enabled = currentGameState == GameState.GS_LEVELCOMPLETED;
        endGameCanvas.enabled = currentGameState == GameState.GS_GAME_OVER;
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }
    
    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    private void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
         SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void LevelCompleted()
    {
        var finalScore = 100 * enemiesDefeated + 25 * lives + 10 * batteries - (int)(timer % 60);
        if (finalScore < 0) finalScore = 0;
        score.text = finalScore.ToString();
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }

    public void AddDefeatedEnemy()
    {
        enemiesDefeated++;
        enemiesDefeatedText.text = enemiesDefeated.ToString();
    }

    public void AddLive()
    {
        lives++;
        UpdateLivesTab();
    }

    public void RemoveLive()
    {
        lives--;
        UpdateLivesTab();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void ResetLightPower()
    {
        lightController.BatteryTakenEvent();
    }

    public void AddBattery()
    {
        batteries++;
        ResetLightPower();
        var shownPrefix = "";
        if (batteries < 10) shownPrefix += "0";
        batteriesText.text = shownPrefix + batteries;
    }

    public void AddHue(Hue hue)
    {
        switch (hue)
        {
            case Hue.RED:
                hasRedHue = true;
                lightsTab[0].color = Color.red;
                break;
            case Hue.GREEN:
                hasGreenHue = true;
                lightsTab[1].color = Color.green;
                break;
            case Hue.BLUE:
                hasBlueHue = true;
                lightsTab[2].color = Color.blue;
                break;
        }
    }

    public bool AreAllHoesTaken()
    {
        return hasBlueHue && hasGreenHue && hasRedHue;
    }

    public bool IsPlayerDead()
    {
        return lives <= 0;
    }

    public int GetLives()
    {
        return lives;
    }

    public float GetTimer()
    {
        return timer;
    }

    private void UpdateLivesTab()
    {
        foreach (var element in livesTab)
        {
            element.enabled = false;
        }

        for (var i = 0; i < lives; i++)
        {
            livesTab[i].enabled = true;
        }
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;
        timerText.text = $"{Mathf.Floor(timer/60f):00}:{(int)timer%60:00}";
    }
}

