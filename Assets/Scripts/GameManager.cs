using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI scoreText;

    public int score = 0;

    private bool playerHasDied = false;
    private const string HighScoreKey = "HighScore";


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
        UpdateScoreText();
        deathText.gameObject.SetActive(false);
    }



    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

   
    public void UpdateDeathText()
    {
        // ?????? pq vide
    }

    private void Update()
    {
        if (playerHasDied)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        UpdateScoreText();

    }

    public void PlayerDied()
    {
        playerHasDied = true; 
        deathText.gameObject.SetActive(true);
        deathText.text = "YOU DIED\nScore: " + score + "\nPress SPACE to Restart";

    }

    public void RestartGame()
    {
        playerHasDied = false;
        score = 0;
        FindObjectOfType<QuonkController>().currentHealth = FindObjectOfType<QuonkController>().maxHealth;
        FindObjectOfType<QuonkController>().UpdateHealthText();
        UpdateDeathText();
    }
}
