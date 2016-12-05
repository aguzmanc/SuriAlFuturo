using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SuriAlFuturo;

public class RepairController : MonoBehaviour {
    public List<Repair> Repairs;
    public List<Repair> Cant;

    public GameObject RepairButton;
    private Talkable _talkable;

    void Start () {
        // RepairButton = GameObject.FindGameObjectWithTag(Tag.RepairButton);
    }

    public void NotifyRepair (Repair r) {
        _talkable.ForceDialogue(r.GetMessage());
    }

    public void TriggerRepair () {
        foreach (Repair r in Repairs) {
            if (r.TryToRepair()) {
                return;
            }
        }

        foreach (Repair r in Cant) {
            if (r.TryToRepair()) {
                return;
            }
        }
    }

    public void RegisterTalkable (Talkable t) {
        _talkable = t;
    }

    public void RegisterRepair (Repair r) {
        if (r.CantRepair) {
            Cant.Add(r);
        } else {
            Repairs.Add(r);
        }
    }

    public void Unregister (Repair r) {
        if (false == Repairs.Remove(r)) {
            Cant.Remove(r);
        }
    }
}
