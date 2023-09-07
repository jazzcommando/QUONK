using UnityEngine;

public class ShootingGroundEnemy : GroundEnemy
{
    public int burstSize = 3;           // Number of shots in each burst
    public float burstInterval = 2f;    // Time between each burst
    public float burstFireRate = 0.2f;  // Fire rate between each shot in the burst
    public float movementPause = 1f;    // Duration of movement pause when shooting
    public float projectileSpeed;
    public GameObject projectilePrefab;
    public Transform shootPoint;
  
    private bool isShooting = false;
    private float timeSinceLastBurst = 0f;
    private int shotsFired = 0;

    protected override void Update()
    {
        base.Update();

        if (!isShooting && player != null && canTakeDamage)
        {
            MoveTowardsPlayer();
        }

        // Check if it's time to shoot another burst
        if (isShooting && shotsFired < burstSize)
        {
            if (Time.time - timeSinceLastBurst >= burstFireRate)
            {
                FireShot();
                timeSinceLastBurst = Time.time;
            }
        }
        else if (isShooting && shotsFired >= burstSize)
        {
            // Reset shooting state after the burst is complete
            isShooting = false;
            shotsFired = 0;
            StartCoroutine(MovementPause());
        }
        else if (!isShooting && Time.time - timeSinceLastBurst >= burstInterval)
        {
            // Start a new burst
            isShooting = true;
            timeSinceLastBurst = Time.time;
        }
    }

    protected override void Die()
    {
        // Add any special effects for this shooting enemy's death

        // Call the base Die() method to handle common death functionality
        base.Die();
    }

    private void FireShot()
    {
        // Calculate the direction to the player
        Vector2 directionToPlayer = (player.position - shootPoint.position).normalized;

        // Instantiate the projectile and set its direction
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = directionToPlayer * projectileSpeed;

        shotsFired++; // pour logique burst
    }

    private System.Collections.IEnumerator MovementPause()
    {
        // Pause the enemy's movement while shooting
        canTakeDamage = false;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(movementPause);

        canTakeDamage = true;
    }
}
