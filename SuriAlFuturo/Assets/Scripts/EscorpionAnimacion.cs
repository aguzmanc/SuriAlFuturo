using UnityEngine;

public class EscorpionAnimacion : MonoBehaviour
{
    [SerializeField]
    Animator _animator;

    Rana _rana;

    void Start()
    {
        _rana = GetComponent<Rana>();    
    }


    void Update()
    {
        _animator.SetBool("walking", _rana.Moving);
    }
}
