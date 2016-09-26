using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Talkable : MonoBehaviour {
    public List<TextAsset> Dialogues;
    public GameObject InteractIndicator;
    public bool WasRead;
    public bool IsForcedToTalk;
    public bool WillTalkForcedDialogue;
    public Dialogue ForcedDialogue;
    public string DefaultName = "...";
    public string DefaultAvatar = "Cholita";

    private int _currentLine;
    private int _currentDialogue;
    private bool _canInteract;    
    private DialogueController _controller;
    private Dialogue[]  _digestedDialogue;
    
    void Start () {
        _currentLine = -1;
        _currentDialogue = 0;
        _DigestDialogue();
        InteractIndicator.SetActive(false);
        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController)
            .GetComponent<DialogueController>();
    }

    void Update () {
        int qtyOfLines = _digestedDialogue.Length;
        if (WillTalkForcedDialogue) {
            qtyOfLines = 1;
        }

        if (IsInteractTriggered()) {
            IsForcedToTalk = false;
            // _currentLine iterates between [-1, _digestedDialogue.Length-1]
            _currentLine = (_currentLine + 2) % (qtyOfLines + 1) - 1;

            if (_currentLine == -1) {
                WasRead = true;
                if (WillTalkForcedDialogue) {
                    WillTalkForcedDialogue = false;
                }
            }
        }
    }

    void OnTriggerEnter (Collider c) {
        _canInteract = true;
        _currentLine = -1;
        InteractIndicator.SetActive(true);
        _controller.ActiveTalkable = this;
    }

    void OnTriggerExit (Collider c) {
        InteractIndicator.SetActive(false);
        _canInteract = false;
        _currentLine = -1;
        if (_controller.ActiveTalkable == this) {
            _controller.ActiveTalkable = null;
        }
    }



    private void _DigestDialogue () {
        if (Dialogues.Count == 0) {
            _digestedDialogue = new Dialogue[0];
        } else {
            _digestedDialogue =
                JsonUtility.FromJson<JsonDialogueData>(Dialogues[_currentDialogue].text).
                Dialogues;
        }
    }

    public Dialogue GetDialogue () {
        if (WillTalkForcedDialogue) {
            return ForcedDialogue;
        }
        return _digestedDialogue[_currentLine];
    }

    public bool IsTalking () {
        return _currentLine >= 0;
    }

    public void SetDialogueIndex (int index) {
        _currentDialogue = index;
        WasRead = false;
        _DigestDialogue();
    }

    public bool IsInteractTriggered () {
        return _canInteract && (Input.GetButtonDown("Interact") || IsForcedToTalk ||
                                (Input.anyKeyDown && _currentLine >= 0));
    }

    public void ForceDialogue (Dialogue forcedDialogue) {
        ForcedDialogue = forcedDialogue;
        WillTalkForcedDialogue = true;
        IsForcedToTalk = true;
    }

    public void SayIDontHaveThat () {
        ForceDialogue(_controller.DontNeedThat);
    }
}
