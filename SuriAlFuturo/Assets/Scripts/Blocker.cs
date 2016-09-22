using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blocker : MonoBehaviour {
    public bool IsUnblocked;
    public List<Collectable> Requirements;
    public GameObject UnblockedPosition;
    public float Speed = 5;
    public GameObject Obstacle;
    public GameObject Model;
    public Talkable TheTalkable;

    private CollectionSystem _controller;
    private NavMeshObstacle _navmeshObstacle;
    private float _timeOnState = 0;
    private Vector3 _cachedPosition;
    private float _totalTime;
    private Animator _animator;
    private bool _areRequirementsMeet = false;

    void Start () {
        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController)
            .GetComponent<CollectionSystem>();
        _navmeshObstacle = Obstacle.GetComponent<NavMeshObstacle>();
        _animator = Model.GetComponent<Animator>();
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

        if (!IsUnblocked && TheTalkable.WasRead && _areRequirementsMeet) {
            TakeRequirements();
            Unblock();
            _controller.UnblockedDudes.Add(this);
        }

    }

    public void Unblock () {
        IsUnblocked = true;
        _cachedPosition = Obstacle.transform.position;
        _timeOnState = 0;
        _navmeshObstacle.enabled = false;
        _totalTime = (UnblockedPosition.transform.position - _cachedPosition)
            .magnitude / Speed;
    }

    public bool AreRequirementsMeet () {
        for (int i=0; i<Requirements.Count; i++) {
            if (false == _controller.IngameCollector.Inventory[Requirements[i].Image]) {
                return false;
            }
        }
        return true;
    }

    public void TakeRequirements () {
        for (int i=0; i<Requirements.Count; i++) {
            _controller.IngameCollector.Give(Requirements[i]);
        }
    }

    void OnTriggerEnter(Collider player) 
    {
        _areRequirementsMeet = AreRequirementsMeet();
        if (_areRequirementsMeet) {
            TheTalkable.SetDialogueIndex(1);
        }
    }
}
