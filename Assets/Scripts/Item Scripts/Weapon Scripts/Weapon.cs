using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    [Space(10f)]
    [Header("IK Position")]
    public Vector3 handPosition;
    public Vector3 handRotation;

    public Vector3 subPosition;
    public Vector3 subRotation;

    public Transform subHandIK_target;
    public Transform subHandIK_hint;


    public abstract void Shot();
}
