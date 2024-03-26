using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

[Flags]
public enum ItemKey
{
    Not = 0,
    Ammo = 1 << 0,
    AmmoBox = 1 << 1,
    Magazine = 1 << 2,
    Weapon = 1 << 3,
    Medical = 1 << 4,
    Food = 1 << 5,
    Money = 1 << 6,
    Etc = 1 << 7,
    Helmat = 1 << 8,
    Accesary = 1 << 9,
    Armor = 1 << 10,
    Bag = 1 << 11,
    None = Ammo | AmmoBox | Magazine | Weapon | Medical | Food | Money | Etc | Helmat | Accesary | Armor | Bag
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public Transform itemParent;
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

    public void MoveToInventoryFindCell(GridXY grid, Item item)
    {
        for (int y = grid.GridArray.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < grid.GridArray.GetLength(0); x++)
            {
                if (grid.GridArray[x, y].slotcurrentItem == null)
                {
                    MoveToInventory(grid.GridArray[x, y], item, out bool isInInventory);

                    if (isInInventory == true) return;
                }
            }
        }
    }

    public void MoveToInventory(Cell cell, Item item, out bool isInInventory)
    {
        List<Cell> tempCells = new List<Cell>();
        bool Finish, isRotation;
        if (cell is EquipmentCell == false)
        {
            CheckToInventory(cell, item, out tempCells, out isRotation, out Finish);
        }
        else
        {
            Finish = true;
            isRotation = false;
        }

        if (Finish == true)
        {
            GameObject imageObj = new GameObject(item.name + "Image");
            cell.slotcurrentItem = item;
            imageObj.transform.SetParent(cell.transform, false);
            UIElementClickHandler handler = imageObj.AddComponent<UIElementClickHandler>();
            handler.parentAfterCell = cell;
            imageObj.AddComponent<Image>();

            handler.HanlderInit(tempCells, item, isRotation);

            if (cell is EquipmentCell == false) item.gameObject.SetActive(false);
            isInInventory = true;
        }
        else
        {
            isInInventory = false;
        }
    }

    public void CreateItemBackGround(UIElementClickHandler uIElement)
    {
        GameObject gameObject = new GameObject("Item Image");
        gameObject.transform.SetParent(uIElement.transform.parent);
        gameObject.transform.SetAsFirstSibling();
        Image image = gameObject.AddComponent<Image>();
        image.color = UIManager.Instance.GetItemTypeColor(uIElement.myItem.type);

        RectTransform rect = gameObject.GetComponent<RectTransform>();
        uIElement.ImagePropertyCellType(uIElement.parentAfterCell, rect, image);

        if (uIElement.isRotation == true) rect.Rotate(new Vector3(0, 0, 90));
    }

    public void DropItem(Item item)
    {
        item.gameObject.SetActive(true);
        item.transform.SetParent(itemParent);
        item.transform.position = Data.Instance.Player.transform.position + new Vector3(0, 1.6f, 0);
        item.transform.rotation = Quaternion.identity;
        item.rigid.isKinematic = false;
        item.rigid.AddForce(Data.Instance.Player.transform.forward * 5f, ForceMode.Impulse);
    }

}
