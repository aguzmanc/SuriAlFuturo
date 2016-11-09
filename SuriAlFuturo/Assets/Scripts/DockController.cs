using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DockController : MonoBehaviour {
    public List<Dock> Docks;
    // TODO: this would be better...
    // public Ship TheShip; // there is only one ship
    // public Suri Sailor;
    public bool CanUseDock;

    public void NotifyInterfaceInteraction () {
        foreach (Dock dock in Docks) {
            dock.TriggerInterfaceInteraction();
        }
    }
}
