using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseable {
    public float Delay { get; }
    public void Use(float value);
    public void ShowSetUsageValuePanel();

}

public enum InteractType { Item, RootBox, Door }

public interface IInteractable {
    public InteractType Type { get; }
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

public interface INoiseWeight {
    public float NoiseWeight { get; set;}
    public Vector3 NoisePosition { get; set;}
    public Transform MyTransform{get;}
}

