using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class DisableOnRead : MonoBehaviour {
    private Talkable _talkable;
    private DialogueController _dialogueController;

    void Start () {
        _dialogueController = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).GetComponent<DialogueController>();
        _talkable = GetComponent<Talkable>();
    }
    
    void Update () {
        if (_talkable.WasRead) {
            gameObject.SetActive(false);
        }
    }
}
