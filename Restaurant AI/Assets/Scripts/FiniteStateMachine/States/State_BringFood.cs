using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class State_BringFood : State {

    private FSM fsm;

    private GameObject table;
    private TableStatus tableStatus;
    private int tableNr;

    private Vector3 origin;
    private bool originReached;

    private Vector3 destination;
    private bool walkingToDestination;
    private bool destinationReached;

    private Vector3 foodDestination;
    private bool foodReached;

    private NavMeshAgent agent;

    private State_FindState sf;

    public override void EnterState(FSM _fsm) {
        Debug.Log("Entered: " + this.GetType());
        fsm = _fsm;

        agent = GetComponent<NavMeshAgent>();
        sf = GetComponent<State_FindState>();

        foodDestination = GameObject.FindGameObjectWithTag("FoodTable").transform.position;

        origin = new Vector3(0, 1, 4);
        SetDefaultValues();

        int pickRandomTable = Random.Range(0, sf.readyFood.Count);

        tableNr = sf.readyFood[pickRandomTable];
        table = sf.tables[tableNr];
        tableStatus = table.GetComponent<TableStatus>();

        Debug.Log("Found table at :" + table.transform.position);
        Debug.Log("Table Nr: " + tableNr);

        MoveToFoodTable();
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
            if (Vector3.Distance(transform.position, destination) < 1.5f) {
                Debug.Log("At the table.");
                Invoke("GiveFood", 2f);
                destinationReached = true;
            }
        }

        if (!foodReached) {
            if (Vector3.Distance(transform.position, foodDestination) < 1.5f) {
                Debug.Log("At the food Table.");
                Invoke("MoveToTable", 2f);
                foodReached = true;
            }
        }

        if (!originReached) {
            if (!walkingToDestination) {
                if (Vector3.Distance(transform.position, origin) < 1.5f) {
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

    private void MoveBack() {
        walkingToDestination = false;
        destination = origin;
        agent.destination = destination;
    }

    private void MoveToFoodTable() {
        walkingToDestination = true;
        foodDestination = new Vector3(foodDestination.x, foodDestination.y, foodDestination.z - 2f);
        agent.destination = foodDestination;
    }

    private void GiveFood() {
        Debug.Log("Gave Food.");
        tableStatus.readyFood = false;
        tableStatus.eating = true;
        tableStatus.timeSinceOrder = 0f;
        sf.readyFood.RemoveAt(sf.readyFood.IndexOf(tableNr));
        Debug.Log("Removed Table " + tableNr);
        MoveBack();
    }

    private void SetDefaultValues() {
        originReached = false;
        walkingToDestination = false;
        destinationReached = false;
        foodReached = false;
    }
}
