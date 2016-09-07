using UnityEngine;
using System.Collections;

public class HoleDetector : MonoBehaviour {
    public CameraFollow TheCamera;

    void OnTriggerEnter (Collider c) {
        if(c.name == "Hole") // patch!... delete later
            TheCamera.OnHoleEnter();
    }

    void OnTriggerExit (Collider c) {
        if(c.name == "Hole") // patch!... delete later
            TheCamera.OnHoleExit();
    }
}
