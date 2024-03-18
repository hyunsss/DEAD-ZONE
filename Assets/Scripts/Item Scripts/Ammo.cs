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

    private void OnEnable() {
        rigid.useGravity = true;
        rigid.isKinematic = true;
    }

    private void OnDisable() {
        rigid.position = Vector3.zero;
    }

    public void AmmoShot() {
        rigid.isKinematic = false;
    }


    //Todo 샷 상태에서 맞았을 경우 데미지를 주고 despawn함
}
