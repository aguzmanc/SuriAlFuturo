using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChapuDetector : MonoBehaviour 
{
    public List<Transform> FollowPoints;
    public float WalkingThreshold = 2f;

    public Transform _currentFollowPoint;
    public UnityEngine.AI.NavMeshAgent _chapuAgent;
    public Animator _chapuAnimator;

	void Start () 
    {
        if(FollowPoints == null)
            throw new UnityException("Follow Points in ChapuDetector must be set");

        GameObject chapu = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.Chapu);
        if(chapu == null)
            throw new UnityException("Chapu prefab must be inside the scene");
        
        _chapuAgent = chapu.GetComponent<UnityEngine.AI.NavMeshAgent>();
        _chapuAnimator = chapu.GetComponentInChildren<Animator>();
	}
	
	void Update () 
    {
        if(_chapuAgent != null){
            if(_currentFollowPoint != null) {
                if(_chapuAgent.isOnNavMesh) {
                    _chapuAgent.SetDestination(_currentFollowPoint.position);
                }
            }

            _chapuAnimator.SetFloat("velocity", _chapuAgent.velocity.magnitude);

            //if(_chapuAgent.velocity.magnitude == 0)
              //  _currentFollowPoint = null;
        }
	}


    void OnTriggerEnter(Collider coll)
    {
        _currentFollowPoint = null;
    }


    void OnTriggerExit(Collider coll)
    {
        int n = Random.Range(0, FollowPoints.Count);
        _currentFollowPoint = FollowPoints[n];
    }
}
