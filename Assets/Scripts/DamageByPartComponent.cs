using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageByPartComponent : MonoBehaviour
{
    public enum DamageType { Head, L_Arm, R_Arm, Chest, L_Leg, R_Leg }

    public int head_HP;
    public int l_Arm_HP;
    public int r_Arm_HP;
    public int l_Leg_HP;
    public int r_Leg_HP;
    public int chest_HP;

    private Transform hips;

    public Dictionary<DamageType, int> hp_by_Part;
    public Dictionary<Collider, DamageType> part_cols = new Dictionary<Collider, DamageType>();

    private void Awake()
    {
        hp_by_Part = new Dictionary<DamageType, int>() {
            {DamageType.Head, head_HP},
            {DamageType.L_Arm, l_Arm_HP},
            {DamageType.R_Arm, r_Arm_HP},
            {DamageType.Chest, chest_HP},
            {DamageType.L_Leg, l_Leg_HP},             //총 체력 475
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
        Debug.Log($"Name :: {gameObject.name}, TakeDamage :: {damage}");
        Debug.Log($"{hp_by_Part[DamageType.Head]} :: {hp_by_Part[DamageType.L_Arm]} :: {hp_by_Part[DamageType.R_Arm]} :: {hp_by_Part[DamageType.Chest]} :: {hp_by_Part[DamageType.L_Leg]} :: {hp_by_Part[DamageType.R_Leg]}");
        if (hit_Part == DamageType.Head || hit_Part == DamageType.Chest)
        {
            if (hp_by_Part[hit_Part] == 0)
            {
                //공격당한 부위가 머리나 몸통일 때 또한 그곳의 체력이 0이었을 때 한번더 공격을 받는다면 바로 사망처리

                //죽음
                death?.Invoke();
                Debug.Log("death Invoke");
            }
        }

        if (hp_by_Part[hit_Part] > damage)
        {
            hp_by_Part[hit_Part] -= damage;
            hit_Animation?.Invoke();
        }
        else if (hp_by_Part[hit_Part] < damage)
        {
            int currentHp = hp_by_Part[hit_Part];
            int remainDamage = damage - currentHp;

            hp_by_Part[hit_Part] = 0;
            hit_Animation?.Invoke();

            SplitDamage(remainDamage);
        }

        if (DeathCondition())
        {
            //죽음
            death?.Invoke();
            Debug.Log("death Invoke");
        }
    }

    private void SplitDamage(int damage)
    {
        int index = 0;

        foreach (int hp in hp_by_Part.Values) if (hp > 0) index++;

        if(index != 0) {
            foreach (var key in new List<DamageType>(hp_by_Part.Keys))
            {
                hp_by_Part[key] -= damage / index;

                if(hp_by_Part[key] < 0) hp_by_Part[key] = 0;
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

    public int GetFullHP() {
        return head_HP + l_Arm_HP + r_Arm_HP + l_Leg_HP + r_Leg_HP + chest_HP;
    }

    public int GetCurrentHP() {
        int total_HP = 0;
        foreach(var hp in hp_by_Part.Values) {
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
