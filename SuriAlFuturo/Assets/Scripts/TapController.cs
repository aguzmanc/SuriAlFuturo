using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TapController : MonoBehaviour {
    public List<WaterSource> Taps = new List<WaterSource>();

    public void NotifyInteractionTriggered () {
        for (int i=0; i<Taps.Count; i++) {
            Taps[i].TriggerInteraction();
        }
    }
}
