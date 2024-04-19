using System;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("PlayerStatus Text Value")]
    private float weight;
    [SerializeField] private float maxWeight = 80f;
    private float hp;
    private float full_hp;
    private float moisture;
    private float hungry;

    public float Weight
    {
        get => weight; set
        {
            weight = value;
            text_Weight.text = $"{Math.Round(weight, 1)}KG";

            float weight_ratio = weight / maxWeight;

            PlayerManager.move.WalkSpeed = 5 - (5 * weight_ratio);
            PlayerManager.move.RunSpeed = 7 - (7 * weight_ratio);
        }
    }

    public float HP
    {
        get => hp; set
        {
            hp = value;
            text_HP.text = $"{Math.Round(hp, 1)}/{Math.Round(full_hp)}";
            if(hp <= 0) {
                //Todo :: GameOver
            } else if(hp >= full_hp){
                hp = full_hp;
            }

            if(hp < 60) {
                text_HP.color = Color.red;
            }
        }
    }

    public float Full_Hp { get => full_hp; set => full_hp = value; }

    public float Moisture
    {
        get => moisture; set
        {
            moisture = value;
            text_Moisture.text = $"{Math.Round(moisture, 1)}/100";
            if(moisture <= 0) {
                moisture = 0;
                //Todo 수분 제로에 따른 디버프 발생
            } else if(moisture >= 100) {
                moisture = 100;
            }

            if(moisture < 5) {
                text_Moisture.color = Color.red;
            }
        }
    }

    public float Hungry
    {
        get => hungry; set
        {
            hungry = value;
            text_Hungry.text = $"{Math.Round(hungry, 1)}/100";
            if(hungry <= 0) {
                hungry = 0;
                //Todo 배고픔 제로에 따른 디버프 발생
            } else if(hungry >= 100) {
                hungry = 100;
            }

            if(hungry < 5) {
                text_Moisture.color = Color.red;
            }
        }
    }

    [Header("PlayerStatus Text Value")]
    public TextMeshProUGUI text_Weight;
    public TextMeshProUGUI text_HP;
    public TextMeshProUGUI text_Moisture;
    public TextMeshProUGUI text_Hungry;

    [Header("isSecondOver")]
    private bool isOneSecondOver;
    private float startTime;

    private bool isZeroHungry;
    private bool isZeroMoisture;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        text_Weight = UIManager.Instance.StatusPanel.Find("BackGround/WeightIcon").GetComponentInChildren<TextMeshProUGUI>();
        text_HP = UIManager.Instance.StatusPanel.Find("BackGround/HealthIcon").GetComponentInChildren<TextMeshProUGUI>();
        text_Moisture = UIManager.Instance.StatusPanel.Find("BackGround/MoistureIcon").GetComponentInChildren<TextMeshProUGUI>();
        text_Hungry = UIManager.Instance.StatusPanel.Find("BackGround/HungryIcon").GetComponentInChildren<TextMeshProUGUI>();

        Hungry = 40f;
        Moisture = 40f;
        Full_Hp = 440;
        HP = 200;

        startTime = Time.time;
        WeightCalculation();
    }

    void Update() {
        StatChangeOverTime(true);

        // Weight = 
    }

    public void WeightCalculation() {
        Weight = UIManager.Instance.GetInventoryWeight();
    }

    public void StatChangeOverTime(bool inGame)
    {
        float currentOverTime = Time.time - startTime;
        isOneSecondOver = currentOverTime > 1f ? true : false;
        if (isOneSecondOver == true)
        {
            if (inGame == true)
            {
                //수치 감소
                Moisture -= 0.04f;
                Hungry -= 0.04f;
            }
            else
            {
                //수치 증가
                Moisture += 0.05f;
                Hungry += 0.05f;
            }
            isOneSecondOver = false;
            startTime = Time.time;
        }
    }


}
