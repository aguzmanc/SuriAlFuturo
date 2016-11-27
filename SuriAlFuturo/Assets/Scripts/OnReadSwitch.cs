using UnityEngine;
using System.Collections;

public class OnReadSwitch : MonoBehaviour {
    public Talkable TheTalkable;

    public int OnDialogueIndexRead;
    public int SwitchTo;
    
    void Update () {
	if (TheTalkable.GetDialogueIndex() == OnDialogueIndexRead &&
            TheTalkable.WasRead) {
            TheTalkable.SetDialogueIndex(SwitchTo);
            this.enabled = false;
        }
    }
}
