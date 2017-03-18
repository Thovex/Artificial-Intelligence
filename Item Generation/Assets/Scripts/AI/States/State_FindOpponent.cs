using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FindOpponent : State {
    private FSM fsm;

    public override void EnterState(FSM _fsm) {
        Debug.Log("Entered: " + this.GetType());
        fsm = _fsm;

        GameObject[] opps = GameObject.FindGameObjectsWithTag("ArtificialIntelligence");

        foreach (GameObject g in opps) {
            if (g != this.gameObject) {
                fsm.opponents.Add(g);
            }
        }

        if (fsm.opponents.Count > 0) {
            fsm.currentTarget = fsm.opponents[Random.Range(0, fsm.opponents.Count)];
        }

        ExitState();
    }

    public override void UpdateState() {

    }

    public override void ExitState() {
        Debug.Log("Exitting: " + this.GetType());
        fsm.SwitchState("Idle");
    }
}
