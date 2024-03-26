using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerEquipManagment : MonoBehaviour
{
    [Header("Componets")]
    private PlayerAttack playerAttack;
    private PlayerInput playerInput;
    [Space(20f)]
    [Header("1번 2번 장착될 무기 Transform")]
    public Transform hand_R;
    public Transform spine_03;

    [Space(20f)]
    [Header("Weapon Properties")]
    private Weapon[] weapons = new Weapon[2];
    public Weapon[] Weapons { get { return weapons; } set { weapons = value; } }
    public int currentindex;
    public Action<Weapon> CurrentWeaponSetting;
    private TwoBoneIKConstraint subHandIK;

    [Space(20f)]
    [Header("Bag Properties")]
    public Bag currentBag;

    void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerInput = GetComponent<PlayerInput>();
        subHandIK = transform.Find("IK Rig/SubHandIK").GetComponent<TwoBoneIKConstraint>();
    }

    #region Weapon equip rosics
    public void RemoveWeapon(Weapon weapon)
    {
        int index = Array.IndexOf(weapons, weapon);
        weapons[index] = null;
        if (index == currentindex) playerAttack.CurrentWeapon = null;

        if (currentindex == index && weapons[1 - index] != null)
        {
            playerInput.AssignmentWeapon(1 - index);
        }
    }

    public void RemoveWeapon(int index)
    {
        weapons[index] = null;
        if (index == currentindex) playerAttack.CurrentWeapon = null;

        if (currentindex == index && weapons[1 - index] != null)
        {
            playerInput.AssignmentWeapon(1 - index);
        }
    }

    public void InsertWeapon(Weapon weapon, int index)
    {
        print("Insert Weapon Enable");
        weapon.gameObject.SetActive(true);
        weapons[index] = weapon;
        weapon.rigid.isKinematic = true;

        if (playerAttack.CurrentWeapon == null)
        {
            playerInput.AssignmentWeapon(index);
        }
        else
        {
            SubWeaponSetting(weapon);
        }

    }
    
    public void CurrentChangeWeaponSetting(Weapon currentWeapon)
    {
        print("CurrentWeaponSetting");
        currentWeapon.transform.SetParent(hand_R);
        subHandIK.data.target = currentWeapon.subHandIK_target;
        subHandIK.data.hint = currentWeapon.subHandIK_hint;
        currentWeapon.transform.localPosition = currentWeapon.handPosition;
        currentWeapon.transform.localRotation = Quaternion.Euler(currentWeapon.handRotation);

        if (weapons[1 - currentindex] != null)
        {
            Weapon subWeapon = weapons[1 - currentindex];
            SubWeaponSetting(subWeapon);
        }
    }

    public void SubWeaponSetting(Weapon subWeapon)
    {
        print("subweapon setting enable");
        subWeapon.transform.SetParent(spine_03);
        subWeapon.transform.localPosition = subWeapon.subPosition;
        subWeapon.transform.localRotation = Quaternion.Euler(subWeapon.subRotation);
    }

    #endregion

    public void InsertBag(Bag bag) {
        currentBag = bag;
        UIManager.Instance.player_Inven = bag.currentBagInventory;
        bag.currentBagInventory.ShowInventory(UIManager.Instance.inven_trasform);

        //아직 등에 장착을 안하기 때문에 액티브 비활성화
        bag.gameObject.SetActive(false);
    }

    public void RemoveBag() {
        currentBag.currentBagInventory.HideInventory();
        currentBag = null;
    }

    public void RemoveItemofCelltype(EquipmentType type) {
        switch (type)
        {
            case EquipmentType.Helmat:
            // Helmet
            case EquipmentType.Armor:
            // Armor
            case EquipmentType.Accesary1:
            case EquipmentType.Accesary2:
            // Accessory
            case EquipmentType.Weapon1:
                RemoveWeapon(0);
                break;
            case EquipmentType.Weapon2:
                RemoveWeapon(1);
                break;
            case EquipmentType.Bag:
                RemoveBag();
                break;
            default:
                break;
        }
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
