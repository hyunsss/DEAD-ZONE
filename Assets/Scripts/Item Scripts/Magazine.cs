using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Magazine : Item, IDurable
{
    [Header("Ammo Type")]
    public AmmoType ammoType;

    [Space(5f)]
    [Header("Ammo Count")]
    [SerializeField] private int maxAmmoCount;
    public int currentAmmoCount;

    
    public Ammo thisAmmo;

    public int Durability { get => currentAmmoCount; set { currentAmmoCount = value; } }
    public int MaxDurability { get => maxAmmoCount; }


    public GameObject loadingUI;
    private bool isInteract;
    public bool IsInteract
    {
        get => isInteract;
        set
        {
            isInteract = value;
            
            if(isInteract == false) {
                LeanPool.Despawn(loadingUI);    
            }
        }
    }


    public void UseMagazine(out bool isEnough)
    {
        if (currentAmmoCount != 0)
        {
            currentAmmoCount--;
            isEnough = true;
        }
        else
        {
            isEnough = false;
        }
    }

    public IEnumerator InsertAmmo(Ammo ammo)
    {
        Debug.Log("Insert Ammo Corotine enable");
        while (currentAmmoCount <= maxAmmoCount && ammo.Count != 0 && isInteract == true)
        {

            yield return new WaitForSeconds(0.7f);
            Durability++;
            ammo.Count--;
            Debug.Log(currentAmmoCount);
        }
        IsInteract = false;
    }

    public Ammo RemoveAmmo()
    {
        if (currentAmmoCount != 0)
        {
            currentAmmoCount--;
            return thisAmmo;
        }
        else
        {
            return null;
        }
    }
}
