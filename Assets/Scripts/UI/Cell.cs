using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item slotcurrentItem;
    public Vector2 current_lotation;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if(dropped.TryGetComponent(out UIElementClickHandler component)) {
            component.parentAfterDrag = transform;
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.handler_focus = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UIManager.Instance.handler_focus == gameObject) UIManager.Instance.handler_focus = null;
    }
}
