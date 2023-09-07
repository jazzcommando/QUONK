using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   
    public int damage;
    public float bulletPushForce = 100f; 


    void Start()
    {
        // auto détruit après 2 secondes
        Destroy(gameObject, 2f); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
       

        if (enemy != null)
        {
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>(); // get rb pour appliquer pushforce 
            if (enemyRb != null)
            {
                // applique bulletPushForce dans la direction du projectile (collision.relativeVelocity.normalized)
                enemyRb.AddForce(collision.relativeVelocity.normalized * -bulletPushForce, ForceMode2D.Impulse); 
            }

            enemy.TakeDamage(damage);
            Destroy(gameObject); 
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("DeleteProjectiles"))
        {
            Destroy(gameObject);
            Debug.Log("Projectile collided with ground");
        }
    }
}
