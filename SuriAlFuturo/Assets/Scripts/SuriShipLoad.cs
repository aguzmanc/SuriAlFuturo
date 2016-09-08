using UnityEngine;
using System.Collections;

public class SuriShipLoad : MonoBehaviour 
{
    public bool HasShipInRange;


    GameObject _shipInRange;


	void Start () 
    {
                
	}
	
	void Update () {
	
	}


    void OnTriggerEnter(Collider coll)
    {
        HasShipInRange = true;
    }
}
