using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set;}
    public static PlayerAttack attack { get; private set; }
    public static PlayerMove move { get; private set; }
    public static PlayerInput input { get; private set; }
    public static PlayerLook look { get; private set; }
    public static PlayerStatus status { get; private set; }
    public static PlayerEquipManagment equip { get; private set; }

    private void Awake() {
        Instance = this;

        attack = GetComponent<PlayerAttack>();
        move = GetComponent<PlayerMove>();
        input = GetComponent<PlayerInput>();
        look = GetComponent<PlayerLook>();
        status = GetComponent<PlayerStatus>();;
        equip = GetComponent<PlayerEquipManagment>();
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
