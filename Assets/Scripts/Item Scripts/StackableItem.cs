using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StackableItem : MonoBehaviour
{
    TextMeshProUGUI count_Text;
    public GameObject text_Prefab;

    public Item currentItem;
    public int ItemCount { 
        get { 
            if(currentItem.TryGetComponent(out IStackable stackable)) {
                return stackable.Count;
            } else {
                return 0;
            }

        } set { 
            if(currentItem.TryGetComponent(out IStackable stackable)) {
                stackable.Count = value;
                UpdateCountText();
            }
        }}
    
     public int ItemMaxCount { 
        get { 
            if(currentItem.TryGetComponent(out IStackable stackable)) {
                return stackable.MaxCount;
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
        ItemCount = currentItem.GetComponent<IStackable>().Count;
    }

    private void Update() {
        if(currentItem == null && TryGetComponent(out UIElementClickHandler component)) {
            currentItem = component.myItem;
        }

        if(currentItem != null) {
            ItemCount = currentItem.GetComponent<IStackable>().Count;
            UpdateCountText();
            if(ItemCount <= 0) {
                ObjectDestroy();
            }
        }
    }

    public void UpdateCountText()
    {
        count_Text.text = $"{ItemCount}";
    }

    void ObjectDestroy() {
        UIElementClickHandler uIElement = GetComponent<UIElementClickHandler>();
        uIElement.parentAfterCell.DestoryChild();
        uIElement.RemoveCellItem();
        Destroy(this);
    }



}
