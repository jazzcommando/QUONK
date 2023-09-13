using System.Collections;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    public bool shootProjectiles = true;
    public float projectileSpeed;
    public float enemyFireRate = 2f; // Time between enemy shots
    public GameObject projectilePrefab;
    public Transform shootPoint;


    private bool canShoot = true;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(ShootRoutine());
    }

    protected override void Update()
    {
        FlyTowardsPlayer();
    }

    private IEnumerator ShootRoutine()
    {
        while (shootProjectiles)
        {
            if (canShoot)
            {
                Shoot();
            }
            yield return new WaitForSeconds(1f / enemyFireRate);
        }
    }

    private void Shoot()
    {
        if (playerTransform != null)
        {
            // Calculate the direction to the player
            Vector2 directionToPlayer = (playerTransform.position - shootPoint.position).normalized;

            // Instantiate the projectile and set its direction
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = directionToPlayer * projectileSpeed;

            // Set canShoot to false to prevent rapid firing
            canShoot = false;

            // Start the shoot cooldown
            StartCoroutine(ShootCooldown());
        }
    }

    private IEnumerator ShootCooldown()
    {
        // Wait for the enemy fire rate before allowing shooting again
        yield return new WaitForSeconds(1f / enemyFireRate);
        canShoot = true;
    }
}
