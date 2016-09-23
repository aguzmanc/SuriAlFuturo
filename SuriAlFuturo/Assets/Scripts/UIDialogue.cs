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
        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).
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
            string avatar = _activeDialogue.Avatar,
                name = _activeDialogue.Name;

            if (avatar == "default") {
                avatar = _controller.ActiveTalkable.DefaultAvatar;
            }
            if (name == "default") {
                name = _controller.ActiveTalkable.DefaultName;
            }

            // emotion...
            _controller.GetTalkingCharacter(avatar).
                SetEmotion(_activeDialogue.Emotion);
            // animooted picture...
            IconHolder.sprite =
                _controller.GetTalkingCharacter(avatar).GetSprite();
            // name...
            NameHolder.text = name;
            // dialogue text...
            TextHolder.text = _activeDialogue.Text;
        } else {
            DialogueHolder.SetActive(false);
        }
    }
}
