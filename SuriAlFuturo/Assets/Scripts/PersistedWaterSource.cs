using UnityEngine;
using System.Collections;

[System.Serializable]
public class PersistedWaterSource 
{
    public bool IsOn;
    public bool IsUsable;

    public PersistedWaterSource (bool isOn, bool isUsable) 
    {
        IsOn = isOn;
        IsUsable = isUsable;
    }
}
