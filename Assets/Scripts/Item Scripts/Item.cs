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
    
    [HideInInspector] public MeshCollider meshCollider;
    [HideInInspector] public MeshRenderer meshRenderer;
    [HideInInspector] public Rigidbody rigid;

    protected virtual void Awake() {
        meshCollider = GetComponentInChildren<MeshCollider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        rigid = GetComponent<Rigidbody>();
    }

    public void Interact()
    {
        ItemManager.Instance.MoveToInventoryFindCell(UIManager.Instance.player_Inven.grid, this);
        Debug.Log("interact enable");
    }

    
}
