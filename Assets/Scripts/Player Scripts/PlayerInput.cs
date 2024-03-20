using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    bool isInteraction;
    IInteractable interactable;

    public InputActionAsset inputActions;
    private InputActionMap uiActionMap;
    private InputActionMap playerActionMap;

    private void Start() {
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
            } else {
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
        Debug.Log("press E");
        if (isInteraction == true)
        {
            interactable.Interact();
            interactable = null;
        }
    }

    public void OnInventory(InputValue value) {
        Debug.Log("OnInventory Enable");
        bool isActive = UIManager.Instance.Inventory.activeSelf;
        CameraManager.Instance.CursorVisible(!isActive);
        UIManager.Instance.Inventory.SetActive(!isActive);
        ChangeActionMap();
    }

    public void OnDropItem(InputValue value) {
        if(UIManager.Instance.handler_focus != null) {
            Cell cell;
            UIManager.Instance.handler_focus.TryGetComponent(out cell);
            ItemManager.Instance.DropItem(cell.slotcurrentItem);

            cell.item_ParentCell.GetComponentInChildren<UIElementClickHandler>().RemoveCellItem();
            cell.slotcurrentItem = null;

            Destroy(cell.item_ParentCell.transform.GetChild(0).gameObject);
        }
    }

    private void ChangeActionMap() {
        bool isInventoryActive = UIManager.Instance.Inventory.activeSelf;

        if(isInventoryActive == true) {
            playerActionMap.Disable();
            uiActionMap.Enable();
        } else {
            playerActionMap.Enable();
            uiActionMap.Disable();
        }
    }
}
