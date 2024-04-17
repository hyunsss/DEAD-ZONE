using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using Steamworks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas topCanvas;
    public static UIManager Instance;

    [Space(30f)]
    [Header("Focus Properties")]
    public bool isfocusEnable;
    public GameObject handler_focus;
    public Color defaultColor = new Color(132f / 255f, 162f / 255f, 198f / 255f, 125f / 255f);
    public Color focusColor = new Color(226f / 255f, 226f / 255f, 226f / 255f, 125f / 255f);

    [Space]
    [Header("상호작용 패널")]
    public RectTransform interactionParent;
    public UserInteractionPanel interactionPanel_prefab;
    public UserInteractionPanel current_interactionPanel;

    [Space]
    [Header("루팅 박스 패널")]
    public RootingBox currentRootBox;
    public Button searchButton;

    [Space]
    [Header("로딩 UI 리스트")]
    public LoadingUI loadingUI_prefab;
    public List<LoadingUI> loadingUI_list = new List<LoadingUI>();

    [Space]
    [Header("Magazine Coroutine")]
    [HideInInspector] public Coroutine MagInsertCoroutine;


    [Space]
    [Header("Cell Lists")]
    public Cell[] AllCells;
    private EquipmentCell[] equipmentCells;
    public Dictionary<EquipmentType, EquipmentCell> equipCell_Dic = new Dictionary<EquipmentType, EquipmentCell>();
    public UIElementClickHandler current_MoveItem;

    [Space]
    [Header("Cell Prefab")]
    public Cell cell;

    [Space]
    [Header("Item Text Prefab")]
    public TextMeshProUGUI itemtext_prefab;

    [Space]
    [Header("Transform")]
    public RectTransform inven_transform;
    public RectTransform rig_transform;
    public RectTransform pocket_transform;
    public RectTransform belt_transform;

    public RectTransform rooting_transform;
    [Space]
    [Header("ItemCellPanel")]
    public ItemCellPanel _6by6Panel;

    [Space]
    [Header("Player Input Action")]
    public InputActionAsset inputActions;
    private InputActionMap uiActionMap;
    private InputActionMap playerActionMap;

    [Space]
    [Header("UIPopup")]
    public RectTransform PopUpTransform;
    public PopUpUI popUpUI_prefab;
    public Dictionary<GameObject, PopUpUI> popUp_dic = new Dictionary<GameObject, PopUpUI>();

    [HideInInspector] public GameObject Inventory;
    public List<ItemCellPanel> player_Inven = new List<ItemCellPanel>();
    public List<ItemCellPanel> target_Inven = new List<ItemCellPanel>();

    bool firstInit = false;
    float startTime;

    public void TryDespawnInteractPanel() {
        if(current_interactionPanel != null) {
            LeanPool.Despawn(current_interactionPanel);
            current_interactionPanel = null;
        }
    }

    public void InsertCellInventory(GameObject target, RectTransform parent)
    {
        target.transform.SetParent(parent, false);
        target.SetActive(true);

        RectTransform child_rect = target.GetComponent<RectTransform>();
        parent.sizeDelta = new Vector2(child_rect.sizeDelta.x + 10f, child_rect.sizeDelta.y + 10f);

        ItemCellPanel[] itemCellPanels = target.GetComponentsInChildren<ItemCellPanel>();
        foreach (ItemCellPanel itemCell in itemCellPanels)
        {
            itemCell.boxType = BoxType.PlayerBox;
            player_Inven.Add(itemCell);
        }
    }

    public void RemoveCellInventory(GameObject target, Transform parent)
    {
        target.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(180f, 180f);
        target.transform.SetParent(parent, false);
        target.SetActive(false);

        ItemCellPanel[] itemCellPanels = target.GetComponentsInChildren<ItemCellPanel>();
        foreach (ItemCellPanel itemCell in itemCellPanels)
        {
            itemCell.boxType = BoxType.None;
            player_Inven.Remove(itemCell);
        }
    }

    public void ShowPlayerInventory()
    {
        bool isActive = Inventory.activeSelf;
        CameraManager.Instance.CursorVisible(!isActive);
        Inventory.SetActive(!isActive);

        if (isActive == true)
        {
            if (currentRootBox != null)
            {
                rooting_transform.gameObject.SetActive(false);
                currentRootBox.currentItemCellPanel.transform.SetParent(currentRootBox.transform, false);
                currentRootBox.StopMyCoroutine();
                currentRootBox = null;
            }

            if (MagInsertCoroutine != null)
            {
                StopCoroutine(MagInsertCoroutine);
                MagInsertCoroutine = null;
            }

            foreach (LoadingUI loadingUI in loadingUI_list)
            {
                loadingUI.Destroy();
            }
            loadingUI_list.Clear();

        } else {
            TryDespawnInteractPanel();
        }

        CellRayCastTarget(false);
        ChangeActionMap();
        CloseAllPopup();
    }

    private void CloseAllPopup()
    {
        List<PopUpUI> popup_list = new List<PopUpUI>(popUp_dic.Values);
        foreach (var popup in popup_list)
        {
            popup.CloseUI();
        }
    }

    public IEnumerator GameObjectActiveOper(GameObject gameObject, bool active)
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(active);
        yield return new WaitForSeconds(0.3f);
    }

    private void ChangeActionMap()
    {
        bool isInventoryActive = Inventory.activeSelf;

        if (isInventoryActive == true)
        {
            playerActionMap.Disable();
            uiActionMap.Enable();
        }
        else
        {
            playerActionMap.Enable();
            uiActionMap.Disable();
        }
    }

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

        isfocusEnable = true;
        Inventory = GameObject.Find("Inventory Canvas").gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        uiActionMap = inputActions.FindActionMap("UI");
        playerActionMap = inputActions.FindActionMap("Player");
    }

    private void InventoryInit()
    {
        RectTransform[] rects = { inven_transform, rig_transform, pocket_transform, belt_transform };

        foreach (RectTransform rect in rects)
        {
            ItemCellPanel[] itemcells = rect.GetComponentsInChildren<ItemCellPanel>();

            if (itemcells.Length > 0)
            {
                foreach (ItemCellPanel itemcell in itemcells)
                {
                    player_Inven.Add(itemcell);
                }
            }
        }
    }

    public void LateStart()
    {
        equipmentCells = FindObjectsOfType<EquipmentCell>(true);
        AllCells = FindObjectsOfType<Cell>(true);

        foreach (EquipmentCell cell in equipmentCells)
        {
            equipCell_Dic.Add(cell.equiptype, cell);
        }
        Inventory.SetActive(false);
        InventoryInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (firstInit == false && 0.2f < Time.time - startTime)
        {
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

    public void ShiftQuickMoveItem(BoxType boxType, Item item, out bool Finish)
    {
        switch (boxType)
        {
            case BoxType.PlayerBox:
                if (currentRootBox != null)
                {
                    ItemManager.Instance.MoveToInventoryFindCell(currentRootBox.currentItemCellPanel.grid, item, out Finish);
                    if (Finish == true) return;
                }

                break;
            case BoxType.RootBox:
                if (player_Inven.Count > 0)
                {
                    foreach (ItemCellPanel itemCell in player_Inven)
                    {
                        Debug.Log(itemCell);
                        ItemManager.Instance.MoveToInventoryFindCell(itemCell.grid, item, out Finish);

                        if (Finish == true) return;
                    }
                }
                break;
            case BoxType.EquipCell:
                if (player_Inven.Count > 0)
                {
                    foreach (ItemCellPanel itemCell in player_Inven)
                    {
                        Debug.Log(itemCell);
                        ItemManager.Instance.MoveToInventoryFindCell(itemCell.grid, item, out Finish);

                        if (Finish == true) return;
                    }
                }
                break;

            default:
                break;
        }

        Finish = false;
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
            case ItemKey.Helmat:
                return new Color(54f / 255f, 25f / 255f, 121f / 255f, 125f / 255f);
            case ItemKey.Etc:
                return new Color(86f / 255f, 106f / 255f, 142f / 255f, 125f / 255f);
            case ItemKey.Magazine:
                return new Color(79f / 255f, 72f / 255f, 80f / 255f, 125f / 255f);
            case ItemKey.Food:
                return new Color(132f / 255f, 162f / 255f, 198f / 255f, 125f / 255f);
            case ItemKey.Medical:
                return new Color(181f / 255f, 87f / 255f, 77f / 255f, 125f / 255f);
            case ItemKey.Ammo:
            case ItemKey.AmmoBox:
                return new Color(210f / 255f, 180f / 255f, 140f / 255f, 125f / 255f);
            case ItemKey.Key:
                return new Color(76f / 255f, 0f / 255f, 153f / 255f, 125f / 255f);
            default:
                return new Color(132f / 255f, 162f / 255f, 198f / 255f, 125f / 255f);
        }

    }
}
