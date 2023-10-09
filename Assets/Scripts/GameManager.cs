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

    public bool playerHasDied = false;


    private void Awake(){
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start(){

        Cursor.lockState = CursorLockMode.Confined;

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        
        deathText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        UpdateScoreText();

    }

    private void UpdateScoreText(){
        scoreText.text = "Score: " + score.ToString() + "\nHigh Score: " + highScore.ToString(); 

        if (score > highScore){
            scoreText.text += "\nNew High Score!";
        }

    }

    private void Update(){
        if (playerHasDied){
            if (Input.GetKeyDown(KeyCode.Space)){
                Debug.Log("GameManager received spacebar input");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        UpdateScoreText();
    }

    public void PlayerDied(){
        playerHasDied = true; 
        deathText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);

        if (score > highScore){
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        deathText.text = "YOU DIED\nScore: " + score + "\nPress SPACE to Restart" + "\nHigh Score: " + highScore;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            return;
        }
    }

    public void RestartGame(){
        playerHasDied = false;
        score = 0;
        FindObjectOfType<QuonkController>().currentHealth = FindObjectOfType<QuonkController>().maxHealth;
        FindObjectOfType<QuonkController>().UpdateHealthText();
    }
}
