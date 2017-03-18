using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToFloor : MonoBehaviour {

    public MeshFilter meshFilter;

    public Transform backRight;
    public Transform backLeft;
    public Transform frontLeft;
    public Transform frontRight;

    public Vector3 hitBackRight;
    public Vector3 hitBackLeft;
    public Vector3 hitFrontRight;
    public Vector3 hitFrontLeft;

    void Start() {
        Invoke("AdjustPositionToGround", 1f);

    }

    void AdjustPositionToGround() {

        RaycastHit rayHitBackRight;
        RaycastHit rayHitBackLeft;
        RaycastHit rayHitFrontLeft;
        RaycastHit rayHitFrontRight;
        RaycastHit rayHitMiddle;

        Debug.DrawRay(backRight.transform.position, Vector3.down);
        Debug.DrawRay(backLeft.transform.position, Vector3.down);
        Debug.DrawRay(frontLeft.transform.position, Vector3.down);
        Debug.DrawRay(frontRight.transform.position, Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down);

        Ray rayBackRight = new Ray(backRight.transform.position, Vector3.down);
        Ray rayBackLeft = new Ray(backLeft.transform.position, Vector3.down);
        Ray rayFrontLeft = new Ray(frontLeft.transform.position, Vector3.down);
        Ray rayFrontRight = new Ray(frontRight.transform.position, Vector3.down);
        Ray rayMiddle = new Ray(transform.position, Vector3.down);


        if (Physics.Raycast(rayBackRight, out rayHitBackRight, Mathf.Infinity)) {
            hitBackRight = rayHitBackRight.point;
            Debug.DrawRay(rayHitBackRight.point, Vector3.up);
        }

        if (Physics.Raycast(rayBackLeft, out rayHitBackLeft, Mathf.Infinity)) {
            hitBackLeft = rayHitBackLeft.point;
            Debug.DrawRay(rayHitBackLeft.point, Vector3.up);

        }

        if (Physics.Raycast(rayFrontLeft, out rayHitFrontLeft, Mathf.Infinity)) {
            hitFrontLeft = rayHitFrontLeft.point;
            Debug.DrawRay(rayHitFrontLeft.point, Vector3.up);

        }

        if (Physics.Raycast(rayFrontRight, out rayHitFrontRight, Mathf.Infinity)) {
            hitFrontRight = rayHitFrontRight.point;
            Debug.DrawRay(rayHitFrontRight.point, Vector3.up);

        }

        Vector3 upDir = (
             Vector3.Cross(rayHitBackRight.point - Vector3.up, rayHitBackLeft.point - Vector3.up) +
             Vector3.Cross(rayHitBackLeft.point - Vector3.up, rayHitFrontLeft.point - Vector3.up) +
             Vector3.Cross(rayHitFrontLeft.point - Vector3.up, rayHitFrontRight.point - Vector3.up) +
             Vector3.Cross(rayHitFrontRight.point - Vector3.up, rayHitBackRight.point - Vector3.up)
            ).normalized;

        transform.rotation = Quaternion.LookRotation(-upDir, -transform.forward);
        transform.Rotate(Vector3.right, 90f);

        if (Physics.Raycast(rayMiddle, out rayHitMiddle, Mathf.Infinity)) {
            transform.position = rayHitMiddle.point;
            Destroy(this);
        }


    }
}
