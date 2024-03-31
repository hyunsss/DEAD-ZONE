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
    
    protected override void Awake()
    {
        base.Awake();
        currentBagInventory = LeanPool.Spawn(bagInventory_prefab, transform.position, Quaternion.identity, transform);
    }


}
