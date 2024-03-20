using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIElementClickHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item myItem;

    public Cell parentAfterCell;
    public Cell dropCell;

    private RectTransform rect;
    private Image image;
    public bool isRotation;
    public List<Cell> itemCells = new List<Cell>();

    public void HanlderInit(List<Cell> itemcells, Item item, bool isRotation)
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        myItem = item;
        image.sprite = item.inventorySprite;
        //Rect Transform 초기화
        rect.pivot = new Vector2(1f / (myItem.cellwidth * 2), 1f - (1f / (myItem.cellheight * 2)));
        rect.sizeDelta = new Vector2(myItem.cellwidth * 100, myItem.cellheight * 100);
        rect.localPosition = Vector3.zero;
        //Rect Transform 초기화 

        itemCells = itemcells;
        this.isRotation = isRotation;
        if (this.isRotation == true) { rect.Rotate(new Vector3(0, 0, 90)); }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RemoveCellItem();
        parentAfterCell = transform.parent.GetComponent<Cell>();
        // myItem = parentAfterCell.GetComponent<Cell>().slotcurrentItem;
        parentAfterCell.slotcurrentItem = null;
        transform.SetParent(UIManager.Instance.topCanvas.transform, false);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        UIManager.Instance.CellRayCastTarget(true);
        GetComponent<Image>().raycastTarget = false;
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(dropCell == null) {
            dropCell = parentAfterCell;
        }
        InsertCellItem(dropCell);

        dropCell = null;
        CompleteMoveCell(parentAfterCell);
        UIManager.Instance.CellRayCastTarget(false);
    }

    /// <summary>
    /// 아이템이 셀로 들어가기 전에 들어가려는 자리로부터 셀영역을 탐색 후 배치될 수 있는지 여부를 판단함.
    /// </summary>
    /// <param name="currentcell"></param>
    public void InsertCellItem(Cell currentcell)
    {
        itemCells = dropCell.parentPanel.grid.SizeofItemCellList(myItem, currentcell, out bool isComplete);
        if (isComplete == true && currentcell.parentPanel.grid.IsCellInItemPossible(itemCells) == true)
        {
            foreach (Cell cell in itemCells)
            {
                cell.slotcurrentItem = myItem;
                cell.item_ParentCell = currentcell;

            }

            parentAfterCell = currentcell;
        }

    }

    public void RemoveCellItem()
    {
        foreach (Cell cell in itemCells)
        {
            cell.slotcurrentItem = null;
            cell.item_ParentCell = null;
        }

        itemCells.Clear();
    }

    private void CompleteMoveCell(Cell parentCell)
    {
        transform.SetParent(parentCell.transform, false);
        transform.position = Vector3.zero;

        rect.localPosition = Vector3.zero;
        parentCell.slotcurrentItem = myItem;
        GetComponent<Image>().raycastTarget = true;
    }


}
