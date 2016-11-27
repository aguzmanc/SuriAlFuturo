using UnityEngine;
using System.Collections;

public class CustomDontNeedThat : MonoBehaviour {
    public Collectable.Tag Item;
    public TextAsset Dialogue;
    public Blocker TheBlocker;

    private Dialogue[] _digestedDialogues;

    void Start () {
        if (TheBlocker == null) {
            TheBlocker = GetComponent<Blocker>();
        }

        _DigestDialogue();

        TheBlocker.Register(this);
    }
    
    void Update () {
	
    }

    private void _DigestDialogue () {
        _digestedDialogues =
            JsonUtility.FromJson<JsonDialogueData>(Dialogue.text).Dialogues;
    }

    public Dialogue[] GetDialogues () {
        if (_digestedDialogues == null) {
            _DigestDialogue();
        }

        return _digestedDialogues;
    }
}
