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
    public bool isItemDrop;

    public GameObject myBackground;
    private RectTransform rect;
    public Image image;
    private bool current_Rotation;
    public bool isRotation;
    public List<Cell> itemCells = new List<Cell>();

    private void Awake()
    {
        myBackground = null;
    }

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
        if (myBackground == null) ItemManager.Instance.CreateItemBackGround(this);
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

    public void ClosePopup()
    {
        if (UIManager.Instance.popUp_dic.ContainsKey(myItem.gameObject))
        {
            LeanPool.Despawn(UIManager.Instance.popUp_dic[myItem.gameObject]);
            UIManager.Instance.popUp_dic.Remove(myItem.gameObject);
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        print("begindrag");
        RemoveCellItem();
        UIManager.Instance.current_MoveItem = this;
        parentAfterCell = transform.parent.GetComponent<Cell>();
        parentAfterCell.slotcurrentItem = null;
        transform.SetParent(UIManager.Instance.topCanvas.transform, false);
        SetDefaultImageSize(rect);
        LeanPool.Despawn(myBackground);
        myBackground = null;
        dropCell = null;

        transform.SetAsLastSibling();
        ClosePopup();

        UIManager.Instance.isfocusEnable = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("drag");
        UIManager.Instance.CellRayCastTarget(true);
        image.raycastTarget = false;
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool isSuccess = false;
        print("enddrag");
        //아이템이 드랍 되었을 때 그 셀의 currentitem이 인벤토리 패널을 가진 아이템인지 판단.
        //만약 true라면 다른 로직을 수행하게 되는데 
        //자식에 있는 모든 패널들을 조사하고 그 패널안에 직접 들어갈 수 있도록함
        //cell에 있는 drop 메시지 함수에서는 해당 셀의 아이템과 트랜스폼 값만 넘겨주기로 할 것. 
        //onenddrag 함수에서 조건 체크를 한 후 해당하는 곳에 넣어주면 해결 될 것 같음.
        if (dropCell != null && dropCell.TryGetComponent(out EquipmentCell equipmentCell))
        {
            ItemRotation(false);
            parentAfterCell = dropCell;
            itemCells.Clear();
            itemCells.Add(parentAfterCell);
            CompleteMoveCell(parentAfterCell);
            EndDropReset();
        }
        else if (dropCell != null && isItemDrop == false)
        {
            InsertCellItem(dropCell);
            CompleteMoveCell(parentAfterCell);
            EndDropReset();
        }
        else if (dropCell != null && isItemDrop == true)
        {
            if (dropCell.slotcurrentItem is Bag bag)
            {
                ItemCellPanel itemcell = bag.currentBagInventory.GetComponentInChildren<ItemCellPanel>();
                ItemManager.Instance.MoveToInventoryFindCell(itemcell.grid, myItem, out bool Finish, this);
                if (Finish == true)
                {
                    EndDropReset();
                    isSuccess = true;
                    return;
                }

                if (isSuccess == false)
                {
                    if (parentAfterCell is EquipmentCell equipment)
                    {
                        equipment.EquipItem(equipment.equiptype, myItem);
                    }
                    CompleteMoveCell(parentAfterCell);
                    EndDropReset();
                }
            }
            else if (dropCell.slotcurrentItem is Armor armor)
            {
                ItemCellPanel[] itemcells = armor.currentRigInventory.GetComponentsInChildren<ItemCellPanel>();
                foreach (ItemCellPanel itemcell in itemcells)
                {
                    ItemManager.Instance.MoveToInventoryFindCell(itemcell.grid, myItem, out bool Finish, this);
                    if (Finish == true)
                    {
                        EndDropReset();
                        isSuccess = true;
                        return;
                    }
                }

                if (isSuccess == false)
                {
                    if (parentAfterCell is EquipmentCell equipment)
                    {
                        equipment.EquipItem(equipment.equiptype, myItem);
                    }
                    CompleteMoveCell(parentAfterCell);
                    EndDropReset();
                }
            }
        }
        else
        {
            if (parentAfterCell is EquipmentCell equipment)
            {
                equipment.EquipItem(equipment.equiptype, myItem);
            }
            CompleteMoveCell(parentAfterCell);
            EndDropReset();
        }



        UIManager.Instance.isfocusEnable = true;
    }

    private void EndDropReset()
    {
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
        if (parentAfterCell is EquipmentCell equipmentCell == true)
        {
            parentAfterCell.slotcurrentItem.transform.SetParent(ItemManager.Instance.itemParent);
            parentAfterCell.slotcurrentItem.gameObject.SetActive(false);
            Data.Instance.Player.GetComponent<PlayerEquipManagment>().RemoveItemofCelltype(equipmentCell.equiptype);
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

    public void CompleteMoveCell(Cell parentCell)
    {
        transform.SetParent(parentCell.transform, false);

        ImagePropertyCellType(parentCell, rect, image);
        if (myBackground == null) ItemManager.Instance.CreateItemBackGround(this);
        // rect.localPosition = Vector3.zero;
        parentCell.slotcurrentItem = myItem;
        image.raycastTarget = true;
        isItemDrop = false;
    }


}
