using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blocker : MonoBehaviour {
    public bool IsUnblocked;
    public List<Collectable> Requirements;
    public GameObject UnblockedPosition;
    public float Speed = 5;
    public GameObject Obstacle;

    private NavMeshObstacle _navmeshObstacle;
    private float _timeOnState = 0;
    private Vector3 _cachedPosition;
    private float _totalTime;

    void Start () {
        _navmeshObstacle = Obstacle.GetComponent<NavMeshObstacle>();
    }

    void Update () {
        _timeOnState += Time.deltaTime;
        if (IsUnblocked) {
            Obstacle.transform.position =
                Vector3.Lerp(_cachedPosition, UnblockedPosition.transform.position,
                             _timeOnState/_totalTime);
        }
    }

    public void Unblock () {
        IsUnblocked = true;
        _cachedPosition = Obstacle.transform.position;
        _timeOnState = 0;
        _navmeshObstacle.enabled = false;
        _totalTime = (UnblockedPosition.transform.position - _cachedPosition)
            .magnitude / Speed;
    }

    void OnTriggerEnter(Collider player) {
        Collector collector = player.GetComponent<Collector>();

        for (int i=0; i<Requirements.Count; i++) {
            if (false == collector.Inventory[Requirements[i].Image]) {
                return;
            } else {
                collector.Give(Requirements[i]);
            }
        }

        Unblock();
    }
}
