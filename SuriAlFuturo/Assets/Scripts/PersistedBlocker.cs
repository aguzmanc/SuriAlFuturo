using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PersistedBlocker 
{
    public bool IsUnblocked;
    public List<Requirement> UnmetRequirements = new List<Requirement>();

    public PersistedBlocker (bool isUnblocked,
                             List<Requirement> unmetRequirements) {
        IsUnblocked = isUnblocked;
        foreach (Requirement requirement in unmetRequirements) {
            UnmetRequirements.Add(requirement);
        }
    }
}
