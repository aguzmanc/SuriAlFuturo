using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Talkable : MonoBehaviour {
    public List<string> Dialogue = new List<string>();
    public int _currentDialogue = -1; //debugging
    public List<int> DialogueState = new List<int>();
    public int _currentState = 0; // debugging

    public Sprite Icon;
    public string Name;
    
    private bool _isActive;
    private DialogueController _controller;

    void Start () {
        try {
            _controller = GameObject.FindGameObjectWithTag(Tag.GameController).
                GetComponent<DialogueController>();
        } catch {
            throw(new UnityException("I need a GameController with a DialogueController component"));
        }
    }

    void Update () {
        if (_isActive && Input.GetButtonDown("Fire1")) {
            if (_currentDialogue == -1) { // beginning of the dialogue...
                if (_currentState == 0) {
                    _currentDialogue = 0;
                } else {
                    _currentDialogue = DialogueState[_currentState-1] + 1;
                }
            } else { // not the beginning... just advancing
                _currentDialogue++;
            }

            if (_currentDialogue > DialogueState[_currentState]) { // checking end...
                _currentDialogue = -1;
                _controller.DoneTalking = true;
            }
        }
    }

    void OnTriggerEnter (Collider c) {
        _isActive = true;
        _currentDialogue = -1;
        _controller.ActiveTalkable = this;
    }

    void OnTriggerExit (Collider c) {
        _isActive = false;
        _currentDialogue = -1;
        if (_controller.ActiveTalkable == this) {
            _controller.ActiveTalkable = null;
        }
    }

    public Sprite GetIcon () {
        return Icon;
    }

    public string GetName () {
        return Name;
    }

    public string GetText () {
        return Dialogue[_currentDialogue];
    }

    public void SetState (int newState) {
        _currentState = newState;
    }

    public bool IsTalking () {
        return _currentDialogue >= 0;
    }
}
