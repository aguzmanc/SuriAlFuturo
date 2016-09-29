using UnityEngine;
using System.Collections;

public class SuriRandomIdle : StateMachineBehaviour {
    override public void OnStateMachineEnter(Animator animator, int StateMachinePathHash) {
        animator.SetInteger("IdleIndex", Random.Range(0, 5));
    }
}
