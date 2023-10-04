using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    public int weaponIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Trigger the pickup in the WeaponSwitcher
            WeaponSwitching weaponSwitcher = collision.GetComponent<WeaponSwitching>();
            if (weaponSwitcher != null)
            {
                weaponSwitcher.UnlockWeapon(weaponIndex); // Unlock the weapon
            }

            // Optionally, destroy the pickup after the player collects it
            Destroy(gameObject);
        }
    }
}
