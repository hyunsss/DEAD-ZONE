using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStatus : MonoBehaviour
{
    private int hp;
    private int maxHp;
    public int HP { get => hp; set { hp = value; } }
    public int MaxHP { get => maxHp; }

    public void TakeDamage(int damage) {
        hp -= damage;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
