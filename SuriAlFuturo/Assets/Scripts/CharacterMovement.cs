using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour 
{
    public float Speed = 0;
    public bool IsControlledByPlayer = false;
    public float CurrentSpeedPercent;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    void Start () {
        _navMeshAgent = GetComponent<NavMeshAgent> ();
        _animator = GetComponent<Animator>();
    }
    
    void Update () 
    {
        float h = IsControlledByPlayer ? Input.GetAxis("Horizontal") : 0;
        float v = IsControlledByPlayer ? Input.GetAxis("Vertical")   : 0;

        // direction relative to camera view
        FollowCamera cam = FollowCamera.Instance;
        Vector3 dir = (cam.Forward * v +
                       cam.Right * h).normalized;

        _navMeshAgent.Move(dir * Time.deltaTime * Speed *
                           Mathf.Max(Mathf.Abs(v),
                                     Mathf.Abs(h)));

        // needed on ShipAnimator.cs
        CurrentSpeedPercent = Mathf.Max(Mathf.Abs(v), Mathf.Abs(h)); 

        if (CurrentSpeedPercent != 0)
            transform.forward = new Vector3(dir.x, 0, dir.z);

        if(_animator != null) 
            _animator.SetBool("IsWalking", CurrentSpeedPercent != 0);
    }

}
