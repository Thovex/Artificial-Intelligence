using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponGenerationForGameplay : MonoBehaviour {

    public string itemName;
    public float itemValue;
    public float itemLength;
    public float itemWeight;
    public float itemDurability;
    public float itemDamage;

    private float meshValue;
    private float materialValue;

    private bool shake;

    public GameObject parentObj;

    private Vector3 offset = new Vector3(0f, .5f, 0f);
    private List<GameObject> part = new List<GameObject>();
    public List<string> materialTypes = new List<string>();

    private void Awake() {
        StartGeneratingSword();

        itemValue = meshValue + materialValue;

        if (CheckMaterialCombo()) {
            itemValue = itemValue * 1.5f;
        }

        //transform.name = NameDatabase.Instance.GenerateName();

        itemLength = CalculateLocalBounds();
        itemWeight = CalculateWeight();
        itemDurability = CalculateDurability();
        itemValue = itemValue * (itemDurability / 100f);

        SuperSpecificStuff();

        itemDamage = itemValue * itemLength * itemDurability * itemWeight / 10000f;

        Invoke("SetParent", 1f);

    }

    private void SetParent() {
        transform.parent = parentObj.transform;
        transform.position = new Vector3(parentObj.transform.position.x, parentObj.transform.position.y + .5f, parentObj.transform.position.z);
    }

    private void SuperSpecificStuff() {
        if (itemName.Contains("Yusha")) {
            itemValue = -itemValue;
            itemWeight = 9001;
            itemDurability = 0;
            itemDamage = itemValue * 10f;
        }

        if (itemName.Contains("Thovex") || itemName.Contains("Jesse")) {
            itemValue = itemValue * 10f;
            transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.GetComponent<MeshRenderer>().material = SwordDatabase.Instance.itemSpecificMaterials[0];
            transform.GetChild(0).GetChild(0).GetChild(0).transform.GetComponent<MeshRenderer>().material = SwordDatabase.Instance.itemSpecificMaterials[0];
            transform.GetChild(0).GetChild(0).transform.GetComponent<MeshRenderer>().material = SwordDatabase.Instance.itemSpecificMaterials[0];
            transform.GetChild(0).transform.GetComponent<MeshRenderer>().material = SwordDatabase.Instance.itemSpecificMaterials[0];

        }

        if (itemName.Contains("Darki")) {
            transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).localScale = new Vector3(0.5f, 0.5f, 0.5f);
            itemLength = itemLength / 2f;
        }

        if (itemName.Contains("Valentijn")) {
            transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).localScale = new Vector3(2.5f, 2.5f, 1f);
            itemLength = itemLength * 3f;
        }

        if (itemName.Contains("Maria")) {
            GameObject g = Instantiate(SwordDatabase.Instance.itemParticles[1], transform.position, Quaternion.identity, transform);
            var theShape = g.GetComponent<ParticleSystem>().shape;
            theShape.shapeType = ParticleSystemShapeType.Mesh;
            theShape.mesh = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.GetComponent<MeshFilter>().sharedMesh;
        }

        if (itemValue >= 12000 && itemValue < 15000) {
            GameObject g = Instantiate(SwordDatabase.Instance.itemParticles[0], transform.position, Quaternion.identity, transform);
            var theShape = g.GetComponent<ParticleSystem>().shape;
            theShape.shapeType = ParticleSystemShapeType.Mesh;
            theShape.mesh = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.GetComponent<MeshFilter>().sharedMesh;
        }

        if (itemValue >= 15000) {
            GameObject g = Instantiate(SwordDatabase.Instance.itemParticles[2], transform.position, Quaternion.identity, transform);
            var theShape = g.GetComponent<ParticleSystem>().shape;
            theShape.shapeType = ParticleSystemShapeType.Mesh;
            theShape.mesh = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.GetComponent<MeshFilter>().sharedMesh;

        }
    }

    private bool CheckMaterialCombo() {
        if (materialTypes[0] == materialTypes[1]) {
            if (materialTypes[1] == materialTypes[2]) {
                if (materialTypes[2] == materialTypes[3]) {
                    return true;
                }
            }
        }
        return false;
    }

    private float CalculateDurability() {
        itemDurability = 100;

        foreach (string s in materialTypes) {
            if (Random.value < .5f) {
                if (s == "Copper (UnityEngine.Material)") {
                    itemDurability -= Random.Range(0, 15f);
                } else if (s == "Bronze (UnityEngine.Material)") {
                    itemDurability -= Random.Range(0, 8f);
                } else if (s == "Steel (UnityEngine.Material)") {
                    itemDurability -= Random.Range(0, 5f);
                } else if (s == "Silver (UnityEngine.Material)") {
                    itemDurability -= Random.Range(0, 1f);
                } else if (s == "Gold (UnityEngine.Material)") {
                    itemDurability -= Random.Range(0, .5f);
                } else if (s == "Ruby (UnityEngine.Material)") {
                    itemDurability -= 0f;
                } else if (s == "Rune (UnityEngine.Material)") {
                    itemDurability -= Random.Range(0, .1f);
                }
            }
        }
        return itemDurability;
    }

    private float CalculateWeight() {
        itemWeight = 10;

        foreach (string s in materialTypes) {
            if (s == "Copper (UnityEngine.Material)") {
                itemWeight = itemWeight * 55.987f;
            }
            else if (s == "Bronze (UnityEngine.Material)") {
                itemWeight = itemWeight * 54.100f;
            }
            else if (s == "Steel (UnityEngine.Material)") {
                itemWeight = itemWeight * 49.421f;
            }
            else if (s == "Silver (UnityEngine.Material)") {
                itemWeight = itemWeight * 65.491f;
            }
            else if (s == "Gold (UnityEngine.Material)") {
                itemWeight = itemWeight * 120.683f;
            }
            else if (s == "Ruby (UnityEngine.Material)") {
                itemWeight = itemWeight * 30.683f;
            }
            else if (s == "Rune (UnityEngine.Material)") {
                itemWeight = itemWeight * 155.423f;
            }

            itemWeight = itemWeight / 50f;
        }
        return itemWeight;
    }

    private float CalculateLocalBounds() {
        Quaternion currentRotation = this.transform.rotation;
        this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Bounds bounds = new Bounds(this.transform.position, Vector3.zero);

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
            bounds.Encapsulate(renderer.bounds);
        }

        Vector3 localCenter = bounds.center - this.transform.position;
        bounds.center = localCenter;
        

        this.transform.rotation = currentRotation;

        return itemLength = bounds.size.y / 10f;
    }

    private void StartGeneratingSword() {
        for (int i = 0; i < SwordDatabase.Instance.swordSize; i++) {
            GameObject aPart;

            if (part.Count < 1) {
                aPart = GeneratePartData(transform.position, gameObject, i);
            } else {
                Vector3 newPos = CalculateTopVertex(part[i - 1]);
                aPart = GeneratePartData(newPos, part[i - 1], i);
            }

            if (Random.value < 0.5f) {
                aPart.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            part.Add(aPart);
        }
    }

    private GameObject GenerateGameObjectData(Mesh theMesh, Material theMaterial) {
        GameObject newObj = new GameObject(theMesh.name);
        newObj.AddComponent<MeshFilter>().mesh = theMesh;
        newObj.AddComponent<MeshRenderer>().material = theMaterial;
        newObj.AddComponent<MeshCollider>().convex = true;
        return newObj;
    }


    private GameObject GeneratePartData(Vector3 startPos, GameObject parent, int index) {

        int meshPicker = Random.Range(0, SwordDatabase.Instance.itemMeshes[index].ItemMeshes.Count);
        int materialPicker = Random.Range(0, SwordDatabase.Instance.itemMaterials.Count);

        Mesh selectedMesh = SwordDatabase.Instance.itemMeshes[index].ItemMeshes[meshPicker].mesh;
        Material selectedMaterial = SwordDatabase.Instance.itemMaterials[materialPicker].material;

        GameObject obj = GenerateGameObjectData(selectedMesh, selectedMaterial);
        obj.transform.position = startPos + parent.transform.position + offset;
        obj.transform.parent = parent.transform;

        CustomizeSettings(index, obj);

        materialTypes.Add(selectedMaterial.ToString());

  
        meshValue += (SwordDatabase.Instance.itemMeshes[index].ItemMeshes[meshPicker].value * obj.transform.localScale.x * obj.transform.localScale.y * obj.transform.localScale.z);
        materialValue += SwordDatabase.Instance.itemMaterials[materialPicker].value;

        return obj;
    }

    private void CustomizeSettings(int index, GameObject obj) {
        switch (index) {
            case 0: // Pommel Settings
                break;
            case 1: // Hilt Settings
                obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z + Random.Range(-.5f, .5f));
                obj.transform.position -= offset * 2.5f;
                break;
            case 2: // Guard Settings
                obj.transform.localScale = new Vector3(obj.transform.localScale.x + Random.Range(-.5f, .5f), obj.transform.localScale.y, obj.transform.localScale.z);
                obj.transform.position -= offset * 2.5f;
                break;
            case 3: // Sword Settings
                obj.transform.position -= offset * 2.5f;
                obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y + Random.Range(-.5f, .5f), obj.transform.localScale.z);
                obj.layer = 8;
                if (Random.value < 0.5f) {
                    obj.transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                break;
        }
    }

    private Vector3 CalculateTopVertex(GameObject obj) {
        Vector3[] verts = obj.GetComponent<MeshFilter>().sharedMesh.vertices;
        Vector3 topVertex = new Vector3(0, float.NegativeInfinity, 0);
        for (int i = 0; i < verts.Length; i++) {
            Vector3 vert = transform.TransformPoint(verts[i]);
            if (vert.y > topVertex.y) {
                topVertex = vert;
            }
        }

        topVertex = new Vector3(obj.GetComponent<MeshRenderer>().bounds.center.x, topVertex.y, obj.GetComponent<MeshRenderer>().bounds.center.z);
        return topVertex;
    }

    public void SetName() {
        name = itemValue.ToString();
    }
}
