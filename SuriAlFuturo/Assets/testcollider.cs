using UnityEngine;
using System.Collections;

public class testcollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag != "Player")
            return;

        UnityEngine.SceneManagement.SceneManager.UnloadScene("Present");
    }
}
