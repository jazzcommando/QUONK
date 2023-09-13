using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuonkController : MonoBehaviour
{
    public float acceleration = 20f;
    public float topSpeed = 5f;
    public float deceleration = 10f;
    public float jumpForce = 5f;
    public float playerDamageFlashDuration = 0.25f;
    public Color damageColor = Color.red;

    public int maxHealth = 100;
    public int currentHealth;

    public bool canDie = true;
    public bool hasDied = false;
    public bool godMode = false;

    public TextMeshProUGUI healthText;
    public AudioClip deathSound;
    public GameObject gunAxis;

    private float moveX; 
    private float currentSpeed = 0f;

    private bool isJumping = false;
    private bool isGrounded = false;
    private bool stopJumping = false; 

    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private GameManager gameManager;
    private AudioSource audioSource;
    private WeaponSwitching weaponSwitcher;
    private SpriteRenderer spriteRenderer;
    private Vector2 inputDirection;
    private LayerMask groundLayer; 


    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundLayer = LayerMask.GetMask("Ground"); // Cache the ground layer mask

        gameManager = FindObjectOfType<GameManager>();

    }

    private void Update()
    {
        

        // movement
        if (hasDied == false)
        {
            HandleGunFlip();
            moveX = Input.GetAxisRaw("Horizontal");
        }

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded && hasDied == false)
        {
            isJumping = true;
        }

        // Detect when the jump key is released
        if (Input.GetButtonUp("Jump"))
        {
            stopJumping = true; 
        }

        if (currentHealth <= 0)
        {
            Die();
        }


    }

    private void HandleGunFlip()
    {
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
        if (godMode || hasDied) //hasDied check to prevent health from going into negative once player has died 
        {
            UpdateHealthText(); // we still need to update health text, otherwise the health will not go to 0 once player dies
            return;
        }
        else
        {
            UpdateHealthText();
            StartCoroutine(ShowDamageFlash());
            currentHealth -= damage;
        }
    }

    private void FixedUpdate() // fixedupdate pour tout ce qui est physique pcq fixedupdate ne dépend pas du framerate
    {
        
        float targetVelocityX = moveX * topSpeed;

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetVelocityX, acceleration * Time.fixedDeltaTime);

       
        Vector2 velocity = new Vector2(currentSpeed, rb.velocity.y);

  
        rb.velocity = velocity;

        // Check if the player is grounded using a circle cast
        float rayLength = 0.1f;
        Vector2 rayOrigin = bc.bounds.center - new Vector3(0, bc.bounds.extents.y);
        float circleRadius = 0.2f; // You can adjust this value as you like
        RaycastHit2D hit = Physics2D.CircleCast(rayOrigin, circleRadius, Vector2.down, rayLength, groundLayer);
        isGrounded = hit.collider != null;


        // jumping logic
        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
        }


        // reduce the vertical velocity when the jump ends
        if (stopJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); 
            stopJumping = false; 
        }
    }

    protected IEnumerator ShowDamageFlash()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(playerDamageFlashDuration);
        spriteRenderer.color = Color.white;
    }

    public void Die()
    {
        hasDied = true;
        gunAxis.SetActive(false); 
        gameManager.PlayerDied();
        AudioSource.PlayClipAtPoint(deathSound, (transform.position), 1f);
        transform.rotation = Quaternion.Euler(0, 0, -90);
    }

}