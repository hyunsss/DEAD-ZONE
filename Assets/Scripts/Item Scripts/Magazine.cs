using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Item
{
    [SerializeField] private int maxAmmoCount;
    public int currentAmmoCount;

    public Ammo thisAmmo;

    public void UseMagazine(out bool isEnough)
    {
        if(currentAmmoCount != 0) {
            currentAmmoCount--;
            isEnough = true;
        } else {
            isEnough = false;
        }
    }

    public void InsertAmmo(Ammo ammo, out bool isSuccess) {
        if(currentAmmoCount <= maxAmmoCount) {
            currentAmmoCount++;
            isSuccess = true;
        } else {
            isSuccess = false;
            return;
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
