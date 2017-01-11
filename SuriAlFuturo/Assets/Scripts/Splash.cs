using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {
    public Collider TerrainDetector;
    public ParticleSystem Particles;
    public bool Colliding = false;

    private int _frameCounter = 0;

    void Start () {
        Particles = GetComponent<ParticleSystem>();
    }

    void Update () {
        if (_frameCounter < 2) {
            _frameCounter++;
        } else if (_frameCounter == 2) {
            _frameCounter++;
            TerrainDetector.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter (Collider c) {
        Colliding = true;
    }
}
