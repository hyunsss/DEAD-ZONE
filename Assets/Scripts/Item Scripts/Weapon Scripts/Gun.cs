using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Lean.Pool;
using Unity.VisualScripting;
using UnityEngine;

public enum GunType { AssultRifle, MachineGun, MP7, P90, Revolver, Rifle, Ump, Vector, None }

public enum FireMode { Single, Burst, Auto }

public class Gun : Weapon
{

    [Header("GunType")]
    public GunType gunType;

    [Header("AmmoType")]
    public AmmoType ammoType;

    [Header("Firemode")]
    public FireMode mode;

    [Space(20f)]
    [Header("Current Magazine")]
    [SerializeField] private Magazine currentMagazine;

    [Space(20f)]
    [Header("Gun Properties")]
    public Transform shotPoint;
    public float shotDelay;
    public Coroutine coroutine;
    public bool isShot;

    [Header("MagazineTransform")]
    public Transform magazine_Trans;


    public Coroutine recoil_coroutine;
    private Vector3 recoil_rotation;

    protected override void Awake()
    {
        base.Awake();
        mode = global::FireMode.Single;
        subHandIK_target = transform.Find("SubHandIK_target").transform;
        subHandIK_hint = transform.Find("SubHandIK_hint").transform;
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

            if(recoil_coroutine != null) StopCoroutine(recoil_coroutine);
            recoil_coroutine = StartCoroutine(Recoil());
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
    //Todo List 탄창 교체 잘 되는지 체크하기

    public void Reload()
    {
        UIElementClickHandler itemHandler = FindAmmo(gunType);
        if (itemHandler != null)
        {
            ItemCellPanel[] itemCellPanels = UIManager.Instance.rig_transform.GetComponentsInChildren<ItemCellPanel>();

            foreach (ItemCellPanel cellPanel in itemCellPanels)
            {
                ItemManager.Instance.MoveToInventoryFindCell(cellPanel.grid, currentMagazine, out bool Finish);

                if (Finish == true)
                {
                    //Finish가 true이면 전술 조끼에 들어갈 공간이 있었다는 뜻.
                    //기존 총의 탄창
                    currentMagazine.transform.SetParent(ItemManager.Instance.itemParent);
                    currentMagazine.meshCollider.enabled = true;


                    //교체 되는 총의 탄창
                    itemHandler.parentAfterCell.RemoveItem();
                    itemHandler.RemoveCellItem();

                    currentMagazine = itemHandler.myItem as Magazine;
                    currentMagazine.transform.SetParent(magazine_Trans, false);
                    currentMagazine.transform.localPosition = Vector3.zero;
                    currentMagazine.rigid.isKinematic = true;
                    currentMagazine.meshCollider.enabled = false;
                    return;
                }
            }
            //전술 조끼의 공간을 모두 탐색했는데도 부족하다면 기존 탄창은 떨구고 새탄창을 장착함.
            currentMagazine.transform.SetParent(ItemManager.Instance.itemParent);
            currentMagazine.rigid.isKinematic = false;
            currentMagazine.meshCollider.enabled = true;

            itemHandler.parentAfterCell.RemoveItem();
            itemHandler.RemoveCellItem();

            currentMagazine = itemHandler.myItem as Magazine;
            currentMagazine.transform.SetParent(magazine_Trans, false);
            currentMagazine.transform.localPosition = Vector3.zero;
            currentMagazine.rigid.isKinematic = true;
            currentMagazine.meshCollider.enabled = false;
            return;
        }
    }

    public UIElementClickHandler FindAmmo(GunType _gunType)
    {
        ItemCellPanel[] itemCellPanels = UIManager.Instance.rig_transform.GetComponentsInChildren<ItemCellPanel>();

        foreach (ItemCellPanel cellPanel in itemCellPanels)
        {
            for (int x = 0; x < cellPanel.grid.GridArray.GetLength(0); x++)
            {
                for (int y = 0; y < cellPanel.grid.GridArray.GetLength(1); y++)
                {
                    if (cellPanel.grid.GridArray[x, y].slotcurrentItem is Magazine magazine && magazine.gunType == _gunType)
                    {
                        Debug.Log("return true");
                        return cellPanel.grid.GridArray[x, y].Item_ParentCell.GetComponentInChildren<UIElementClickHandler>();
                    }
                    else
                    {
                        continue;
                    }

                }
            }
        }
        return null;
    }

    public bool IsHaveMagazine()
    {
        UIElementClickHandler returnValue = FindAmmo(gunType);
        Debug.Log(returnValue);
        return returnValue != null ? true : false;
    }

    IEnumerator Recoil() {
        float x = Random.Range(-0.2f, 0);
        float z = Random.Range(-0.2f, 0.2f);
        recoil_rotation += new Vector3(x, 0, z);
        Vector3 originalRotation = PlayerManager.look.cam.transform.localEulerAngles;
        originalRotation.z = 0f;
        

        float recoilTime = 0.1f;
        float time = 0;

        while (time < recoilTime)
        {
            // 시간에 따라 카메라 회전을 부드럽게 적용
            PlayerManager.look.cam.transform.localRotation = Quaternion.Lerp(PlayerManager.look.cam.transform.localRotation, Quaternion.Euler(originalRotation + recoil_rotation), time / recoilTime);
            time += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        time = 0;
        while (time < recoilTime)
        {
            PlayerManager.look.cam.transform.localRotation = Quaternion.Lerp(PlayerManager.look.cam.transform.localRotation, Quaternion.Euler(originalRotation), time / recoilTime);
            time += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        PlayerManager.look.cam.transform.localRotation = Quaternion.Euler(originalRotation);
        recoil_rotation = Vector3.zero;
    }

}
