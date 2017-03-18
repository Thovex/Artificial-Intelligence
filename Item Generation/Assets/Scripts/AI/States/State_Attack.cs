using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Attack : State {
    private FSM fsm;

    public override void EnterState(FSM _fsm) {
        Debug.Log("Entered: " + this.GetType());
        fsm = _fsm;

        fsm.anim.SetTrigger("Attack");

        Invoke("ExitState", 2f);

    }

    public override void UpdateState() {
        if (fsm.currentTarget != null) {
            fsm.RotateTowards(fsm.currentTarget.transform);
        }
    }

    public override void ExitState() {
        Debug.Log("Exitting: " + this.GetType());
        fsm.SwitchState("Idle");
    }
}
