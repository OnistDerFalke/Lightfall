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
    public Text batteriesText;
    public Text timerText;
    public Text enemiesDefeatedText;
    public Image[] lightsTab;
    public Image[] livesTab;
    public GameObject pauseMenu;
    public GameObject endGameMenu;
    public LightController lightController;

    private float timer = 0;
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
        PauseMenu();
        UpdateLivesTab();
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.S) && currentGameState == GameState.GS_PAUSEMENU)
        {
            InGame();
        }

        if(Input.GetKey(KeyCode.S) && currentGameState == GameState.GS_GAME_OVER)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (currentGameState == GameState.GS_GAME)
        {
            UpdateTimer();
        }
    }

    public void SetGameState(GameState newGameState)
    {
        inGameCanvas.enabled = newGameState == GameState.GS_GAME;
        currentGameState = newGameState;
    }

    public void InGame()
    {
        currentGameState = GameState.GS_GAME;
        pauseMenu.SetActive(false);
        endGameMenu.SetActive(false);
    }
    
    public void GameOver()
    {
        currentGameState = GameState.GS_GAME_OVER;
        endGameMenu.SetActive(true);
    }

    public void PauseMenu()
    {
        currentGameState = GameState.GS_PAUSEMENU;
        pauseMenu.SetActive(true);
    }

    public void LevelCompleted()
    {
        currentGameState = GameState.GS_LEVELCOMPLETED;
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

