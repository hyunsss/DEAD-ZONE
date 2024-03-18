using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data Instance;
    public GameObject Player;
    public Vector3 playerPos;

    private void Awake() {
        Instance = this;
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = Player.transform.position;
    }
}
