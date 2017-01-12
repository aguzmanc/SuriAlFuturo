using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class OnReadSwitch : MonoBehaviour {
    public Talkable TheTalkable;

    public int OnDialogueIndexRead;
    public int SwitchTo;
    public Event TriggeredEvent = Event.Ninguno;

    private EventController _controller;
    
    void Start () {
        if (TheTalkable == null) {
            TheTalkable = GetComponent<Talkable>();
        }

        _controller = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<EventController>();
    }

    void Update () {
        if (TheTalkable.ReadDialogues[OnDialogueIndexRead] &&
            TheTalkable.GetDialogueIndex() == OnDialogueIndexRead) {
            try{
                TheTalkable.SetDialogueIndex(SwitchTo);
            }catch{
                Debug.Log("Error trying to switch from " + OnDialogueIndexRead + " to " + SwitchTo + " at " + name);
            }
            this.enabled = false;

            _controller.TriggerEvent(TriggeredEvent);
        }
    }
}
