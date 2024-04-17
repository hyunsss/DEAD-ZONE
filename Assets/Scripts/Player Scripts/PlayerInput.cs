using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using Lean.Pool;

public class PlayerInput : MonoBehaviour
{
    bool isInteraction;
    IInteractable interactable;
    PlayerEquipManagment playerEquipManagment;
    PlayerAttack playerAttack;

    public InputActionAsset inputActions;
    private InputActionMap uiActionMap;
    private InputActionMap playerActionMap;

    private bool isOnShift_UI;

    Plane plane;
    Vector3 plane_RayCastPos;

    #region double click properties
    float doubleClickTimeLimit = 0.3f;
    float lastClickTime;
    #endregion

    void Awake()
    {
        playerEquipManagment = GetComponent<PlayerEquipManagment>();
        playerAttack = GetComponent<PlayerAttack>();
        plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Start()
    {
        uiActionMap = inputActions.FindActionMap("UI");
        playerActionMap = inputActions.FindActionMap("Player");

    }

    void FixedUpdate()
    {
        if (UIManager.Instance.Inventory.activeSelf == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            Collider[] colliders;
            //Raycast(ray, out hit, Mathf.Infinity, 1 << 7 | 1 << 8)
            if (plane.Raycast(ray, out float enter))
            {
                plane_RayCastPos = ray.GetPoint(enter);

                float playerToRay = Vector3.Distance(plane_RayCastPos, transform.position);

                if (playerToRay < 2f)
                {
                    colliders = Physics.OverlapSphere(plane_RayCastPos, 0.5f, 1 << 7 | 1 << 8);

                    float shortDistance = 999f;
                    GameObject nearObject = null;
                    foreach (Collider collider in colliders)
                    {
                        if (Vector3.Distance(plane_RayCastPos, collider.transform.position) < shortDistance)
                        {
                            shortDistance = Vector3.Distance(plane_RayCastPos, collider.transform.position);
                            nearObject = collider.gameObject;
                        }
                    }

                    if (nearObject != null)
                    {
                        if (UIManager.Instance.current_interactionPanel == null)
                        {
                            UIManager.Instance.current_interactionPanel = LeanPool.Spawn(UIManager.Instance.interactionPanel_prefab, UIManager.Instance.interactionParent, false);
                        }

                        if (nearObject.transform.parent.TryGetComponent(out Item item))
                        {
                            UIManager.Instance.current_interactionPanel.SetText(item.name, "줍기 E");
                        }
                        else if (nearObject.transform.parent.TryGetComponent(out RootingBox rootingBox))
                        {
                            UIManager.Instance.current_interactionPanel.SetText(rootingBox.name, "열기 E");
                        }

                        interactable = nearObject.GetComponentInParent<IInteractable>();
                    }
                    else
                    {
                        interactable = null;
                        UIManager.Instance.TryDespawnInteractPanel();
                    }

                    // 아이템인 경우에만 처리
                    if (interactable != null)
                    {
                        isInteraction = true;
                    }
                    else
                    {
                        isInteraction = false;
                    }
                }
                else
                {
                    UIManager.Instance.TryDespawnInteractPanel();
                }
            }
            else
            {
                isInteraction = false;
            }
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;  // 기즈모의 색 설정
        if (plane_RayCastPos != null && Vector3.Distance(plane_RayCastPos, transform.position) < 2f)
        {
            Gizmos.DrawWireSphere(plane_RayCastPos, 0.5f);  // 와이어프레임 스피어 그리기
        }
    }

    public void OnInteraction(InputValue value)
    {
        if (isInteraction == true)
        {
            interactable.Interact();
            interactable = null;
        }
    }

    public void OnInventory(InputValue value)
    {
        UIManager.Instance.ShowPlayerInventory();

        if (UIManager.Instance.Inventory.activeSelf == false)
        {
            if (UIManager.Instance.handler_focus != null && UIManager.Instance.handler_focus.transform.childCount > 0)
            {
                UIManager.Instance.handler_focus.transform.Find("ItemBackgroundImage").GetComponent<Image>().color =
                    UIManager.Instance.GetItemTypeColor(UIManager.Instance.handler_focus.GetComponent<Cell>().slotcurrentItem.item_Key);
            }
        }
    }

    public void OnItemRotation(InputValue value)
    {
        if (UIManager.Instance.current_MoveItem != null)
        {
            UIManager.Instance.current_MoveItem.ItemRotation();
        }
    }

    public void OnWeapon1(InputValue value)
    {
        if (playerEquipManagment.Weapons[0] != null)
        {
            AssignmentWeapon(0);
        }
    }


    public void OnWeapon2(InputValue value)
    {
        if (playerEquipManagment.Weapons[1] != null)
        {
            AssignmentWeapon(1);
        }
    }

    public void AssignmentWeapon(int index)
    {
        playerEquipManagment.currentindex = index;
        playerAttack.CurrentWeapon = playerEquipManagment.Weapons[index];

    }

    public void OnDropItem(InputValue value)
    {
        if (UIManager.Instance.handler_focus != null)
        {
            if (UIManager.Instance.handler_focus.TryGetComponent(out EquipmentCell equipmentcell))
            {
                if (equipmentcell.slotcurrentItem != null)
                {
                    //장착 아이템을 해제하는 로직
                    //해당 슬롯의 장비 유형에 따라 아이템을 해체하는 로직을 수행합니다.
                    playerEquipManagment.RemoveItemofCelltype(equipmentcell.equiptype);
                    equipmentcell.RemoveItem();
                }
            }
            else if (UIManager.Instance.handler_focus.TryGetComponent(out Cell cell))
            {
                if (cell.slotcurrentItem != null && cell.slotcurrentItem.isSearchable == true)
                {
                    // 안전하게 UIElementClickHandler 찾기 및 RemoveCellItem 호출
                    UIElementClickHandler clickHandler = cell.Item_ParentCell.GetComponentInChildren<UIElementClickHandler>();
                    if (clickHandler != null)
                    {
                        //아래 두 함수 아이템 참조 해제 시키는 부분 순서 결정이 필요함
                        cell.RemoveItem();
                        clickHandler.RemoveCellItem();
                    }
                    else
                    {
                        Debug.Log("UIElementClickHandler not found on Item_ParentCell.");
                    }
                }
            }
        }
    }

    public void OnShift(InputValue value)
    {
        isOnShift_UI = value.isPressed;
        Debug.Log("isOnShift" + isOnShift_UI);
    }

    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            //쉬프트를 눌렀을 때 현재 마우스 위치에 있는 오브젝트가 아이템이 존재하는 셀 일경우 아이템 이동
            if (isOnShift_UI == true && UIManager.Instance.handler_focus != null)
            {
                Cell focus_cell = UIManager.Instance.handler_focus.GetComponent<Cell>();

                if (focus_cell.slotcurrentItem != null && focus_cell is EquipmentCell equipmentCell)
                {
                    UIElementClickHandler clickHandler = focus_cell.GetComponentInChildren<UIElementClickHandler>();
                    UIManager.Instance.ShiftQuickMoveItem(BoxType.EquipCell, focus_cell.slotcurrentItem, out bool Finish);
                    if (Finish == true)
                    {
                        focus_cell.DestoryChild();
                        clickHandler.RemoveCellItem();
                    }
                }
                else if (focus_cell.slotcurrentItem != null)
                {
                    UIElementClickHandler clickHandler = focus_cell.GetComponentInChildren<UIElementClickHandler>();
                    UIManager.Instance.ShiftQuickMoveItem(focus_cell.GetComponentInParent<ItemCellPanel>().boxType, focus_cell.slotcurrentItem, out bool Finish);
                    if (Finish == true)
                    {
                        focus_cell.DestoryChild();
                        clickHandler.RemoveCellItem();
                    }
                }
            }

            float clickTime = Time.time - lastClickTime;
            if (clickTime <= doubleClickTimeLimit)
            {
                //더블 클릭 함수 실행
                DoubleClick();
                lastClickTime = 0;
            }
            else
            {
                lastClickTime = Time.time;
            }
        }

    }

