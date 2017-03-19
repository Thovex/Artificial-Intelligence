using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FindState: State {
    private FSM fsm;

    private float timeInFind;

    private float timeTillRandomAction;

    public override void EnterState(FSM _fsm) {
        Debug.Log("Entered: " + this.GetType());
        fsm = _fsm;

        timeTillRandomAction = Random.Range(1f, 3f);
        timeInFind = 0f;

        if (fsm.opponents.Count == 0) {
            Debug.Log("Exitting: " + this.GetType());
            fsm.SwitchState("FindOpponent");
            
        }

        if (fsm.currentTarget != null) {
            if (!fsm.IsInMeleeRangeOf(fsm.currentTarget.transform)) {
                fsm.SwitchState("MoveToOpponent");
            }
        }
    }

    public override void UpdateState() {
        timeInFind += Time.deltaTime;

        if (fsm.currentTarget != null) {
            State attack = null;
            if (fsm.fsmStates.TryGetValue("Attack", out attack)) {

                if (fsm.currentTarget.GetComponent<FSM>().currentState.stateName == attack.stateName) {
                    if (Random.value < .5f) {
                        fsm.SwitchState("Defend");
                    } else {
                        if (Random.value < .3f) {
                            fsm.SwitchState("Flee");
                        } else {
                            fsm.SwitchState("Attack");
                        }
                    }
                }
            }

            if (timeInFind > timeTillRandomAction) {
                if (Random.value < .3f) {
                    fsm.SwitchState("Flee");
                }
                else {
                    fsm.SwitchState("Attack");
                }
            }
        }
    }

    public override void ExitState() {
        Debug.Log("Exitting: " + this.GetType());
        fsm.SwitchState("Idle");
    }
}
