using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public float movementSpeed = 2f;
    public float bobbingSpeed = 1.5f;
    public float bobbingAmount = 0.2f;
    public int damageOnCollide = 25;
    public int scoreValue = 15;
    public float damageFlashDuration = 0.2f; 
    public Color damageColor = Color.red;
   // private GameManager gameManager;

    protected Transform player; // Reference to the player's transform
    protected Vector2 initialPosition;
    protected float currentHealth;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected bool canTakeDamage = true;
    

    protected virtual void Start()
    {
        initialPosition = transform.position;
        currentHealth = maxHealth;

        // Find the player by tag during runtime
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player GameObject not found! Make sure the player has a tag 'Player'.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); // <-- Assign the Rigidbody2D component
    }

    protected virtual void Update()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector2 directionToPlayer = (player.position - transform.position).normalized;

            // Apply bobbing motion
            float bobbingOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
            Vector2 bobbingMovement = Vector2.up * bobbingOffset;

            // Calculate the final movement vector
            Vector2 movementVector = (directionToPlayer + bobbingMovement).normalized;

            // Move the enemy towards the player
            transform.Translate(movementVector * movementSpeed * Time.deltaTime);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        QuonkController quonkController = collision.gameObject.GetComponent<QuonkController>();
        if (quonkController != null)
        {
            quonkController.TakeDamage(damageOnCollide);
            Debug.Log("Enemy collided with player");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(ShowDamageFlash());

        if (currentHealth <= 0f)
        {
            Die();
        }
    }


    protected virtual void Die()
    {
        Destroy(gameObject);

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.score += scoreValue;
        }

    }

    protected IEnumerator ShowDamageFlash()
    {
        // Set the flag to prevent taking damage during the red flash
        canTakeDamage = false;

        // Change sprite color to damage color
        spriteRenderer.color = damageColor;

        // Wait for the red flash duration
        yield return new WaitForSeconds(damageFlashDuration);

        // Reset sprite color back to normal
        spriteRenderer.color = Color.white;

        // Set the flag back to allow taking damage
        canTakeDamage = true;
    }
}