using SuriAlFuturo;
using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {
    public Collider TerrainDetector;
    public ParticleSystem Particles;
    public bool Colliding = false;

    void Start () {
        Particles = GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter (Collider c) {
        Colliding = true;
    }
}
