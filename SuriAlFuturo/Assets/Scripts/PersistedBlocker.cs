using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersistedBlocker {
    public bool IsUnblocked;
    public List<Requirement> UnmetRequirements = new List<Requirement>();

    public PersistedBlocker (Blocker blocker) {
        IsUnblocked = blocker.IsUnblocked;

        foreach (Requirement r in blocker.UnmetRequirements) {
            UnmetRequirements.Add(r);
            Debug.Log("?? " + r.Image);
        }

    }
}
