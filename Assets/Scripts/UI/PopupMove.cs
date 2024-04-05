using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupMove : MonoBehaviour, IDragHandler
{
    
    public void OnDrag(PointerEventData eventData)
    {
        transform.parent.GetComponent<RectTransform>().position = eventData.position;
    }

}
