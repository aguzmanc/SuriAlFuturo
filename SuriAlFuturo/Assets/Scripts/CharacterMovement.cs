using SuriAlFuturo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

public class CharacterMovement : MonoBehaviour
{
    public float Speed = 0;

    public bool IsControlledByPlayer = false;
    public bool IsControlledByArrows = true;
    public float CurrentSpeedPercent;
    public Vector3 Direction;
    public UnityEngine.AI.NavMeshAgent NavMeshAgent;

    [Header("ROLL")]
    public bool canRoll = false;
    public float RollSpeed = 20;
    public float rollTime;
    

    [Header("DAMAGE")]
    public bool isDamaged = false;
    public bool isBlinking = false;
    public float damageTime = 0.2f;
    public float blinkTime = 3f;



    [HideInInspector] public bool isRolling;


    bool _isInteracting;
    GameObject _gizmos;
    Animator _gizmosAnimator;
    Animator _animator;
    GameController _controller;
    GameObject[] _floors;
    EventSystem _eventSystem;

    Touch _tap;
    bool _tapped;


    public static event System.Action onRoll;


    float GetSpeed()
    {
        if (isDamaged)
            return 0;

        return isRolling ? RollSpeed : Speed;
    }


    public void Warp(Vector3 pos)
    {
        FixNavMeshPosition(pos);
    }


    // Temporal disable Nav Mesh while scene change is made
    public void TimeTravel()
    {
        StartCoroutine(TemporalDisableNavMesh());
    }




    Coroutine _rollCoroutine;

    public void Roll()
    {
        if (!canRoll)
            return;

        if(isBlinking)
            return;


        if(GameController.ResetStamina())
        { 
            if (_rollCoroutine == null) {
                _rollCoroutine = StartCoroutine(_Roll());
            }
        }
    }


    IEnumerator _Roll()
    {
        onRoll ? . Invoke();

        isRolling = true;
        _animator.SetTrigger("Roll");

        yield return new WaitForSeconds(rollTime);
        isRolling = false;

        _rollCoroutine = null;
    }


    public void Damage()
    {
        if (isDamaged || isBlinking)
            return;


        if (_rollCoroutine != null)
        {
            StopCoroutine(_rollCoroutine);
            isRolling = false;
            _rollCoroutine = null;
        }

        GameController.Damage();
        StartCoroutine(_Damage());
    }

    IEnumerator _Damage()
    {
        isDamaged = true;
        isBlinking = true;

        _animator.SetBool("IsBlinking", true);

        yield return new WaitForSeconds(damageTime);
        isDamaged = false;

        yield return new WaitForSeconds(blinkTime - damageTime);
        isBlinking = false;

        _animator.SetBool("IsBlinking", false);
    }




