using System.Collections;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    public bool shootProjectiles = true;
    public float projectileSpeed;
    public float enemyFireRate = 2f; 
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
            Vector2 directionToPlayer = (playerTransform.position - shootPoint.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = directionToPlayer * projectileSpeed;

            canShoot = false;

            StartCoroutine(ShootCooldown());
        }
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(1f / enemyFireRate);
        canShoot = true;
    }
}
