using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_MoveToOpponent : State {
    private FSM fsm;


    public override void EnterState(FSM _fsm) {
        Debug.Log("Entered: " + this.GetType());
        fsm = _fsm;

    }

    public override void UpdateState() {
        if (fsm.currentTarget != null) {
            if (!fsm.IsInMeleeRangeOf(fsm.currentTarget.transform)) {
                fsm.MoveTowards(fsm.currentTarget.transform);
            }
            else {
                fsm.agent.SetDestination(transform.position);
                ExitState();
            }
        }
    }


    public override void ExitState() {
        Debug.Log("Exitting: " + this.GetType());
        fsm.SwitchState("Idle");
    }
}
