using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas topCanvas;
    public static UIManager Instance;
    public GameObject handler_focus;
    public UserInteractionPanel interactionPanel;

    public Cell[] AllCells;
    private EquipmentCell[] equipmentCells;
    public Dictionary<EquipmentType, EquipmentCell> equipCell_Dic = new Dictionary<EquipmentType, EquipmentCell>();

    public UIElementClickHandler current_MoveItem;

    public Cell cell;
    [HideInInspector] public GameObject Inventory;
    [HideInInspector] public ItemCellPanel player_Inven;
    public GameObject inventory_prefab;

    bool firstInit = false;
    float startTime;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }


        Inventory = GameObject.Find("Inventory Canvas").gameObject;
        player_Inven = GameObject.Find("BackPack Panel/Inventory").GetComponent<ItemCellPanel>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
        startTime = Time.time;
    }

    void LateStart() {
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
        if(firstInit == false && 0.001f < Time.time - startTime) {
            Debug.Log("eanble latestart");
            LateStart();
            firstInit = true;
        }
    }

    public void CellRayCastTarget(bool isallow) {
        // if(AllCells.Length == 0) AllCells = GameObject.FindObjectsOfType<Cell>();
        foreach(Cell cell in AllCells) {
            cell.GetComponent<Image>().raycastTarget = isallow;
        }
    }
}
