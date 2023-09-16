using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI scoreText;


    public int score = 0;
    public int highScore = 0;

    private bool playerHasDied = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        
        deathText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        UpdateScoreText();

    }



    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString() + "\nHigh Score: " + highScore.ToString(); 

        if (score > highScore)
        {
            scoreText.text += "\nNew High Score!";
        }

    }

   

    private void Update()
    {
        if (playerHasDied)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("GameManager received spacebar input");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        UpdateScoreText();

      // DEBUG
      // if (Input.GetKeyDown(KeyCode.R))
      // {
      //    highScore = 0;
      //    Debug.Log("Set high score to 0");
      // }

    }

    public void PlayerDied()
    {
        playerHasDied = true; 
        deathText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        deathText.text = "YOU DIED\nScore: " + score + "\nPress SPACE to Restart" + "\nHigh Score: " + highScore;

    }

    public void RestartGame()
    {
        playerHasDied = false;
        score = 0;
        FindObjectOfType<QuonkController>().currentHealth = FindObjectOfType<QuonkController>().maxHealth;
        FindObjectOfType<QuonkController>().UpdateHealthText();
    }
}
