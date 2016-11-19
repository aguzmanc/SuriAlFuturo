using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnReadTrigger : MonoBehaviour {
    public Blocker Target;
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
            if (GetsUnblocked) {
                Target.Unblock();
            }
            if (GetsDisabled) {
                Target.gameObject.SetActive(false);
            }
            Target.TheTalkable.SetDialogueIndex(DialogueIndex);
        }
    }
}
