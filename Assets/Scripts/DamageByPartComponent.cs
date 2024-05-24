using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageByPartComponent : MonoBehaviour
{
    public enum DamageType { Head, L_Arm, R_Arm, Chest, L_Leg, R_Leg }

    public Dictionary<DamageType, int> hp_by_Part;
    public Dictionary<Collider, DamageType> part_cols = new Dictionary<Collider, DamageType>();

    public int head_HP;
    public int l_Arm_HP;
    public int r_Arm_HP;
    public int l_Leg_HP;
    public int r_Leg_HP;
    public int chest_HP;

    private Transform hips;


    public float Head_Amount { get => (float)hp_by_Part[DamageType.Head] / head_HP; }
    public float L_Arm_Amount { get => (float)hp_by_Part[DamageType.L_Arm] / l_Arm_HP; }
    public float R_Arm_Amount { get => (float)hp_by_Part[DamageType.R_Arm] / r_Arm_HP; }
    public float Chest_Amount { get => (float)hp_by_Part[DamageType.Chest] / chest_HP; }
    public float L_Leg_Amount { get => (float)hp_by_Part[DamageType.L_Leg] / l_Leg_HP; }
    public float R_Leg_Amount { get => (float)hp_by_Part[DamageType.R_Leg] / r_Leg_HP; }

    private void Awake()
    {
        hp_by_Part = new Dictionary<DamageType, int>() {
            {DamageType.Head, head_HP},
            {DamageType.L_Arm, l_Arm_HP},
            {DamageType.R_Arm, r_Arm_HP},
            {DamageType.Chest, chest_HP},
            {DamageType.L_Leg, l_Leg_HP},
            {DamageType.R_Leg, r_Leg_HP},
        };

        hips = transform.Find("Root/Hips");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (hips != null)
        {
            FindPartColliders();
        }
    }

    public void TakeDamage(DamageType hit_Part, int damage, Action death, Action hit_Animation = null)
    {
        //공격 받은 부위가 머리 또는 몸통인지 먼저 체크
        if (hit_Part == DamageType.Head || hit_Part == DamageType.Chest)
        {
            if (hp_by_Part[hit_Part] - damage < 0)
            {
                death?.Invoke();
                Debug.Log("death Invoke");
            }
        }
        //피격 받은 부위에 데미지가 들어가는 부분
        if (hp_by_Part[hit_Part] > damage)
        {
            hp_by_Part[hit_Part] -= damage;
            hit_Animation?.Invoke();
        }
        //받은 데미지가 현재 부위 체력보다 높을 때 남은 데미지는 다른 부위에 나눠서 적용
        else if (hp_by_Part[hit_Part] < damage)
        {
            int currentHp = hp_by_Part[hit_Part];
            int remainDamage = damage - currentHp;

            hp_by_Part[hit_Part] = 0;
            hit_Animation?.Invoke();

            SplitDamage(remainDamage);
        }
        //데미지 적용 후 사망처리
        if (DeathCondition())
        {
            death?.Invoke();
            Debug.Log("death Invoke");
        }
    }

    private void SplitDamage(int damage)
    {
        int index = 0;

        foreach (int hp in hp_by_Part.Values) if (hp > 0) index++;

        if (index != 0)
        {
            foreach (var key in new List<DamageType>(hp_by_Part.Keys))
            {
                hp_by_Part[key] -= damage / index;

                if (hp_by_Part[key] < 0) hp_by_Part[key] = 0;
            }
        }

    }

    public bool DeathCondition()
    {
        int current_Total_HP = 0;

        foreach (var hp in hp_by_Part.Values)
        {
            current_Total_HP += hp;
        }

        return current_Total_HP <= 0 ? true : false;
    }

    public int GetFullHP()
    {
        return head_HP + l_Arm_HP + r_Arm_HP + l_Leg_HP + r_Leg_HP + chest_HP;
    }

    public int GetCurrentHP()
    {
        int total_HP = 0;
        foreach (var hp in hp_by_Part.Values)
        {
            total_HP += hp;
        }

        return total_HP;
    }


    void FindPartColliders()
    {
        Collider collider;
        collider = hips.transform.Find("Spine_01/Spine_02/Spine_03/Neck/Head").GetComponent<Collider>();
        part_cols.Add(collider, DamageType.Head);

        collider = hips.transform.Find("Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L").GetComponent<Collider>();
        part_cols.Add(collider, DamageType.L_Arm);

        collider = hips.transform.Find("Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L/Elbow_L").GetComponent<Collider>();
        part_cols.Add(collider, DamageType.L_Arm);

        collider = hips.transform.Find("Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L/Elbow_L/Hand_L").GetComponent<Collider>();
        part_cols.Add(collider, DamageType.L_Arm);

        collider = hips.transform.Find("Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R").GetComponent<Collider>();
        part_cols.Add(collider, DamageType.R_Arm);

        collider = hips.transform.Find("Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R").GetComponent<Collider>();
        part_cols.Add(collider, DamageType.R_Arm);

        collider = hips.transform.Find("Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R").GetComponent<Collider>();
        part_cols.Add(collider, DamageType.R_Arm);

        collider = hips.transform.Find("UpperLeg_L").GetComponent<Collider>();
        part_cols.Add(collider, DamageType.L_Leg);

        collider = hips.transform.Find("UpperLeg_L/LowerLeg_L").GetComponent<Collider>();
        part_cols.Add(collider, DamageType.L_Leg);

        collider = hips.transform.Find("UpperLeg_R").GetComponent<Collider>();
        part_cols.Add(collider, DamageType.R_Leg);

        collider = hips.transform.Find("UpperLeg_R/LowerLeg_R").GetComponent<Collider>();
        part_cols.Add(collider, DamageType.R_Leg);

        collider = hips.transform.Find("Spine_01").GetComponent<Collider>();
        part_cols.Add(collider, DamageType.Chest);
    }
}
