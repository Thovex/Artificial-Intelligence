using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    public GameObject boidPrefab;
    public int amountBoids;

    public float maxDist = 5f;
    public float maxVelocity = 3f;

    private GameObject[] boids;
    private GameObject center;

    private void Start () {
        boids = new GameObject[amountBoids];
        center = new GameObject("CenterOfMass");

        for (int i = 0; i < amountBoids; i++) {
            boids[i] = Instantiate(boidPrefab, Random.onUnitSphere, transform.rotation) as GameObject;
        }
	}

    private void Update() {
        for (int i = 0; i < amountBoids; i++) {
            GameObject boid = boids[i];

            if (boid != null && boid.GetComponent<Rigidbody>() != null) {
                Vector3 v1 = Rule1(boid);
                Vector3 v2 = Rule2(boid);
                Vector3 v3 = Rule3(boid);

                Rigidbody rb = boid.GetComponent<Rigidbody>();

                rb.velocity += (v1 + v2 + v3) / 10f;

                if (rb.velocity.sqrMagnitude > maxVelocity) {
                    rb.velocity *= 0.99f;
                }

                boid.transform.up = rb.velocity.normalized;

                DebugRayVector(1, boid, v1);
                DebugRayVector(2, boid, v2);
                DebugRayVector(3, boid, v3);

            }
        }
    }

    private Vector3 Rule1(GameObject boid) {
        Vector3 centerOfMass = Vector3.zero;

        for (int i = 0; i < amountBoids; i++) {
            float dist = Vector3.Distance(boids[i].transform.localPosition, boid.transform.localPosition);

            if (dist > 0 && dist < maxDist) {
                centerOfMass += boids[i].transform.localPosition;
            }
        }

        return ((centerOfMass / (amountBoids - 1)) - boid.transform.localPosition).normalized;
    }

    private Vector3 Rule2(GameObject boid) {
        Vector3 velocity = Vector3.zero;

        for (int i = 0; i < amountBoids; i++) {
            float dist = Vector3.Distance(boids[i].transform.localPosition, boid.transform.localPosition);

            if (dist > 0 && dist < maxDist) {
                velocity += boids[i].GetComponent<Rigidbody>().velocity;
            }
        }
        return (velocity / (amountBoids - 1)).normalized;
    }

    private Vector3 Rule3(GameObject boid) {
        Vector3 velocity = Vector3.zero;

        for (int i = 0; i < amountBoids; i++) {
            float dist = Vector3.Distance(boids[i].transform.localPosition, boid.transform.localPosition);

            if (dist > 0 && dist < maxDist) {
                velocity -= (boids[i].transform.localPosition - boid.transform.localPosition).normalized / dist;
            }
        }
        return (velocity / (amountBoids - 1)).normalized;
    }

    public void DebugRayVector(int RuleNr, GameObject boid, Vector3 velocity) {
        switch (RuleNr) {
            case 1:
                Debug.DrawRay(boid.transform.localPosition, velocity, Color.blue);
                break;
            case 2:
                Debug.DrawRay(boid.transform.localPosition, velocity, Color.green);
                break;
            case 3:
                Debug.DrawRay(boid.transform.localPosition, velocity, Color.red);
                break;
        }
    }
}
