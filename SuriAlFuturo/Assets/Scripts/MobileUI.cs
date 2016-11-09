using UnityEngine;
using System.Collections;

public class MobileUI : MonoBehaviour {
    public GameObject TalkButton;
    public GameObject GiveButton;
    public GameObject DockButton;
    public GameObject TapButton;

    private GameController _controller;
    private DialogueController _dialogueController;
    private DockController _dockController;

    void Start () {
        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).
            GetComponent<GameController>();
        _dialogueController = _controller.GetComponent<DialogueController>();
        _dockController = _controller.GetComponent<DockController>();
    }
    
    void Update () {
        TalkButton.SetActive(_controller.CanTalk);
        GiveButton.SetActive(_controller.CanGive);
        DockButton.SetActive(_dockController.CanUseDock &&
                             false == _dialogueController.IsTalkingToSomeone());
        TapButton.SetActive(_controller.CanUseTap);
    }
}
