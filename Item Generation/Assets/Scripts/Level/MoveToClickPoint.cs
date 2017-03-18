// MoveToClickPoint.cs
using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : MonoBehaviour {
    NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000)) {
                agent.destination = hit.point;
            }
        }

        for (int i = 0; i < agent.path.corners.Length - 1; i++) {
            Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red);
        }
    }
}