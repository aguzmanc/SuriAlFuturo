using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventController : MonoBehaviour {
    public Dictionary<Vector3, PersistedOnTouchTrigger> SavedTouchTriggers =
        new Dictionary<Vector3, PersistedOnTouchTrigger>();

    public Dictionary<Vector3, PersistedSubscriber> SavedSubscribers =
        new Dictionary<Vector3, PersistedSubscriber>();

    public Dictionary<Event, int> TriggeredEvents =
        new Dictionary<Event, int>();

    public List<Subscriber> AliveSubscribers = new List<Subscriber>();

    void Start () {
        
    }
    
    public void TriggerEvent (Event e) {
        try {
            TriggeredEvents[e]++;
        } catch {
            TriggeredEvents[e] = 1;
        }

        foreach (Subscriber s in AliveSubscribers) {
            if (s.EventSubscribed == e) {
                s.OnEventTriggered();
            }
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
        SavedSubscribers[s.PersistenceKey] = s.GetPersistedObject();
    }

    public void Load (Subscriber s) {
        try {
            s.Load(SavedSubscribers[s.PersistenceKey]);
        } catch {}
    }

    public int TimesTriggered (Event e) {
        try {
            return TriggeredEvents[e];
        } catch {
            return 0;
        }
    }

    public void RegisterAsAlive (Subscriber s) {
        AliveSubscribers.Add(s);
    }

    public void UnregisterAsAlive (Subscriber s) {
        AliveSubscribers.Remove(s);
    }
}
