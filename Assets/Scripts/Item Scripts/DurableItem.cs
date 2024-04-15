using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using TMPro;
using UnityEngine;

public class DurableItem : MonoBehaviour
{
    TextMeshProUGUI count_Text;
    public GameObject text_Prefab;

    public Item currentItem;
    private int item_Duration;
    private int item_MaxDuration;
    public int ItemDuration { 
        get { 
            if(currentItem.TryGetComponent(out IDurable durable)) {
                return durable.Durability;
            } else {
                return 0;
            }

        } set { 
            if(currentItem.TryGetComponent(out IDurable durable)) {
                durable.Durability = value;
                UpdateCountText();
            }
        }}

    public int ItemMaxDuration { 
        get { 
            if(currentItem.TryGetComponent(out IDurable durable)) {
                return durable.MaxDurability;
            } else {
                return 0;
            }
        }}

    void Awake()
    {
        currentItem = GetComponent<Item>();
    }

    void Start()
    {
        text_Prefab = UIManager.Instance.itemtext_prefab.gameObject;
        count_Text = LeanPool.Spawn(text_Prefab, transform).GetComponent<TextMeshProUGUI>();

        count_Text.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        UpdateCountText();
    }

    public void Init() {
        item_MaxDuration = currentItem.GetComponent<IDurable>().MaxDurability;
        item_Duration = currentItem.GetComponent<IDurable>().Durability;
    }

    private void Update() {
        if(currentItem == null && TryGetComponent(out UIElementClickHandler component)) {
            currentItem = component.myItem;
        }

        if(currentItem != null) {
            // item_Duration = currentItem.GetComponent<IDurable>().Durability;
            // item_MaxDuration = currentItem.GetComponent<IDurable>().MaxDurability;
            UpdateCountText();

            if(item_Duration <= 0 && currentItem.TryGetComponent(out Magazine magazine) == false) {
                ObjectDestroy();
            }
        }
    }

    public void UpdateCountText()
    {
        count_Text.text = $"{ItemDuration}/{ItemMaxDuration}";
    }

    void ObjectDestroy() {

    }

}
