using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Cell : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item slotcurrentItem;
    private Vector2Int current_lotation;
    private ItemCellPanel parentPanel;
    private Cell item_ParentCell;

    public ItemCellPanel ParentPanel { get { return parentPanel; } set { parentPanel = value; } }
    public Vector2Int Current_lotation { get { return current_lotation; } set { current_lotation = value; } }
    public Cell Item_ParentCell { get { return item_ParentCell; } set { item_ParentCell = value; } }

    private void Awake()
    {
        parentPanel = GetComponentInParent<ItemCellPanel>();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (slotcurrentItem == null && dropped.TryGetComponent(out UIElementClickHandler component))
        {
            component.dropCell = this;
            component.image.preserveAspect = false;
        }
        else if (slotcurrentItem != null && dropped.TryGetComponent(out component))
        {
            if ((slotcurrentItem.type & (ItemKey.Bag | ItemKey.Armor)) != ItemKey.Not)
            {
                if (slotcurrentItem is Bag bag)
                {  
                    ItemCellPanel itemcell = bag.currentBagInventory.GetComponentInChildren<ItemCellPanel>();
                    ItemManager.Instance.MoveToInventoryFindCell(itemcell.grid, component.myItem, out bool Finish);
                    if (Finish == true) return;
                }
                else if (slotcurrentItem is Armor armor)
                {
                    ItemCellPanel[] itemcells = armor.currentRigInventory.GetComponentsInChildren<ItemCellPanel>();
                    foreach (ItemCellPanel itemcell in itemcells)
                    {
                        ItemManager.Instance.MoveToInventoryFindCell(itemcell.grid, component.myItem, out bool Finish);
                        if (Finish == true) return;
                    }
                }
            }
        }

    }

    public virtual void RemoveItem()
    {
        ItemManager.Instance.DropItem(slotcurrentItem);
        DestoryChild();
    }

    public virtual void DestoryChild()
    {
        LeanPool.Despawn(Item_ParentCell.transform.GetChild(0).gameObject);
        LeanPool.Despawn(Item_ParentCell.transform.GetChild(0).gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.handler_focus = gameObject;

        if (item_ParentCell != null)
        {
            item_ParentCell.transform.GetChild(0).GetComponent<Image>().color = UIManager.Instance.focusColor;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UIManager.Instance.handler_focus == gameObject)
        {
            UIManager.Instance.handler_focus = null;

            if (item_ParentCell != null)
            {
                item_ParentCell.transform.GetChild(0).GetComponent<Image>().color = UIManager.Instance.GetItemTypeColor(item_ParentCell.slotcurrentItem.type);
            }
        }
    }
}
