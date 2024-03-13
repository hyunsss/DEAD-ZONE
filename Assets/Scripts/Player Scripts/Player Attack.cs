using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Weapon currentWeapon;


    void OnFire(InputValue value) {
        Debug.Log("Shot!");
        currentWeapon.Shot();
    }
}
