using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour, IState {
    public string stateName;

    public abstract void EnterState(FSM fsm);
    public abstract void ExitState();
    public abstract void UpdateState();
}
