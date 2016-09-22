using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Talkable : MonoBehaviour {
    public List<TextAsset> Dialogues;
    public GameObject InteractIndicator;
    public bool WasRead;

    public int _currentLine;
    public int _currentDialogue;
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
        if (_canInteract && Input.GetButtonDown("Interact")) {
            // _currentLine iterates between [-1, _digestedDialogue.Length-1]
            _currentLine = (_currentLine + 2) % (_digestedDialogue.Length + 1) - 1;

            if (_currentLine == -1) {
                WasRead = true;
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
        _digestedDialogue =
            JsonUtility.FromJson<JsonDialogueData>(Dialogues[_currentDialogue].text).
            Dialogues;
    }

    public Dialogue GetDialogue () {
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
}
