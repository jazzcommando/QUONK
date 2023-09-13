using UnityEngine;

public class GroundEnemy : Enemy
{

    protected override void Update()
    {
        if (playerTransform != null && canTakeDamage)
        {
            MoveTowardsPlayer();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
       
        QuonkController player = collision.gameObject.GetComponent<QuonkController>();
        if (player != null)
        {  
            player.TakeDamage(damageOnCollide);
        }
        //   else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        //   {
        //       rb.velocity = Vector2.zero;
        //    rb.angularVelocity = 0f;
        //   }
    }

    protected virtual void MoveTowardsPlayer()
    {
        // Calculate the direction to the player along the horizontal axis
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Vector2 horizontalDirection = new Vector2(directionToPlayer.x, 0f).normalized;

        // Move the ground enemy towards the player on the horizontal axis
        transform.Translate(horizontalDirection * movementSpeed * Time.deltaTime);
    }

}