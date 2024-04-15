using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Item, IDurable
{
    [SerializeField] private int maxAmmoCount;
    public int currentAmmoCount;
    public Ammo thisAmmo;

    public int Durability { get => currentAmmoCount; set { currentAmmoCount = value; } }
    public int MaxDurability { get => maxAmmoCount; }

    public void UseMagazine(out bool isEnough)
    {
        if(currentAmmoCount != 0) {
            currentAmmoCount--;
            isEnough = true;
        } else {
            isEnough = false;
        }
    }

    public IEnumerator InsertAmmo(Ammo ammo) {
        Debug.Log("Insert Ammo Corotine enable");
        while(currentAmmoCount <= maxAmmoCount && ammo.Count != 0) {
            Durability++;
            ammo.Count--;
            yield return new WaitForSeconds(0.7f);
            Debug.Log(currentAmmoCount);
        }
        Debug.Log("End! Insert!");
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
