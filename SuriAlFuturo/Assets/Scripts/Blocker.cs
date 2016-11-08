using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blocker : MonoBehaviour {
    public bool IsUnblocked;
    public List<Requirement> Requirements;
    public GameObject UnblockedPosition;
    public float Speed = 5;
    public GameObject Obstacle;
    public GameObject Model;
    public Talkable TheTalkable;
    public int RequirementsMeetDialogueIndex = 1;

    private bool _canTake;
    private CollectionSystem _controller;
    private NavMeshObstacle _navmeshObstacle;
    private float _timeOnState = 0;
    private Vector3 _cachedPosition;
    private float _totalTime;
    private Animator _animator;
    private bool _areRequirementsMeet = false;
    private bool _interactionTriggered;

    void Start () {
        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController)
            .GetComponent<CollectionSystem>();
        _controller.Blockers.Add(this);
        _navmeshObstacle = Obstacle.GetComponent<NavMeshObstacle>();
        _animator = Model.GetComponent<Animator>();

        _controller.RegisterBlocker(this);
        if (_controller.HasRegisteredRequirements(this)) {
            Requirements = _controller.GetRequirements(this);
        } else {
            _controller.RegisterRequirements(this);
        }

        if (_controller.IsUnblocked(this)) {
            this.Unblock();
            _timeOnState = 100;
        }
    }

    void Update () {
        _timeOnState += Time.deltaTime;

        if (IsUnblocked) {
            Obstacle.transform.position =
                Vector3.Lerp(_cachedPosition, UnblockedPosition.transform.position,
                             _timeOnState/_totalTime);
            _animator.SetBool("IsWalking", _timeOnState/_totalTime <= 1);
        } else {
            _animator.SetBool("IsWalking", false);
        }

        if ((Input.GetButtonDown("Give") || _interactionTriggered)
            && _canTake && !TheTalkable.IsTalking() ) {
            _interactionTriggered = false;
            if (false == TryToTakeRequirement(_controller.GetActiveRequirement())) {
                TheTalkable.SayIDontHaveThat();
            }
        }

        if (TheTalkable.WasRead && _areRequirementsMeet) {
            Unblock();
        }
    }

    void OnDestroy () {
        _controller.Blockers.Remove(this);
    }

    void OnTriggerEnter (Collider player) {
        _canTake = true;
    }

    void OnTriggerExit (Collider player) {
        _canTake = false;
    }

    public void Unblock () {
        _controller.RegisterAsUnblocked(this);
        IsUnblocked = true;
        _cachedPosition = Obstacle.transform.position;
        _timeOnState = 0;
        _navmeshObstacle.enabled = false;
        _totalTime = (UnblockedPosition.transform.position - _cachedPosition)
            .magnitude / Speed;
    }

    public bool AreRequirementsMeet () {
        return Requirements.Count == 0;
    }

    public bool TryToTakeRequirement (Sprite requirement) {
        int i = IndexOfRequirement(requirement);
        if (i >= 0) {
            TheTalkable.SetDialogueIndex(Requirements[i].IndexOfDialogue);
            _controller.RegisterAsGiven(requirement);
            Requirements.RemoveAt(i);
            _areRequirementsMeet = AreRequirementsMeet();
            TheTalkable.IsForcedToTalk = true;
            return true;
        } else {
            return false;
        }
    }

    public int IndexOfRequirement (Sprite requirement) {
        for (int i=0; i<Requirements.Count; i++) {
            if (Requirements[i].Image == requirement) {
                return i;
            }
        }
        return -1;
    }

    public void TriggerInteraction () {
        _interactionTriggered = true;
    }
}
