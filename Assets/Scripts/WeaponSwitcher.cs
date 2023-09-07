using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitching : MonoBehaviour
{
    public GameObject[] weapons; //logique de switch d'arme géré par cette array
    public Sprite[] weaponIcons; // idem pour les icones dans le HUD 

    public Image weaponIconImage;
    public AudioClip equipSound;

    public int currentWeaponIndex = 0;

    private void Start()
    {
        SwitchWeapon(currentWeaponIndex);
    }

    private void Update()
    {

     for (int i = 0; i < weapons.Length; i++)
        {
            if (Input.GetKeyDown((KeyCode)(49 + i))) // KeyCode 48 = Alpha0 
            {
                SwitchWeapon(i); // si la touche chiffrée correspond au weapon index correspondant --> switch à ce weapon index 
            }
        }


    }

    public void SwitchWeapon(int newWeaponIndex)
    {

        if (newWeaponIndex >= 0 && newWeaponIndex < weapons.Length)
        {
            //désactive current weapon, active nouvelle 
            DeactivateWeapon(currentWeaponIndex);
            ActivateWeapon(newWeaponIndex);
            currentWeaponIndex = newWeaponIndex;

            // get gun component de la nouvelle arme, update ammo text 
            Gun newGun = weapons[currentWeaponIndex].GetComponent<Gun>();
            newGun.UpdateAmmoText();
            newGun.canShoot = true; // évite que la nouvelle arme soit bloquée si jamais canShoot n'a pas eu le temps de se reset avant le switch 

            weaponIconImage.sprite = weaponIcons[currentWeaponIndex];
        
        }
    }

    private void ActivateWeapon(int weaponIndex)
    {
        weapons[weaponIndex].SetActive(true);
    }

    public void DeactivateWeapon(int weaponIndex) // public parceque doit être accédé par playercontroller pour desactiver l'arme quand il meurt 
    {
        weapons[weaponIndex].SetActive(false);
    }

    public int GetCurrentWeaponIndex() // idem: playercontroller a besoin de spécifier l'index pour que DeactivateWeapon() fonctionne 
    {
        return currentWeaponIndex;
    }

}
