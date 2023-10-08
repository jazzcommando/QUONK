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
    public float grapplingHookForce = 10f;
    public float coyoteTime = 0.12f;

    public int currentHealth;
    public int maxHealth = 100;
    public float playerDamageFlashDuration = 0.25f;

    public bool canDie = true;
    public bool hasDied = false;
    public bool godMode = false;

    public TextMeshProUGUI healthText;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    public GameObject gunAxis;
    public LineRenderer hookLineRender;
    public LayerMask hookableLayer;
    public Color damageColor = Color.red;

    private float moveX;
    private float currentSpeed = 0f;
    private float coyoteTimeTimer;

    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isHooked = false;
    private bool stopJumping = false;
    private bool canTakeDamage = true;

    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private AudioSource audioSource;
    private WeaponSwitching weaponSwitcher;
    private SpriteRenderer spriteRenderer;
    private Vector2 inputDirection;
    private Vector2 hookPoint;
    private GameObject hookObject;
    private LayerMask groundLayer;

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundLayer = LayerMask.GetMask("Ground"); // Cache the ground layer mask
    }

    private void Update()
    {
        if (hasDied == false)
        {
            HandleGunFlip();
            moveX = Input.GetAxisRaw("Horizontal");
        }

        if (isGrounded)
        {
            coyoteTimeTimer = coyoteTime;
        }
        else
        {
            coyoteTimeTimer -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && !hasDied)
        {
            if (isHooked || coyoteTimeTimer > 0f){
                isJumping = true;
                DisconnectGrapplingHook();
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            stopJumping = true;
            coyoteTimeTimer = 0f;
        }

        if (Input.GetMouseButtonDown(1) && !isHooked)
        {
            ShootGrapplingHook();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            DisconnectGrapplingHook();
        }

        if (currentHealth <= 0 && hasDied == false)
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
        if (godMode || hasDied)
        { //hasDied check to prevent health from going into negative once player has died 
            UpdateHealthText(); // we still need to update health text, otherwise the health will not go to 0 once player dies
            return;
        }
        else
        {
            UpdateHealthText();
            StartCoroutine(ShowDamageFlash());
            currentHealth -= damage;
            AudioSource.PlayClipAtPoint(hurtSound, transform.position, 5500f);
        }
    }

    private void ShootGrapplingHook()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, hookableLayer);

        if (hit.collider != null)
        {
            isHooked = true;
            hookPoint = hit.point;
        }
    }

    private void DisconnectGrapplingHook()
    {
        isHooked = false;
        hookLineRender.positionCount = 0;
    }

    private void FixedUpdate()
    {

        if (isHooked)
        {
            Vector2 direction = (hookPoint - (Vector2)transform.position).normalized;
            rb.velocity = direction * grapplingHookForce;
            hookLineRender.positionCount = 2;
            hookLineRender.SetPosition(0, transform.position);
            hookLineRender.SetPosition(1, hookPoint);
        }
        else
        {
            float targetVelocityX = moveX * topSpeed;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetVelocityX, acceleration * Time.fixedDeltaTime);
            Vector2 velocity = new Vector2(currentSpeed, rb.velocity.y);
            rb.velocity = velocity;

            // ground check using a circlecast
            float rayLength = 0.1f;
            Vector2 rayOrigin = bc.bounds.center - new Vector3(0, bc.bounds.extents.y);
            float circleRadius = 0.2f;
            RaycastHit2D hit = Physics2D.CircleCast(rayOrigin, circleRadius, Vector2.down, rayLength, groundLayer);
            isGrounded = hit.collider != null;


            if (isJumping)
            { // jumping logic
                rb.AddForce(new Vector2(0f, jumpForce));
                isJumping = false;
            }

            if (stopJumping)
            { // reduce the vertical velocity when the jump ends
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                stopJumping = false;
            }

        }

    }

    protected IEnumerator ShowDamageFlash()
    {
        canTakeDamage = false;
        Debug.Log("Player now has iframes");
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(playerDamageFlashDuration);
        spriteRenderer.color = Color.white;
        canTakeDamage = true;
        Debug.Log("Player no longer has iframes");
    }

    public void Die()
    {
        Debug.Log("Die called");
        hasDied = true;
        gunAxis.SetActive(false);
        GameManager.Instance.PlayerDied();
        AudioSource.PlayClipAtPoint(deathSound, (transform.position), 1f);
        transform.rotation = Quaternion.Euler(0, 0, -90);
    }

}