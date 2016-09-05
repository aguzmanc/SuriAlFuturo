using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectionSystem : MonoBehaviour {
    public List<Collectable> IngameCollectables;
    public Collector IngameCollector;

    public UIInventory UIController;

    public void RegisterCollectable (Collectable collectable) {
        if (IngameCollectables == null) {
            IngameCollectables = new List<Collectable>();
        }
        IngameCollectables.Add(collectable);
        IngameCollector.RegisterCollectable(collectable);
    }

    void Start () {
        if (!IngameCollector) {
            throw (new UnityException("who is the collector? (PROTIP: Collector prefab to Player as a child, and drag it into \"Ingame Collector\" field)"));
        }

        if (!UIController) {
            throw(new UnityException("I need a canvas with a UI Inventory component attached, when created, drag it into the UI Controller field!"));
        }
    }

    void Update () {
        
    }

}
