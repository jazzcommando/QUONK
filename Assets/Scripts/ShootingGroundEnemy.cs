using UnityEngine;

public class ShootingGroundEnemy : GroundEnemy
{
    public int burstSize = 3;           
    public float burstInterval = 2f;    
    public float burstFireRate = 0.2f;  
    public float movementPause = 1f; //pause after shooting
    public float projectileSpeed;
    public GameObject projectilePrefab;
    public Transform shootPoint; 
  
    private bool isShooting = false;
    private float timeSinceLastBurst = 0f;
    private int shotsFired = 0;

    protected override void Update()
    {
       // base.Update();

        if (!isShooting && playerTransform != null && canTakeDamage)
        {
            MoveTowardsPlayer();
        }

      
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
            
            isShooting = false;
            shotsFired = 0;
            StartCoroutine(MovementPause());
        }
        else if (!isShooting && Time.time - timeSinceLastBurst >= burstInterval)
        {
            
            isShooting = true;
            timeSinceLastBurst = Time.time;
        }
    }

    protected override void Die()
    {
        base.Die();
    }

    private void FireShot()
    {

        Vector2 directionToPlayer = (playerTransform.position - shootPoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = directionToPlayer * projectileSpeed;

        shotsFired++; 
    }

    private System.Collections.IEnumerator MovementPause()
    {
        canTakeDamage = false;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(movementPause);

        canTakeDamage = true;
    }
}
