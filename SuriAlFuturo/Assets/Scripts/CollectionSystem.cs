using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectionSystem : MonoBehaviour {
    public List<Blocker> Blockers;
    public List<Collectable> IngameCollectables;
    public Dictionary<Vector3, PersistedCollectable> SavedCollectables =
        new Dictionary<Vector3, PersistedCollectable>();
    public Dictionary<Vector3, PersistedBlocker> SavedBlockers =
        new Dictionary<Vector3, PersistedBlocker>();

    public List<Collectable.Tag> Inventory = new List<Collectable.Tag>();

    public UIInventory UIController; // TODO: make this private

    private Dictionary<Collectable.Tag, bool> _takenStuff =
        new Dictionary<Collectable.Tag, bool>();
    private Dictionary<Collectable.Tag, bool> _givenStuff =
        new Dictionary<Collectable.Tag, bool>();


    void Start () {
        if (!UIController) {
            throw(new UnityException("I need a canvas with a UI Inventory component attached, when created, drag it into the UI Controller field!"));
        }

        UIController.Refresh();
    }

    public Collectable.Tag GetActiveRequirement () {
        return UIController.GetActiveRequirement();
    }

    public int CountOwnedItems () {
        return Inventory.Count;
    }

    public void NotifyInteractionTriggered () {
        foreach (Blocker b in Blockers) {
            b.TriggerInteraction();
        }
    }

    public void RegisterAsTaken (Collectable collectable) {
        RegisterAsTaken(collectable.Name);
    }

    public void RegisterAsTaken (Collectable.Tag collectable) {

        Debug.Log("taken: " + collectable);

        _takenStuff[collectable] = true;
        if(false == Inventory.Contains(collectable)) {
            Inventory.Add(collectable);
        }
        UIController.Refresh();
    }

    public void RegisterAsGiven (Collectable collectable) {
        RegisterAsGiven(collectable.Name);
    }



    public void RegisterAsGiven (Collectable.Tag collectable) 
    {
        _givenStuff[collectable] = true;
        Inventory.Remove(collectable);
        UIController.Refresh();
    }


    #region persistence
    #region collectable
    public void Save (Collectable collectable) {
        SavedCollectables[collectable.PersistenceKey] =
            collectable.GetPersistedObject();
    }

    public void Load (Collectable collectable) {
        if (SavedCollectables.ContainsKey(collectable.PersistenceKey)) {
            collectable.LoadPersistedObject(SavedCollectables[collectable.PersistenceKey]);
        }
    }
    #endregion

    #region blocker
    public void Save (Blocker blocker) {
        SavedBlockers[blocker.PersistenceKey] = blocker.GetPersistedObject();
    }

    public void Load (Blocker blocker) {
        if (SavedBlockers.ContainsKey(blocker.PersistenceKey)) {
            blocker.LoadPersistedObject(SavedBlockers[blocker.PersistenceKey]);
        }
    }
    #endregion
    #endregion

    public bool HasItem (Collectable.Tag item) {
        foreach (Collectable.Tag ownedItem in Inventory) {
            if (item == ownedItem) {
                return true;
            }
        }

        return false;
    }
}
