using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public LayerMask damageableLayers;
    public int damage = 50;
    public float explosionRadius = 5f;
    public float explosionForce = 1000f;

    void Start()
    {
        // Explode!
        Explode();
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageableLayers);
        foreach (Collider2D hitCollider in colliders)
        {
            Rigidbody2D rb = hitCollider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Apply explosion force to rigidbodies in the radius
                Vector2 direction = rb.transform.position - transform.position;
                rb.AddForce(direction.normalized * explosionForce, ForceMode2D.Impulse);
            }

            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Deal damage to Enemy component
                enemy.TakeDamage(damage);
            }
        }

        // Destroy the explosion after some time (optional)
        Destroy(gameObject, 0.5f);
    }

    // For debugging, draw the explosion radius in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
