using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas topCanvas;
    public static UIManager Instance;

    [Space(30f)]
    [Header("Focus Properties")]
    public GameObject handler_focus;
    public Color defaultColor = new Color(132f / 255f, 162f / 255f, 198f / 255f, 125f / 255f);
    public Color focusColor = new Color(226f / 255f, 226f / 255f, 226f / 255f, 125f / 255f);
    public UserInteractionPanel interactionPanel;

    public Cell[] AllCells;
    private EquipmentCell[] equipmentCells;
    public Dictionary<EquipmentType, EquipmentCell> equipCell_Dic = new Dictionary<EquipmentType, EquipmentCell>();

    public UIElementClickHandler current_MoveItem;

    public Cell cell;
    [HideInInspector] public GameObject Inventory;
    [HideInInspector] public ItemCellPanel player_Inven;
    public Transform inven_trasform;

    bool firstInit = false;
    float startTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }


        Inventory = GameObject.Find("Inventory Canvas").gameObject;
        // player_Inven = GameObject.Find("BackPack Panel/Inventory").GetComponent<ItemCellPanel>();
        inven_trasform = GameObject.Find("BackPack Panel").transform;
    }
    // Start is called before the first frame update
    void Start()
    {

        startTime = Time.time;
    }

    void LateStart()
    {
        equipmentCells = FindObjectsOfType<EquipmentCell>();
        AllCells = FindObjectsOfType<Cell>();
        Debug.Log(equipmentCells.Length);

        foreach (EquipmentCell cell in equipmentCells)
        {
            equipCell_Dic.Add(cell.equiptype, cell);
        }
        Inventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (firstInit == false && 0.1f < Time.time - startTime)
        {
            Debug.Log("eanble latestart");
            LateStart();
            firstInit = true;
        }
    }

    public void CellRayCastTarget(bool isallow)
    {
        // if(AllCells.Length == 0) AllCells = GameObject.FindObjectsOfType<Cell>();
        foreach (Cell cell in AllCells)
        {
            cell.GetComponent<Image>().raycastTarget = isallow;
        }
    }

    public Color GetItemTypeColor(ItemKey key = ItemKey.Not)
    {
        switch (key)
        {
            case ItemKey.Not:
                return new Color(96f / 255f, 96f / 255f, 96f / 255f, 125f / 255f);
            case ItemKey.Bag:
                return new Color(112f / 255f, 95f / 255f, 130f / 255f, 125f / 255f);
            case ItemKey.Weapon:
                return new Color(33f / 255f, 33f / 255f, 33f / 255f, 125f / 255f);
            case ItemKey.Etc:
                return new Color(86f / 255f, 106f / 255f, 142f / 255f, 125f / 255f);
            case ItemKey.Food:
                return new Color(132f / 255f, 162f / 255f, 198f / 255f, 125f / 255f);
            case ItemKey.Medical:
                return new Color(181f / 255f, 87f / 255f, 77f / 255f, 125f / 255f);
            default:
                return new Color(132f / 255f, 162f / 255f, 198f / 255f, 125f / 255f);
        }

    }
}
