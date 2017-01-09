using UnityEngine;
using System.Collections;

public class DroneInInventory : MonoBehaviour 
{
    CollectionSystem _collection;

    public GameObject DroneBody;
    public GameObject DroneShadow;

	void Start () 
    {
        _collection = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).GetComponent<CollectionSystem>();
	}
	
	void Update () 
    {
        if(_collection.HasItem(Collectable.Tag.dron_completo)){
            DroneBody.SetActive(true);
            DroneShadow.SetActive(true);
        } else {
            DroneBody.SetActive(false);
            DroneShadow.SetActive(false);
        }
	
	}
}
