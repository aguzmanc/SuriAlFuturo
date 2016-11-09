using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using SuriAlFuturo;

public class CharacterMovement : MonoBehaviour
{
    public float Speed = 0;
    public bool IsControlledByPlayer = false;
    public bool IsControlledByArrows = true;
    public float CurrentSpeedPercent;
    public Vector3 Direction;

    private GameObject _gizmos;
    private Animator _gizmosAnimator;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private GameController _controller;
    private GameObject[] _floors;
    private UIController _uiController;
    private EventSystem _eventSystem;

    private Touch _tap;
    private bool _tapped;

    void Start () {
        _navMeshAgent = GetComponent<NavMeshAgent> ();
        _animator = GetComponent<Animator>();
        _navMeshAgent.speed = Speed;

        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).
            GetComponent<GameController>();
        _gizmos = _controller.MovementGizmos;
        _gizmosAnimator = _gizmos.GetComponent<Animator>();
        _uiController = GameObject.FindGameObjectWithTag(Tag.Canvas).
            GetComponent<UIController>();
        _eventSystem = GameObject.FindGameObjectWithTag(Tag.EventSystem).
            GetComponent<EventSystem>();
    }

    void Update ()
    {
        if (IsControlledByPlayer) {

            IsControlledByArrows = Mathf.Abs(Input.GetAxis("Horizontal")) > 0 ||
                Mathf.Abs(Input.GetAxis("Vertical")) > 0;

            UpdateTapDetector();
            UpdateDirection();
            UpdateMovement();
            UpdateAnimatorParameters();

            _controller.ControlledCharacter = this.gameObject;

        } else {
            _navMeshAgent.Stop();
            _navMeshAgent.velocity = Vector3.zero;
            CurrentSpeedPercent = 0;
        }
    }

    public void UpdateAnimatorParameters () {
        CurrentSpeedPercent = GetSpeedPercent();

        if (CurrentSpeedPercent != 0) {
            transform.forward = new Vector3(Direction.x, 0, Direction.z);
        }

        if (_animator != null) {
            _animator.SetBool("IsWalking", CurrentSpeedPercent != 0);
        }
    }

    public void UpdateMovement () {
        if (IsControlledByArrows) {
            _navMeshAgent.Stop();
            _navMeshAgent.Move(this.Direction * Time.deltaTime * Speed *
                               Mathf.Max( Mathf.Abs(Input.GetAxis("Vertical")),
                                          Mathf.Abs(Input.GetAxis("Horizontal")) ));
        } else if (Interacted() && !_IsInteractionBlocked()) {
            Vector3 destination = GetInteractionDestination();
            _gizmos.transform.position = destination;
            _gizmos.SetActive(true);
            _gizmosAnimator.SetTrigger("Born");
            _navMeshAgent.Resume();
            _navMeshAgent.SetDestination(destination);
        }
    }

    public void UpdateDirection () {

        if (IsControlledByPlayer) {
            this.Direction =  new Vector3(0,0,0);
        }

        if (IsControlledByArrows) {
            this.Direction =
                (FollowCamera.Instance.Forward * Input.GetAxis("Vertical") +
                 FollowCamera.Instance.Right * Input.GetAxis("Horizontal")).normalized;
        } else {
            this.Direction = _navMeshAgent.velocity.normalized;
        }
    }

    public float GetSpeedPercent () {
        if (IsControlledByArrows) {
            return Mathf.Max(Mathf.Abs(Input.GetAxis("Horizontal")),
                             Mathf.Abs(Input.GetAxis("Vertical")));
        } else {
            return _navMeshAgent.velocity.magnitude / Speed;
        }
    }

    public void UpdateTapDetector () {
        _tapped = false;

        for (int i=0; i < Input.touchCount; i++) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                _tap = Input.GetTouch(i);
                _tapped = true;
            }
        }
    }

    public Vector2 GetInteractionPosition () {
        if (Input.GetMouseButtonDown(0)) {
            return Input.mousePosition;
        }

        if (_tapped) {
            return _tap.position;
        }

        return new Vector2(Mathf.NegativeInfinity, Mathf.NegativeInfinity);
    }

    public Vector3 GetInteractionDestination () {
        Vector3 destination = new Vector3(Mathf.NegativeInfinity,
                                          Mathf.NegativeInfinity,
                                          Mathf.NegativeInfinity);;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(GetInteractionPosition());
        _floors = GameObject.FindGameObjectsWithTag(Tag.Floor);

        foreach (GameObject floor in _floors) {
            if (floor.GetComponent<Collider>().Raycast(ray, out hit, 200)) {
                if (hit.point.y > destination.y) {
                    destination = hit.point;
                }
            }
        }

        return destination;
    }

    private bool Interacted () {
        return _tapped || Input.GetMouseButtonDown(0);
    }

    private bool _IsInteractionBlocked () {
        return (_eventSystem.IsPointerOverGameObject() ||
                (_tapped && _eventSystem.IsPointerOverGameObject(_tap.fingerId)));
    }
}
