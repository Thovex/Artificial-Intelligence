using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.AI;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour {

    public int worldDivisions;
    public float worldSize;
    public float worldHeight;

    private Vector3[] worldVerts;
    private Vector2[] uvs;
    private int[] tris;
    private int worldVertCount;

    public GameObject arenaGenerator;

    public GameObject ai01;
    public GameObject ai02;

    public float waitTime = 30f;
    private float waitTimer;
    public Text timer;

    private void Start() {
        CreateTerrain();
        NavMeshBuilder.BuildNavMesh();

        waitTimer = waitTime;
        SetAIsActive();
        Invoke("CreateArena", waitTime);
    }

    private void Update() {
        if (waitTimer > 0) {
            waitTimer -= Time.deltaTime;
        }
        timer.text = waitTimer.ToString("F0");
    }

    private void CreateTerrain() {
        GetVertexCount();
        InitVertexArrays();
        CalculateVertsUvsTris();
        SetEdgeVerts();
        PerformDiamondSquare();
        SetMesh(CreateMesh());
    }

    private void GetVertexCount() {
        worldVertCount = (worldDivisions + 1) * (worldDivisions + 1);
    }

    private void InitVertexArrays() {
        worldVerts = new Vector3[worldVertCount];
        uvs = new Vector2[worldVertCount];
        tris = new int[worldDivisions * worldDivisions * 6];
    }

    private void CalculateVertsUvsTris() {
        float halfSize = worldSize * 0.5f;
        float divisionSize = worldSize / worldDivisions;

        int triOffset = 0;

        for (int i = 0; i <= worldDivisions; i++) {
            for (int j = 0; j <= worldDivisions; j++) {
                worldVerts[i * (worldDivisions + 1) + j] = new Vector3(-halfSize + j * divisionSize, 0.0f, halfSize - i * divisionSize);
                uvs[i * (worldDivisions + 1) + j] = new Vector2((float)i / worldDivisions, (float)j / worldDivisions);
                if (i < worldDivisions && j < worldDivisions) {
                    int topLeft = i * (worldDivisions + 1) + j;
                    int botLeft = (i + 1) * (worldDivisions + 1) + j;
                    SetTris(triOffset, topLeft, botLeft);
                    triOffset += 6;
                }
            }
        }
    }

    private void SetTris(int triOffset, int topLeft, int botLeft) {
        tris[triOffset] = topLeft;
        tris[triOffset + 1] = topLeft + 1;
        tris[triOffset + 2] = botLeft + 1;
        tris[triOffset + 3] = topLeft;
        tris[triOffset + 4] = botLeft + 1;
        tris[triOffset + 5] = botLeft;
    }

    private void SetEdgeVerts() {
        worldVerts[0].y = Random.Range(-worldHeight, worldHeight);
        worldVerts[worldDivisions + 1].y = Random.Range(-worldHeight, worldHeight);
        worldVerts[worldVerts.Length - 1].y = Random.Range(-worldHeight, worldHeight);
        worldVerts[worldVerts.Length - 1 - worldDivisions].y = Random.Range(-worldHeight, worldHeight);
    }

    private void PerformDiamondSquare() {
        int iterations = (int)Mathf.Log(worldDivisions, 2);
        int numSquares = 1;
        int squareSize = worldDivisions;

        for (int i = 0; i < iterations; i++) {
            int row = 0;
            for (int j = 0; j < numSquares; j++) {
                int col = 0;
                for (int k = 0; k < numSquares; k++) {
                    PerformDiamond(row, col, squareSize, worldHeight);
                    col += squareSize;
                }
                row += squareSize;
            }
            numSquares *= 2;
            squareSize /= 2;
            worldHeight *= 0.5f;
        }
    }

    private void PerformDiamond(int row, int col, int size, float offset) {
        int halfSize = (int)(size * 0.5f);
        int topLeft = row * (worldDivisions + 1) + col;
        int botLeft = (row + size) * (worldDivisions + 1) + col;

        int mid = (row + halfSize) * (worldDivisions + 1) + col + halfSize;
        worldVerts[mid].y = (worldVerts[topLeft].y + worldVerts[topLeft + size].y + worldVerts[botLeft].y + worldVerts[botLeft + size].y) * .25f + Random.Range(-offset, offset);

        worldVerts[topLeft + halfSize].y = (worldVerts[topLeft].y + worldVerts[topLeft + size].y + worldVerts[mid].y) / 3 + Random.Range(-offset, offset);
        worldVerts[mid - halfSize].y = (worldVerts[topLeft].y + worldVerts[botLeft].y + worldVerts[mid].y) / 3 + Random.Range(-offset, offset);
        worldVerts[mid + halfSize].y = (worldVerts[topLeft + size].y + worldVerts[botLeft + size].y + worldVerts[mid].y) / 3 + Random.Range(-offset, offset);
        worldVerts[botLeft + halfSize].y = (worldVerts[botLeft].y + worldVerts[botLeft + size].y + worldVerts[mid].y) / 3 + Random.Range(-offset, offset);
    }

    private Mesh CreateMesh() {
        Mesh mesh = new Mesh();
        return GetComponent<MeshFilter>().mesh = mesh;
    }

    private void SetMesh(Mesh mesh) {
        mesh.vertices = worldVerts;
        mesh.uv = uvs;
        mesh.triangles = tris;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        gameObject.AddComponent<MeshCollider>();
    }


    private void SetAIsActive() {
        ai01.SetActive(true);
        ai02.SetActive(true);
    }

    private void CreateArena() {
        arenaGenerator.SetActive(true);
        Invoke("SetAIScriptsActive", 2f);
        GameObject.Find("BettingCanvas").SetActive(false);
    }

    private void SetAIScriptsActive() {
        ai01.GetComponent<NavMeshAgent>().enabled = true;
        ai01.GetComponent<FSM>().enabled = true;
        ai02.GetComponent<NavMeshAgent>().enabled = true;
        ai02.GetComponent<FSM>().enabled = true;
    }

    public void SetTime(float time) {
        CancelInvoke("CreateArena");
        CreateArena();
        waitTimer = time;
    }








}
