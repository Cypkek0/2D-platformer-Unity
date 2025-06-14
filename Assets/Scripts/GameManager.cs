using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Transactions;

public enum GameState { [InspectorName("Gameplay")] GAME, [InspectorName("Pause")] PAUSE_MENU, [InspectorName("Level completed")] LEVEL_COMPLETED, [InspectorName("Options")] OPTIONS, [InspectorName("Game Over")] GAMEOVER }
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentGameState = GameState.GAME;
    public Canvas inGameCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas optionsCanvas;
    public Canvas gameOverCanvas;
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public TMP_Text killCount;
    public TMP_Text highScoreText;
    
    public TMP_Text scoreTextEnd;
    public TMP_Text graphicsNameText;
    private string keyHighScore1 = "HighScoreLevel1";
    private string keyHighScore2 = "HighScoreLevel2";
    private float gameTime = 0f;
    private int score = 0;
    private int lives = 3;
    private int MaxHealth = 5;
    private int enemiesKilled = -1;
    public Image[] LifeIcon;
    public Image Killcounter;
    public Image redKeyIcon;
    public Image greenKeyIcon;
    public Image blueKeyIcon;
    [SerializeField] private AudioClip soundTrack;
    private AudioSource source;
  

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.Play();
    }

    public void SetGameState(GameState newGameState)
    {
        if(currentGameState == newGameState) { return; }

        currentGameState = newGameState;
        if(currentGameState == GameState.GAME)
        {
            
        
        optionsCanvas.enabled = false;
            Time.timeScale = 1.0f;
            pauseMenuCanvas.enabled = false;
            inGameCanvas.enabled = true;
            if(!source.isPlaying) { source.Play(); }
            

        }else if (currentGameState == GameState.PAUSE_MENU)
        {
            
            optionsCanvas.enabled = false;
            inGameCanvas.enabled = false;
            pauseMenuCanvas.enabled = true;
            source.Pause();
            Time.timeScale = 0.0f;
        }else if(currentGameState == GameState.LEVEL_COMPLETED)
        {
            
            Scene currentScene = SceneManager.GetActiveScene();
            if(currentScene.name == "Level 1")
            {
                int highScore = PlayerPrefs.GetInt(keyHighScore1);
                if(highScore < score)
                {
                    highScore = score;
                    PlayerPrefs.SetInt(keyHighScore1, score);
               
                }
                highScoreText.text = ("High Score:" + highScore);
                scoreTextEnd.text = ("Score:" + score);

            } else if(currentScene.name == "Level 2")
            {
                int highScore = PlayerPrefs.GetInt(keyHighScore2);
                if (highScore < score)
                {
                    highScore = score;
                    PlayerPrefs.SetInt(keyHighScore2, score);

                }
                highScoreText.text = ("High Score:" + highScore);
                scoreTextEnd.text = ("Score:" + score);
            }
            Time.timeScale = 0.0f;
            pauseMenuCanvas.enabled = false;
            inGameCanvas.enabled = false;
            levelCompletedCanvas.enabled = true;
        }
        else if (currentGameState == GameState.OPTIONS) 
        {
            
            Time.timeScale = 0.0f;
            inGameCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            optionsCanvas.enabled = true;
            
       
        } else if(currentGameState == GameState.GAMEOVER)
        {
            Time.timeScale = 0.0f;
            inGameCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            optionsCanvas.enabled = false;
            gameOverCanvas.enabled = true;

        }
       
    }
    public void SetVolume(float vol) 
    { 
        AudioListener.volume = vol;
    }
    public void Options()
    {
        SetGameState(GameState.OPTIONS);
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void PauseMenu()
    {
        SetGameState(GameState.PAUSE_MENU);
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void InGame()
    {
        SetGameState(GameState.GAME);
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void LevelCompleted()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
        EventSystem.current.SetSelectedGameObject(null);
    }
    // Update is called once per frame
    void Update()
    {
        if(currentGameState == GameState.GAME)
        {
            gameTime += Time.deltaTime; 
            DisplayTime(gameTime);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if(currentGameState == GameState.GAME)
            {
                PauseMenu();
  
            } else if(currentGameState == GameState.PAUSE_MENU)

            {
                InGame();
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void  AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }
    public void EnemyKilled()
    {
        enemiesKilled += 1;
        killCount.text = enemiesKilled.ToString();
    }
    public int GetLives()
    {
        return lives;
    }
    public void ManageHealth(int health)
    {
        lives += health;
        if(lives >MaxHealth)
        {
            lives = MaxHealth;
        } 
        for(int i = 0; i < MaxHealth; i++) 
        { 
            if(lives > i) 
            {
                LifeIcon[i].enabled = true;
            } else
            {
                LifeIcon[i].enabled=false;
            }
        }

    }
    public void AddKeys(Color gemColor)
    {
        if(gemColor == Color.red)
        {
            redKeyIcon.enabled = true;
        }
        else if (gemColor == Color.green)
        {
            greenKeyIcon.enabled = true;
        }
        else if (gemColor == Color.blue)
        {
            blueKeyIcon.enabled = true;
        }
    }
    public void OnResumeButtonClicked()
    {
        InGame();
    }
    public void OnPlusButtonClicked()
    {
        QualitySettings.IncreaseLevel();
        graphicsNameText.text = QualitySettings.names[QualitySettings.GetQualityLevel()].ToString();
    }
    public void OnLevel2ButtonClicked()
    {
        ResetGameManager();
        SceneManager.LoadScene("Level 2");
    }
    public void OnMinusButtonClicked()
    {
        QualitySettings.DecreaseLevel();
        graphicsNameText.text = QualitySettings.names[QualitySettings.GetQualityLevel()].ToString();
    }
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnReturnToMainMenuButtonClicked()
    {
        ResetGameManager();
        SceneManager.LoadScene("MainMenu");
    }
    public void GemReset()
    {
        redKeyIcon.enabled = false;
        greenKeyIcon.enabled = false;
        blueKeyIcon.enabled = false;
    }
    void Awake()
    {
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
            Time.timeScale = 1.0f;
            optionsCanvas.enabled = false;
            gameOverCanvas.enabled = false;
            levelCompletedCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            inGameCanvas.enabled = true;
            
            AddPoints(10000);
            ManageHealth(0);
            EnemyKilled();
            GemReset();
            graphicsNameText.text = QualitySettings.names[QualitySettings.GetQualityLevel()].ToString();
            if (PlayerPrefs.HasKey(keyHighScore1) == false)
            {
                PlayerPrefs.SetInt(keyHighScore1,0);
            }
            if (PlayerPrefs.HasKey(keyHighScore2) == false)
            {
                PlayerPrefs.SetInt(keyHighScore2, 0);
            }


        } else
        {
            Debug.LogError("Duplicated Game Manager", gameObject);
        }
    }
    public void ResetGameManager()
        {
            Time.timeScale = 1.0f;
            currentGameState = GameState.GAME;
            gameTime = 0f;
            score = 0;
            lives = 3;
            enemiesKilled = -1;

            AddPoints(0);
            ManageHealth(0);
            EnemyKilled();

        GemReset();
        }


    
}
