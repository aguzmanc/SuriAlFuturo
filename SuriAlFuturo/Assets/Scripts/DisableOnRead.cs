using UnityEngine;
using System.Collections;

public class DisableOnRead : MonoBehaviour {
    private Talkable _talkable;

    void Start () {
        _talkable = GetComponent<Talkable>();
    }
    
    void Update () {
	gameObject.SetActive(!_talkable.WasRead);
    }
}
