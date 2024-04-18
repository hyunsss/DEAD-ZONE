using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Bag : Item
{
    [Space(10f)]
    [Header("Bag Properties")]
    public GameObject currentBagInventory;
    public GameObject bagInventory_prefab;

    public Vector3 bagPosition;
    public Vector3 bagRotation;
    
    public float totalWeight;

    protected override void Awake()
    {
        base.Awake();
        currentBagInventory = LeanPool.Spawn(bagInventory_prefab, transform.position, Quaternion.identity, transform);
    }

    public float GetcurrentInvenWeight(GameObject bagInventory) {

        UIElementClickHandler[] bag_handlers = bagInventory.GetComponentsInChildren<UIElementClickHandler>();

        foreach (var handler in bag_handlers)
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
