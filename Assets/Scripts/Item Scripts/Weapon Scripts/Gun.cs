using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Lean.Pool;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private Magazine currentMagazine;
    public Transform shotPoint;

    public override void Shot()
    {
        currentMagazine.UseMagazine(out bool isEnough);

        if(isEnough == true) {
            Ammo ammo = LeanPool.Spawn(currentMagazine.thisAmmo, shotPoint.position, shotPoint.rotation);
            ammo.rigid.velocity = shotPoint.forward * ammo.bulletSpeed;

            LeanPool.Despawn(ammo, 3f);
        }

    }

}
