using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_WalkToFlank : State {

    private GameObject player;
    private Vector3 destination;

    public bool flankLeft = true;

    public override void EnterState(FSM fsm) {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void ExitState(FSM fsm) {
        fsm.SwitchState("Idle");
    }

    public override void UpdateState(FSM fsm) {

        Debug.DrawLine(transform.position, destination);
        Debug.DrawLine(transform.position, player.transform.position, Color.red);

        destination = player.transform.position;
        if (flankLeft) {
            destination += Vector3.left;
        }
        else {
            destination += Vector3.right;
        }

        if (Vector3.Distance(transform.position, player.transform.position) > player.GetComponent<PlayerVision>().playerVision) {
            transform.position = Vector3.MoveTowards(transform.position, destination, 1 * Time.deltaTime);
        } else {
            if (flankLeft) {
                transform.position += Vector3.left * Time.deltaTime;
            } else {
                transform.position += Vector3.right * Time.deltaTime;
            }
        }

        if (Vector3.Distance(transform.position, destination) == 0f) {
            ExitState(fsm);
        }
    }
}
