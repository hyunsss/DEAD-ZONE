using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item slotcurrentItem;
    public Vector2Int current_lotation;
    public ItemCellPanel parentPanel;
    public Cell item_ParentCell;

    private void Awake() {
        parentPanel = GetComponentInParent<ItemCellPanel>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if(slotcurrentItem == null && dropped.TryGetComponent(out UIElementClickHandler component)) {
            component.dropCell = this;
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.handler_focus = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UIManager.Instance.handler_focus == gameObject) UIManager.Instance.handler_focus = null;
    }
}
