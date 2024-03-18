using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Animations.Rigging;
using Lean.Pool;

public abstract class Item : SerializedMonoBehaviour, IInteractable
{
    public ItemKey type;
    public string item_name;
    public string item_desc;
    public Sprite inventorySprite;
    public int prize;
    
    [HideInInspector] public MeshCollider meshCollider;
    [HideInInspector] public MeshRenderer meshRenderer;
    [HideInInspector] public Rigidbody rigid;

    private void Awake() {
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        rigid = GetComponent<Rigidbody>();
    }

    public void Interact()
    {
        ItemManager.Instance.MoveToInventory(this);
    }

    
}
