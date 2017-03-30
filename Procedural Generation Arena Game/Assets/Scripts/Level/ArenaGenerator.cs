using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArenaGenerator : MonoBehaviour {

    public GameObject tower;
    public GameObject wall;
    public GameObject gate;

    private GameObject[] ai = new GameObject[2];

    private Vector3[] spawnPos = new Vector3[2];

    public Vector2 arenaSize;

    public float sWall;
    public float sTower;
    public float sGate;

    private GameObject[] gates = new GameObject[2];

    void Start () {
        arenaSize = new Vector2(Random.Range(2, 6), Random.Range(2, 6));

        GetModelSizes();
        CreateBase();
        Invoke("SetAISpawn", 2f);
	}

    void SetAISpawn() {
        ai = GameObject.FindGameObjectsWithTag("ArtificialIntelligence");

        spawnPos[0] = new Vector3(transform.position.x - 15f, gates[0].transform.position.y, transform.position.z + (Mathf.FloorToInt(arenaSize.x / 2) * sWall));
        spawnPos[1] = new Vector3(transform.position.x - (arenaSize.y * sWall + sTower) + 15f, gates[1].transform.position.y, transform.position.z + (Mathf.FloorToInt(arenaSize.x / 2) * sWall));

        ai[0].GetComponent<NavMeshAgent>().enabled = false;
        ai[1].GetComponent<NavMeshAgent>().enabled = false;

        Debug.Log(spawnPos[0]);
        Debug.Log(spawnPos[1]);

        ai[0].transform.position = spawnPos[0];
        ai[1].transform.position = spawnPos[1];

        ai[0].GetComponent<NavMeshAgent>().enabled = true;
        ai[1].GetComponent<NavMeshAgent>().enabled = true;
    }
	
	void CreateBase () {

        for (int i = 0; i < arenaSize.x; i++) {
            if (i == Mathf.FloorToInt(arenaSize.x / 2)) {

                gates[0] = Instantiate(gate, new Vector3(transform.position.x, transform.position.y, transform.position.z + (i * sWall)), transform.rotation, transform);
                gates[1] = Instantiate(gate, new Vector3(transform.position.x - (arenaSize.y * sWall + sTower), transform.position.y, transform.position.z + (i * sWall)), Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y - 180f, transform.rotation.z)),transform);

            }
            else {
                Instantiate(wall, new Vector3(transform.position.x, transform.position.y, transform.position.z + (i * sWall)), transform.rotation, transform);
                Instantiate(wall, new Vector3(transform.position.x - (arenaSize.y * sWall + sTower), transform.position.y, transform.position.z + (i * sWall)), Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y - 180f, transform.rotation.z)), transform);
            }

            if (i == arenaSize.x - 1) { 
                Instantiate(tower, new Vector3(transform.position.x, transform.position.y, transform.position.z - sTower * 1.3f), transform.rotation, transform);
                Instantiate(tower, new Vector3(transform.position.x, transform.position.y, transform.position.z + (i * sWall) + sTower * 1.3f), Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y - 90f, transform.rotation.z)), transform);
                Instantiate(tower, new Vector3(transform.position.x - (arenaSize.y * sWall + sTower), transform.position.y, transform.position.z - sTower * 1.3f), Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y + 90f, transform.rotation.z)), transform);
                Instantiate(tower, new Vector3(transform.position.x - (arenaSize.y * sWall + sTower), transform.position.y, transform.position.z + (i * sWall) + sTower * 1.3f), Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y - 180f, transform.rotation.z)), transform);
            }
        }

        for (int j = 0; j < arenaSize.y; j++) {

            GameObject g = Instantiate(wall, new Vector3((transform.position.x - (j * sWall + sTower * 1.3f)), transform.position.y, transform.position.z - sTower *1.3f), Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y + 90f, transform.rotation.z)), transform);
            Instantiate(wall, new Vector3((transform.position.x - (j * sWall + sTower * 1.3f)), transform.position.y, transform.position.z + (arenaSize.x * sWall - sTower / 2f)), Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y + -90f, transform.rotation.z)), transform);

            if (j == Mathf.FloorToInt(arenaSize.y / 2f)) {
                Camera.main.transform.parent = g.transform;
                Camera.main.transform.position = new Vector3(g.transform.position.x, g.transform.position.y + 20f, g.transform.position.z);
                Camera.main.transform.rotation = Quaternion.Euler(new Vector3(25f, -90f, 0f));
            }
        }
    }

    // Find fix for this?
    private void GetModelSizes() {
        GameObject _wall = Instantiate(wall, transform.position, transform.rotation);
        GameObject _tower = Instantiate(tower, transform.position, transform.rotation);
        GameObject _gate = Instantiate(gate, transform.position, transform.rotation);

        sWall = _wall.GetComponent<MeshFilter>().sharedMesh.bounds.extents.z * 2f;
        sTower = _tower.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.extents.z * 2f;
        sGate = _gate.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.extents.z * 2f;

        Destroy(_wall);
        Destroy(_tower);
        Destroy(_gate);
    }
}
