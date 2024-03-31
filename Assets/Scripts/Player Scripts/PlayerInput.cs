using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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

    void Awake()
    {
        playerEquipManagment = GetComponent<PlayerEquipManagment>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Start()
    {
        uiActionMap = inputActions.FindActionMap("UI");
        playerActionMap = inputActions.FindActionMap("Player");

    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 7 | 1 << 8))
        {
            interactable = hit.collider.GetComponentInParent<IInteractable>();
            // 여기에서 hit.collider를 사용하여 충돌한 객체에 접근할 수 있습니다.
            // 아이템인 경우에만 처리
            if (interactable != null) // "Item"은 충돌한 객체의 태그와 일치해야 합니다.
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
            isInteraction = false;
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

        if(UIManager.Instance.Inventory.activeSelf == false) {
            if(UIManager.Instance.handler_focus != null && UIManager.Instance.handler_focus.transform.childCount > 0) {
                UIManager.Instance.handler_focus.transform.GetChild(0).GetComponent<Image>().color = 
                    UIManager.Instance.GetItemTypeColor(UIManager.Instance.handler_focus.GetComponent<Cell>().slotcurrentItem.type);
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

    public void OnShift(InputValue value) {
        isOnShift_UI = value.isPressed;
        Debug.Log("isOnShift" + isOnShift_UI);
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
                if (cell.slotcurrentItem != null)
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

    
}
