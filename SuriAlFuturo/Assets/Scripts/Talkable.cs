using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Talkable : MonoBehaviour {
    public List<TextAsset> Dialogue;
    public GameObject InteractIndicator;
    public Sprite Icon;
    public string Name;
    public bool WasRead;

    public int _currentLine;
    public int _currentDialogue;
    private bool _canInteract;
    private DialogueController _controller;
    private List<List<string>> _digestedDialogue;
    private char[] _delimiterSymbols = {'\n'};

    void Start () {
        _DigestDialogue();
        _currentLine = -1;
        _currentDialogue = 0;
        InteractIndicator.SetActive(false);
        _controller = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<DialogueController>();
        Debug.Log(_controller);
    }

    void Update () {
        if (_canInteract && Input.GetButtonDown("Interact")) {
            // _currentLine iterates between
            // -1 and _digestedDialogue[_currentDialogue].Count-1
            _currentLine = (_currentLine + 2) % (_digestedDialogue[_currentDialogue].Count + 1) - 1;
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
        _digestedDialogue = new List<List<string>>();
        for (int i=0; i<Dialogue.Count; i++) {
            _digestedDialogue.Add(new List<string>());
            _digestedDialogue[i].AddRange(Dialogue[i].text.Split(_delimiterSymbols, StringSplitOptions.RemoveEmptyEntries));
        }        
    }

    public string GetText () {
        return _digestedDialogue[_currentDialogue][_currentLine];
    }

    public bool IsTalking () {
        return _currentLine >= 0;
    }

    public void SetDialogueIndex (int index) {
        _currentDialogue = index;
        WasRead = false;
    }
}
