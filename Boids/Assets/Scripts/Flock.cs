using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flock : MonoBehaviour {

    public GameObject boidPrefab;
    public int amountBoids;

    private GameObject center;

    public GameObject[] boids;

    public Slider leftSlider;
    public Slider rightSlider;
    public Slider upSlider;
    public Slider downSlider;

    private void Start () {
        boids = new GameObject[amountBoids];
        center = new GameObject("CenterOfMass");

        for (int i = 0; i < amountBoids; i++) {
            boids[i] = Instantiate(boidPrefab, Random.onUnitSphere * 10f, transform.rotation) as GameObject;
        }
    }

    private void Update() {
        boids = GameObject.FindGameObjectsWithTag("Boid");
        center.transform.position = new Vector3(leftSlider.value + -rightSlider.value, upSlider.value + -downSlider.value, 0f);
    }
}
