using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class UIElementClickHandler : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item myItem;
    public Transform parentAfterDrag;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        myItem = parentAfterDrag.GetComponent<Cell>().slotcurrentItem;
        parentAfterDrag.GetComponent<Cell>().slotcurrentItem = null;
        transform.SetParent(UIManager.Instance.topCanvas.transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Vector2 localPointerPosition;
        // if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentAfterDrag.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition))
        // {
        //     rect.localPosition = localPointerPosition;
        // }
        GetComponent<Image>().raycastTarget = false;
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);

        rect.localPosition = Vector3.zero;
        parentAfterDrag.GetComponent<Cell>().slotcurrentItem = myItem;
        GetComponent<Image>().raycastTarget = true;

    }

    
}
