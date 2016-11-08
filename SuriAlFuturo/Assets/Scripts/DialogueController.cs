using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueController : MonoBehaviour {
    public UIDialogue UiController;
    public Talkable ActiveTalkable;
    public bool DoneTalking;
    public List<Talkable> Talkables;
    public Dictionary<string, TalkingCharacter> TalkingCharacterDictionary
        = new Dictionary<string, TalkingCharacter>();
    public Dialogue DontNeedThat;
    public CharacterMovement Player;
    public Dictionary<Vector3, bool> SavedActiveState
        = new Dictionary<Vector3, bool>();

    void Update () {
        // player can't be controlled while talking
        Player.IsControlledByPlayer = (false == IsTalkingToSomeone());

    }

    public TalkingCharacter GetTalkingCharacter (string name) {
        return TalkingCharacterDictionary[name];
    }

    public void RegisterTalkingCharacter (string name, TalkingCharacter character) {
        TalkingCharacterDictionary[name] = character;
    }

    public void NotifyInteractionTriggered () {
        foreach (Talkable t in Talkables) {
            t.TriggerInteraction();
        }
    }

    public bool IsTalkingToSomeone () {
        return ActiveTalkable != null && ActiveTalkable.IsTalking();
    }
}
