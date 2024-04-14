using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StackableItem : MonoBehaviour
{
    TextMeshProUGUI count_Text;
    public GameObject text_Prefab;

    private Item currentItem;
    private uint item_Count = 1;
    public uint ItemCount { get { return item_Count; } set { item_Count = value; UpdateCountText(); } }

    void Awake()
    {
        currentItem = GetComponent<Item>();
    }

    void Start()
    {
        text_Prefab = UIManager.Instance.itemtext_prefab.gameObject;
        count_Text = LeanPool.Spawn(text_Prefab, transform).GetComponent<TextMeshProUGUI>();

        count_Text.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public void UpdateCountText()
    {
        count_Text.text = $"{item_Count}";
    }




}
