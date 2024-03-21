using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

[Flags]
public enum ItemKey { 
    Not = 0,
    Ammo = 1 << 0, 
    AmmoBox = 1 << 1, 
    Magazine = 1 << 2, 
    Weapon = 1 << 3, 
    Medical = 1 << 4, 
    Food = 1 << 5, 
    Money = 1 << 6, 
    Etc = 1 << 7, 
    None = Ammo | AmmoBox | Magazine | Weapon | Medical | Food | Money | Etc }

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public List<Item> ItemList = new List<Item>();
    public Dictionary<ItemKey, List<Item>> itemDic = new Dictionary<ItemKey, List<Item>>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SetItemDictionary();
    }

    public void SetItemDictionary()
    {
        foreach (Item item in ItemList)
        {
            if (!itemDic.ContainsKey(item.type))
            {
                itemDic[item.type] = new List<Item>();
            }
            itemDic[item.type].Add(item);
        }
    }

    public List<Item> GetItemsOfKey(ItemKey key)
    {
        foreach (var currentitem in itemDic)
        {
            if (currentitem.Key == key)
            {
                return itemDic[key];
            }
        }
        return null;
    }

    public Item GetItemOfClone(Item item)
    {
        foreach (var currentitem in ItemList)
        {
            if (currentitem.name == item.name)
            {
                return currentitem;
            }
        }

        return null;
    }

    public void CheckToInventory(Cell cell, Item cloneitem, out List<Cell> tempCells, out bool isRotation, out bool Finish)
    {
        isRotation = false;
        bool isComplete;
        tempCells = cell.ParentPanel.grid.SizeofItemCellList(cloneitem, cell, out isComplete, isRotation);
        if (isComplete == true && cell.ParentPanel.grid.IsCellInItemPossible(tempCells) == true)
        {
            Debug.Log("rotation false rosics");
            foreach (Cell list_cell in tempCells)
            {
                list_cell.slotcurrentItem = cloneitem;
                Debug.Log(list_cell + "," + cell);
                list_cell.Item_ParentCell = cell;
            }
            Finish = true;
            return;
        }
        else
        {
            isRotation = true;
            tempCells = cell.ParentPanel.grid.SizeofItemCellList(cloneitem, cell, out isComplete, isRotation);
            if (isComplete == true && cell.ParentPanel.grid.IsCellInItemPossible(tempCells) == true)
            {
                Debug.Log("rotation true rosics");
                foreach (Cell list_cell in tempCells)
                {
                    list_cell.slotcurrentItem = cloneitem;
                    Debug.Log(list_cell + "," + cell);
                    list_cell.Item_ParentCell = cell;
                }
                Finish = true;
                return;
            }
            else
            {
                Finish = false;
                return;
            }
        }
    }

    public void MoveToInventoryFindCell(GridXY grid, Item item) {
        for (int y = grid.GridArray.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < grid.GridArray.GetLength(0); x++)
            {
                if(grid.GridArray[x, y].slotcurrentItem == null) {
                    MoveToInventory(grid.GridArray[x, y], item, out bool isInInventory);

                    if(isInInventory == true) return;
                }
            }
        }
    }

    public void MoveToInventory(Cell cell, Item item, out bool isInInventory)
    {
        Item cloneitem = GetItemOfClone(item);

        List<Cell> tempCells = new List<Cell>();

        CheckToInventory(cell, cloneitem, out tempCells, out bool isRotation, out bool Finish);

        if(Finish == true) {
            GameObject imageObj = new GameObject(item.name + "Image");
            cell.slotcurrentItem = cloneitem;
            imageObj.transform.SetParent(cell.transform, false);
            UIElementClickHandler handler = imageObj.AddComponent<UIElementClickHandler>();
            imageObj.AddComponent<Image>();
            handler.HanlderInit(tempCells, cloneitem, isRotation);

            LeanPool.Despawn(item);

            isInInventory = true;
        } else {
            isInInventory = false;
        }
    }

    public void DropItem(Item item)
    {
        Item currentitem = LeanPool.Spawn(item, Data.Instance.Player.transform.position + new Vector3(0, 1.6f, 0), Quaternion.identity).GetComponent<Item>();
        currentitem.rigid.AddForce(Data.Instance.Player.transform.forward * 5f, ForceMode.Impulse);

    }

}
