using UnityEngine;
using System.Collections;

[System.Serializable]
public class PersistedOnTouchTrigger 
{
    public bool Triggered;

    public PersistedOnTouchTrigger (bool triggered) {
        Triggered = triggered;
    }
}
