using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateSwords : MonoBehaviour {

    public GameObject swordGen;

    public GameObject highestValue;
    public GameObject lowestValue;

    public List<GameObject> swordGenObjects = new List<GameObject>();
    public List<float> values = new List<float>();

    void Start () {
	    for (int i = 0; i < 100; i++) {
            GameObject temp = Instantiate(swordGen, Vector3.zero, Quaternion.identity);
            temp.name = "SwordGen_" + i;
            swordGenObjects.Add(temp);
        }

        Invoke("UpdatePositions", 1f);
    }

    void UpdatePositions() {
        foreach (GameObject g in swordGenObjects) {
            string[] tempStringArray = g.name.Split('_');
            int pos = int.Parse(tempStringArray[1]);

            g.transform.position = new Vector3(pos * 35, 0, 0);

            g.GetComponent<WeaponGeneration>().SetName();

            values.Add(g.GetComponent<WeaponGeneration>().itemValue);
        }

        values.Sort();
        GetValue(false);

        values.Reverse();
        GetValue(true);
    }

    void GetValue(bool highest) {
        string opOne = values[0].ToString();
        for (int i = 0; i < swordGenObjects.Count; i++) {
            if (opOne == swordGenObjects[i].name) {
                if (highest) {
                    Instantiate(highestValue, swordGenObjects[i].transform.position + (Vector3.down * 19f), Quaternion.identity);
                } else {
                    Instantiate(lowestValue, swordGenObjects[i].transform.position + (Vector3.down * 19f), Quaternion.identity);
                }
            }
        }
    }
}
