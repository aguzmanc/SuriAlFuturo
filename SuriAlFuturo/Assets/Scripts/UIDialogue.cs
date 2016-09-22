using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDialogue : MonoBehaviour {
    public GameObject DialogueHolder;
    public Image IconHolder;
    public Text NameHolder;
    public Text TextHolder;

    private Dialogue _activeDialogue;
    private DialogueController _controller; // TODO: put the dictionary

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
            _activeDialogue = _controller.ActiveTalkable.GetDialogue();

            // emotion...
            _controller.GetTalkingCharacter(_activeDialogue.Name).
                SetEmotion(_activeDialogue.Emotion);
            // animooted picture...
            IconHolder.sprite =
                _controller.GetTalkingCharacter(_activeDialogue.Name).GetSprite();
            // name...
            NameHolder.text = _activeDialogue.Name;
            // dialogue text...
            TextHolder.text = _activeDialogue.Text;
        } else {
            DialogueHolder.SetActive(false);
        }
    }
}
