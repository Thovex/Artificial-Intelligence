using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Fractal : MonoBehaviour {

    public int maxDepth;
    public float childScale;

    public Material material;
    public Mesh[] mesh;

    private int depth;

    private bool superFractal = true;

    public FractalCounter fc;

	void Start () {
        GetComponent<Rigidbody>().useGravity = false;
        this.gameObject.AddComponent<MeshRenderer>().material = material;
        this.gameObject.AddComponent<MeshFilter>().mesh = mesh[Random.Range(0, mesh.Length)];

        if (fc.count < 1000) {
            if (depth < maxDepth) {
                StartCoroutine("InstantiateNewFractal");
            }
        }
    }

    private IEnumerator InstantiateNewFractal() {
        new GameObject("Fractal").AddComponent<Fractal>().InitFractal(this, Vector3.up, Quaternion.identity);
        yield return new WaitForSeconds(1f);

        if (depth == 5) {
            new GameObject("Fractal").AddComponent<Fractal>().InitFractal(this, Vector3.left, Quaternion.Euler(0f, 0f, 90f));
            new GameObject("Fractal").AddComponent<Fractal>().InitFractal(this, Vector3.right, Quaternion.Euler(0f, 0f, -90f));
            new GameObject("Fractal").AddComponent<Fractal>().InitFractal(this, Vector3.back, Quaternion.Euler(-90f, 0f, 0f));
            new GameObject("Fractal").AddComponent<Fractal>().InitFractal(this, Vector3.forward, Quaternion.Euler(90f, 0f, 0f));
        }
    }

    private void Update() {
        if (depth == 5 && !superFractal) {
            if (Random.value < Random.Range(.1f,.5f)) {
                transform.Rotate(0f, 30f * Time.deltaTime, 0f);
            }
        }

        if (depth == 10 && !superFractal) {
            transform.parent = null;

        }
    }

    private void InitFractal(Fractal parent, Vector3 direction, Quaternion rotation) {
        fc = parent.fc;
        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth + Random.Range(-4,5);
        depth = parent.depth + 1;
        childScale = parent.childScale;
        superFractal = parent.superFractal;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localRotation = rotation;
        transform.localPosition = direction * (0.5f + 0.5f * childScale);

        if (this.name == "Fractal" && depth == Random.Range(8,10)) {
            if (superFractal) {
                superFractal = false;
                depth = 1;
            } else if (Random.value < .3f) {
                depth = 1;
            }
        }

        fc.count++;
    }
}
