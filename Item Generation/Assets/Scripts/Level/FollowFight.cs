using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFight : MonoBehaviour {

    private GameObject[] p = new GameObject[2];
    private Vector3 center;

    private bool winnerP1;
    private bool winnerP2;

    public GameObject levelGenerator;

    private void Start() {
        Invoke("SetLevelActive", 2f);
    }

    private void SetLevelActive() {
        levelGenerator.SetActive(true);
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
        Vector3 dir = point - pivot; 
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }

    private void GetPlayers() {
        p = GameObject.FindGameObjectsWithTag("ArtificialIntelligence");
    }

    private void Update() {
        if (p[0] == null && p[1] == null) {
            Invoke("GetPlayers", 5f);
        }
        else {
            if (p[0] != null && p[1] != null) {
                if (p[0].gameObject.activeInHierarchy && p[1].gameObject.activeInHierarchy) {
                    center = ((p[1].transform.position - p[0].transform.position) / 2.0f) + p[0].transform.position;
                    center = new Vector3(center.x, center.y + 10f, center.z);
                    transform.LookAt(center);


                    if (p[0].GetComponent<FSM>().enabled == true && p[1].GetComponent<FSM>().enabled == true) {
                        if (Vector3.Distance(transform.position, center) > 15f) {
                            transform.position = Vector3.MoveTowards(transform.position, center, .1f);
                            transform.position = RotatePointAroundPivot(transform.position, center, Vector3.down * .1f);
                        }

                        if (Vector3.Distance(transform.position, center) < 10f) {
                            transform.position = Vector3.MoveTowards(transform.position, -center, .1f);
                            transform.position = RotatePointAroundPivot(transform.position, center, Vector3.down * .1f);

                        }
                    }
                }
            }

            if (p[0] != null && p[1] == null) {
                transform.LookAt(new Vector3(p[0].transform.position.x, p[0].transform.position.y + 10f, p[0].transform.position.z));
                winnerP1 = true;
                Invoke("RestartScene", 5f);
            }

            if (p[1] != null && p[0] == null) {
                transform.LookAt(new Vector3(p[1].transform.position.x, p[1].transform.position.y + 10f, p[1].transform.position.z));
                winnerP2 = true;
                Invoke("RestartScene", 5f);
            }
        }
    }

    private void RestartScene() {
        if (winnerP1) {
            MoneyManager.Instance.AIWon(1);
        }
        else if (winnerP2) {
            MoneyManager.Instance.AIWon(2);
        }

        SceneManaging.Instance.RestartScene();
    }
}
