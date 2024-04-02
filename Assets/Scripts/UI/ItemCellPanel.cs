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

}
