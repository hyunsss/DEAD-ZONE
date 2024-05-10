using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthConditionPanel : MonoBehaviour
{
    private Image head;
    private Image l_Arm;
    private Image r_Arm;
    private Image l_Leg;
    private Image r_Leg;
    private Image chest;

    private TextMeshProUGUI head_text;
    private TextMeshProUGUI l_Arm_text;
    private TextMeshProUGUI r_Arm_text;
    private TextMeshProUGUI l_Leg_text;
    private TextMeshProUGUI r_Leg_text;
    private TextMeshProUGUI chest_text;

    private DamageByPartComponent playerHP_Part;

    void Awake()
    {
        head = transform.Find("RawImage/Head Bar/BackGround/Image").GetComponent<Image>();
        l_Arm = transform.Find("RawImage/L_Arm Bar/BackGround/Image").GetComponent<Image>();
        r_Arm = transform.Find("RawImage/R_Arm Bar/BackGround/Image").GetComponent<Image>();
        l_Leg = transform.Find("RawImage/L_Leg Bar/BackGround/Image").GetComponent<Image>();
        r_Leg = transform.Find("RawImage/R_Leg Bar/BackGround/Image").GetComponent<Image>();
        chest = transform.Find("RawImage/Chest Bar/BackGround/Image").GetComponent<Image>();

        head_text = transform.Find("RawImage/Head Bar/HP Text").GetComponent<TextMeshProUGUI>();
        l_Arm_text = transform.Find("RawImage/L_Arm Bar/HP Text").GetComponent<TextMeshProUGUI>();
        r_Arm_text = transform.Find("RawImage/R_Arm Bar/HP Text").GetComponent<TextMeshProUGUI>();
        l_Leg_text = transform.Find("RawImage/L_Leg Bar/HP Text").GetComponent<TextMeshProUGUI>();
        r_Leg_text = transform.Find("RawImage/R_Leg Bar/HP Text").GetComponent<TextMeshProUGUI>();
        chest_text = transform.Find("RawImage/Chest Bar/HP Text").GetComponent<TextMeshProUGUI>();
    }

    void SetValue(Image fillimage, TextMeshProUGUI hp_text, float fillamount, string text)
    {
        fillimage.fillAmount = fillamount;
        hp_text.text = text;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHP_Part = PlayerManager.status.damageByPart;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        SetValue(head, head_text, playerHP_Part.Head_Amount, $"{playerHP_Part.hp_by_Part[DamageByPartComponent.DamageType.Head]} / {playerHP_Part.head_HP}");
        SetValue(l_Arm, l_Arm_text, playerHP_Part.L_Arm_Amount, $"{playerHP_Part.hp_by_Part[DamageByPartComponent.DamageType.L_Arm]} / {playerHP_Part.l_Arm_HP}");
        SetValue(r_Arm, r_Arm_text, playerHP_Part.R_Arm_Amount, $"{playerHP_Part.hp_by_Part[DamageByPartComponent.DamageType.R_Arm]} / {playerHP_Part.r_Arm_HP}");
        SetValue(l_Leg, l_Leg_text, playerHP_Part.L_Leg_Amount, $"{playerHP_Part.hp_by_Part[DamageByPartComponent.DamageType.L_Leg]} / {playerHP_Part.l_Leg_HP}");
        SetValue(r_Leg, r_Leg_text, playerHP_Part.R_Leg_Amount, $"{playerHP_Part.hp_by_Part[DamageByPartComponent.DamageType.R_Leg]} / {playerHP_Part.r_Leg_HP}");
        SetValue(chest, chest_text, playerHP_Part.Chest_Amount, $"{playerHP_Part.hp_by_Part[DamageByPartComponent.DamageType.Chest]} / {playerHP_Part.chest_HP}");
    }
}
