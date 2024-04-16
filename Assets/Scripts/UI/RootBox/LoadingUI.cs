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

    void OnEnable()
    {
        UIManager.Instance.loadingUI_list.Add(this);
    }

    public void Destroy()
    {
        LeanPool.Despawn(this);
    }
}
