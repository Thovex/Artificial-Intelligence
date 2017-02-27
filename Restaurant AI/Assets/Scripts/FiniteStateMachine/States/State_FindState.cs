using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FindState: State {

    [HideInInspector]
    public GameObject[] tables;

    [HideInInspector]
    public TableStatus[] tableStatus;

    public List<int> readyToOrder = new List<int>();
    public List<int> readyToPay = new List<int>();
    public List<int> readyFood = new List<int>();

    private FSM fsm;

    public override void EnterState(FSM _fsm) {
        Debug.Log("Entered: " + this.GetType());
        fsm = _fsm;

        readyToOrder.Clear();
        readyToPay.Clear();
        readyFood.Clear();

        GetTables();
        CheckTables();
    }

    public override void ExitState() {
        if (readyToOrder.Count == 0 && readyToPay.Count == 0 && readyFood.Count == 0) {
            Debug.Log("Nothing to do...");
            Debug.Log("Exitting: " + this.GetType());
            fsm.SwitchState("Idle");
        } else {
            int stateTodo = Random.Range(0, 3);

            switch(stateTodo) {
                case 0:
                    if (readyToOrder.Count > 0) {
                        Debug.Log("Fetch Orders");
                        Debug.Log("Exitting: " + this.GetType());

                        fsm.SwitchState("Order");
                    } else {
                        ExitState();
                    }
                    break;
                case 1:
                    if (readyToPay.Count > 0) {
                        Debug.Log("Fetch Payments");
                        Debug.Log("Exitting: " + this.GetType());

                        fsm.SwitchState("Pay");
                    } else {
                        ExitState();
                    }
                    break;
                case 2:
                    if (readyFood.Count > 0) {
                        Debug.Log("Fetch Food");
                        Debug.Log("Exitting: " + this.GetType());

                        fsm.SwitchState("Food");
                    } else {
                        ExitState();
                    }
                    break;
            }
        }
    }

    public override void UpdateState() {

    }
    
    private void GetTables() {

        tables = GameObject.FindGameObjectsWithTag("Table");
        Debug.Log("Finding Tables... Found: " + tables.Length + " Tables.");

        tableStatus = new TableStatus[tables.Length];

        for (int i = 0; i < tables.Length; i++) {
            tableStatus[i] = tables[i].GetComponent<TableStatus>();
        }
    }

    private void CheckTables() {
        for (int i = 0; i < tables.Length; i++) {
            if (tableStatus[i].readyToOrder) {
                readyToOrder.Add(i);
            }

            if (tableStatus[i].readyToPay) {
                readyToPay.Add(i);
            }

            if (tableStatus[i].readyFood) {
                readyFood.Add(i);
            }
        }

        Debug.Log("Ready to order tables: " + readyToOrder.Count);
        Debug.Log("Ready to pay tables: " + readyToPay.Count);
        Debug.Log("Ready to get food tables: " + readyFood.Count);

        ExitState();
    }
}
