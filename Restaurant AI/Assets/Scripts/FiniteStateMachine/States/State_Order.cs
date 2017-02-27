using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class State_Order : State {

    private FSM fsm;

    private GameObject table;
    private TableStatus tableStatus;
    private int tableNr;

    private Vector3 origin;
    private bool originReached;

    private Vector3 destination;
    private bool walkingToDestination;
    private bool destinationReached;

    private NavMeshAgent agent;

    private State_FindState sf;

    public override void EnterState(FSM _fsm) {
        Debug.Log("Entered: " + this.GetType());
        fsm = _fsm;

        agent = GetComponent<NavMeshAgent>();
        sf = GetComponent<State_FindState>();

        SetDefaultValues();

        int pickRandomTable = Random.Range(0, sf.readyToOrder.Count);

        tableNr = sf.readyToOrder[pickRandomTable];
        table = sf.tables[tableNr];
        tableStatus = table.GetComponent<TableStatus>();

        Debug.Log("Found table at :" + table.transform.position);
        Debug.Log("Table Nr: " + tableNr);

        MoveToTable();
        

    }

    public override void ExitState() {
        Debug.Log("Exitting: " + this.GetType());
        fsm.SwitchState("Idle");
    }

    public override void UpdateState() {

        for (int i = 0; i < agent.path.corners.Length - 1; i++) {
            Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red);
        }

        if (!destinationReached) {
            if (Vector3.Distance(transform.position, destination) < 1f) {
                Debug.Log("At the table.");
                Invoke("TakeOrder", 2f);
                destinationReached = true;
            }
        }
        if (!originReached) {
            if (!walkingToDestination) {
                if (Vector3.Distance(transform.position, origin) < 1f) {
                    Debug.Log("At waitress place.");
                    Invoke("ExitState", 2f);
                    originReached = true;
                }
            }
        }
    }

    private void MoveToTable() {
        walkingToDestination = true;
        destination = new Vector3(table.transform.position.x, table.transform.position.y, table.transform.position.z - 2f);
        agent.destination = destination;
    }

    private void TakeOrder() {
        Debug.Log("Took order");
        tableStatus.readyToOrder = false;
        tableStatus.orderTaken = true;
        sf.readyToOrder.RemoveAt(sf.readyToOrder.IndexOf(tableNr));
        Debug.Log("Removed Table " + tableNr);
        MoveBack();
    }

    private void MoveBack() {
        walkingToDestination = false;
        destination = origin;
        agent.destination = destination;
    }

    private void SetDefaultValues() {
        originReached = false;
        walkingToDestination = false;
        destinationReached = false;
    }
}
