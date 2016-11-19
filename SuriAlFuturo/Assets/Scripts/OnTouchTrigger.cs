using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class OnTouchTrigger : MonoBehaviour {
    public Event TriggeredEvent;
    public bool Triggered;

    public Vector3 PersistenceKey;

    private EventController _controller;

    void Start () {
        PersistenceKey = transform.position;
        _controller = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<EventController>();
        _controller.Load(this);
    }

    void OnDisable () {
        _controller.Save(this);
    }

    void OnDestroy () {
        _controller.Save(this);
    }

    void OnTriggerEnter (Collider c) {
        Triggered = true;
        _controller.TriggerEvent(TriggeredEvent);
    }

    public void Load (PersistedOnTouchTrigger persisted) {
        Triggered = persisted.Triggered;
    }

    public PersistedOnTouchTrigger GetPersistedObject () {
        return new PersistedOnTouchTrigger(this.Triggered);
    }
}
