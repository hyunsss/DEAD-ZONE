using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseable {
    public void Use();
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

