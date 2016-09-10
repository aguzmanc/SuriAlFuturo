using UnityEngine;
using System.Collections;

public class ShipLoadReceiver : MonoBehaviour 
{
    public Ship Ship;


	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}


    void OnTriggerEnter(Collider coll)
    {
        Ship.StartBlinking();
    }


    void OnTriggerExit(Collider coll)
    {
        Ship.StopBlinking();
    }
}
