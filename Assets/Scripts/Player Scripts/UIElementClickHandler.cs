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
    private bool current_Rotation;
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
        current_Rotation = isRotation;
        if (this.isRotation == true) { rect.Rotate(new Vector3(0, 0, 90)); }
    }

    public void ItemRotation()
    {
        isRotation = !isRotation;
        if (isRotation == true) { rect.eulerAngles = new Vector3(0, 0, 90); }
        else { rect.eulerAngles = new Vector3(0, 0, 0); }
    }

    public void ItemRotation(bool rotation)
    {
        isRotation = rotation;
        if (isRotation == true) { rect.eulerAngles = new Vector3(0, 0, 90); }
        else { rect.eulerAngles = new Vector3(0, 0, 0); }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        RemoveCellItem();
        UIManager.Instance.current_MoveItem = this;
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
        if (dropCell == null)
        {
            dropCell = parentAfterCell;
        }
        InsertCellItem(dropCell);

        dropCell = null;
        CompleteMoveCell(parentAfterCell);
        UIManager.Instance.CellRayCastTarget(false);
        UIManager.Instance.current_MoveItem = null;
    }

    /// <summary>
    /// 아이템이 셀로 들어가기 전에 들어가려는 자리로부터 셀영역을 탐색 후 배치될 수 있는지 여부를 판단함.
    /// </summary>
    /// <param name="currentcell"></param>
    public void InsertCellItem(Cell currentcell)
    {
        List<Cell> tempList = currentcell.ParentPanel.grid.SizeofItemCellList(myItem, currentcell, out bool isComplete, isRotation);
        if (isComplete == true && currentcell.ParentPanel.grid.IsCellInItemPossible(tempList) == true)
        {
            foreach (Cell cell in tempList)
            {
                cell.slotcurrentItem = myItem;
                cell.Item_ParentCell = currentcell;

            }

            parentAfterCell = currentcell;
            itemCells = tempList;
            // current_Rotation = rotation;
        }
        else
        {
            ItemRotation(!isRotation);
            tempList = currentcell.ParentPanel.grid.SizeofItemCellList(myItem, currentcell, out isComplete, isRotation);
            if (isComplete == true && currentcell.ParentPanel.grid.IsCellInItemPossible(tempList) == true)
            {
                foreach (Cell cell in tempList)
                {
                    cell.slotcurrentItem = myItem;
                    cell.Item_ParentCell = currentcell;
                }

                parentAfterCell = currentcell;
                itemCells = tempList;
                // current_Rotation = rotation;
            } else {
                InsertCellItem(parentAfterCell);
            }
        }
    }

    public void RemoveCellItem()
    {
        foreach (Cell cell in itemCells)
        {
            cell.slotcurrentItem = null;
            cell.Item_ParentCell = null;
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
