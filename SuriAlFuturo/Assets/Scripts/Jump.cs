using UnityEngine;
using System.Collections;

public class Jump : StateMachineBehaviour {
    public ParticleSystem Exit;
    public ParticleSystem Enter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Exit.Stop();
        Exit.Play();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Enter.Stop();
        Enter.Play();
    }
}
