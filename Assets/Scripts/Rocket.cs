using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosionPrefab;
    public AudioClip bounceSound;

    public bool explodeOnContactGround = true;
    public bool explodeOnContactEnemies = false;
    public bool enableSoundOnBounce = true;

    public int angularVelocity; // For grenades
    public int explodeTimer = 0;

    private AudioSource rocketAudioSource;

    void Start()
    {
        Destroy(gameObject, 6f);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rocketAudioSource = GetComponent<AudioSource>();

        if (explodeTimer > 0)
        {
            StartCoroutine(StartExplodeTimer());
        }

        if (angularVelocity == 0)
        {
            rb.freezeRotation = true; // Rockets (freeze rot)
        }
        else
        {
            rb.angularVelocity = angularVelocity; // Grenades (needs to SPEEN)
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
            AudioSource.PlayClipAtPoint(bounceSound, (transform.position), 1f);
            // PlayClipAtPoint() creates an audio source and destroys it once the sound is over,
            // this makes it so that the bounce sound doesn't abruptly stop once the grenade has exploded
        }
    }

    private void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator StartExplodeTimer()
    {
        while (explodeTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            explodeTimer--;

            if (explodeTimer <= 0)
            {
                Explode();
            }
        }
    }
}
