using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnReadTrigger : MonoBehaviour {
    public Blocker Target;
    public List<Talkable> Requirements;
    public int DialogueIndex;
    public bool GetsUnblocked;

    void Start () {
        
    }
    
    void Update () {
        foreach (Talkable requirement in Requirements) {
            if (requirement.WasRead && GetsUnblocked) {
                Target.Unblock();
            }
            Target.TheTalkable.SetDialogueIndex(DialogueIndex);
        }
    }
}
