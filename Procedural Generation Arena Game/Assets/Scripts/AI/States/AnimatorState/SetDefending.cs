using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDefending : StateMachineBehaviour {
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.GetComponent<FSM>().isDefending = true;
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.GetComponent<FSM>().isDefending = false;
    }
}
