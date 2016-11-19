using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class Subscriber : MonoBehaviour {
    public Vector3 PersistenceKey;

    public Event EventSubscribed;
    public bool Triggered = false;

    public bool GetsUnblocked;
    public Blocker TheBlocker;
    public int DialogueIndex;
    public Talkable TheTalkable;

    private EventController _controller;

    void Start () {
        _controller = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<EventController>();
        PersistenceKey = transform.position;

        _controller.Load(this);
        _controller.RegisterAsAlive(this);
    }

    void OnDestroy () {
        _controller.Save(this);
        _controller.UnregisterAsAlive(this);
    }

    public void OnEventTriggered () {
        if (!Triggered) {
            Trigger();
        }
    }

    public void Trigger () {
        Debug.Log("Trigger!! " + EventSubscribed);
        Triggered = true;
        Debug.Log("DING!1");
        if (GetsUnblocked) {
            try {
                TheBlocker.Unblock();
            } catch {}
        }
        Debug.Log("DING!2");
        if (TheTalkable != null) {
            TheTalkable.TriggerDialogue(DialogueIndex);
        }
        Debug.Log("DING!3");
        this.enabled = false;
        Debug.Log("DING!4");
    }

    public PersistedSubscriber GetPersistedObject () {
        return new PersistedSubscriber(Triggered);
    }

    public void Load (PersistedSubscriber persisted) {
        Triggered = persisted.Triggered;

        if (!Triggered && _controller.TimesTriggered(EventSubscribed) > 0) {
            Trigger();
            TheBlocker.ForcedUnblock();
        }
    }
}
