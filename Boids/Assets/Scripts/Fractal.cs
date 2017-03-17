using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

    public int maxDepth;
    public float childScale;

    public Material material;
    public Mesh[] mesh;

    private int depth;

	void Start () {
        this.gameObject.AddComponent<MeshRenderer>().material = material;
        this.gameObject.AddComponent<MeshFilter>().mesh = mesh[Random.Range(0, mesh.Length)];

        if (depth < maxDepth) {
            StartCoroutine("InstantiateNewFractal");
        }

        if (this.GetComponent<BoxCollider>() == null) {
            this.gameObject.AddComponent<BoxCollider>().isTrigger = true;
        }
    }

    private IEnumerator InstantiateNewFractal() {
        yield return new WaitForSeconds(1f);
        new GameObject("Fractal").AddComponent<Fractal>().InitFractal(this, Vector3.up, Quaternion.identity);
        new GameObject("Fractal").AddComponent<Fractal>().InitFractal(this, Vector3.left, Quaternion.Euler(0f, 0f, 90f));
        new GameObject("Fractal").AddComponent<Fractal>().InitFractal(this, Vector3.right, Quaternion.Euler(0f, 0f, -90f));
    }

    private void InitFractal(Fractal parent, Vector3 direction, Quaternion rotation) {
        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localScale = new Vector3(Random.Range(0.1f,5)* childScale, Random.Range(0.1f, 5)* childScale, Random.Range(0.1f, 5) * childScale);
        transform.localRotation = rotation;
        transform.localPosition = direction * (0.5f + 0.5f * childScale);
    }

    private void OnTriggerEnter(Collider coll) {
        if (coll.tag == "Laser") {
            transform.root.GetComponent<Boid>().GetHit();
        }
    }
}
