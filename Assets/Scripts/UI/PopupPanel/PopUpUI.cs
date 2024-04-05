using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpUI : MonoBehaviour {
    public Item currentItem;

    private TextMeshProUGUI header_text;
    private Button closeButton;
    private RectTransform content_rect;
    private GameObject cellPanel;

    public void PopupInit(Item item, GameObject itemcellPanel = null)
    {
        currentItem = item;
        header_text = transform.Find("Header/Item Title").GetComponent<TextMeshProUGUI>();
        closeButton = transform.Find("Header/Close Button").GetComponent<Button>();
        content_rect = transform.Find("Content").GetComponent<RectTransform>();
        UIManager.Instance.popUp_dic.Add(currentItem.gameObject, this);

        header_text.text = item.item_name;
        closeButton.onClick.AddListener(CloseUI);
        if (itemcellPanel != null)
        {
            GetComponent<RectTransform>().sizeDelta = itemcellPanel.GetComponent<RectTransform>().sizeDelta + new Vector2(10f, 10f);
            cellPanel = itemcellPanel;
            // content_rect.sizeDelta = new Vector2(item.cellwidth * 70 + 10f, item.cellheight * 70 + 10f);
            cellPanel.transform.SetParent(content_rect, false);
            cellPanel.gameObject.SetActive(true);
        } else {
            cellPanel = null;
        }
    }

    public void CloseUI()
    {
        if (cellPanel != null)
        {
            cellPanel.transform.SetParent(currentItem.transform, false);
            cellPanel.gameObject.SetActive(false);
        }

        UIManager.Instance.popUp_dic.Remove(currentItem.gameObject);
        LeanPool.Despawn(this);
    }

}
