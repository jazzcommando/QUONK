using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitching : MonoBehaviour
{
    public GameObject[] weapons; // stores weapons indices
    public Sprite[] weaponIcons; // stores weapon HUD elements
    public bool[] i, unlockedWeapons; // stores weapon pickups status


    public Image weaponIconImage;
    public AudioClip equipSound;

    public int currentWeaponIndex = 0;

    private void Start(){
        SwitchWeapon(currentWeaponIndex);
    }

    private void Update(){

     for (int i = 0; i < weapons.Length; i++){
            if (Input.GetKeyDown((KeyCode)(49 + i))){ // KeyCode 48 = Alpha0 
                SwitchWeapon(i); 
            }
        }


    }

    public void UnlockWeapon(int weaponIndex){
        if (weaponIndex >= 0 && weaponIndex < weapons.Length){
            unlockedWeapons[weaponIndex] = true;
        }
    }

    public void SwitchWeapon(int newWeaponIndex){
        if (newWeaponIndex >= 0 && newWeaponIndex < weapons.Length){
            DeactivateWeapon(currentWeaponIndex);
            ActivateWeapon(newWeaponIndex);
            currentWeaponIndex = newWeaponIndex;

            Gun newGun = weapons[currentWeaponIndex].GetComponent<Gun>();
            newGun.UpdateAmmoText();
            newGun.canShoot = true; // force reset canShoot, otherwise it stays set to false if the player switches weapons fast enough while firing

            weaponIconImage.sprite = weaponIcons[currentWeaponIndex];    
        }
    }

    private void ActivateWeapon(int weaponIndex){
        weapons[weaponIndex].SetActive(true);
    }

    public void DeactivateWeapon(int weaponIndex){ // public -> needs to be accessed by QuonkController for death logic
        weapons[weaponIndex].SetActive(false);
    }

    public int GetCurrentWeaponIndex(){ // same as above
        return currentWeaponIndex;
    }

}
