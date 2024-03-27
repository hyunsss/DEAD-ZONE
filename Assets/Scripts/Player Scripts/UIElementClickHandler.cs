using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using Lean.Pool;

public class UIElementClickHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item myItem;

    public Cell parentAfterCell;
    public Cell dropCell;

    private RectTransform rect;
    public Image image;
    private bool current_Rotation;
    public bool isRotation;
    public List<Cell> itemCells = new List<Cell>();

    public void HanlderInit(List<Cell> itemcells, Item item, bool isRotation = false)
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        myItem = item;
        image.sprite = item.inventorySprite;

        //Rect Transform 초기화
        ImagePropertyCellType(parentAfterCell, rect, image);
        //Rect Transform 초기화 
        itemCells = itemcells;
        this.isRotation = isRotation;
        current_Rotation = isRotation;
        if (this.isRotation == true) { rect.Rotate(new Vector3(0, 0, 90)); }
        ItemManager.Instance.CreateItemBackGround(this);
    }

    private void SetDefaultImageSize(RectTransform rect)
    {
        rect.anchorMin = new Vector2(0.5f, 0.5f); // 왼쪽 하단 앵커
        rect.anchorMax = new Vector2(0.5f, 0.5f); // 오른쪽 상단 앵커

        rect.pivot = new Vector2(1f / (myItem.cellwidth * 2), 1f - (1f / (myItem.cellheight * 2)));
        rect.sizeDelta = new Vector2(myItem.cellwidth * 70, myItem.cellheight * 70);
        rect.localPosition = Vector3.zero;
    }

    public void ImagePropertyCellType(Cell cell, RectTransform rect, Image image)
    {
        if (cell is EquipmentCell == true)
        {
            Debug.Log("true");
            // 부모 컴포넌트의 전체를 채우도록 앵커 설정
            rect.anchorMin = new Vector2(0, 0); // 왼쪽 하단 앵커
            rect.anchorMax = new Vector2(1, 1); // 오른쪽 상단 앵커

            // 오프셋을 0으로 설정하여 정확히 부모를 채우도록 함
            rect.offsetMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.eulerAngles = new Vector3(0, 0, 0);
            rect.localPosition = Vector3.zero;
            image.preserveAspect = true;

        }
        else
        {
            Debug.Log("false");
            SetDefaultImageSize(rect);
        }
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
        SetDefaultImageSize(rect);
        LeanPool.Despawn(parentAfterCell.transform.Find("ItemBackgroundImage").gameObject);
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
            print("drop cell null");
            dropCell = parentAfterCell;
            if (dropCell is EquipmentCell equipmentcell)
            {
                equipmentcell.EquipItem(equipmentcell.equiptype, myItem);
                print("parentaftercell == equpment");
            }
            //Todo Drop되는 셀의 타입에 따라 rect를 설정해주는 함수가 필요할 것 같음.
        }
        else if (dropCell.TryGetComponent(out EquipmentCell equipmentCell))
        {
            ItemRotation(false);
            parentAfterCell = dropCell;
            itemCells.Clear();
            itemCells.Add(parentAfterCell);
        }
        else if (dropCell.TryGetComponent(out Cell cell))
        {
            InsertCellItem(dropCell);
        }

        CompleteMoveCell(parentAfterCell);
        dropCell = null;
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
            }
            else
            {
                if (parentAfterCell is EquipmentCell equipmentCell == false)
                {
                    InsertCellItem(parentAfterCell);
                }
                else
                {
                    ItemRotation(false);
                    equipmentCell.EquipItem(equipmentCell.equiptype, myItem);
                    itemCells.Clear();
                    itemCells.Add(parentAfterCell);
                }
            }
        }
    }

    public void RemoveCellItem()
    {
        if (parentAfterCell is EquipmentCell == true)
        {
            parentAfterCell.slotcurrentItem.transform.SetParent(ItemManager.Instance.itemParent);
            parentAfterCell.slotcurrentItem.gameObject.SetActive(false);
            Data.Instance.Player.GetComponent<PlayerEquipManagment>().RemoveWeapon(parentAfterCell.slotcurrentItem as Weapon);
            // Data.Instance.Player.GetComponent<PlayerEquipManagment>().Weapons
            //                         [Array.IndexOf(Data.Instance.Player.GetComponent<PlayerEquipManagment>().Weapons, parentAfterCell.slotcurrentItem)] = null;
            parentAfterCell.slotcurrentItem = null;
        }
        else
        {
            foreach (Cell cell in itemCells)
            {
                cell.slotcurrentItem = null;
                cell.Item_ParentCell = null;
                cell.GetComponent<Image>().color = UIManager.Instance.GetItemTypeColor();
            }

            itemCells.Clear();
        }

    }

    private void CompleteMoveCell(Cell parentCell)
    {
        transform.SetParent(parentCell.transform, false);
        transform.position = Vector3.zero;

        ImagePropertyCellType(parentCell, rect, image);
        ItemManager.Instance.CreateItemBackGround(this);
        // rect.localPosition = Vector3.zero;
        parentCell.slotcurrentItem = myItem;
        GetComponent<Image>().raycastTarget = true;



    }


}
