using UnityEngine;
using System.Collections;

public class OnReadTrigger : MonoBehaviour {
    public GameObject TargetGameObject;
    public Blocker TheBlocker;
    public Talkable TheTalkable;
    public Talkable Requirement;
    public int DialogueIndex;
    public bool GetsUnblocked;
    public bool GetsDisabled;

    void Start () {

    }

    void Update () {
        if (Requirement.WasDialogueIndexRead(DialogueIndex)) {
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
