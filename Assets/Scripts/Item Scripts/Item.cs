using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Animations.Rigging;
using Lean.Pool;

public abstract class Item : SerializedMonoBehaviour, IInteractable
{
    [Header("Cell Parameter")]
    public ItemKey type;
    public Sprite inventorySprite;
    public int cellwidth;
    public int cellheight;
    [Space]
    [Header("Item Status")]
    public string item_name;
    public string item_desc;
    public int prize;

    public bool isSearchable;

    [HideInInspector] public MeshCollider meshCollider;
    [HideInInspector] public MeshRenderer meshRenderer;
    [HideInInspector] public Rigidbody rigid;

    protected virtual void Awake()
    {
        meshCollider = GetComponentInChildren<MeshCollider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        rigid = GetComponent<Rigidbody>();
    }

    public virtual void ItemInit() { }

    public void Interact()
    {
        // 아이템 타입이 웨폰이나 장착가능한 아이템일 때 주웠을 경우 바로 장착할 수 있게끔 기능 만들기 먼저 타입과 equipment셀이 비었는지 체크한 후 들어가도록 세팅해주면 될 듯.
        foreach (var equipmentCell in UIManager.Instance.equipCell_Dic.Values)
        {
            if (equipmentCell.slotcurrentItem == null && equipmentCell.IsItemAllowed(equipmentCell.equiptype, type) == true)
            {
                equipmentCell.EquipItem(equipmentCell.equiptype, this);
                ItemManager.Instance.MoveToInventory(equipmentCell, this, out bool isInInventory);
                return;
            }
        }

        if (UIManager.Instance.player_Inven.Count > 0)
        {
            foreach (ItemCellPanel itemCell in UIManager.Instance.player_Inven)
            {
                ItemManager.Instance.MoveToInventoryFindCell(itemCell.grid, this, out bool Finish);

                if (Finish == true) return;
            }
        }
    }


}
