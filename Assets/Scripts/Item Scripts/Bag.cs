using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Bag : Item
{
    [Space(10f)]
    [Header("Bag Properties")]
    public ItemCellPanel currentBagInventory;


    protected override void Awake() {
        base.Awake();
        currentBagInventory = LeanPool.Spawn(UIManager.Instance.inventory_prefab, transform.position, Quaternion.identity, transform).GetComponent<ItemCellPanel>();
    }




}
