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
    public Transform body;

    [Space(40f)]
    [Header("Weapon Properties")]
    private Weapon[] weapons = new Weapon[2];
    public Weapon[] Weapons { get { return weapons; } set { weapons = value; } }
    public int currentindex;
    public Action<Weapon> CurrentWeaponSetting;
    

    [Space(20f)]
    [Header("Bag Properties")]
    public Bag currentBag;

    [Space(20f)]
    [Header("Armor Properties")]
    public Armor currentArmor;

    [Space(20f)]
    [Header("Armor Properties")]
    public Helmet currentHelmet;

    void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerInput = GetComponent<PlayerInput>();
        
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
        UIManager.Instance.InsertCellInventory(bag.currentBagInventory, UIManager.Instance.inven_transform);

        bag.gameObject.SetActive(true);
        bag.rigid.isKinematic = true;

        bag.transform.SetParent(spine_03, false); 
        bag.transform.localPosition = bag.bagPosition;
        bag.transform.localRotation = Quaternion.Euler(bag.bagRotation);
    }

    public void RemoveBag()
    {
        UIManager.Instance.RemoveCellInventory(currentBag.currentBagInventory, currentBag.transform);
        currentBag = null;
    }

    #endregion
    #region Armor equip rosics
    public void InsertArmor(Armor armor)
    {
        armor.gameObject.SetActive(true);
        armor.rigid.isKinematic = true;

        currentArmor = armor;
        armor.transform.SetParent(body, false);
        UIManager.Instance.InsertCellInventory(armor.currentRigInventory, UIManager.Instance.rig_transform);

        armor.transform.localPosition = armor.armorPosition;
        armor.transform.localRotation = Quaternion.Euler(armor.armorRotation);
    }

    public void RemoveArmor()
    {
        UIManager.Instance.RemoveCellInventory(currentArmor.currentRigInventory, currentArmor.transform);
        currentArmor = null;
    }
    #endregion

    #region Helemt equip rosics
    public void InsertHelmet(Helmet helmet)
    {
        helmet.gameObject.SetActive(true);
        helmet.rigid.isKinematic = true;

        currentHelmet = helmet;
        helmet.transform.SetParent(head, false);

        helmet.transform.localPosition = helmet.HelmetPosition;
        helmet.transform.localRotation = Quaternion.Euler(helmet.HelmetRotation);
    }

    public void RemoveHelmet()
    {
        currentHelmet = null;
    }
    #endregion

    public void RemoveItemofCelltype(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Helmat:
                RemoveHelmet();
                break;
            case EquipmentType.Armor:
                RemoveArmor();
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
