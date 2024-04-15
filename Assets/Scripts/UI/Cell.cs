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
            component.isItemDrop = false;
        }
        else if (slotcurrentItem != null && dropped.TryGetComponent(out component))
        {
            //드랍된 아이템이 가방이나 아머라면 
            if ((slotcurrentItem.item_Key & (ItemKey.Bag | ItemKey.Armor)) != ItemKey.Not)
            {
                component.isItemDrop = true;
                component.image.preserveAspect = false;
                component.dropCell = this;
            //현재 나의 아이템이 탄약이고 드랍된 곳의 아이템이 탄창 이라면
            } else if(((component.myItem.item_Key & ItemKey.Ammo) != ItemKey.Not) && (slotcurrentItem.item_Key & (ItemKey.Magazine)) != ItemKey.Not) {
                if(slotcurrentItem is Magazine magazine && magazine.Durability != magazine.MaxDurability) {
                    magazine.IsInteract = true;
                    StartCoroutine(magazine.InsertAmmo(component.myItem as Ammo));
                    magazine.loadingUI = LeanPool.Spawn(UIManager.Instance.loadingUI_prefab, item_ParentCell.GetComponentInChildren<UIElementClickHandler>().transform, false).gameObject;
                    magazine.loadingUI.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                }
            }
            else
            {
                component.isItemDrop = false;
                component.image.preserveAspect = false;
                component.dropCell = null;
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
        GetComponentInChildren<UIElementClickHandler>().myBackground = null;
        LeanPool.Despawn(Item_ParentCell.transform.GetChild(0).gameObject);
        LeanPool.Despawn(Item_ParentCell.transform.GetChild(0).gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UIManager.Instance.isfocusEnable == true)
        {
            UIManager.Instance.handler_focus = gameObject;

            if (item_ParentCell != null)
            {
                item_ParentCell.transform.Find("ItemBackgroundImage").TryGetComponent(out Image image);
                if (image != null) image.color = UIManager.Instance.focusColor;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UIManager.Instance.isfocusEnable == true && UIManager.Instance.handler_focus == gameObject)
        {
            UIManager.Instance.handler_focus = null;

            if (item_ParentCell != null)
            {
                Transform image_trans = item_ParentCell.transform.Find("ItemBackgroundImage");
                if(image_trans != null && image_trans.TryGetComponent(out Image image)) {
                    image.color = UIManager.Instance.GetItemTypeColor(item_ParentCell.slotcurrentItem.item_Key);
                }
            }
        }
    }
}
