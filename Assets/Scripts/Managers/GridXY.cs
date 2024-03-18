using UnityEngine;

//그리드 클래스
public class GridXY
{   //기본 Plane 오브젝트 스케일의 0.5배가 게임 내의 cell 1칸
    private int width;
    private int height;
    private float cellSize;
    private Cell[,] nodeArray;
    private Transform trans_parent;

    public int Width => width;
    public int Height => height;
    public float CellSize => cellSize;
    public Cell[,] GridArray => nodeArray;
    public Transform Trans_parent => trans_parent;



    public int MaxSize { get => width * height; }

    public GridXY(int width, int height, Transform trans_parent)
    {
        this.width = width;
        this.height = height;
        this.trans_parent = trans_parent;

        //각 cell 위치를 저장하는 배열
        nodeArray = new Cell[width, height];

        GenerateGrid();
        //위치에 따라 이미지가 들어갈 수 있게끔 변경
    }

    public void GenerateGrid()
    {
        Debug.Log("그리드 생성 로직 enable");
        Debug.Log(trans_parent);
        for (int y = 0; y < nodeArray.GetLength(1); y++)
        {
            for (int x = 0; x < nodeArray.GetLength(0); x++)
            {
                Cell cell = GameObject.Instantiate(UIManager.Instance.cell, trans_parent).GetComponent<Cell>();
                cell.current_lotation = new Vector2(y, x);
                nodeArray[x, y] = cell;

                cell.name = UIManager.Instance.cell.name;
            }
        }
    }

    public Cell GetCell(int x, int y) {
        return nodeArray[x, y];
    }

    public Cell GetEmptyCell() {
        for (int y = 0; y < nodeArray.GetLength(1); y++)
        {
            for (int x = 0; x < nodeArray.GetLength(0); x++)
            {
                if(nodeArray[x, y].slotcurrentItem == null) {
                    return nodeArray[x, y];
                }
            }
        }
        
        return null;
    }

    public string GetString(int x, int y)
    {
        return $"{x}, {y}";
    }

}
