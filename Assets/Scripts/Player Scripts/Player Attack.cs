using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private PlayerEquipManagment playerEquipManagment;


    private Weapon currentWeapon;
    Animator animator;


    public Weapon CurrentWeapon
    {
        get { return currentWeapon; }
        set
        {
            currentWeapon = value;
            if (currentWeapon != null)
            {
                playerEquipManagment.CurrentWeaponSetting.Invoke(currentWeapon);
                animator.SetLayerWeight(1, 1);
            }
            else
            {
                animator.SetLayerWeight(1, 0);
            }
        }
    }

    private void Awake()
    {
        currentWeapon = null;
        animator = GetComponent<Animator>();
        playerEquipManagment = GetComponent<PlayerEquipManagment>();
    }

    void OnFire(InputValue value)
    {
        bool isFire = value.Get<float>() == 1.0f ? true : false;
        if (WeaponType<Gun>(currentWeapon) == true)
        {
            currentWeapon.TryGetComponent(out Gun gun);
            gun.isShot = isFire;
            gun.Fire();
            if (isFire == false) animator.SetBool("AutoFire", false);
            else
            {
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
            Reload();
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
            if(currentWeapon is Gun gun && gun.IsHaveMagazine() == true) {
                animator.SetTrigger("Reload");
                gun.Reload();
            }
        }
    }

    public bool WeaponType<T>(Weapon weapon)
    {
        return weapon is T;
    }


}
