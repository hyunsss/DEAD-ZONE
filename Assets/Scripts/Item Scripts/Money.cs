using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Item, IStackable
{
    public int Count { get => count; set => count = value; }

    public int MaxCount => maxCount;

    [SerializeField] private int maxCount;
    [SerializeField] private int count;

}
