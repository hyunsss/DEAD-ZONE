using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCellPanel : MonoBehaviour
{
    public int width;
    public int height;

    RectTransform rect;

    GridXY grid;


    private void Awake() {
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width * 100, height * 100);
    }
    // Start is called before the first frame update
    void Start()
    {
        grid = new GridXY(width, height, transform);
        /*
            그리드를 그릴 때    5번째 1 2 3 4 5
                            4번째 1 2 3 4 5
                            .
                            .
                            .
                            순서이므로 반복문으로 셀마다의 위치를 지정해줄 때 주의할 것.
        
        */
    }

    public void InsertItem(Item item, int x, int y, out bool isInsert) {
        Cell cell = grid.GetCell(x, y);

        if(cell == null) {
            isInsert = false;
            return;
        }

        cell.slotcurrentItem = item;
        isInsert = true;
        return;
    }

    public void QuickInsterItem(Item item, out bool isInsert, out Cell cell) {
        cell = grid.GetEmptyCell();

        if(cell == null) {
            isInsert = false;
            return;
        }

        cell.slotcurrentItem = item;
        isInsert = true;
        return;
    }
}
