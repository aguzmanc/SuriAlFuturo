using UnityEngine;
using System.Collections;

public class StaticNVFollower : MonoBehaviour 
{
    public Transform FollowTo;

    NavMeshAgent _agent;

	void Start () 
    {
        _agent = GetComponent<NavMeshAgent>();
	}
	
	void Update () 
    {
        if(_agent.isOnNavMesh)
            _agent.SetDestination(FollowTo.position);
	}
}
