using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuonkController : MonoBehaviour
{
    public float acceleration = 20f;
    public float topSpeed = 5f;
    public float deceleration = 10f;
    public float jumpForce = 5f;

    public int maxHealth = 100;
    public int currentHealth;

    public bool canDie = true;
    public bool godMode = false;

    public TextMeshProUGUI healthText;
    public AudioClip deathSound;

    private float moveX; // Added a variable to store the horizontal input
    private float currentSpeed = 0f;

    private bool isJumping = false;
    private bool isGrounded = false;
    private bool stopJumping = false; // Added a flag to indicate when to stop jumping

    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private GameManager gameManager;
    private AudioSource audioSource;
    private WeaponSwitching weaponSwitcher;

    private Vector2 inputDirection;
    private LayerMask groundLayer; // Added a variable to cache the ground layer mask


    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        groundLayer = LayerMask.GetMask("Ground"); // Cache the ground layer mask

        gameManager = FindObjectOfType<GameManager>();

    }

    private void Update()
    {
        HandleGunFlip();

        // movement
        moveX = Input.GetAxisRaw("Horizontal");

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }

        // Detect when the jump key is released
        if (Input.GetButtonUp("Jump"))
        {
            stopJumping = true; // Set the flag to true
        }

        if (currentHealth <= 0)
        {
            Die();
        }


    }

    private void HandleGunFlip()
    {
        // flipping logic
        if (moveX > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    public void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void TakeDamage(int damage)
    {
        if (godMode)
        {
            return;
        }
        else
        {
            UpdateHealthText();
            currentHealth -= damage;
        }
    }

    private void FixedUpdate() // fixedupdate pour tout ce qui est physique pcq fixedupdate ne dépend pas du framerate
    {
        // Calculate the desired horizontal velocity based on the input direction
        float targetVelocityX = moveX * topSpeed;

        // Accelerate the player towards the target velocity
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetVelocityX, acceleration * Time.fixedDeltaTime);

        // Set the final velocity with respect to the y-axis (jumping)
        Vector2 velocity = new Vector2(currentSpeed, rb.velocity.y);

        // Apply the movement to the rigidbody
        rb.velocity = velocity;

        // Check if the player is grounded using a circle cast
        float rayLength = 0.1f;
        Vector2 rayOrigin = bc.bounds.center - new Vector3(0, bc.bounds.extents.y);
        float circleRadius = 0.2f; // You can adjust this value as you like
        RaycastHit2D hit = Physics2D.CircleCast(rayOrigin, circleRadius, Vector2.down, rayLength, groundLayer);
        isGrounded = hit.collider != null;


        // Jumping logic
        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
        }


        // Reduce the vertical velocity when the jump ends
        if (stopJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // You can adjust this factor as you like
            stopJumping = false; // Reset the flag to false
        }
    }

    public void Die()
    {
        gameManager.PlayerDied();
        audioSource.PlayOneShot(deathSound);
    }

    private void ResetScene()
    {
        // Reload the current scene to reset the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}