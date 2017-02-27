using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class State_Pay : State {

    private FSM fsm;

    private GameObject table;
    private TableStatus tableStatus;
    private int tableNr;

    private Vector3 origin;
    private bool originReached;

    private Vector3 destination;
    private bool walkingToDestination;
    private bool destinationReached;

    private Vector3 binDestination;
    private bool binReached;

    private NavMeshAgent agent;

    private State_FindState sf;

    public override void EnterState(FSM _fsm) {

        Debug.Log("Entered: " + this.GetType());
        fsm = _fsm;

        agent = GetComponent<NavMeshAgent>();
        sf = GetComponent<State_FindState>();

        binDestination = GameObject.FindGameObjectWithTag("Bin").transform.position;

        origin = new Vector3(0, 1, 4);
        SetDefaultValues();

        int pickRandomTable = Random.Range(0, sf.readyToPay.Count);

        tableNr = sf.readyToPay[pickRandomTable];
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
                Invoke("TakePayment", 2f);
                destinationReached = true;
            }
        }

        if (!binReached) {
            if (Vector3.Distance(transform.position, binDestination) < 1f) {
                Debug.Log("At the bin.");
                Invoke("MoveBack", 2f);
                binReached = true;
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

    private void TakePayment() {
        Debug.Log("Took payment.");
        tableStatus.readyToPay = false;
        tableStatus.finishedTable = true;
        tableStatus.timeSinceEating = 0f;
        sf.readyToPay.RemoveAt(sf.readyToPay.IndexOf(tableNr));
        Debug.Log("Removed Table " + tableNr);
        MoveToBin();
    }

    private void MoveBack() {
        walkingToDestination = false;
        destination = origin;
        agent.destination = destination;
    }

    private void MoveToBin() {
        walkingToDestination = true;
        binDestination = new Vector3(binDestination.x, binDestination.y, binDestination.z - 2f);
        agent.destination = binDestination;
    }

    private void MoveToTable() {
        walkingToDestination = true;
        destination = new Vector3(table.transform.position.x, table.transform.position.y, table.transform.position.z - 2f);
        agent.destination = destination;
    }

    private void SetDefaultValues() {
        originReached = false;
        walkingToDestination = false;
        destinationReached = false;
        binReached = false;
    }
}
