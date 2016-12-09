using UnityEngine;
using System.Collections;

public class TalkingCharacter : MonoBehaviour {
    public string Name;

    private DialogueController _controller;
    private SpriteRenderer _renderer;
    private Animator _mecanim;

    void Start () {
        _renderer = GetComponent<SpriteRenderer>();
        _mecanim = GetComponent<Animator>();

        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).
            GetComponent<DialogueController>();
        _controller.RegisterTalkingCharacter(Name, this);
    }

    public Sprite GetSprite () {
        return _renderer.sprite;
    }

    public void SetEmotion (string newEmotion) {
        // _mecanim.SetTrigger(newEmotion);
        _mecanim.Play(newEmotion);

        _controller.GetComponent<SFXController>().PlayDialogue(Name, newEmotion);
    }
}
