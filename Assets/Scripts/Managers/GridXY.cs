using System;
using System.Collections.Generic;
using UnityEngine;

//그리드 클래스
[Serializable]
public class GridXY
{   //기본 Plane 오브젝트 스케일의 0.5배가 게임 내의 cell 1칸
    private int width;
    private int height;
    private float cellSize;
    private Cell[,] gridArray;
    private Transform trans_parent;

    public int Width => width;
    public int Height => height;
    public float CellSize => cellSize;
    public Cell[,] GridArray => gridArray;
    public Transform Trans_parent => trans_parent;



    public int MaxSize { get => width * height; }

    public GridXY(int width, int height, Transform trans_parent)
    {
        this.width = width;
        this.height = height;
        this.trans_parent = trans_parent;

        //각 cell 위치를 저장하는 배열
        gridArray = new Cell[width, height];

        GenerateGrid();
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
            for(int y = celly; y > celly - item.cellheight; y--) {
                for(int x = cellx; x < cellx + item.cellwidth; x++) {
                    tempCells.Add(GetCell(x, y));
                }
            }
        } else {
            //인덱스 범위를 넘어간다면 리턴
            if(celly + item.cellwidth > height || cellx + item.cellheight > width) {
                isComplete = false;
                return null;
            }
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
