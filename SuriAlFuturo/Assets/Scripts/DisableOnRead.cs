using UnityEngine;
using System.Collections;

public class DisableOnRead : MonoBehaviour {
    private Talkable _talkable;
    private DialogueController _dialogueController;

    void Start () {
        _dialogueController = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).GetComponent<DialogueController>();
        _talkable = GetComponent<Talkable>();
        if (_dialogueController.SavedActiveState.ContainsKey(this.transform.position)) {
            Debug.Log(_dialogueController.SavedActiveState[this.transform.position]);
            gameObject.SetActive(_dialogueController.SavedActiveState[this.transform.position]);
        } else {
            _dialogueController.SavedActiveState[this.transform.position] = true;
        }
    }
    
    void Update () {
        if (_talkable.WasRead) {
            Debug.Log(_dialogueController.SavedActiveState[this.transform.position]);
            _dialogueController.SavedActiveState[this.transform.position] = false;
            gameObject.SetActive(false);
        }
    }
}
