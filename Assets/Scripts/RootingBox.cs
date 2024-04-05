using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

public class RootingBox : MonoBehaviour, IInteractable
{
    public List<Item> currentitemList = new List<Item>();
    public int itemCount;
    public ItemCellPanel currentItemCellPanel;
    public GameObject cellPanelPrefab;
    public ItemKey allowItemKey;

    bool isSearch;

    private void Awake()
    {
        itemCount = Random.Range(2, 10);
        currentItemCellPanel = LeanPool.Spawn(cellPanelPrefab).GetComponent<ItemCellPanel>();
        currentItemCellPanel.boxType = BoxType.RootBox;
    }

    private void Start()
    {
        currentitemList = ItemManager.Instance.SearchListbyKey(allowItemKey);
        if(currentItemCellPanel.grid.isInit == false) {
            currentItemCellPanel.grid.GenerateGrid();
        }
        GetBoxItems();
    }

    public void GetBoxItems()
    {
        for (int i = 0; i < itemCount; i++)
        {
            int index = Random.Range(0, currentitemList.Count);
            BoxInItem(currentitemList[index], out bool success);
            if (success == true)
            {
                continue;
            }
            else
            {
                break;
            }
        }
    }

    public void BoxInItem(Item item, out bool success)
    {
        GameObject item_obj = LeanPool.Spawn(item, Vector3.zero, Quaternion.identity, ItemManager.Instance.itemParent).gameObject;
         
        if(item_obj.TryGetComponent(out Bag bag)) {
            ItemCellPanel bag_Inven = bag.currentBagInventory.GetComponent<ItemCellPanel>();
            if(bag_Inven.grid == null) {
                bag_Inven.grid = new GridXY(bag_Inven.width, bag_Inven.height, bag_Inven.transform);
            }
            if(bag_Inven.grid.isInit == false) {
                bag_Inven.grid.GenerateGrid();
            }
        } else if(item_obj.TryGetComponent(out Armor armor)) {
            ItemCellPanel[] armor_Invens = armor.currentRigInventory.GetComponentsInChildren<ItemCellPanel>();

            foreach (ItemCellPanel armor_Inven in armor_Invens) {
                if(armor_Inven.grid == null) {
                    armor_Inven.grid = new GridXY(armor_Inven.width, armor_Inven.height, armor_Inven.transform);
                }
                if(armor_Inven.grid.isInit == false) {
                    armor_Inven.grid.GenerateGrid();
                }
            }

        } 

        ItemManager.Instance.MoveToInventoryFindCell(currentItemCellPanel.grid, item_obj.GetComponent<Item>(), out bool Finish);

        if (Finish == false)
        {
            success = false;
            LeanPool.Despawn(item_obj);
        }
        else
        {
            success = true;
        }
    } 

    public void Interact()
    {
        if (isSearch == false)
        {
            currentItemCellPanel.transform.SetParent(UIManager.Instance.rooting_transform, false);
            UIManager.Instance.rooting_transform.gameObject.SetActive(true);
            UIManager.Instance.currentRootBox = this;
            UIManager.Instance.ShowPlayerInventory();
            isSearch = true;
        }
        else
        {
            currentItemCellPanel.transform.SetParent(UIManager.Instance.rooting_transform, false);
            UIManager.Instance.rooting_transform.gameObject.SetActive(true);
            UIManager.Instance.currentRootBox = this;
            UIManager.Instance.ShowPlayerInventory();
        }
    }
}
