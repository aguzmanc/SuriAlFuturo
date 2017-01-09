using UnityEngine;
using System.Collections;

public class DroneShadow : MonoBehaviour 
{
    public Transform Drone;

	void Start () 
    {
	    
	}
	
	void Update () 
    {
        Vector3 p = Drone.position;
        transform.position = new Vector3(p.x, transform.position.y, p.z);
    
        Vector3 r = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(new Vector3(r.x, Drone.rotation.eulerAngles.y, r.z));
	}
}
