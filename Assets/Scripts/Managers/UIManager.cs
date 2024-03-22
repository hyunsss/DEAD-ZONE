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
    public EquipmentCell[] equipmentCells;

    public UIElementClickHandler current_MoveItem;

    public Cell cell;
    [HideInInspector] public GameObject Inventory;
    [HideInInspector] public ItemCellPanel player_Inven;

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
        Inventory.SetActive(false);
        equipmentCells = FindObjectsOfType<EquipmentCell>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CellRayCastTarget(bool isallow) {
        if(AllCells.Length == 0) AllCells = GameObject.FindObjectsOfType<Cell>();
        foreach(Cell cell in AllCells) {
            cell.GetComponent<Image>().raycastTarget = isallow;
        }
    }
}
