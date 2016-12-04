using UnityEngine;
using System.Collections;

[System.Serializable]
public class PersistedCollectable {
    public bool IsTaken;

    public PersistedCollectable (bool isTaken) {
        IsTaken = isTaken;
    }
}
