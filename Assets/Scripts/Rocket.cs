using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosionPrefab;
    public AudioClip bounceSound;

    public int angularVelocity; // For grenades
    public bool explodeOnContactGround = true;
    public bool explodeOnContactEnemies = false;
    public int explodeTimer = 0;
    public bool enableSoundOnBounce = true;

    private AudioSource rocketAudioSource;

    void Start()
    {
        Destroy(gameObject, 6f);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rocketAudioSource = GetComponent<AudioSource>();

        if (explodeTimer > 0)
        {
            StartExplodeTimer();
        }

        if (angularVelocity == 0)
        {
            rb.freezeRotation = true; // Rockets (freeze rot)
        }
        else
        {
            rb.angularVelocity = angularVelocity; // Grenades (doit SPEEN)
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (explodeOnContactGround && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Explode();
        }

        if (explodeOnContactEnemies && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Explode();
        }

        if (enableSoundOnBounce && rocketAudioSource != null && collision.relativeVelocity.magnitude > 2f)
        {
            rocketAudioSource.PlayOneShot(bounceSound);
        }
    }

    private void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator StartExplodeTimer()
    {
        {
            yield return new WaitForSeconds(1f);
            explodeTimer--;
            Debug.Log("Explode timer:" + explodeTimer);

            if (explodeTimer <= 0)
            {
                Explode();
            }
        }
    }
}
