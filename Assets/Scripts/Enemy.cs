using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int damageOnCollide = 25;
    public int scoreValue = 15;
    public float movementSpeed = 2f;
    public float bobbingSpeed = 1.5f;
    public float bobbingAmount = 0.2f;
    public float damageFlashDuration = 0.2f;
    public float playerPushbackForce = 0.5f;

    public Color damageColor = Color.red;
    public GameObject corpsePrefab;

    protected Vector2 initialPosition;
    protected Transform playerTransform;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected Rigidbody2D playerRb;

    protected float currentHealth;
    
    protected bool canTakeDamage = true; // flag for iframe during red flash 
    protected bool hasDied = false;
    

    protected virtual void Start()
    {
        initialPosition = transform.position;
        currentHealth = maxHealth;

        // find the player by tag (can't assign player transform in the inspector due to enemies being prefabs)
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>(); 


        if (playerTransform == null)
        {
            Debug.LogError("Player GameObject not found! Make sure the player has a tag 'Player'.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); 
    }

    protected virtual void Update()
    {
        FlyTowardsPlayer();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        QuonkController player = collision.gameObject.GetComponent<QuonkController>();
        if (player != null)
        {
            player.TakeDamage(damageOnCollide);
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            playerRb.AddForce(-directionToPlayer * playerPushbackForce);
            
        }
    }


    protected virtual void FlyTowardsPlayer()
    {
        if (playerTransform != null)
        {

            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

            // bobbing motion logic
            float bobbingOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
            Vector2 bobbingMovement = Vector2.up * bobbingOffset;

            Vector2 movementVector = (directionToPlayer + bobbingMovement).normalized;
            transform.Translate(movementVector * movementSpeed * Time.deltaTime);
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

        if (hasDied == true)
        {
            return; // prevents Die() from being called multiple times if multiple projectiles kill one enemy at the same time (eg, from the shotgun)
        }

        Destroy(gameObject);

        GameObject corpse = Instantiate(corpsePrefab, transform.position, Quaternion.identity);
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized; // get vector facing away from player for corpse travel direction
        corpse.GetComponent<Rigidbody2D>().velocity = -directionToPlayer * -currentHealth; // * -currentHealth --> the less health, the further the corpse flies
        corpse.GetComponent<Rigidbody2D>().angularVelocity = -currentHealth * 10;

        GameManager.Instance.score += scoreValue;

        hasDied = true;

    }

    protected IEnumerator ShowDamageFlash()
    {
        canTakeDamage = false;
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = Color.white;
        canTakeDamage = true;
    }
}