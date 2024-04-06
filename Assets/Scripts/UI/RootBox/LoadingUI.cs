using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    Animation animation;

    void Awake()
    {
        animation = GetComponent<Animation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animation.Play();
    }

    public void Destroy()
    {
        animation.Stop();
        LeanPool.Despawn(this);
    }
}
