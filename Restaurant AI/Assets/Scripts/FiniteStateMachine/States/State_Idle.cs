using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class State_Idle : State {

    private FSM fsm;

    private Vector3 origin;
    private NavMeshAgent agent;


    public override void EnterState(FSM _fsm) {
        Debug.Log("Entered: " + this.GetType());
        agent = GetComponent<NavMeshAgent>();

        fsm = _fsm;
        StartCoroutine(Delay(2f));
    }

    public override void ExitState() {
        Debug.Log("Exitting: " + this.GetType());
        fsm.SwitchState("BaseState");
    }

    public override void UpdateState() {

    }

    private IEnumerator Delay(float delay) {
        yield return new WaitForSeconds(delay);
        ExitState();
    }
}
