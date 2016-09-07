using UnityEngine;
using System.Collections;

public class CameraLook : MonoBehaviour {
    public GameObject Target;
    
    void Update () {
	transform.forward = Target.transform.position - transform.position;
    }
}
