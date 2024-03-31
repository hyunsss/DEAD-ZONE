using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BoxType { PlayerBox, RootBox, None }

public class ItemCellPanel : MonoBehaviour
{
    public int width;
    public int height;

    public BoxType boxType;
    RectTransform rect;
    GridLayoutGroup gridLayoutGroup;

    public Transform parent_Bag;

    public GridXY grid;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        rect.sizeDelta = new Vector2(width * gridLayoutGroup.cellSize.x + 10, height * gridLayoutGroup.cellSize.y + 10);
        gridLayoutGroup.padding.left = 5;
        gridLayoutGroup.padding.top = 5;
    }
    // Start is called before the first frame update
    void Start()
    {
        grid = new GridXY(width, height, transform);
    }

    // public void ShowInventory(Transform UIParent)
    // {
    //     transform.SetParent(UIParent, false);
    //     gameObject.SetActive(true);
    // }

    // public void HideInventory()
    // {
    //     transform.SetParent(parent_Bag, false);
    //     gameObject.SetActive(false);
    // }



    public void InsertItem(Item item, int x, int y, out bool isInsert)
    {
        Cell cell = grid.GetCell(x, y);

        if (cell == null)
        {
            isInsert = false;
            return;
        }

        isInsert = true;
        return;
    }

    public void QuickInsterItem(Item item, out bool isInsert, out Cell cell)
    {
        cell = grid.GetEmptyCell();

        if (cell == null)
        {
            isInsert = false;
            return;
        }

        isInsert = true;
        return;
    }



    /*
    1. 아이템을 인벤토리로 옮겼을 때 남는 크기에 맞는 셀이 존재하는지 확인.
    2. 존재한다면 그거에 맞게 셀 크기를 조정해준 뒤 이미지를 삽입한다. 
    
    
    */
}
