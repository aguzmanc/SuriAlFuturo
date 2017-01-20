using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SuriAlFuturo;

public class RepairController : MonoBehaviour {
    public List<Repair> Repairs;
    public List<Repair> Cant;

    public GameObject RepairButton;
    public TextAsset DefaultRejection;

    private DialogueController _dialogueController;
    private Talkable _talkable;
    private Dialogue[] _digestedRejection;

    void Start () {
        _dialogueController = GetComponent<DialogueController>();
    }

    void Update () {
        if (Input.GetButtonDown("LoadUnloadToShip")) {
            Debug.Log("ZOMGZ!! that's the button! " + Time.time);
        }

        if (RepairButton.activeSelf) {
            RepairButton.SetActive(!_dialogueController.IsTalkingToSomeone());
            if (Input.GetButtonDown("Give")) {
                TriggerRepair();
            }
        }
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

        _talkable.ForceDialogue(GetRejectionMessage());
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

    public Dialogue[] GetRejectionMessage () {
        if (_digestedRejection == null) {
            _digestedRejection = JsonUtility.FromJson<JsonDialogueData>(DefaultRejection.text).Dialogues;
        }

        return _digestedRejection;
    }
}
