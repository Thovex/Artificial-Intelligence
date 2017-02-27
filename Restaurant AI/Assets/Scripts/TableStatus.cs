using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableStatus : MonoBehaviour {

    public bool readyToOrder = false;
    public bool readyToPay = false;
    public bool readyFood = false;

    public bool orderTaken = false;
    public bool eating = false;
    public bool waitToPay = false;

    public bool finishedTable = false;

    public float timeSinceOrder = 0f;
    public float timeSinceWaitToPay = 0f;
    public float timeSinceEating = 0f;

    private void Update() {
        if (orderTaken) {
            timeSinceOrder += Time.deltaTime;
        }

        if (waitToPay) {
            timeSinceWaitToPay += Time.deltaTime;
        }

        if (eating) {
            timeSinceEating += Time.deltaTime;
        }

        if (!orderTaken) {
            if (!waitToPay) {
                if (!eating) {
                    if (PassFail(0.0001f)) {
                        readyToOrder = true;
                    }
                }
            }
        }

        if (timeSinceOrder > 30f) {
            readyFood = true;
            orderTaken = false;
        }

        if (timeSinceEating > 100f) {
            waitToPay = true;
            readyToPay = true;
        }

        if (finishedTable) {
            Reset();
        }
    }

    private void Reset() {
        readyToOrder = false;
        readyToPay = false;
        readyFood = false;
        orderTaken = false;
        waitToPay = false;
        finishedTable = false;
        eating = false;
        timeSinceOrder = 0f;
        timeSinceWaitToPay = 0f;
        timeSinceEating = 0f;
    }

    private bool PassFail(float chanceOfSuccess) {
        return Random.value < chanceOfSuccess;
    }
}
