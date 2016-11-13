using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SuriAlFuturo;

public class Blocker : MonoBehaviour {
    public Vector3 PersistenceKey;

    public bool IsUnblocked;
    public List<Requirement> UnmetRequirements;
    public GameObject UnblockedPosition;
    public float Speed = 5;
    public GameObject Obstacle;
    public GameObject Model;
    public Talkable TheTalkable;
    public int RequirementsMetDialogueIndex = 1;

    private bool _canTake;
    private CollectionSystem _controller;
    private GameController _gameController;
    private NavMeshObstacle _navmeshObstacle;
    private float _timeOnState = 0;
    private Vector3 _cachedPosition;
    private float _totalTime;
    private Animator _animator;
    private bool _interactionTriggered;

    void Start () {
        PersistenceKey = transform.position;

        _gameController = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<GameController>();
        _controller = _gameController.GetComponent<CollectionSystem>();
        _navmeshObstacle = Obstacle.GetComponent<NavMeshObstacle>();
        _animator = Model.GetComponent<Animator>();

        _controller.Blockers.Add(this);
        _controller.RegisterBlocker(this);

        if (_controller.HasSavedData(this)) {
            _controller.Load(this);
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

        if (Input.GetButtonDown("Give") || _interactionTriggered) {
            _interactionTriggered = false;
            if (_canTake && !TheTalkable.IsTalking() &&
                false == TryToTakeRequirement(_controller.GetActiveRequirement())) {
                TheTalkable.SayIDontHaveThat();
            }
        }

        if (TheTalkable.WasRead && AreRequirementsMet()) {
            Unblock();
        }
    }

    void OnDisable () {
        _controller.Save(this); // TODO: es necesario grabar ondisabled y ondestroy?
    }

    void OnDestroy () {
        _controller.Blockers.Remove(this);
        _controller.Save(this);
    }

    void OnTriggerEnter (Collider player) {
        _gameController.CloseToBlocker = _canTake = true;
    }

    void OnTriggerExit (Collider player) {
        _gameController.CloseToBlocker = _canTake = false;
    }

    public void Unblock () {
        IsUnblocked = true;
        _cachedPosition = Obstacle.transform.position;
        _timeOnState = 0;
        _navmeshObstacle.enabled = false;
        _totalTime = (UnblockedPosition.transform.position - _cachedPosition)
            .magnitude / Speed;
    }

    public bool AreRequirementsMet () {
        return UnmetRequirements.Count == 0;
    }

    public bool TryToTakeRequirement (Sprite requirement) {
        int i = IndexOfRequirement(requirement);
        if (i >= 0) {
            TheTalkable.SetDialogueIndex(UnmetRequirements[i].IndexOfDialogue);
            _controller.RegisterAsGiven(requirement);
            UnmetRequirements.RemoveAt(i);
            TheTalkable.IsForcedToTalk = true;
            return true;
        } else {
            return false;
        }
    }

    public int IndexOfRequirement (Sprite requirement) {
        for (int i=0; i<UnmetRequirements.Count; i++) {
            if (UnmetRequirements[i].Image == requirement) {
                return i;
            }
        }
        return -1;
    }

    public void TriggerInteraction () {
        _interactionTriggered = true;
    }

    public PersistedBlocker GetPersistedObject () {
        return new PersistedBlocker(this);
    }

    public void Load (PersistedBlocker persisted) {
        if (persisted.IsUnblocked) {
            Obstacle.transform.position = UnblockedPosition.transform.position;
            // they could equal anything but it must be < 0
            _totalTime = _timeOnState = 1;
            _navmeshObstacle.enabled = false;
        }
        IsUnblocked = persisted.IsUnblocked;

        foreach (Requirement r in UnmetRequirements) {
            UnmetRequirements.Add(r);
        }
    }
}
