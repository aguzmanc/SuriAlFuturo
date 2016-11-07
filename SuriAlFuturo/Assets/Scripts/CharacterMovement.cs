using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
    public float Speed = 0;
    public bool IsControlledByPlayer = false;
    public bool IsControlledByArrows = true;
    public float CurrentSpeedPercent;
    public Vector3 Direction;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private GameController _controller;
    private GameObject[] _floors;

    void Start () {
        _navMeshAgent = GetComponent<NavMeshAgent> ();
        _animator = GetComponent<Animator>();
        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).
            GetComponent<GameController>();
        _navMeshAgent.speed = Speed;
    }

    void Update ()
    {
        if (IsControlledByPlayer) {

            IsControlledByArrows = Mathf.Abs(Input.GetAxis("Horizontal")) > 0 ||
                Mathf.Abs(Input.GetAxis("Vertical")) > 0;

            UpdateDirection();
            UpdateMovement();
            UpdateAnimatorParameters();

            _controller.ControlledCharacter = this.gameObject;

        } else {
            _navMeshAgent.Stop();
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
        } else if (Input.GetMouseButton(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            _floors = GameObject.FindGameObjectsWithTag(SuriAlFuturo.Tag.Floor);

            foreach (GameObject floor in _floors) {
                if (floor.GetComponent<Collider>().Raycast(ray, out hit, 200)) {
                    _navMeshAgent.Resume();
                    _navMeshAgent.SetDestination(hit.point);
                }
            }

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

}
