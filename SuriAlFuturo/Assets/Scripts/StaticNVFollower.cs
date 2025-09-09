using UnityEngine;
using System.Collections;

public class StaticNVFollower : MonoBehaviour 
{
    public Transform FollowTo;

    UnityEngine.AI.NavMeshAgent _agent;

	void Start () 
    {
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	void Update () 
    {
        if(_agent.isOnNavMesh)
            _agent.SetDestination(FollowTo.position);
	}
}
