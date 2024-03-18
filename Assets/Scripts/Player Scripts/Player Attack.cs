using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Weapon currentWeapon;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void OnFire(InputValue value)
    {
        bool isFire = value.Get<float>() == 1.0f ? true : false;
        if (WeaponType<Gun>(currentWeapon) == true)
        {
            currentWeapon.TryGetComponent(out Gun gun);
            gun.isShot = isFire;
            gun.Fire();
            if(isFire == false) animator.SetBool("AutoFire", false);
            else {
                if (gun.mode == FireMode.Single) animator.SetTrigger("SingleFire");
                if (gun.mode == FireMode.Burst) animator.SetTrigger("SingleFire");
                if (gun.mode == FireMode.Auto) animator.SetBool("AutoFire", true);
            }
        }
    }

    void OnReload(InputValue value)
    {
        if (WeaponType<Gun>(currentWeapon) == true)
        {
            animator.SetTrigger("Reload");
        }
    }

    void OnFireMode(InputValue value)
    {
        if (WeaponType<Gun>(currentWeapon) == true)
        {
            currentWeapon.TryGetComponent(out Gun gun);
            gun.FireMode();
        }
    }

    void Reload()
    {
        if (WeaponType<Gun>(currentWeapon) == true)
        {
            currentWeapon.TryGetComponent(out Gun gun);
            gun.Reload();
        }
    }

    public bool WeaponType<T>(Weapon weapon)
    {
        return weapon is T;
    }


}
