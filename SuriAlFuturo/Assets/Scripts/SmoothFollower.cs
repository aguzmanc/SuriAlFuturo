using UnityEngine;
using System.Collections;

public class SmoothFollower : MonoBehaviour 
{
    public Transform Target;
    public float Smoothness = 0.1f;

	
	void Start () {
	
	}
	
	
	void Update () 
    {
        Vector3 to = new Vector3(Target.position.x, transform.position.y, Target.position.z);
        transform.position = Vector3.Lerp(transform.position, to, Smoothness * Time.deltaTime);
	}
}
