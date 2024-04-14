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
    int Durability { get; set;}
}

public interface IStackable {
    int Count { get; set;}
}
