using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SuriAlFuturo;

public class Blocker : MonoBehaviour {
    public Vector3 PersistenceKey;

    public float Speed = 5;
    public bool IsUnblocked;
    public bool WasForcedToUnblock = false;
    public List<Requirement> UnmetRequirements = new List<Requirement>();

    public GameObject UnblockedPosition;
    public GameObject Model;
    public GameObject Obstacle;
    public Talkable TheTalkable;

    private CollectionSystem _controller;
    private GameController _gameController;

    private int _receibedDialogueIndex;
    private bool _canTake;
    private NavMeshObstacle _navMeshObstacle;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private bool _interactionTriggered;
    private Dictionary<Collectable.Tag, Dialogue[]> _customDontNeedThat
        = new Dictionary<Collectable.Tag, Dialogue[]>();

    void Start () {
        PersistenceKey = transform.position;

        _gameController = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<GameController>();

        _controller = _gameController.GetComponent<CollectionSystem>();

        _navMeshObstacle = Obstacle.GetComponent<NavMeshObstacle>();
        _animator = Model.GetComponent<Animator>();

        _controller.Blockers.Add(this);

        _navMeshAgent = TheTalkable.GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = false;

        _controller.Load(this);
    }

    void Update () {
        if (Input.GetButtonDown("Give") || _interactionTriggered) {
            _interactionTriggered = false;

            if (_canTake && !TheTalkable.IsTalking() &&
                false == TryToTakeRequirement(_controller.GetActiveRequirement())) {
                Dialogue[] customRejection = null;
                bool hasCustomRejection = false;

                try {
                    customRejection =
                        _customDontNeedThat[_controller.GetActiveRequirement()];
                    hasCustomRejection = true;
                } catch {}
                if (hasCustomRejection) {
                    TheTalkable.ForceDialogue(customRejection);
                } else {
                    TheTalkable.SayIDontWantThat();
                }
            }

        }

        if (false == IsUnblocked &&
            AreRequirementsMet() &&
            TheTalkable.WasDialogueIndexRead(_receibedDialogueIndex)) {
            Unblock();
        }

        _animator.SetBool("IsWalking", _navMeshAgent.velocity.magnitude > 0.1f);
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
        _navMeshObstacle.enabled = false;
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(UnblockedPosition.transform.position);
    }

    public void ForcedUnblock () {
        UnmetRequirements = new List<Requirement>();
        Unblock();
        WasForcedToUnblock = true;
    }

    public bool AreRequirementsMet () {
        return UnmetRequirements.Count == 0;
    }

    public bool TryToTakeRequirement (Collectable.Tag requirement) {
        // int i = UnmetRequirements.IndexOf(requirement);
        int i = _IndexOfRequirement(requirement);

        if (i >= 0) {
            _receibedDialogueIndex = UnmetRequirements[i].IndexOfDialogue;
            TheTalkable.SetDialogueIndex(_receibedDialogueIndex);
            _controller.RegisterAsGiven(requirement);
            UnmetRequirements.RemoveAt(i);
            TheTalkable.IsForcedToTalk = true;
            return true;
        }

        return false;
    }

    public void TriggerInteraction () {
        _interactionTriggered = true;
    }

    #region persistence
    public PersistedBlocker GetPersistedObject () {
        return new PersistedBlocker(IsUnblocked, UnmetRequirements);
    }

    public void LoadPersistedObject (PersistedBlocker persisted) {
        IsUnblocked = persisted.IsUnblocked || WasForcedToUnblock;

        if (IsUnblocked) {
            transform.position = UnblockedPosition.transform.position;
            _navMeshObstacle.enabled = false;
        }

        if (!WasForcedToUnblock) {
            if (persisted.UnmetRequirements.Count > 0) {
                UnmetRequirements = new List<Requirement>();
            }

            foreach (Requirement requirement in persisted.UnmetRequirements) {
                UnmetRequirements.Add(requirement);
            }
        }
    }
    #endregion

    private int _IndexOfRequirement (Collectable.Tag requirement) {
        for (int i=0; i<UnmetRequirements.Count; i++) {
            if (UnmetRequirements[i].Name == requirement) {
                return i;
            }
        }

        return -1;
    }

    public void Register (CustomDontNeedThat rejection) {
        _customDontNeedThat[rejection.Item] = rejection.GetDialogues();
    }
}
