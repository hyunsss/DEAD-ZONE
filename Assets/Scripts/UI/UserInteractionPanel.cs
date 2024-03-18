using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserInteractionPanel : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI interaction_name;

    private void Awake() {
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        interaction_name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
    }
    
    public void SetText(string interaction_name, string text) {
        this.interaction_name.text = interaction_name;
        this.text.text = text;
    }
}
