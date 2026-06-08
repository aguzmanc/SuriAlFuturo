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


    void Start () 
    {
        PersistenceKey = transform.position;

        EventController.Instance.Load(this);
        EventController.Instance.RegisterAsAlive(this);
    }


    void OnDestroy () 
    {
        EventController.Instance.Save(this);
        EventController.Instance.UnregisterAsAlive(this);
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
            Debug.Log("Load:" + this.name);// + ":  " + persisted.Triggered);
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

        int timesTriggered = EventController.Instance.TimesTriggered(EventSubscribed);

        if (!Triggered && timesTriggered   > 0) {
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
