using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {
    public Splash Exit;
    public Splash Enter;

    public float MinWaitingTime;
    public float MaxWaitingTime;

    private Animator _mecanim;
    private Jump _mecanimBehaviour;

    void Start () {
        _mecanim = GetComponent<Animator>();
        _mecanimBehaviour = _mecanim.GetBehaviour<Jump>();
        _mecanimBehaviour.Exit = Exit.Particles;
        _mecanimBehaviour.Enter = Enter.Particles;
        StartCoroutine(EventuallyJump());
    }

    void Update () {
        if (!SpawnsOnWater()) {
            Destroy(this.gameObject);
        }
    }

    IEnumerator EventuallyDie () {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

    IEnumerator EventuallyJump () {
        yield return new WaitForSeconds(0.5f);
        _mecanim.SetTrigger("Jump");
        StartCoroutine(EventuallyDie());
    }

    public bool SpawnsOnWater () {
        return !Exit.Colliding && !Enter.Colliding;
    }

}
