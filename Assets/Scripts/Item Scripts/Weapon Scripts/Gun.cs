using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Lean.Pool;
using Unity.VisualScripting;
using UnityEngine;

public enum FireMode { Single, Burst, Auto }

public class Gun : Weapon
{
    public FireMode mode;
    [SerializeField] private Magazine currentMagazine;
    public Transform shotPoint;
    public float shotDelay;
    public Coroutine coroutine;
    public bool isShot;
    protected override void Awake()
    {
        base.Awake();
        mode = global::FireMode.Single;
    }

    public override void Shot()
    {
        currentMagazine.UseMagazine(out bool isEnough);

        if (isEnough == true)
        {
            Ammo ammo = LeanPool.Spawn(currentMagazine.thisAmmo, shotPoint.position, shotPoint.rotation);
            ammo.AmmoShot();
            ammo.rigid.velocity = shotPoint.forward * ammo.bulletSpeed;

            LeanPool.Despawn(ammo, 3f);
        }
    }

    public void Fire()
    {
        if (isShot == true)
        {
            switch (mode)
            {
                case global::FireMode.Single:
                    coroutine = StartCoroutine(SingleShotCoroutine());
                    break;
                case global::FireMode.Burst:
                    coroutine = StartCoroutine(BurstShotCoroutine());
                    break;
                case global::FireMode.Auto:
                    coroutine = StartCoroutine(AutoShotCoroutine());
                    break;
            }
        }

    }

    IEnumerator SingleShotCoroutine()
    {
        Shot();
        yield return new WaitForSeconds(shotDelay);
    }

    IEnumerator BurstShotCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            Shot();
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(shotDelay);
    }

    IEnumerator AutoShotCoroutine()
    {
        while (currentMagazine.currentAmmoCount != 0 && isShot == true)
        {
            Shot();

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void FireMode()
    {
        for (int i = 0; i < 3; i++)
        {
            if ((int)mode == i)
            {
                if (i == 2)
                {
                    mode = global::FireMode.Single;
                    return;
                }
                else
                {
                    mode = (FireMode)i + 1;
                    return;
                }
            }
        }
    }

    public void Reload()
    {
        currentMagazine.currentAmmoCount = 30;
    }

}
