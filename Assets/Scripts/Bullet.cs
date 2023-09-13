using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float bulletPushForce = 10f; 

    void Start()
    {
        Destroy(gameObject, 2f); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
       
        if (enemy != null)
        {
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>(); 
            if (enemyRb != null)
            {
                enemyRb.AddForce(collision.relativeVelocity.normalized * -bulletPushForce, ForceMode2D.Impulse);
                // applique bulletPushForce dans la direction du projectile ( --> collision.relativeVelocity.normalized)
            }

            enemy.TakeDamage(damage);
            Destroy(gameObject); 

        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("DeleteProjectiles"))
        {
            Destroy(gameObject);
        }
    }
} 
