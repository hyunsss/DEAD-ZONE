using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    Animator loading_animation;

    void Awake()
    {
        loading_animation = GetComponent<Animator>();
    }

    public void Destroy()
    {
        LeanPool.Despawn(this);
    }
}
