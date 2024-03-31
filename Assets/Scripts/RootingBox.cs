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
        item_obj.SetActive(false);

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
            GetBoxItems();
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
