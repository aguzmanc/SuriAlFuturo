using UnityEngine;
using System.Collections;

public class MobileUI : MonoBehaviour {
    public GameObject TalkButton;
    public GameObject GiveButton;

    private GameController _controller;
    private DialogueController _dialogueController;

    void Start () {
        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).
            GetComponent<GameController>();
        _dialogueController = _controller.GetComponent<DialogueController>();
    }
    
    void Update () {
        TalkButton.SetActive(_controller.CanTalk);
        GiveButton.SetActive(_controller.CanGive);
    }
}
