using UnityEngine;
using System.Collections;

public class UnpressButtonPatch : MonoBehaviour {
    private Animator _animator;

    void Start () {
        _animator = GetComponent<Animator>();
    }

    void OnDisable () {
        _animator.Play("Normal");
    }
    
    void Update () {
	
    }
}
