using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSM : MonoBehaviour {


    public float health;
    public float maxHealth;
    public float meleeRange;
    public float damage;
    public float speed;

    public int timesSuccesHits;
    public int timesSuccesBlocks;

    public float rotationSpeed = 10f;

    public bool isDefending = false;
    public bool isAttacking = false;

    public State currentState;
    public State[] states;
    public Dictionary<string, State> fsmStates;

    public NavMeshAgent agent;

    private Rigidbody rigidBody;

    public Animator anim;

    public WeaponGenerationForGameplay weaponStats;

    public GameObject currentTarget;
    public List<GameObject> opponents = new List<GameObject>();

    private void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        weaponStats = transform.GetChild(0).GetComponent<WeaponGenerationForGameplay>();
        fsmStates = new Dictionary<string, State>();

        SetStats();

        states = GetComponents<State>();

        foreach (State state in states) {
            fsmStates.Add(state.stateName, state);
        }

        State idle = null;
        if (fsmStates.TryGetValue("Idle", out idle)) {
            currentState = idle;
            currentState.EnterState(this);
        }

    }

    private void SetStats() {
        maxHealth = weaponStats.itemValue * weaponStats.itemDurability / 25f;
        damage = weaponStats.itemDamage;
        speed = weaponStats.itemWeight * .2f;
        agent.speed = speed;
        health = maxHealth;
        meleeRange = weaponStats.itemLength * 7.5f;

    }

    private void Update () {
        if (rigidBody.velocity.x > 5f || rigidBody.velocity.z > 5f || rigidBody.velocity.x < -5f || rigidBody.velocity.z < -5f) {
            rigidBody.velocity = Vector3.one;
        }

        currentState.UpdateState();

        for (int i = 0; i < agent.path.corners.Length - 1; i++) {
            Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red);
        }

        if (health <= 0) {
            for (int i = 0; i < states.Length; i++) {
                Destroy(states[i]);
                Destroy(weaponStats);

                List<GameObject> childrenObjects = new List<GameObject>();

                Component[] children;
                children = GetComponentsInChildren(typeof(Transform));

                foreach (Component c in children) {
                    if (c.gameObject != this.gameObject) {
                        if (c.gameObject != this.gameObject.transform.GetChild(0).gameObject) {
                            childrenObjects.Add(c.gameObject);
                        }
                    }
                }

                foreach (GameObject g in childrenObjects) {
                    Debug.Log(g);
                    g.AddComponent<Rigidbody>();
                    g.transform.parent = null;

                    Mesh m = g.GetComponent<MeshFilter>().mesh;
                    Vector3[] oldVertices = m.vertices;
                    Vector3[] newVertices = new Vector3[oldVertices.Length];

                    for (int k = 0; k < oldVertices.Length; k++) {
                        newVertices[k] = new Vector3(oldVertices[k].x + Random.Range(-.1f, .1f), oldVertices[k].y + Random.Range(-.1f, .1f), oldVertices[k].z + Random.Range(-.1f, .1f));
                    }

                    m.vertices = newVertices;
                    g.layer = 0;
                    g.GetComponent<Rigidbody>().mass = weaponStats.itemWeight;
                }

                transform.DetachChildren();

                Destroy(gameObject);
            }
        }

    }

    public void SwitchState(string stateName) {
        State temp = null;
        if (fsmStates.TryGetValue(stateName, out temp)) {
            currentState = temp;
            currentState.EnterState(this);
        }
    }

    public bool IsInMeleeRangeOf(Transform target) {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < meleeRange;
    }

    public void MoveTowards(Transform target) {
        agent.SetDestination(target.position);
    }

    public void RotateTowards(Transform target) {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.GetComponent<FSM>() != null) {
            FSM collFSM = coll.gameObject.GetComponent<FSM>();

            if (isAttacking) {
                if (!collFSM.isDefending) {
                    collFSM.health -= damage;
                    timesSuccesHits++;
                } else {
                    collFSM.timesSuccesBlocks++;
                }
            }

            if (isDefending) {
                timesSuccesBlocks++;
            }
        }
    }
}