    public void DoubleClick()
    {
        if (UIManager.Instance.handler_focus != null &&
                                UIManager.Instance.handler_focus.TryGetComponent(out EquipmentCell equipmentCell) == false)
        {
            UIManager.Instance.handler_focus.TryGetComponent(out Cell focus_cell);

            //딕셔너리에 등록되어있으면 상단 없으면 하단 (새로 생성후 추가)
            if (focus_cell.slotcurrentItem != null && UIManager.Instance.popUp_dic.ContainsKey(focus_cell.slotcurrentItem.gameObject))
            {
                UIManager.Instance.popUp_dic[focus_cell.slotcurrentItem.gameObject].transform.SetAsLastSibling();
            }
            else if (focus_cell.slotcurrentItem != null && focus_cell.slotcurrentItem.isSearchable == true)
            {
                PopUpUI popup = LeanPool.Spawn(UIManager.Instance.popUpUI_prefab, UIManager.Instance.PopUpTransform, false);
                if (focus_cell != null && (focus_cell.slotcurrentItem.item_Key & (ItemKey.Bag | ItemKey.Armor)) != ItemKey.Not)
                {
                    if (focus_cell.slotcurrentItem is Bag bag)
                    {
                        popup.PopupInit(focus_cell.slotcurrentItem, bag.currentBagInventory);
                    }
                    else if (focus_cell.slotcurrentItem is Armor armor)
                    {
                        popup.PopupInit(focus_cell.slotcurrentItem, armor.currentRigInventory);
                    }
                }
                else
                {
                    popup.PopupInit(focus_cell.slotcurrentItem);
                }
            }

        }
    }

}