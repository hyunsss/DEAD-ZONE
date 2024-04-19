using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsageValueSettingPanel : MonoBehaviour
{
    private bool isUpdating = false;

    public Item currentItem;

    public TextMeshProUGUI itemNameText;
    public TMP_InputField set_Durability;
    public TextMeshProUGUI max_Durability;
    public Scrollbar scrollbar;
    public Button confirm_Btn;
    public Button cancel_Btn;

    // Start is called before the first frame update
    public void Init()
    {
        itemNameText.text = $"{currentItem.item_name} 사용";
        max_Durability.text = currentItem.GetComponent<IDurable>().Durability.ToString();

        confirm_Btn.onClick.AddListener(ConfirmButtonClick);
        cancel_Btn.onClick.AddListener(CancelButtonClick);

        set_Durability.onValueChanged.AddListener((x) => SetScrollBarValue(x));
        scrollbar.onValueChanged.AddListener((x) => SetInpuFieldValue(x));
        SetInpuFieldValue(scrollbar.value);
    }

    void ConfirmButtonClick()
    {
        if(int.Parse(set_Durability.text.ToString()) == 0) {
            CancelButtonClick();
            return;
        }

        IUseable useable = currentItem.GetComponent<IUseable>();
        TimeDelay delay_panel = LeanPool.Spawn(UIManager.Instance.timeDelay_prefab, UIManager.Instance.topCanvas.transform);
        delay_panel.Init(useable.Delay, 1 - scrollbar.value, useable.Use);

        Reset();
        LeanPool.Despawn(this);
    }

    public void CancelButtonClick()
    {
        Reset();
        LeanPool.Despawn(this);
    }

    void SetInpuFieldValue(float value)
    {
        if (isUpdating) return;
        isUpdating = true;

        int current_Durable = currentItem.GetComponent<IDurable>().Durability;

        int currentValue = current_Durable - (int)Math.Round(current_Durable * value);
        set_Durability.text = currentValue.ToString();

        isUpdating = false;
    }

    void SetScrollBarValue(string Fieldvalue)
    {
        if (isUpdating) return;
        isUpdating = true;

        if (float.TryParse(Fieldvalue, out float value))
        {
            scrollbar.value = 1 - (value / currentItem.GetComponent<IDurable>().Durability);
        }

        isUpdating = false;
    }

    private void Reset()
    {
        confirm_Btn.onClick.RemoveAllListeners();
        cancel_Btn.onClick.RemoveAllListeners();
        scrollbar.value = 1;
    }
}
