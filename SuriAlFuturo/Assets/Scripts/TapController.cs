using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TapController : MonoBehaviour {
    public List<WaterTap> Taps = new List<WaterTap>();
    public Dictionary<Vector3, bool> SavedTaps = new Dictionary<Vector3, bool>();

    public void NotifyInteractionTriggered () {
        for (int i=0; i<Taps.Count; i++) {
            Taps[i].TriggerInteraction();
        }
    }

    public void Save (WaterTap tap) {
        SavedTaps[tap.PersistenceKey] = tap.GetPersistedObject();
    }

    public void Load (WaterTap tap) {
        try {
            tap.Load(SavedTaps[tap.PersistenceKey]);
        } catch {}
    }

    public int CountWaterTapsOff () {
        int count = 0;

        foreach (WaterTap tap in Taps) {
            if (false == tap.IsOn) {
                count++;
            }
        }

        return count;
    }
}
