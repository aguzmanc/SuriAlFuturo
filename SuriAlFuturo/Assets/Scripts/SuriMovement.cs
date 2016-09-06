using UnityEngine;
using System.Collections;

public class SuriMovement : MonoBehaviour {
    public float Speed = 0;
    public GameObject ActiveCamera;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private CameraFollow _follow;

    void Start () {
        _navMeshAgent = GetComponent<NavMeshAgent> ();
        _animator = GetComponent<Animator>();
        _follow = ActiveCamera.GetComponent<CameraFollow>();
    }
    
    void Update () {
        bool isWalking = Input.GetAxis("Vertical") != 0 ||
            Input.GetAxis("Horizontal") != 0;
        Vector3 Direction = (_follow.GetForward() * Input.GetAxis("Vertical") +
                             ActiveCamera.transform.right *
                             Input.GetAxis("Horizontal")).normalized;

        _navMeshAgent.Move(Direction * Time.deltaTime * Speed *
                           Mathf.Max(Mathf.Abs(Input.GetAxis("Vertical")),
                                     Mathf.Abs(Input.GetAxis("Horizontal"))));

        _animator.SetBool("IsWalking", isWalking);
        if (isWalking) {
            transform.forward = new Vector3(Direction.x, 0, Direction.z);
        }
    }
}
