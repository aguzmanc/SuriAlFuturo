using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnReadTrigger : MonoBehaviour {
    public GameObject TargetGameObject;
    public Blocker TheBlocker;
    public Talkable TheTalkable;
    public List<Talkable> Requirements;
    public int DialogueIndex;
    public bool GetsUnblocked;
    public bool GetsDisabled;

    void Start () {

    }

    void Update () {
        bool met = true;
        foreach (Talkable requirement in Requirements) {
            if (!requirement.WasRead) {
                met = false;
            }
        }

        if (met) {
            if (GetsUnblocked && TheBlocker != null) {
                TheBlocker.Unblock();
            }
            if (GetsDisabled) {
                TargetGameObject.SetActive(false);
            }
            if (TheTalkable != null) {
                TheTalkable.SetDialogueIndex(DialogueIndex);
            }
            this.enabled = false;
        }
    }
}
