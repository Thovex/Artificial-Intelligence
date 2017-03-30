using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemMeshesListWrapper {
    public List<ItemMesh> ItemMeshes;
}

[Serializable]
public class ItemMaterial {
    public Material material;
    public int value;
}

[Serializable]
public class ItemMesh {
    public Mesh mesh;
    public int value;
}

public class SwordDatabase : MonoBehaviour {

    public int swordSize = 4;

    public List<ItemMeshesListWrapper> itemMeshes = new List<ItemMeshesListWrapper>();
    public List<ItemMaterial> itemMaterials = new List<ItemMaterial>();
    public List<GameObject> itemParticles = new List<GameObject>();
    public List<GameObject> itemLights = new List<GameObject>();
    public List<Material> itemSpecificMaterials = new List<Material>();

    private static SwordDatabase instance = null;
    public static SwordDatabase Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        instance = this;
    }
}
