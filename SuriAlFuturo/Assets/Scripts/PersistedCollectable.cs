using UnityEngine;
using System.Collections;

[System.Serializable]
public class PersistedCollectable {
    public bool IsTaken;
    public Collectable.Tag Name;

    public PersistedCollectable (bool isTaken, Collectable.Tag name) {
        IsTaken = isTaken;
        Name = name;
    }
}
