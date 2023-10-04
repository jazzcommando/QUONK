using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryGun : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject shellPrefab;
    public GameObject sentryHead;
    public Transform gunPivot;
    public Transform ejectionPoint;

    public float projectileSpeed;
    public float shellEjectionSpeed;
    public float fireRate;
    public float spreadAngle = 5f;
    public float pivotSpeed = 5f;

    public int shellSpreadAngle = 30;

    public AudioClip firingSound;

    private AudioSource gunAudioSource;

    private bool canShoot = true;

    void Start()
    {
        gunAudioSource = GetComponent<AudioSource>();
        StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        while (true)
        {
            if (canShoot)
            {
                FindNearestEnemyAndFire();
                canShoot = false;
                StartCoroutine(ResetCanShoot());
            }

            yield return new WaitForSeconds(1f / fireRate);
        }
    }

    void FindNearestEnemyAndFire()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Assuming enemies have a "Enemy" tag
        Transform nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestEnemy = enemy.transform;
                nearestDistance = distance;
            }
        }

        if (nearestEnemy != null)
        {
            RotateSentryHead(nearestEnemy.position);
            Shoot();
        }
    }

    void RotateSentryHead(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)sentryHead.transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        sentryHead.transform.rotation = Quaternion.RotateTowards(sentryHead.transform.rotation, targetRotation, pivotSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        gunAudioSource.PlayOneShot(firingSound);

        GameObject projectile = Instantiate(projectilePrefab, gunPivot.position, sentryHead.transform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = sentryHead.transform.right * projectileSpeed;

        EjectShells();
    }

    void EjectShells()
    {
        GameObject shell = Instantiate(shellPrefab, ejectionPoint.position, Quaternion.identity);
        Vector2 ejectionDirection = ejectionPoint.right;
        ejectionDirection = ApplySpread(ejectionDirection, shellSpreadAngle);
        Rigidbody2D shellRb2D = shell.GetComponent<Rigidbody2D>();

        if (shellRb2D != null)
        {
            shellRb2D.velocity = ejectionDirection.normalized * shellEjectionSpeed;
            shellRb2D.angularVelocity = 360f;
        }
    }

    Vector2 ApplySpread(Vector2 direction, float spreadAngle)
    {
        float randomAngle = Random.Range(-spreadAngle, spreadAngle);
        return Quaternion.Euler(0f, 0f, randomAngle) * direction;
    }

    IEnumerator ResetCanShoot()
    {
        yield return new WaitForSeconds(1f / fireRate);
        canShoot = true;
    }
}
