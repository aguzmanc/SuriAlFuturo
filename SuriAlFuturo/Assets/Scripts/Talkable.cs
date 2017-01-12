using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SuriAlFuturo;

public class Talkable : MonoBehaviour {
    private bool _triggeredByCollider;
    public List<bool> ReadDialogues;
    public bool WasTriggered = false;
    public List<TextAsset> Dialogues;
    public GameObject InteractIndicator;
    public bool WasRead;
    public bool IsForcedToTalk;
    public bool WillTalkForcedDialogue;
    public Dialogue[] ForcedDialogue;
    public string DefaultName = "...";
    public string DefaultAvatar = "Cholita";
    public Vector3 PersistenceKey;

    private int asdf;

    private bool _interactionTriggered;
    private int _currentLine;
    private int _currentDialogue;
    private bool _canInteract;
    private DialogueController _controller;
    private GameController _gameController;
    private Dialogue[]  _digestedDialogue;
    private MobileUI _mobileUI;

    void Start () {
        PersistenceKey = this.transform.position;

        foreach (TextAsset x in Dialogues) {
            ReadDialogues.Add(false);
        }

        _gameController = GameObject.
            FindGameObjectWithTag(Tag.GameController).GetComponent<GameController>();
        _controller = _gameController.GetComponent<DialogueController>();

        _mobileUI = GameObject.
            FindGameObjectWithTag(Tag.Canvas).GetComponent<MobileUI>();

        if (!WasTriggered) {
            if (_controller.HasSavedData(this)) {
                _controller.Load(this);
            } else {
                _currentLine = -1;
                _currentDialogue = 0;
            }
        }

        _DigestDialogue();
        _controller.Talkables.Add(this);
    }

    void Update () {
        int qtyOfLines = _digestedDialogue.Length;
        if (WillTalkForcedDialogue) {
            qtyOfLines = ForcedDialogue.Length;
        }

        if (IsInteractTriggered()) {
            IsForcedToTalk = false;
            // _currentLine iterates between [-1, _digestedDialogue.Length-1]
            _currentLine = (_currentLine + 2) % (qtyOfLines + 1) - 1;

            if (_currentLine == -1) { // when dialog finished
                _gameController.CanTalk = false;
                WasRead = true;
                if (WillTalkForcedDialogue) {
                    if (!_triggeredByCollider) {
                        _gameController.CanTalk = false;
                    }
                    WillTalkForcedDialogue = false;
                } else {
                    ReadDialogues[_currentDialogue] = true;
                }
            } else {
                _gameController.CanTalk = true;
            }
        }
    }

    void OnDisable () {
        if (_canInteract) {
            _canInteract = _gameController.CanTalk = false;
        }

        try{
            _controller.Save(this);
        } catch{
            Debug.LogError("error Talkable OnDisable(): " + name); 
        }
    }

    void OnTriggerEnter (Collider c) {
        _gameController.CanTalk = _canInteract = true;
        _currentLine = -1;
        _controller.ActiveTalkable = this;
        _triggeredByCollider = true;
    }

    void OnTriggerExit (Collider c) {
        _gameController.CanTalk = _canInteract = false;
        _currentLine = -1;
        if (_controller.ActiveTalkable == this) {
            _controller.ActiveTalkable = null;
        }
        _triggeredByCollider = false;
    }

    void OnDestroy () {
        try{
        _controller.Save(this);
        } catch{
            Debug.LogError("data: " + name);
        }
        _controller.Talkables.Remove(this);
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
            return ForcedDialogue[_currentLine];
        }
        return _digestedDialogue[_currentLine];
    }

    public bool IsTalking () {
        return _currentLine >= 0;
    }

    public void TriggerDialogue (int index)
    {
        SetDialogueIndex(index);
        WasTriggered = true;
    }

    public void SetDialogueIndex (int index) {
        _currentDialogue = index;
        WasRead = false;
        _DigestDialogue();
    }

    public bool IsInteractTriggered () {
        bool triggereded = false;
        if (_interactionTriggered) {
            triggereded = true;
            _interactionTriggered = false;
        }

        return _canInteract && ( Input.GetButtonDown("Interact") || IsForcedToTalk ||
                                 ( Input.GetButtonDown("Give") && _currentLine >= 0 )
                                 || triggereded );
    }

    public void ForceDialogue (Dialogue[] forcedDialogue) {
        ForcedDialogue = forcedDialogue;
        WillTalkForcedDialogue = true;
        IsForcedToTalk = true;
    }

    public void SayIDontWantThat () {
        _gameController.GetComponent<SFXController>().PlayError();
        ForceDialogue(new Dialogue[1] {_controller.DontNeedThat});
    }

    public void TriggerInteraction () {
        _interactionTriggered = true;
    }

    public PersistedTalkable GetPersistedTalkable () {
        return new PersistedTalkable(_currentDialogue, IsForcedToTalk, WasRead,
                                     ReadDialogues);
    }

    public void Load (PersistedTalkable persisted) {
        if (!WasTriggered) {
            this._currentDialogue = persisted.DialogueIndex;
            this.IsForcedToTalk = persisted.IsForcedToTalk;
            this.WasRead = persisted.WasRead;
            this.ReadDialogues = persisted.ReadDialogues;
        }
    }

    public int GetDialogueIndex () {
        return _currentDialogue;
    }

    public bool WasDialogueIndexRead (int dialogueIndex) {
        return ReadDialogues[dialogueIndex];
    }
}
