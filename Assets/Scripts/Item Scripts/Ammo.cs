using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AmmoState { None, Shot }

public class Ammo : Item
{
    public AmmoState state;
    public int damage;
    public int bulletSpeed;
    Rigidbody rigid;

    private void Awake() {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
    }

    private void Update() {
        if(state == AmmoState.Shot) {
            rigid.velocity = Vector3.forward * bulletSpeed * Time.deltaTime;
        }
    }

    //Todo 샷 상태에서 맞았을 경우 데미지를 주고 despawn함
}
