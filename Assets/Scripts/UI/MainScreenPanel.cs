using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenPanel : MonoBehaviour
{
    #region player_silhouette
    private Image head;
    private Image l_Arm;
    private Image r_Arm;
    private Image l_Leg;
    private Image r_Leg;
    private Image chest;

    void Player_Silhouette_Init()
    {
        head = transform.Find("Bottom Panel/Player Image/Head").GetComponent<Image>();
        l_Arm = transform.Find("Bottom Panel/Player Image/L_Arm").GetComponent<Image>();
        r_Arm = transform.Find("Bottom Panel/Player Image/R_Arm").GetComponent<Image>();
        l_Leg = transform.Find("Bottom Panel/Player Image/L_Leg").GetComponent<Image>();
        r_Leg = transform.Find("Bottom Panel/Player Image/R_Leg").GetComponent<Image>();
        chest = transform.Find("Bottom Panel/Player Image/Chest").GetComponent<Image>();
    }

    void SetPlayer_Silhouette_Color(Image image, float fill)
    {
        Vector3 default_color = new Vector3(221, 255, 25);
        float range = 155;


        image.color = new Color(default_color.x / 255f, (default_color.y - (155 - range * fill)) / 255f, default_color.z / 255f);
    }
    #endregion
    #region player_Weapon_Image
    private Image currentWeapon_Image;
    private TextMeshProUGUI weapon_Mode;
    private TextMeshProUGUI weapon_remain_Ammo;

    void WeaponImage_Init()
    {
        currentWeapon_Image = transform.Find("Bottom Panel/Current Weapon/Weapon/Current Weapon Image").GetComponent<Image>();
        weapon_Mode = transform.Find("Bottom Panel/Current Weapon/Texts/Bottom Text").GetComponent<TextMeshProUGUI>();
        weapon_remain_Ammo = transform.Find("Bottom Panel/Current Weapon/Texts/Remain Ammo Text").GetComponent<TextMeshProUGUI>();
    }

    void SetCurrentWeaponProperties()
    {
        if (PlayerManager.attack.CurrentWeapon != null)
        {
            currentWeapon_Image.enabled = true;
            currentWeapon_Image.sprite = PlayerManager.attack.CurrentWeapon.inventorySprite;
            currentWeapon_Image.preserveAspect = true;

            if (PlayerManager.attack.CurrentWeapon is Gun gun)
            {
                weapon_Mode.text = $"발사 모드 : {gun.GetFireMode()}";
                weapon_remain_Ammo.text = $"{gun.CurrentMagazine.Durability} / {gun.CurrentMagazine.MaxDurability}";
            }

        }
        else
        {
            currentWeapon_Image.enabled = false;
            weapon_Mode.text = null;
            weapon_remain_Ammo.text = null;
        }
    }

    #endregion
    #region Time & ExitList Text
    private TextMeshProUGUI timeText;
    private RectTransform ExitList_Trans;

    public GameObject exit_prefab;

    void TopPropertiesInit()
    {
        timeText = transform.Find("Top Panel/Remain Time Panel & Exit List/Image/Time").GetComponent<TextMeshProUGUI>();
        ExitList_Trans = transform.Find("Top Panel/Remain Time Panel & Exit List/Exit List").GetComponent<RectTransform>();
    }

    void SetTimeText()
    {
        timeText.text = string.Format("{0:D2}:{1:D2}", TimeManager.Instance.Min, TimeManager.Instance.Sec);
    }

    #endregion
    void Awake()
    {
        Player_Silhouette_Init();
        WeaponImage_Init();
        TopPropertiesInit();
    }

    void LateUpdate()
    {
        SetPlayer_Silhouette_Color(head, PlayerManager.status.damageByPart.Head_Amount);
        SetPlayer_Silhouette_Color(l_Arm, PlayerManager.status.damageByPart.L_Arm_Amount);
        SetPlayer_Silhouette_Color(r_Arm, PlayerManager.status.damageByPart.R_Arm_Amount);
        SetPlayer_Silhouette_Color(l_Leg, PlayerManager.status.damageByPart.L_Leg_Amount);
        SetPlayer_Silhouette_Color(r_Leg, PlayerManager.status.damageByPart.R_Leg_Amount);
        SetPlayer_Silhouette_Color(chest, PlayerManager.status.damageByPart.Chest_Amount);

        SetCurrentWeaponProperties();
        SetTimeText();
    }

}
