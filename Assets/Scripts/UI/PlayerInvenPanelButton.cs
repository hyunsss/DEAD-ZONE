using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInvenPanelButton : MonoBehaviour
{
    public enum PanelKey { Equip, Health_Condition }

    private PanelKey currentPanel;
    private Button equipPanel_Button;
    private Button healthCondition_Button;

    private Dictionary<PanelKey, RectTransform> panelList;
    private Dictionary<PanelKey, Image> frameList;

    void OpenPanel(PanelKey key)
    {
        foreach (var rect in panelList.Values)
        {
            rect.gameObject.SetActive(false);
        }

        foreach (var frame in frameList.Values)
        {
            frame.color = Color.white;
        }

        panelList[key].gameObject.SetActive(true);
        panelList[key].transform.SetAsFirstSibling();
        frameList[key].color = Color.green;

    }

    // Start is called before the first frame update
    void Start()
    {
        equipPanel_Button = transform.Find("Equip Panel Button").GetComponent<Button>();
        healthCondition_Button = transform.Find("Health Condition Button").GetComponent<Button>();

        panelList = new Dictionary<PanelKey, RectTransform>() {
            { PanelKey.Equip, UIManager.Instance.equip_transform  },
            { PanelKey.Health_Condition, UIManager.Instance.healthCondition},
        };

        frameList = new Dictionary<PanelKey, Image>() {
            { PanelKey.Equip, equipPanel_Button.transform.Find("Frame").GetComponent<Image>()},
            { PanelKey.Health_Condition, healthCondition_Button.transform.Find("Frame").GetComponent<Image>()},
        };

        equipPanel_Button.onClick.AddListener(() => OpenPanel(PanelKey.Equip));
        healthCondition_Button.onClick.AddListener(() => OpenPanel(PanelKey.Health_Condition));

        OpenPanel(PanelKey.Equip);
    }

}
