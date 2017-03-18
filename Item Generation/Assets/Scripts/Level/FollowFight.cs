using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFight : MonoBehaviour {

    public GameObject p1, p2;
    private Vector3 center;

    public GameObject levelGenerator;

    void Start() {
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

    void Update() {
        if (p1 != null && p2 != null) {
            if (p1.gameObject.activeInHierarchy && p2.gameObject.activeInHierarchy) {
                center = ((p2.transform.position - p1.transform.position) / 2.0f) + p1.transform.position;
                center = new Vector3(center.x, center.y + 10f, center.z);
                transform.LookAt(center);


                if (p1.GetComponent<FSM>().enabled == true && p2.GetComponent<FSM>().enabled == true) {
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

        if (p1 != null && p2 == null) {
            transform.LookAt(new Vector3(p1.transform.position.x, p1.transform.position.y + 10f, p1.transform.position.z));
        }

        if (p2 != null && p1 == null) {
            transform.LookAt(new Vector3(p2.transform.position.x, p2.transform.position.y + 10f, p2.transform.position.z));
        }

    }
}
