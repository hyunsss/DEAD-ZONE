using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] private int maxAmmoCount;
    [SerializeField] private int currentAmmoCount;

    Stack<Ammo> currentAmmoList = new Stack<Ammo>();

    public bool UseMagazine(out Ammo ammo)
    {
        if(currentAmmoList.Count != 0) {
            ammo = currentAmmoList.Pop();
            return true;
        } else {
            ammo = null;
            return false;
        }
    }

    public void InsertAmmo(Ammo ammo, out bool isSuccess) {
        if(currentAmmoList.Count <= maxAmmoCount) {
            currentAmmoList.Push(ammo);
            isSuccess = true;
        } else {
            isSuccess = false;
            return;
        }
    }

    public Ammo RemoveAmmo() {
        if(currentAmmoList.Count != 0) {
            return currentAmmoList.Pop();
        } else {
            return null;
        }
    }
}
