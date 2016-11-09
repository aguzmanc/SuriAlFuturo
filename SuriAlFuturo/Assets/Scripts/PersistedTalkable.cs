using UnityEngine;
using System.Collections;

public class PersistedTalkable {
    public int DialogueIndex;
    public bool IsForcedToTalk;
    public bool WasRead;
    
    public PersistedTalkable (int dialogueIndex, bool isForcedToTalk, bool wasRead) {
        this.DialogueIndex = dialogueIndex;
        this.IsForcedToTalk = isForcedToTalk;
        this.WasRead = wasRead;
    }
}
