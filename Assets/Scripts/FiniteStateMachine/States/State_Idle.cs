using UnityEngine;
using System.Collections;

public class State_Idle : State {

    public override void EnterState(FSM fsm) {
        StartCoroutine(Delay(fsm, 2f));
    }

    public override void ExitState(FSM fsm) {
        fsm.SwitchState("Walk");
    }

    public override void UpdateState(FSM fsm) {

    }

    private IEnumerator Delay(FSM fsm, float delay) {
        yield return new WaitForSeconds(delay);
        ExitState(fsm);
    }
}
