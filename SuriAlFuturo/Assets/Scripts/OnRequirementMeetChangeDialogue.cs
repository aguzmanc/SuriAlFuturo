using UnityEngine;
using System.Collections;

public class OnRequirementMeetChangeDialogue : MonoBehaviour {
    public Talkable TheTalkable;
    public Blocker TheBlocker;

    public int DialogueState;

    void Start () {
        
    }
    
    void Update () {
	if (TheBlocker.IsUnblocked) {
            TheTalkable.SetState(DialogueState);
        }
    }
}
