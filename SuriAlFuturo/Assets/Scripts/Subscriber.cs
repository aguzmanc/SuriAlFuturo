using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class Subscriber : MonoBehaviour 
{
    public Vector3 PersistenceKey;

    public Event EventSubscribed;
    public bool Triggered = false;

    public bool DisappearsOnTrigger = false;
    public bool GetsUnblocked;
    public Blocker TheBlocker;
    public int DialogueIndex;
    public Talkable TheTalkable;
    public Collider Collider;

    public GameObject Spawns;

    private EventController _controller;

    void Start () 
    {
        _controller = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<EventController>();
        PersistenceKey = transform.position;

        _controller.Load(this);
        _controller.RegisterAsAlive(this);
    }

    void OnDestroy () 
    {
        try{
        _controller.Save(this);
        } catch {
            Debug.LogError(name);
        }

        try{
            _controller.UnregisterAsAlive(this);
        } catch {
            Debug.LogError("new: " + name);
        }
    }



    public void OnEventTriggered ()
    {
        if (!Triggered) {
            Trigger();
        }
    }

    public void Trigger () 
    {
        Triggered = true;

        if (GetsUnblocked) {
            TheBlocker.Unblock();
        }

        if (TheTalkable != null) {
            TheTalkable.TriggerDialogue(DialogueIndex);
        }

        if(Collider != null) {
            Collider.enabled = true;
            WaterTap tap = GetComponent<WaterTap>();
            tap.EnableUsable();
        }

        if (DisappearsOnTrigger) {
            this.gameObject.SetActive(false);
        }

        if (Spawns != null) {
            Spawns.SetActive(true);
        }

        this.enabled = false;
    }

    public void LoadTriggered () {
        if (DisappearsOnTrigger) {
            this.gameObject.SetActive(false);
        }

        if (Spawns != null) {
            Spawns.SetActive(true);
        }
    }



    public PersistedSubscriber GetPersistedObject () 
    {
        return new PersistedSubscriber(Triggered);
    }



    public void Load (PersistedSubscriber persisted)
    {
        Triggered = persisted.Triggered;

        if (!Triggered && _controller.TimesTriggered(EventSubscribed) > 0) {
            Trigger();
            if (TheBlocker != null) {
                TheBlocker.ForcedUnblock();
            }
        } else if (Triggered) {
            LoadTriggered();
        }
    }
}

/*  TODO. En el futuro mejorar sistema de subscribers y triggers de eventos de forma generica
[System.Serializable]
public class ChangeDialogueAction 
{
    public bool Activated;
    public Talkable talkable;
    public int index;
}


[System.Serializable]
public class DisableObjectAction 
{
    public bool Activated;
}
*/
