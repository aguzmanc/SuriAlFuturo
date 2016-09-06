using UnityEngine;
using System.Collections;

public class HoleDetector : MonoBehaviour {
    public CameraFollow TheCamera;

    void OnTriggerEnter (Collider c) {
        TheCamera.OnHoleEnter();
    }

    void OnTriggerExit (Collider c) {
        TheCamera.OnHoleExit();
    }
}
