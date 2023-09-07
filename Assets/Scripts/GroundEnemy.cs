using UnityEngine;

public class GroundEnemy : Enemy
{
    // The ground enemy runs directly at the player
    protected override void Update()
    {
        if (player != null && canTakeDamage)
        {
            MoveTowardsPlayer();
        }
    }

    // Handle collisions with the ground and the player
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        QuonkController quonkController = collision.gameObject.GetComponent<QuonkController>();
        if (quonkController != null)
        {
            // If the enemy can take damage, deal damage to the player
            if (canTakeDamage)
            {
                quonkController.TakeDamage(damageOnCollide);
                Debug.Log("Ground Enemy collided with player");
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // If the collision is with the ground, stop the ground enemy
            // (you can add additional logic here, like changing direction or attacking the player)
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    protected virtual void MoveTowardsPlayer()
    {
        // Calculate the direction to the player along the horizontal axis
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        Vector2 horizontalDirection = new Vector2(directionToPlayer.x, 0f).normalized;

        // Move the ground enemy towards the player on the horizontal axis
        transform.Translate(horizontalDirection * movementSpeed * Time.deltaTime);
    }

}