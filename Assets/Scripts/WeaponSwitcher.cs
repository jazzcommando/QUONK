using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitching : MonoBehaviour
{
    public GameObject[] weapons; //logique de switch d'arme g�r� par cette array
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
                SwitchWeapon(i); // si la touche chiffr�e correspond au weapon index correspondant --> switch � ce weapon index 
            }
        }


    }

    public void SwitchWeapon(int newWeaponIndex)
    {

        if (newWeaponIndex >= 0 && newWeaponIndex < weapons.Length)
        {
            //d�sactive current weapon, active nouvelle 
            DeactivateWeapon(currentWeaponIndex);
            ActivateWeapon(newWeaponIndex);
            currentWeaponIndex = newWeaponIndex;

            // get gun component de la nouvelle arme, update ammo text 
            Gun newGun = weapons[currentWeaponIndex].GetComponent<Gun>();
            newGun.UpdateAmmoText();
            newGun.canShoot = true; // �vite que la nouvelle arme soit bloqu�e si jamais canShoot n'a pas eu le temps de se reset avant le switch 

            weaponIconImage.sprite = weaponIcons[currentWeaponIndex];
        
        }
    }

    private void ActivateWeapon(int weaponIndex)
    {
        weapons[weaponIndex].SetActive(true);
    }

    public void DeactivateWeapon(int weaponIndex) // public parceque doit �tre acc�d� par playercontroller pour desactiver l'arme quand il meurt 
    {
        weapons[weaponIndex].SetActive(false);
    }

    public int GetCurrentWeaponIndex() // idem: playercontroller a besoin de sp�cifier l'index pour que DeactivateWeapon() fonctionne 
    {
        return currentWeaponIndex;
    }

}
