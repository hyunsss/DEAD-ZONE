using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerEquipManagment : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private PlayerInput playerInput;

    public Transform hand_R;
    public Transform spine_03;

    public Weapon[] weapons = new Weapon[2];
    public int currentindex;

    public Action<Weapon> CurrentWeaponSetting;
    private TwoBoneIKConstraint subHandIK;
    
    void Awake() {
        playerAttack = GetComponent<PlayerAttack>();
        playerInput = GetComponent<PlayerInput>();
        subHandIK = transform.Find("IK Rig/SubHandIK").GetComponent<TwoBoneIKConstraint>();
    }

    public void InsertWeapon(Weapon weapon, int index) {
        print("Insert Weapon Enable");
        Weapon thisweapon = LeanPool.Spawn(weapon, Vector3.zero, Quaternion.identity);
        weapons[index] = thisweapon;
        thisweapon.rigid.isKinematic = true;

        if(playerAttack.CurrentWeapon == null) {
            playerInput.AssignmentWeapon(index);
        } else {
            SubWeaponSetting(thisweapon);
        }

    }

    public Weapon GetWeapon(int index) {
        return weapons[index];
    }

    public void CurrentChangeWeaponSetting(Weapon currentWeapon) {
        print("CurrentWeaponSetting");
        currentWeapon.transform.SetParent(hand_R);
        subHandIK.data.target = currentWeapon.subHandIK_target;
        subHandIK.data.hint = currentWeapon.subHandIK_hint;
        currentWeapon.transform.localPosition = currentWeapon.handPosition;
        currentWeapon.transform.localRotation = Quaternion.Euler(currentWeapon.handRotation);

        if(weapons[1 - currentindex] != null) {
            Weapon subWeapon = weapons[1 - currentindex];
            SubWeaponSetting(subWeapon);
        }
    }

    public void SubWeaponSetting(Weapon subWeapon) {
        print("subweapon setting enable");
        subWeapon.transform.SetParent(spine_03);
        subWeapon.transform.localPosition = subWeapon.subPosition;
        subWeapon.transform.localRotation = Quaternion.Euler(subWeapon.subRotation);
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentWeaponSetting += CurrentChangeWeaponSetting;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
