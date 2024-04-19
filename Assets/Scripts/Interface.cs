using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseable {
    public float Delay { get; }
    public void Use(float value);
    public void ShowSetUsageValuePanel();

}

public interface IInteractable {
    public void Interact();
}

public interface IDurable {
    public int Durability { get; set;}
    public int MaxDurability { get; }
}

public interface IStackable {
    public int Count { get; set;}
    public int MaxCount { get; }
}

