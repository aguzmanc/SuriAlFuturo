using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishSpawner : MonoBehaviour {
    public GameObject FishPrototype;
    public float MinFishlessTime;
    public float MaxFishlessTime;
    public List<GameObject> SpawningPoints;

    private int _attempts = 0;

    void Start () {
        Spawn();
        StartCoroutine(SpawningRoutine());
    }

    IEnumerator SpawningRoutine () {
        yield return new WaitForSeconds(Random.Range(MinFishlessTime, MaxFishlessTime));
        Spawn();
        StartCoroutine(SpawningRoutine());
    }

    public void Spawn () {
        Fish fish = Instantiate(FishPrototype).GetComponent<Fish>();
        fish.transform.position = SpawningPoints[Random.Range(0, SpawningPoints.Count)]
            .transform.position;
        fish.transform.Rotate(0, Random.Range(0, 360), 0);

        if (!fish.SpawnsOnWater()) {
            _attempts++;
            Destroy(fish.gameObject);
            if (_attempts >= 5) {
                return;
            }
            Spawn();
        } else {
            _attempts = 0;
        }
    }
}
