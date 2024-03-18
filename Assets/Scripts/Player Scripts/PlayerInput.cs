using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    bool isInteraction;
    IInteractable interactable;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // 여기에서 hit.collider를 사용하여 충돌한 객체에 접근할 수 있습니다.
            // 아이템인 경우에만 처리
            if (hit.collider.TryGetComponent(out interactable)) // "Item"은 충돌한 객체의 태그와 일치해야 합니다.
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
        if (isInteraction == true)
        {
            interactable.Interact();
        }
    }

    public void OnInventory(InputValue value) {
        Debug.Log("OnInventory Enable");
        bool isActive = UIManager.Instance.Inventory.activeSelf;
        CameraManager.Instance.CursorVisible(!isActive);
        UIManager.Instance.Inventory.SetActive(!isActive);
    }

    public void OnDropItem(InputValue value) {
        if(UIManager.Instance.handler_focus != null) {
            Cell cell;
            UIManager.Instance.handler_focus.TryGetComponent(out cell);
            ItemManager.Instance.DropItem(cell.slotcurrentItem);
            cell.slotcurrentItem = null;

            Destroy(cell.transform.GetChild(0).gameObject);
        }
    }
}
