using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectionSystem : MonoBehaviour {
    public List<Blocker> Blockers;
    public List<Collectable> IngameCollectables;

    public Dictionary<Sprite, bool> TakenStuff = new Dictionary<Sprite, bool>();
    public Dictionary<Sprite, bool> GivenStuff = new Dictionary<Sprite, bool>();

    public Dictionary<Vector3, bool> UnblockedDudes = new Dictionary<Vector3, bool>();
    public Dictionary<Vector3, List<Requirement>> RegisteredRequirements =
        new Dictionary<Vector3, List<Requirement>>();

    public UIInventory UIController;

    public void RegisterCollectable (Collectable collectable) {
        if (false == TakenStuff.ContainsKey(collectable.Image)) { 
            IngameCollectables.Add(collectable);
            TakenStuff[collectable.Image] = false;
            GivenStuff[collectable.Image] = false;
        }
    }

    public void RegisterBlocker (Blocker blocker) {
        if (false == UnblockedDudes.ContainsKey(blocker.transform.position)) {
            UnblockedDudes[blocker.transform.position] = false;
        }
    }

    void Start () {
        if (!UIController) {
            throw(new UnityException("I need a canvas with a UI Inventory component attached, when created, drag it into the UI Controller field!"));
        }
    }

    // readibility counts!
    public bool IsCollected (Collectable collectable) {
        return IsCollected(collectable.Image);
    }

    public bool IsCollected (Sprite image) {
        return TakenStuff[image];
    }

    public void RegisterAsCollected(Collectable collectable) {
        TakenStuff[collectable.Image] = true;
        UIController.AddItem(collectable.Image);
    }



    public bool IsGiven (Sprite image) {
        return GivenStuff[image];
    }

    public void RegisterAsGiven (Sprite image) {
        GivenStuff[image] = true;
        UIController.RemoveItem(image);
    }



    public bool IsUnblocked (Blocker blocker) {
        return UnblockedDudes[blocker.transform.position];
    }

    public void RegisterAsUnblocked (Blocker blocker) {
        UnblockedDudes[blocker.transform.position] = true;
    }


    public void RegisterRequirements (Blocker blocker) {
        List<Requirement> requirements = blocker.Requirements;
        RegisteredRequirements[blocker.transform.position] = new List<Requirement>();
        for (int i=0; i<requirements.Count; i++) {
            RegisteredRequirements[blocker.transform.position].Add(requirements[i]);
        }
    }

    public List<Requirement> GetRequirements (Blocker blocker) {
        return RegisteredRequirements[blocker.transform.position];
    }

    public bool HasRegisteredRequirements (Blocker blocker) {
        return RegisteredRequirements.ContainsKey(blocker.transform.position);
    }

    public Sprite GetActiveRequirement () {
        return UIController.GetActiveRequirement();
    }

    public int CountOwnedItems () {
        return UIController.OwnedItems.Count;
    }

    public void NotifyInteractionTriggered () {
        foreach (Blocker b in Blockers) {
            b.TriggerInteraction();
        }
    }
}
