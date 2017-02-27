using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour {

    public State currentState;
    public Dictionary<string, State> fsmStates;

    private State[] states;

    private void Start () {

        fsmStates = new Dictionary<string, State>();

        states = GetComponents<State>();

        foreach (State state in states) {
            fsmStates.Add(state.stateName, state);
        }

        State idle = null;
        if (fsmStates.TryGetValue("Idle", out idle)) {
            currentState = idle;
            currentState.EnterState(this);
        }

    }

    private void Update () {
        currentState.UpdateState(this);
	}

    public void SwitchState(string stateName) {
        State temp = null;
        if (fsmStates.TryGetValue(stateName, out temp)) {
            currentState = temp;
            currentState.EnterState(this);
        }
    }

}
