using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

public enum ItemKey {Ammo, AmmoBox, Magazine, Weapon, Medical, Food, Money, Etc, None}
    
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public List<Item> ItemList = new List<Item>();
    public Dictionary<ItemKey, Item> itemDic = new Dictionary<ItemKey, Item>();

    private void Awake() {
        Instance = this;    
    }
    private void Start() {
        SetItemDictionary();
    }

    public void SetItemDictionary() {
        foreach(Item item in ItemList) {
            itemDic.Add(item.type, item);
        }
    }

    public List<Item> GetItemsOfKey(ItemKey key) {
        List<Item> TempList = new List<Item>();
        foreach(var currentitem in itemDic) {
            if(currentitem.Key == key) {
                TempList.Add(currentitem.Value);
            }
        }
        return TempList;
    }

    public Item GetItemOfClone(Item item) {
        foreach(var currentitem in ItemList) {
            if(currentitem.name == item.name) {
                return currentitem;
            }
        }

        return null;
    }

    public void MoveToInventory(Item item) {
        UIManager.Instance.player_Inven.QuickInsterItem(GetItemOfClone(item), out bool isInsert, out Cell cell);

        if(isInsert == false) {
            //Full of Inventory
            return;
        }

        GameObject imageObj = new GameObject(item.name + "Image");
        imageObj.transform.SetParent(cell.transform);
        imageObj.AddComponent<UIElementClickHandler>();
        Image image = imageObj.AddComponent<Image>();
        image.sprite = item.inventorySprite;

        LeanPool.Despawn(item);
    }

    public void DropItem(Item item) {
        Item currentitem = LeanPool.Spawn(item, Data.Instance.playerPos + new Vector3(0, 1.6f, 0), Quaternion.identity).GetComponent<Item>();
        currentitem.rigid.AddForce(Data.Instance.Player.transform.forward * 5f, ForceMode.Impulse);

    }
    
}
