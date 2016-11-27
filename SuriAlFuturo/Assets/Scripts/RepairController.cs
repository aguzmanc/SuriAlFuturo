using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SuriAlFuturo;

public class RepairController : MonoBehaviour {
    public List<Repair> Repairs;

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
    }

    public void RegisterTalkable (Talkable t) {
        _talkable = t;
    }
}
