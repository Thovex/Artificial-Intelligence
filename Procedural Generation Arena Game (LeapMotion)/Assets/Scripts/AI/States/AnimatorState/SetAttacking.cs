using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAttacking : StateMachineBehaviour {
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.GetComponent<FSM>().isAttacking = true;
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.GetComponent<FSM>().isAttacking = false;
    }
}
