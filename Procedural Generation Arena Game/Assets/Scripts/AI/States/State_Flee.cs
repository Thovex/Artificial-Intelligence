using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Flee : State {
    private FSM fsm;
    private Mesh theWorld;

    public override void EnterState(FSM _fsm) {
        Debug.Log("Entered: " + this.GetType());
        fsm = _fsm;

        theWorld = GameObject.Find("MapGenerator").GetComponent<MeshFilter>().mesh;

        Vector3 randomVertice = theWorld.vertices[Random.Range(0, theWorld.vertices.Length)];

        fsm.agent.destination = randomVertice;

        Invoke("ExitState", 2.5f);
    }

    public override void UpdateState() {

    }

    public override void ExitState() {
        fsm.agent.destination = transform.position;

        Debug.Log("Exitting: " + this.GetType());
        fsm.SwitchState("Idle");

    }
}
