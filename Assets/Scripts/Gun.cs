using System.Collections;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    public float fireRate;
    public int maxAmmo;
    public float projectileSpeed;
    public float shellEjectionSpeed;
    public float spreadAngle = 5f;
    public int bulletCount = 1;
    public int shellSpreadAngle = 30;
    public int ejectedShellsPerShot = 1;
    public bool isFullAuto = true;
    public bool ejectShells = true;
    public bool canShoot = true;

    public GameObject projectilePrefab;
    public GameObject shellPrefab;
    public Transform gunPivot;
    public Transform ejectionPoint;
    public SpriteRenderer gunSprite;
    public TextMeshProUGUI ammoText;
    public AudioClip firingSound;
    public AudioClip equipSound;

    private AudioSource gunAudioSource;
    private AudioSource weaponSwitchAudioSource;
    private Vector3 mousePosition;
    private int currentAmmo;

 


    private void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();
        gunAudioSource = GetComponent<AudioSource>();
        weaponSwitchAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        weaponSwitchAudioSource.PlayOneShot(equipSound);
    }

    private void Update()
    {
            HandleGunSprite();
            if (isFullAuto)
            {
                if (Input.GetButton("Fire1") && canShoot && currentAmmo > 0)
                {
                    Shoot();
                }
            }
            else if (Input.GetButtonDown("Fire1") && canShoot && currentAmmo > 0)
            {

                Shoot();
            }
    }

    private void HandleGunSprite()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - gunPivot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.Euler(0f, 0f, angle);
        gunSprite.flipY = angle < -90f || angle > 90f;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput != 0)
        {
            gunSprite.flipX = horizontalInput < 0;
        }
    }

    private IEnumerator ResetCanShoot()
    {
        yield return new WaitForSeconds(1f / fireRate);
        canShoot = true;
    }

    private Vector2 ApplySpread(Vector2 direction, float spreadAngle)
    {
        float randomAngle = Random.Range(-spreadAngle, spreadAngle);
        return Quaternion.Euler(0f, 0f, randomAngle) * direction;
    }

    public void UpdateAmmoText()
    {
        ammoText.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();
    }

    private void Shoot()
    {
        if (currentAmmo > 0 && canShoot)
        {
            currentAmmo--;
            UpdateAmmoText();
            gunAudioSource.PlayOneShot(firingSound);

            for (int i = 0; i < bulletCount; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, gunPivot.position, Quaternion.Euler(0f, 0f, Mathf.Atan2(mousePosition.y - gunPivot.position.y, mousePosition.x - gunPivot.position.x) * Mathf.Rad2Deg));
                Vector2 direction = mousePosition - gunPivot.position;
                direction = ApplySpread(direction, spreadAngle);
                projectile.GetComponent<Rigidbody2D>().velocity = direction.normalized * projectileSpeed;
            }

            if (ejectShells)
            {
                EjectShells();
            }

            canShoot = false;
            StartCoroutine(ResetCanShoot());
        }
    }

    private void EjectShells()
    {
        for (int i = 0; i < ejectedShellsPerShot; i++)
        {
            GameObject shell = Instantiate(shellPrefab, ejectionPoint.position, Quaternion.identity);
            Vector2 ejectionDirection = Quaternion.Euler(0f, 0f, ejectionPoint.eulerAngles.z + 45f) * Vector2.right;
            ejectionDirection = ApplySpread(ejectionDirection, shellSpreadAngle);
            Rigidbody2D shellRb2D = shell.GetComponent<Rigidbody2D>();

            if (shellRb2D != null)
            {
                shellRb2D.velocity = ejectionDirection.normalized * shellEjectionSpeed;
                shellRb2D.angularVelocity = 360f;
            }
        }
    }

}
