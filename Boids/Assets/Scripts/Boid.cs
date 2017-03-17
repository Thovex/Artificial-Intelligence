using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Boid : MonoBehaviour {

    public float maxDist = 5f;
    public float maxVelocity = 3f;

    public bool isHit = false;
    public bool isShot = false;

    private GameObject center;
    private GameObject[] hideFrom;

    public GameObject run;
    public GameObject deathObject;

    private Flock flock;

    private Rigidbody rb;

    private void Start() {
        center = GameObject.Find("CenterOfMass");
        flock = GameObject.Find("Flock").GetComponent<Flock>();
        rb = this.GetComponent<Rigidbody>();

    }

    private void LateUpdate() {

        hideFrom = GameObject.FindGameObjectsWithTag("HideFrom");

        Vector3 v1 = Rule1();
        Vector3 v2 = Rule2();
        Vector3 v3 = Rule3();
        Vector3 v4 = Rule4();
        Vector3 v5 = Vector3.zero;

        if (isHit) {
            v5 = Rule5();
        }

        rb.velocity += (v1 + v2 + v3 + v4 + v5) / 10f * (1 + hideFrom.Length);

        if (rb.velocity.sqrMagnitude > maxVelocity) {
            rb.velocity *= 0.99f;
        }

        this.transform.up = rb.velocity.normalized;

        DebugRayVector(1, v1);
        DebugRayVector(2, v2);
        DebugRayVector(3, v3);
        DebugRayVector(4, v4);
        DebugRayVector(5, v5);
    }

    private Vector3 Rule1() {
        Vector3 centerOfMass = Vector3.zero;

        for (int i = 0; i < flock.boids.Length; i++) {
            float dist = Vector3.Distance(flock.boids[i].transform.localPosition, this.transform.localPosition);

            if (dist > 0 && dist < maxDist) {
                centerOfMass += flock.boids[i].transform.localPosition;
            }
        }

        return ((centerOfMass / (flock.boids.Length - 1)) - this.transform.localPosition).normalized;
    }

    private Vector3 Rule2() {
        Vector3 velocity = Vector3.zero;

        for (int i = 0; i < flock.boids.Length; i++) {
            float dist = Vector3.Distance(flock.boids[i].transform.localPosition, this.transform.localPosition);

            if (dist > 0 && dist < maxDist) {
                velocity += flock.boids[i].GetComponent<Rigidbody>().velocity;
            }
        }
        return (velocity / (flock.boids.Length - 1)).normalized;
    }

    private Vector3 Rule3() {
        Vector3 velocity = Vector3.zero;

        for (int i = 0; i < flock.boids.Length; i++) {
            float dist = Vector3.Distance(flock.boids[i].transform.localPosition, this.transform.localPosition);

            if (dist > 0 && dist < maxDist) {
                velocity -= (flock.boids[i].transform.localPosition - this.transform.localPosition).normalized / dist;
            }
        }
        return (velocity / (flock.boids.Length - 1)).normalized;
    }

    private Vector3 Rule4() {
        Vector3 addingVector = center.transform.position;
        return addingVector;
    }

    private Vector3 Rule5() {
        Vector3 moveTo = Vector3.zero;

        if (hideFrom != null) {
            for (int i = 0; i < hideFrom.Length; i++) {
                Vector3 dirTo = this.transform.position - hideFrom[i].transform.position;
                dirTo = dirTo.normalized;
                moveTo = dirTo;
            }
        }
        return moveTo * 10f; 
    }

    public void DebugRayVector(int RuleNr, Vector3 velocity) {
        switch (RuleNr) {
            case 1:
                Debug.DrawRay(this.transform.localPosition, velocity, Color.blue);
                break;
            case 2:
                Debug.DrawRay(this.transform.localPosition, velocity, Color.green);
                break;
            case 3:
                Debug.DrawRay(this.transform.localPosition, velocity, Color.red);
                break;
            case 4:
                Debug.DrawRay(this.transform.localPosition, velocity, Color.yellow);
                break;
        }
    }

    private void OnTriggerEnter(Collider coll) {
        if (coll.tag == "HideFrom") {
            isHit = true;
        }

        if (coll.tag == "Laser") {
            GetHit();
        }
    }

    private void OnTriggerExit(Collider coll) {
        if (coll.tag == "HideFrom") {
            isHit = false;
        }
    }

    public void GetHit() {
        if (!isShot) {
            isShot = true;
            GameObject t = Instantiate(run, transform.position, transform.rotation);
            GameObject.Find("WeaponPivot").GetComponent<Weapon>().kills++;
            Destroy(t, 10f);
            if (gameObject != null) {
                GameObject t2 = Instantiate(deathObject, transform.position, transform.rotation);
                Destroy(t2, 5f);
                Destroy(gameObject);
            }
        }
    }
}
