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
        _agent.SetDestination(FollowTo.position);
	}
}
