using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueController : MonoBehaviour {
    public UIDialogue UiController;
    public Talkable ActiveTalkable;
    public bool DoneTalking;
    public Dictionary<string, TalkingCharacter> TalkingCharacterDictionary
        = new Dictionary<string, TalkingCharacter>();
    public Dialogue DontNeedThat;
    public CharacterMovement Player;
    public Dictionary<Vector3, bool> SavedActiveState
        = new Dictionary<Vector3, bool>();

    void Update () {
        Player.IsControlledByPlayer = !(ActiveTalkable != null &&
                                        ActiveTalkable.IsTalking());

    }

    public TalkingCharacter GetTalkingCharacter (string name) {
        return TalkingCharacterDictionary[name];
    }

    public void RegisterTalkingCharacter (string name, TalkingCharacter character) {
        TalkingCharacterDictionary[name] = character;
    }
}
