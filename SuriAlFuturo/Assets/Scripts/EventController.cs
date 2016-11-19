using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour {
    public Dictionary<Vector3, PersistedOnTouchTrigger> SavedTouchTriggers =
        new Dictionary<Vector3, PersistedOnTouchTrigger>();

    public Dictionary<Vector3, PersistedSubscriber> SavedSubscribers =
        new Dictionary<Vector3, PersistedSubscriber>();

    public Dictionary<Event, int> TriggeredEvents =
        new Dictionary<Event, int>();

    void Start () {
        
    }
    
    public void TriggerEvent (Event event) {
        TriggeredEvents[event] = true;

        for (int i=0; i<Subscribers.Count; i++) {
            Subscribers[i].Trigger(event);
        }
    }

    public void Save (OnTouchTrigger trigger) {
        SavedTouchTriggers[trigger.PersistenceKey] = trigger.GetPersistedObject();
    }

    public void Load (OnTouchTrigger trigger) {
        try {
            trigger.Load(SavedTouchTriggers[trigger.PersistenceKey]);
        } catch {}
    }

    public void Save (Subscriber s) {
        SavedTouchTriggers[s.PersistenceKey] = s.GetPersistedObject();
    }

    public void Load (Subscriber s) {
        try {
            s.Load(SavedSubscribers[s.PersistenceKey]);
        } catch {}
    }
}
