using UnityEngine;
using System.Collections;

public class SuriAnimations : MonoBehaviour {
    public float MinIdleTime = 1;
    public float MaxIdleTime = 4;
    public float ProbabilityOfLookUp = 0.5f;

    private Animator _animator;
    
    void Start () {
        _animator = GetComponent<Animator>();
        StartCoroutine("RandomTrigger");
    }
    
    IEnumerator RandomTrigger () {
        if (Random.value > ProbabilityOfLookUp) {
            _animator.SetTrigger("IddleJumps");            
        } else {
            _animator.SetTrigger("LookUp");
        }
        yield return new WaitForSeconds(Random.Range(MinIdleTime, MaxIdleTime));
        StartCoroutine("RandomTrigger");
    }
}
