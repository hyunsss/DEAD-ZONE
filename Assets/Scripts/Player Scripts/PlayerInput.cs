using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
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

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 7))
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
        bool isActive = UIManager.Instance.Inventory.activeSelf;
        CameraManager.Instance.CursorVisible(!isActive);
        UIManager.Instance.Inventory.SetActive(!isActive);
        UIManager.Instance.CellRayCastTarget(false);
        ChangeActionMap();
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
        if (playerEquipManagment.weapons[0] != null)
        {
            AssignmentWeapon(0);
        }
    }

    public void OnWeapon2(InputValue value)
    {
        if (playerEquipManagment.weapons[1] != null)
        {
            AssignmentWeapon(1);
        }
    }

    public void AssignmentWeapon(int index)
    {
        playerEquipManagment.currentindex = index;
        playerAttack.CurrentWeapon = playerEquipManagment.GetWeapon(index);

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
                    playerEquipManagment.RemoveWeapon(equipmentcell.slotcurrentItem as Weapon);
                    equipmentcell.RemoveItem();
                }
            }
            else if (UIManager.Instance.handler_focus.TryGetComponent(out Cell cell))
            {
                if (cell.slotcurrentItem != null)
                {
                    cell.RemoveItem();
                    cell.Item_ParentCell.GetComponentInChildren<UIElementClickHandler>().RemoveCellItem();
                }
            }
        }
    }

    private void ChangeActionMap()
    {
        bool isInventoryActive = UIManager.Instance.Inventory.activeSelf;

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
}
