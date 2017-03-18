using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCullingModeFix : MonoBehaviour {

    void Awake() {
        Invoke("Invoke_CullCompletely", 0.5f);
    }

    // TODO  bug 5.4.0f3 check if resoved now 
    private void Invoke_CullCompletely() {
        GetComponent<Animator>().cullingMode = AnimatorCullingMode.CullCompletely;
    }
}

