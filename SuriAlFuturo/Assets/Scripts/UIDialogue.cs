using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDialogue : MonoBehaviour {
    public GameObject DialogueHolder;
    public Image IconHolder;
    public Text NameHolder;
    public Text TextHolder;

    private DialogueController _controller;

    void Start () {
        _controller = GameObject.FindGameObjectWithTag(Tag.GameController).
            GetComponent<DialogueController>();
        if (!_controller) {
            throw( new UnityException("I need a DialogueController component attached to the UI Canvas"));
        }
    }
    
    void Update () {
	if (_controller.ActiveTalkable &&
            _controller.ActiveTalkable.IsTalking()) {
            DialogueHolder.SetActive(true);
            IconHolder.sprite = _controller.ActiveTalkable.Icon;
            NameHolder.text = _controller.ActiveTalkable.Name;
            TextHolder.text = _controller.ActiveTalkable.GetText();
        } else {
            DialogueHolder.SetActive(false);
        }
    }
}
