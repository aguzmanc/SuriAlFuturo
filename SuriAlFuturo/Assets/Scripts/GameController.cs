using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public FollowCamera FollowingCamera;
    public GameObject Suri;
    public GameObject MovementGizmos;
    public GameObject ControlledCharacter;
    public bool CanTalk;
    public bool CanGive {
        get {
            return (_collectionSystem.CountOwnedItems() > 0) && CanTalk
                && false == _dialogueController.IsTalkingToSomeone();
        }
    }

    private bool _drivingBoat;
    private bool _canGive;
    private CollectionSystem _collectionSystem;
    private DialogueController _dialogueController;

    void Start () {
        _collectionSystem = GetComponent<CollectionSystem>();
        _dialogueController = GetComponent<DialogueController>();
    }

    public void SetDrivingBoat (bool value) {
        _drivingBoat = value;
        if (value) {
            FollowingCamera.EnterWater();
        } else {
            FollowingCamera.ExitWater();
        }
    }

}
