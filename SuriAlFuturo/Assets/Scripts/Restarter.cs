using UnityEngine;
using System.Collections;

public class Restarter : MonoBehaviour {
	private Talkable _talkable;

	// Use this for initialization
	void Start () {
		_talkable = GetComponent<Talkable> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_talkable.IsDone) {
			Application.LoadLevel ("MainLevel");
		}
	}
}
