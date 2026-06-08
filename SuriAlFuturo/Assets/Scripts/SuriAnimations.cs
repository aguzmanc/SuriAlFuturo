using UnityEngine;
using System.Collections;

public class SuriAnimations : MonoBehaviour
{
    public float MinIdleTime = 1;
    public float MaxIdleTime = 4;
    public float ProbabilityOfLookUp = 0.5f;

    private Animator _animator;

    IEnumerator Start()
    {
        _animator = GetComponent<Animator>();

        while (true)
        {
            if (Random.value > ProbabilityOfLookUp)
            {
                _animator.SetTrigger("IddleJumps");
            }
            else
            {
                _animator.SetTrigger("LookUp");
            }
            yield return new WaitForSeconds(Random.Range(MinIdleTime, MaxIdleTime));
        }
    }
}
