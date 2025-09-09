using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using SuriAlFuturo;

public class CharacterMovement : MonoBehaviour
{
    public float Speed = 0;
    public bool IsControlledByPlayer = false;
    public bool IsControlledByArrows = true;
    public float CurrentSpeedPercent;
    public Vector3 Direction;

    public bool _isInteracting;
    private GameObject _gizmos;
    private Animator _gizmosAnimator;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private GameController _controller;
    private GameObject[] _floors;
    private EventSystem _eventSystem;

    private Touch _tap;
    private bool _tapped;

    void Start () {
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
        _animator = GetComponent<Animator>();
        _navMeshAgent.speed = Speed;
        _isInteracting = false;

        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).
            GetComponent<GameController>();
        _gizmos = _controller.MovementGizmos;
        _gizmosAnimator = _gizmos.GetComponent<Animator>();
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

            _controller.ControlledCharacter = this.gameObject;

        } else {
            if(_navMeshAgent.isActiveAndEnabled){
                _navMeshAgent.Stop();
                _navMeshAgent.velocity = Vector3.zero;
            }
            CurrentSpeedPercent = 0;
        }

        UpdateAnimatorParameters();
    }

    public void UpdateAnimatorParameters () {
        CurrentSpeedPercent = GetSpeedPercent();

        if (CurrentSpeedPercent != 0) {
            if(Direction.x != 0 && Direction.z != 0) {
                transform.forward = new Vector3(Direction.x, 0, Direction.z);
            }
        }

        if (_animator != null) {
            _animator.SetBool("IsWalking", CurrentSpeedPercent != 0);
        }
    }


    public void UpdateMovement ()
    {
        if (IsControlledByArrows) { // keyboard control!
            if(_navMeshAgent.isActiveAndEnabled && _navMeshAgent.isOnNavMesh){
                _navMeshAgent.Stop();
                _navMeshAgent.Move(this.Direction * Time.deltaTime * Speed *
                                    Mathf.Max( Mathf.Abs(Input.GetAxis("Vertical")),
                                             Mathf.Abs(Input.GetAxis("Horizontal")) ));
            }
        } else if (false == _IsInteractionBlocked())
        {
            if(StartInteracting()){
                _gizmosAnimator.SetTrigger("Born");
            }

            if(_isInteracting) {
                Vector3 destination;

                if(GetInteractionDestination(out destination)) {
                    _gizmos.transform.position = destination;
                    _gizmos.SetActive(true);
                    if(_navMeshAgent.isActiveAndEnabled && _navMeshAgent.isOnNavMesh){
                        _navMeshAgent.Resume();
                        _navMeshAgent.SetDestination(destination);
                    }
                }
            }

            if(StopInteracting()) {
                _gizmosAnimator.SetTrigger("Die");
            }
        } else if (StopInteracting()) { // force to stop interact
            _gizmosAnimator.SetTrigger("Die");
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



    public void UpdateTapDetector ()
    {
        _tapped = (Input.touchCount > 0);
        if(_tapped){
            _tap = Input.GetTouch(0);
        }
    }



    public bool GetInteractionPosition (out Vector2 pos)
    {
        pos = new Vector2();

        if (Input.GetMouseButton(0)) {
            pos = Input.mousePosition;
            return true;
        }

        if (_tapped) {
            pos = _tap.position;
            return true;
        }

        return false;
    }



    public bool GetInteractionDestination (out Vector3 destination)
    {
        destination = new Vector3(Mathf.NegativeInfinity,
                                  Mathf.NegativeInfinity,
                                  Mathf.NegativeInfinity);

        Vector2 pos2d;

        if(GetInteractionPosition(out pos2d)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(pos2d);
            _floors = GameObject.FindGameObjectsWithTag(Tag.Floor);

            bool found = false;
            foreach (GameObject floor in _floors) {
                if (floor.GetComponent<Collider>().Raycast(ray, out hit, 200)) {
                    if (hit.point.y > destination.y) {
                        destination = hit.point;
                        found = true;
                    }
                }
            }

            return found;
        }

        return false;
    }



    // Temporal disable Nav Mesh while scene change is made
    public void TimeTravel()
    {
        StartCoroutine(TemporalDisableNavMesh());
    }



    private bool StartInteracting ()
    {
        if(_isInteracting) {
            return false;
        }

        if(Input.GetMouseButtonDown(0))
        {
            _isInteracting = true;
            return true;
        }

        if(_tapped) {
            if(_tap.phase == TouchPhase.Began) {
                _isInteracting = true;
                return true;
            }
        }

        return false;
    }



    private bool StopInteracting()
    {
        if(!_isInteracting) {
            return false;
        }

        if(Input.GetMouseButtonUp(0))
        {
            _isInteracting = false;
            return true;
        }

        if(_tapped) {
            if(_tap.phase == TouchPhase.Ended) {
                _isInteracting = false;
                return true;
            }
        }

        return false;
    }



    private bool _IsInteractionBlocked ()
    {
        return (_eventSystem.IsPointerOverGameObject() ||
                (_tapped && _eventSystem.IsPointerOverGameObject(_tap.fingerId)));
    }



    private IEnumerator TemporalDisableNavMesh()
    {
        _navMeshAgent.enabled = false;
        yield return new WaitForSeconds(2f);
        _navMeshAgent.enabled = true;
    }
}
