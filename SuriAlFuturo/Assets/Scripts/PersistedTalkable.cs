using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PersistedTalkable 
{
    public int DialogueIndex;
    public bool IsForcedToTalk;
    public bool WasRead;
    public List<bool> ReadDialogues;
    
    public PersistedTalkable (int dialogueIndex, bool isForcedToTalk, bool wasRead,
                              List<bool> readDialogues) {
        this.DialogueIndex = dialogueIndex;
        this.IsForcedToTalk = isForcedToTalk;
        this.WasRead = wasRead;
        this.ReadDialogues = readDialogues;
    }
}
