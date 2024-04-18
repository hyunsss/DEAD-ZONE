using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Armor : Item {

    public GameObject currentRigInventory;
    public GameObject rigInventory_prefab;

    public float totalWeight;

    public Vector3 armorPosition;
    public Vector3 armorRotation;
    
    protected override void Awake()
    {
        base.Awake();
        currentRigInventory = LeanPool.Spawn(rigInventory_prefab, transform.position, Quaternion.identity, transform);
    }

    public float GetcurrentInvenWeight(GameObject rigInventory) {

        UIElementClickHandler[] armor_handlers = rigInventory.GetComponentsInChildren<UIElementClickHandler>();

        foreach (var handler in armor_handlers)
        {
            totalWeight += handler.myItem.item_weight;

            if(handler.myItem is Bag bag) {
                totalWeight += bag.GetcurrentInvenWeight(bag.currentBagInventory);
                bag.totalWeight = 0;
            } else if(handler.myItem is Armor armor) {
                totalWeight += armor.GetcurrentInvenWeight(armor.currentRigInventory);
                armor.totalWeight = 0;
            }
        }


        return totalWeight;
    }

}
