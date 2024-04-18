using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item, IUseable, IDurable
{
    [SerializeField] private int durability;
    [SerializeField] private int maxDurability;

    public int Durability { get => durability; set => durability = value; }

    public int MaxDurability => maxDurability;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Use() { }
}
