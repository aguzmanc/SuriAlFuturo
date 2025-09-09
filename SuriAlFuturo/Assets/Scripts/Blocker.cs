using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SuriAlFuturo;

public class Blocker : MonoBehaviour {
    public Vector3 PersistenceKey;

    public GameObject ArtificialTrigger;

    public float Speed = 5;
    public bool IsUnblocked;
    public bool DisablesOnUnblock = false;
    public bool WasForcedToUnblock = false;
    public List<Requirement> UnmetRequirements = new List<Requirement>();
    public WaterTap ImprovesFuturePatch;
    public GameObject EnablesGameObject;
    public GameObject DisablesGameObject;

    public GameObject UnblockedPosition;
    public GameObject Model;
    public GameObject Obstacle;
    public Talkable TheTalkable;

    private CollectionSystem _controller;
    private GameController _gameController;

    private int _receibedDialogueIndex;
    public bool _canTake = false;
    private UnityEngine.AI.NavMeshObstacle _navMeshObstacle;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private bool _interactionTriggered;
    
    private Dictionary<Collectable.Tag, Dialogue[]> _customDontNeedThat
        = new Dictionary<Collectable.Tag, Dialogue[]>();
    private Dictionary<Collectable.Tag, CustomAccept> _customAccept
        = new Dictionary<Collectable.Tag, CustomAccept>();

    void Start () {
        PersistenceKey = transform.position;

        _gameController = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<GameController>();

        _controller = _gameController.GetComponent<CollectionSystem>();

        _navMeshObstacle = Obstacle.GetComponent<UnityEngine.AI.NavMeshObstacle>();
        _animator = Model.GetComponent<Animator>();

        _controller.Blockers.Add(this);

        _navMeshAgent = TheTalkable.GetComponent<UnityEngine.AI.NavMeshAgent>();
        _navMeshAgent.enabled = false;

        _controller.Load(this);
        _canTake = false;
    }

    // needs refactorization
    void Update () {
        if (Input.GetButtonDown("Give") || _interactionTriggered) {
            _interactionTriggered = false;

            // try {
            if (_customAccept.ContainsKey(_controller.GetActiveRequirement())) {
                CustomAccept c = _customAccept[_controller.GetActiveRequirement()];
                TheTalkable.SetDialogueIndex(c.DialogueIndex);
                _controller.RegisterAsGiven(_controller.GetActiveRequirement());
                TheTalkable.IsForcedToTalk = true;
                c.enabled = false;
                c.Activate();
                return;
            }
            // } catch {}

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

    public void TriggerEnter () {
        _gameController.CloseToBlocker = _canTake = true;
    }

    public void TriggerExit () {
        _gameController.CloseToBlocker = _canTake = false;
    }

    void OnTriggerEnter (Collider player) {
        TriggerEnter();
    }

    void OnTriggerExit (Collider player) {
        TriggerExit();
    }

    public void Unblock () {
        IsUnblocked = true;
        _navMeshObstacle.enabled = false;
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(UnblockedPosition.transform.position);

        try {
            if (ImprovesFuturePatch != null) {
                ImprovesFuturePatch.ToggleFountain(false, true);
            }
        } catch {}

        if (EnablesGameObject != null) {
            EnablesGameObject.SetActive(true);
        }

        if (DisablesGameObject != null) {
            DisablesGameObject.SetActive(false);
        }

        try {
            SphereCollider c = GetComponent<SphereCollider>();
            TriggerExit();
            c.enabled = false;
            ArtificialTrigger.SetActive(true);
        } catch {}
    }

    public void ForcedUnblock () {
        UnmetRequirements = new List<Requirement>();
        Unblock();
        WasForcedToUnblock = true;
    }

    public bool AreRequirementsMet () {
        return UnmetRequirements.Count == 0;
    }

    // needs refactorization
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

    public void Register (CustomAccept c) {
        _customAccept[c.Item] = c;
    }
}
