using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaGenerator : MonoBehaviour {

    public GameObject tower;
    public GameObject wall;
    public GameObject gate;

    public Vector2 arenaSize;

    public float sWall;
    public float sTower;
    public float sGate;

    void Start () {
        GetModelSizes();
        CreateBase();
	}
	
	void CreateBase () {

        for (int i = 0; i < arenaSize.x; i++) {
            if (i == Mathf.FloorToInt(arenaSize.x / 2)) {
                Instantiate(gate, new Vector3(transform.position.x, transform.position.y, transform.position.z + (i * sWall)), transform.rotation, transform);
                Instantiate(gate, new Vector3(transform.position.x - (arenaSize.y * sWall + sTower), transform.position.y, transform.position.z + (i * sWall)), Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y - 180f, transform.rotation.z)),transform);

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
            Instantiate(wall, new Vector3((transform.position.x - (j * sWall + sTower * 1.3f)), transform.position.y, transform.position.z - sTower *1.3f), Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y + 90f, transform.rotation.z)), transform);
            Instantiate(wall, new Vector3((transform.position.x - (j * sWall + sTower * 1.3f)), transform.position.y, transform.position.z + (arenaSize.x * sWall - sTower / 2f)), Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y + -90f, transform.rotation.z)), transform);

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
