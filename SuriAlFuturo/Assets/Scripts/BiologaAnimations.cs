using UnityEngine;
using System.Collections;

public class BiologaAnimations : MonoBehaviour {
    public float MinIdleTime = 1;
    public float MaxIdleTime = 4;

    private Animator _animator;
    
    void Start () {
        _animator = GetComponent<Animator>();
        StartCoroutine("RandomTrigger");
    }
    
    IEnumerator RandomTrigger () {
        _animator.SetTrigger("RandomAction");            
    
        yield return new WaitForSeconds(Random.Range(MinIdleTime, MaxIdleTime));
        StartCoroutine("RandomTrigger");
    }
}
