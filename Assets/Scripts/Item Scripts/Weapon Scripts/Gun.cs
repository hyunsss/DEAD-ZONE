using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private Magazine currentMagazine;
    public Transform shotPoint;

    public override void Shot()
    {
        currentMagazine.UseMagazine(out Ammo currentAmmo);

        if(currentAmmo != null) {
            Ammo ShotAmmo = LeanPool.Spawn(currentAmmo);
            ShotAmmo.state = AmmoState.Shot;
        }
    }
}
