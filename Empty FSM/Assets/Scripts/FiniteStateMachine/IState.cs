using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState {
    void EnterState(FSM fsm);
    void UpdateState(FSM fsm);
    void ExitState(FSM fsm);
}
