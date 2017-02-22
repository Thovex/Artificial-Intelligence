using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour {

    public float playerVision = 1f;

	void Start () {
		
	}
	
	void Update () {
        Debug.DrawLine(transform.position, transform.position + Vector3.down);
        
    }
}
