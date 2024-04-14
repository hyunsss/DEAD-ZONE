using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Item, IStackable
{
    [SerializeField] private int maxAmmoCount;
    public int currentAmmoCount;

    public Ammo thisAmmo;

    public int Count { get => currentAmmoCount; set { currentAmmoCount = value; } }

    public void UseMagazine(out bool isEnough)
    {
        if(currentAmmoCount != 0) {
            currentAmmoCount--;
            isEnough = true;
        } else {
            isEnough = false;
        }
    }

    public IEnumerator InsertAmmo(Ammo ammo, int ammoCount) {
        while(currentAmmoCount <= maxAmmoCount || ammoCount != 0) {
            currentAmmoCount++;
            yield return new WaitForSeconds(0.7f);

        }

    }

    public Ammo RemoveAmmo() {
        if(currentAmmoCount != 0) {
            currentAmmoCount--;
            return thisAmmo; 
        } else {
            return null;
        }
    }
}
