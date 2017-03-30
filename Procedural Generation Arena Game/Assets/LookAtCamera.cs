using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LookAtCamera : MonoBehaviour {

    public Transform theTransform;

    private TextMeshPro textMeshPro;
    private float alpha;

    void Start () {
        theTransform = GameObject.Find("Main Camera").transform;
        textMeshPro = GetComponent<TextMeshPro>();
        alpha = textMeshPro.color.a;

        Destroy(gameObject, 5f);
	}
	
	void Update () {
        textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, alpha);
        alpha -= Time.deltaTime;
        transform.position += Vector3.up * Time.deltaTime;
        transform.LookAt(theTransform);

    }
}
