using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
public enum AmmoState { None, Shot }

public class Ammo : Item
{
    public AmmoState state;
    public int damage;
    public int bulletSpeed;
    public Rigidbody rigid;

    private void Awake() {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        rigid.useGravity = false;
    }

    private void OnDisable() {
        rigid.position = Vector3.zero;
    }


    //Todo 샷 상태에서 맞았을 경우 데미지를 주고 despawn함
}
