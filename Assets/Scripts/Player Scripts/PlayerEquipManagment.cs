using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipManagment : MonoBehaviour
{
    public Transform hand_R;
    public Transform spine_03;

    public Weapon[] weapons = new Weapon[2];

    public void InsertWeapon(Weapon weapon, int index) {
        weapons[index] = weapon;
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
