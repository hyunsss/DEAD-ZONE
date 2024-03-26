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
    public Transform head;

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

    [Space(20f)]
    [Header("Helmet / Armor Properties")]
    public Armor[] currentArmors = new Armor[2];

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
    #region Bag equip rosics
    public void InsertBag(Bag bag)
    {
        currentBag = bag;
        UIManager.Instance.player_Inven = bag.currentBagInventory;
        bag.currentBagInventory.ShowInventory(UIManager.Instance.inven_trasform);

        bag.gameObject.SetActive(true);
        bag.rigid.isKinematic = true;

        bag.transform.SetParent(spine_03, false);
        bag.transform.localPosition = bag.bagPosition;
        bag.transform.localRotation = Quaternion.Euler(bag.bagRotation);
    }

    public void RemoveBag()
    {
        currentBag.currentBagInventory.HideInventory();
        currentBag = null;
    }

    #endregion
    #region Armor equip rosics
    public void InsertArmor(Armor armor)
    {
        armor.gameObject.SetActive(true);
        armor.rigid.isKinematic = true;

        switch (armor.type)
        {
            case ItemKey.Helmat:
                currentArmors[0] = armor;
                armor.transform.SetParent(head, false);
                break;
            case ItemKey.Armor:
                currentArmors[1] = armor;
                armor.transform.SetParent(spine_03, false);
                break;
            default:
                break;
        }

        armor.transform.localPosition = armor.armorPosition;
        armor.transform.localRotation = Quaternion.Euler(armor.armorRotation);
    }

    public void RemoveArmor(int index)
    {
        currentArmors[index] = null;
    }
    #endregion

    public void RemoveItemofCelltype(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Helmat:
                RemoveArmor(0);
                break;
            case EquipmentType.Armor:
                RemoveArmor(1);
                break;
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
