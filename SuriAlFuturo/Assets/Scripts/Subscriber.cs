using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class Subscriber : MonoBehaviour {
    public Event EventSubscribed;
    public bool Triggered = false;

    public bool GetsUnblocked;
    public Blocker TheBlocker;
    public int DialogueIndex;
    public Talkable TheTalkable;

    private EventController _controller;

    void Start () {
        _controller = GameObject.FindGameObjectWithTag(Tag.GameController);

        _controller.Load(this);
    }

    public void Trigger (event) {
        if (myEvent == event) {
            Triggered = true;
            if (GetsUnblocked) {
                Unblock();
            }
            if (TheTalkable != null) {
                TheTalkable.SetDialogueIndex(DialogueIndex);
            }
            this.enabled = false;
        }
    }

    public PersistedSubscriber GetPersistedObject () {
        return new PersistedSubscriber(Triggered);
    }
}