    void Awake()
    {
        if (NavMeshAgent == null)
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }
    }


    void Start()
    {
        _animator = GetComponent<Animator>();
        NavMeshAgent.speed = Speed;
        _isInteracting = false;

        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).
            GetComponent<GameController>();
        _gizmos = _controller.MovementGizmos;
        _gizmosAnimator = _gizmos.GetComponent<Animator>();
        GetComponent<UIController>();
        _eventSystem = GameObject.FindGameObjectWithTag(Tag.EventSystem).
            GetComponent<EventSystem>();
    }


    public bool isOnNavMesh;


    void Update()
    {
        isOnNavMesh = NavMeshAgent.isOnNavMesh; 

        if(GameController.gameFinished){
            if(_animator)
                _animator.SetBool("IsWalking", false);

            return; // stop playing
        }

        if (IsControlledByPlayer)
        {
            IsControlledByArrows = Mathf.Abs(Input.GetAxis("Horizontal")) > 0 ||
                Mathf.Abs(Input.GetAxis("Vertical")) > 0;

            UpdateTapDetector();
            UpdateDirection();
            UpdateMovement();

            _controller.ControlledCharacter = this.gameObject;

        }
        else
        {
            if (NavMeshAgent.isActiveAndEnabled)
            {
                NavMeshAgent.isStopped = true;
                NavMeshAgent.velocity = Vector3.zero;
            }
            CurrentSpeedPercent = 0;
        }

        UpdateAnimatorParameters();
    }



    void OnEnable()
    {
        FixNavMeshPosition(transform.position);
    }


    void UpdateAnimatorParameters()
    {
        CurrentSpeedPercent = GetSpeedPercent();

        if (CurrentSpeedPercent != 0)
        {
            if (Direction.x != 0 && Direction.z != 0)
            {
                transform.forward = new Vector3(Direction.x, 0, Direction.z);
            }
        }

        if (_animator != null)
        {
            _animator.SetBool("IsWalking", CurrentSpeedPercent != 0);
        }
    }



    void FixNavMeshPosition(Vector3 pos)
    {
        NavMeshHit hit;

        // evita que el agente vuelva a estar anclado al Nav Mesh (sino isOnNavMesh dara falso)
        if (NavMesh.SamplePosition(pos, out hit, 2f, NavMeshAgent.areaMask))
        {
            NavMeshAgent.Warp(hit.position);
        }
    }



    void UpdateMovement()
    {
        if (IsControlledByArrows)
        { // keyboard control!

            if (NavMeshAgent.isActiveAndEnabled && NavMeshAgent.isOnNavMesh)
            {

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Roll();
                }

                NavMeshAgent.isStopped = true;
                NavMeshAgent.Move(this.Direction * Time.deltaTime * GetSpeed() *
                                    Mathf.Max(Mathf.Abs(Input.GetAxis("Vertical")),
                                             Mathf.Abs(Input.GetAxis("Horizontal"))));
            }
        }
        /* ***** DISABLING CLICK MOVEMENT TYPE
        else if (false == _IsInteractionBlocked())
        {
            if (StartInteracting())
            {
                _gizmosAnimator.SetTrigger("Born");
            }

            if (_isInteracting)
            {
                Vector3 destination;

                if (GetInteractionDestination(out destination))
                {
                    _gizmos.transform.position = destination;
                    _gizmos.SetActive(true);
                    if (NavMeshAgent.isActiveAndEnabled && NavMeshAgent.isOnNavMesh)
                    {
                        NavMeshAgent.isStopped = false;
                        NavMeshAgent.SetDestination(destination);
                    }
                }
            }

            if (StopInteracting())
            {
                _gizmosAnimator.SetTrigger("Die");
            }
        }
        else if (StopInteracting())
        { // force to stop interact
            _gizmosAnimator.SetTrigger("Die");
        }
        */
    }


    void UpdateDirection()
    {

        if (IsControlledByPlayer)
        {
            this.Direction = new Vector3(0, 0, 0);
        }

        if (IsControlledByArrows)
        {
            this.Direction =
                (FollowCamera.Instance.Forward * Input.GetAxis("Vertical") +
                 FollowCamera.Instance.Right * Input.GetAxis("Horizontal")).normalized;
        }
        else
        {
            this.Direction = NavMeshAgent.velocity.normalized;
        }
    }


    float GetSpeedPercent()
    {
        if (IsControlledByArrows)
        {
            return Mathf.Max(Mathf.Abs(Input.GetAxis("Horizontal")),
                             Mathf.Abs(Input.GetAxis("Vertical")));
        }
        else
        {
            return NavMeshAgent.velocity.magnitude / GetSpeed();
        }
    }


    void UpdateTapDetector()
    {
        _tapped = (Input.touchCount > 0);
        if (_tapped)
        {
            _tap = Input.GetTouch(0);
        }
    }



    bool GetInteractionPosition(out Vector2 pos)
    {
        pos = new Vector2();

        if (Input.GetMouseButton(0))
        {
            pos = Input.mousePosition;
            return true;
        }

        if (_tapped)
        {
            pos = _tap.position;
            return true;
        }

        return false;
    }



    bool GetInteractionDestination(out Vector3 destination)
    {
        destination = new Vector3(Mathf.NegativeInfinity,
                                  Mathf.NegativeInfinity,
                                  Mathf.NegativeInfinity);

        Vector2 pos2d;

        if (GetInteractionPosition(out pos2d))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(pos2d);
            _floors = GameObject.FindGameObjectsWithTag(Tag.Floor);

            bool found = false;
            foreach (GameObject floor in _floors)
            {
                if (floor.GetComponent<Collider>().Raycast(ray, out hit, 200))
                {
                    if (hit.point.y > destination.y)
                    {
                        destination = hit.point;
                        found = true;
                    }
                }
            }

            if (!found) return false;


            Debug.DrawRay(destination, Vector3.up * 5, Color.red);


            NavMeshHit navMeshHit;
            if ((NavMesh.SamplePosition(destination,
                                                out navMeshHit,
                                                7f,
                                                NavMeshAgent.areaMask)))
            {
                Debug.Log("Sampled: " + found);
                destination = navMeshHit.position;
                Debug.DrawRay(destination, Vector3.up * 5, Color.blue);
                return true; // cuando esta dentro del navmesh
            }
            else
            {

                Debug.Log("NOT FOUND");
                return false;
            }
        }

        return false;
    }



    bool StartInteracting()
    {
        if (_isInteracting)
        {
            return false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _isInteracting = true;
            return true;
        }

        if (_tapped)
        {
            if (_tap.phase == TouchPhase.Began)
            {
                _isInteracting = true;
                return true;
            }
        }

        return false;
    }



    bool StopInteracting()
    {
        if (!_isInteracting)
        {
            return false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isInteracting = false;
            return true;
        }

        if (_tapped)
        {
            if (_tap.phase == TouchPhase.Ended)
            {
                _isInteracting = false;
                return true;
            }
        }

        return false;
    }



    bool _IsInteractionBlocked()
    {
        return (_eventSystem.IsPointerOverGameObject() ||
                (_tapped && _eventSystem.IsPointerOverGameObject(_tap.fingerId)));
    }



    IEnumerator TemporalDisableNavMesh()
    {
        NavMeshAgent.enabled = false;
        yield return new WaitForSeconds(2f);
        NavMeshAgent.enabled = true;

        FixNavMeshPosition(transform.position);
    }
}
