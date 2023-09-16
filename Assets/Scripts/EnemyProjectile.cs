using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    public int damageToPlayer = 25;

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        QuonkController quonkController = collision.gameObject.GetComponent<QuonkController>();

        if (quonkController != null)
        {
            Debug.Log("Enemy projectile collided with player");
            quonkController.TakeDamage(damageToPlayer);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("DeleteProjectiles") || collision.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            Destroy(gameObject);
        }
    }
}
