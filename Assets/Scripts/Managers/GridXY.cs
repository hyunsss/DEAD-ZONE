using System;
using System.Collections.Generic;
using UnityEngine;

//그리드 클래스
[Serializable]
public class GridXY
{   
    private Cell[,] gridArray;
    private int width;
    private int height;
    private float cellSize;
    private Transform trans_parent;
    public bool isInit;

    public Cell[,] GridArray => gridArray;
    public int Width => width;
    public int Height => height;
    public float CellSize => cellSize;
    public Transform Trans_parent => trans_parent;

    public int MaxSize { get => width * height; }

    public GridXY(int width, int height, Transform trans_parent)
    {
        isInit = false;
        this.width = width;
        this.height = height;
        this.trans_parent = trans_parent;

        //각 cell 위치를 저장하는 배열
        gridArray = new Cell[width, height];

        
        //위치에 따라 이미지가 들어갈 수 있게끔 변경
    }

    public void GenerateGrid()
    {
        for (int y = gridArray.GetLength(1) - 1 ; y >= 0; y--)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                Cell cell = GameObject.Instantiate(UIManager.Instance.cell, trans_parent).GetComponent<Cell>();
                cell.Current_lotation = new Vector2Int(x, y);
                gridArray[x, y] = cell;

                cell.name = UIManager.Instance.cell.name;
            }
        }

        isInit = true;
    }

    public Cell GetCell(int x, int y) {
        return gridArray[x, y];
    }

    public Cell GetEmptyCell() {
        for (int y = gridArray.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                if(gridArray[x, y].slotcurrentItem == null) {
                    return gridArray[x, y];
                }
            }
        }
        
        return null;
    }
    /// <summary>
    /// 아이템을 옮기기 위한 셀의 정보를 리스트에 담아 반환합니다.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="Start_cell">아이템을 이동시키기 위한 첫번 째 셀 위치</param>
    /// <param name="isComplete">아이템의 width, height가 인덱스 인벤토리의 인덱스 범위를 넘는다면 false를 리턴</param>
    /// <param name="isRotation">아이템의 회전여부를 설정 // 디폴트 : false</param>
    /// <returns></returns>
    public List<Cell> SizeofItemCellList(Item item, Cell Start_cell, out bool isComplete, bool isRotation = false) {
        List<Cell> tempCells = new List<Cell>();
        int cellx, celly;
        cellx = Start_cell.Current_lotation.x;
        celly = Start_cell.Current_lotation.y;

        // 할당된 인벤토리 그리드에서 인덱스 범위가 넘지 않도록 조건체크 해주기
        if(isRotation == false) {
            //인덱스 범위를 넘어간다면 리턴
            if(celly - item.cellheight < -1 || cellx + item.cellwidth > width) {
                isComplete = false;
                return null;
            }
            //인덱스 범위를 벗어나지 않았으므로 필요한 셀의 정보를 리스트에 담아 리턴시킵니다.   
            for(int y = celly; y > celly - item.cellheight; y--) {
                for(int x = cellx; x < cellx + item.cellwidth; x++) {
                    tempCells.Add(GetCell(x, y));
                }
            }
        // isRotation == true : 아이템의 회전여부가 true일 때
        } else {
            //인덱스 범위를 넘어간다면 리턴
            if(celly + item.cellwidth > height || cellx + item.cellheight > width) {
                isComplete = false;
                return null;
            }
            //인덱스 범위를 벗어나지 않았으므로 필요한 셀의 정보를 리스트에 담아 리턴시킵니다.  
            for(int y = celly; y < celly + item.cellwidth; y++) {
                for(int x = cellx; x < cellx + item.cellheight; x++) {
                    tempCells.Add(GetCell(x, y));
                }
            }
        }
        
        isComplete = true;
        return tempCells;
    }

    public bool IsCellInItemPossible(List<Cell> cells) {
        foreach(Cell cell in cells) {
            if(cell.slotcurrentItem != null) {
                return false;
            }
        }

        return true;
    }

    public string GetString(int x, int y)
    {
        return $"{x}, {y}";
    }

}
