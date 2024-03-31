using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Armor : Item {

    public GameObject currentRigInventory;
    public GameObject rigInventory_prefab;

    public Vector3 armorPosition;
    public Vector3 armorRotation;
    
    protected override void Awake()
    {
        base.Awake();
        currentRigInventory = LeanPool.Spawn(rigInventory_prefab, transform.position, Quaternion.identity, transform);
    }

}
