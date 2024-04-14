using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using TMPro;
using UnityEngine;

public class DurableItem : MonoBehaviour
{
    TextMeshProUGUI count_Text;
    public GameObject text_Prefab;

    private Item currentItem;
    private uint item_Duration = 100;
    private uint item_MaxDuration = 100;

    public uint ItemDuration { get { return item_Duration; } set { item_Duration = value; } }
    public uint ItemMaxDuration { get { return item_MaxDuration; } set { item_MaxDuration = value; UpdateCountText(); } }

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
        count_Text.text = $"{item_Duration}/{item_MaxDuration}";
    }

}
