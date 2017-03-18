using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArenaGenerator : MonoBehaviour {

    public GameObject tower;
    public GameObject wall;
    public GameObject gate;

    public GameObject ai01;
    public GameObject ai02;

    public Vector3 spawnPos1;
    public Vector3 spawnPos2;

    public Vector2 arenaSize;

    public float sWall;
    public float sTower;
    public float sGate;

    private GameObject gate1;
    private GameObject gate2;

    void Start () {
        arenaSize = new Vector2(Random.Range(2, 6), Random.Range(2, 6));

        GetModelSizes();
        CreateBase();
        Invoke("SetAISpawn", 2f);
	}

    void SetAISpawn() {
        spawnPos1 = new Vector3(transform.position.x - 15f, gate1.transform.position.y, transform.position.z + (Mathf.FloorToInt(arenaSize.x / 2) * sWall));
        spawnPos2 = new Vector3(transform.position.x - (arenaSize.y * sWall + sTower) + 15f, gate2.transform.position.y, transform.position.z + (Mathf.FloorToInt(arenaSize.x / 2) * sWall));

        ai01.GetComponent<NavMeshAgent>().enabled = false;
        ai02.GetComponent<NavMeshAgent>().enabled = false;

        ai01.transform.position = spawnPos1;
        ai02.transform.position = spawnPos2;

        ai01.GetComponent<NavMeshAgent>().enabled = true;
        ai02.GetComponent<NavMeshAgent>().enabled = true;
    }
	
	void CreateBase () {

        for (int i = 0; i < arenaSize.x; i++) {
            if (i == Mathf.FloorToInt(arenaSize.x / 2)) {

                gate1 = Instantiate(gate, new Vector3(transform.position.x, transform.position.y, transform.position.z + (i * sWall)), transform.rotation, transform);
                gate2 = Instantiate(gate, new Vector3(transform.position.x - (arenaSize.y * sWall + sTower), transform.position.y, transform.position.z + (i * sWall)), Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y - 180f, transform.rotation.z)),transform);

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
