using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponGeneration : MonoBehaviour {

    public string itemName;
    public float itemValue;
    public float itemLength;
    public float itemWeight;
    public float itemDurability;
    public float itemDamage;

    private float meshValue;
    private float materialValue;

    private bool shake;

    private Vector3 offset = new Vector3(0f, .5f, 0f);
    private List<GameObject> part = new List<GameObject>();
    public List<string> materialTypes = new List<string>();

    private void Start() {
        StartGeneratingSword();

        itemValue = meshValue + materialValue;

        if (CheckMaterialCombo()) {
            itemValue = itemValue * 1.5f;
        }

        if (Random.value < .05f) {
            itemName = NameDatabase.Instance.GenerateShittyName();
        }
        else {
            itemName = NameDatabase.Instance.GenerateName();
        }

        itemLength = CalculateLocalBounds();
        itemWeight = CalculateWeight();
        itemDurability = CalculateDurability();
        itemValue = itemValue * (itemDurability / 100f);

        SuperSpecificStuff();

        itemDamage = itemValue * itemLength * itemDurability * itemWeight / 10000f;

        transform.GetChild(0).transform.GetComponent<TextMeshPro>().text = itemName;
        transform.GetChild(1).transform.GetComponent<TextMeshPro>().text = "VAL: " + itemValue.ToString("F0");
        transform.GetChild(2).transform.GetComponent<TextMeshPro>().text = "LNG: " + itemLength.ToString("F1");
        transform.GetChild(3).transform.GetComponent<TextMeshPro>().text = "WEI: " + itemWeight.ToString("F1");
        transform.GetChild(4).transform.GetComponent<TextMeshPro>().text = "DUR: " + itemDurability.ToString("F0");
        transform.GetChild(5).transform.GetComponent<TextMeshPro>().text = "DMG: " + itemDamage.ToString("F0");

        Invoke("SwitchName", 2f);
    }

    private void SwitchName() {
        transform.name = itemName;
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
            transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).transform.GetComponent<MeshRenderer>().material = SwordDatabase.Instance.itemSpecificMaterials[0];
            transform.GetChild(6).GetChild(0).GetChild(0).transform.GetComponent<MeshRenderer>().material = SwordDatabase.Instance.itemSpecificMaterials[0];
            transform.GetChild(6).GetChild(0).transform.GetComponent<MeshRenderer>().material = SwordDatabase.Instance.itemSpecificMaterials[0];
            transform.GetChild(6).transform.GetComponent<MeshRenderer>().material = SwordDatabase.Instance.itemSpecificMaterials[0];

        }

        if (itemName.Contains("Darki")) {
            transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).localScale = new Vector3(0.5f, 0.5f, 0.5f);
            itemLength = itemLength / 2f;
            shake = true;
        }

        if (itemName.Contains("Valentijn")) {
            transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).localScale = new Vector3(2.5f, 2.5f, 1f);
            itemLength = itemLength * 3f;
        }

        if (itemName.Contains("Shema")) {
            shake = true;
        }

        if (itemName.Contains("Maria")) {
            GameObject g = Instantiate(SwordDatabase.Instance.itemParticles[1], transform.position, Quaternion.identity, transform);
            var theShape = g.GetComponent<ParticleSystem>().shape;
            theShape.shapeType = ParticleSystemShapeType.Mesh;
            theShape.mesh = transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).transform.GetComponent<MeshFilter>().sharedMesh;
        }

        if (itemValue > 0 && itemValue < 9000) {
            Instantiate(SwordDatabase.Instance.itemLights[3], new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z), Quaternion.Euler(new Vector3(-90f, 0f, 0f)), transform);
        }

        if (itemValue >= 9000 && itemValue < 12000) {
            Instantiate(SwordDatabase.Instance.itemLights[2], new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z), Quaternion.Euler(new Vector3(-90f, 0f, 0f)), transform);
            transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.blue;
        }

        if (itemValue >= 12000 && itemValue < 15000) {
            GameObject g = Instantiate(SwordDatabase.Instance.itemParticles[0], transform.position, Quaternion.identity, transform);
            var theShape = g.GetComponent<ParticleSystem>().shape;
            theShape.shapeType = ParticleSystemShapeType.Mesh;
            theShape.mesh = transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).transform.GetComponent<MeshFilter>().sharedMesh;
            Instantiate(SwordDatabase.Instance.itemLights[0], new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z), Quaternion.Euler(new Vector3(-90f,0f,0f)), transform);
            transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.magenta;
        }

        if (itemValue >= 15000) {
            GameObject g = Instantiate(SwordDatabase.Instance.itemParticles[2], transform.position, Quaternion.identity, transform);
            var theShape = g.GetComponent<ParticleSystem>().shape;
            theShape.shapeType = ParticleSystemShapeType.Mesh;
            theShape.mesh = transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).transform.GetComponent<MeshFilter>().sharedMesh;
            Instantiate(SwordDatabase.Instance.itemLights[1], new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z), Quaternion.Euler(new Vector3(-90f, 0f, 0f)), transform);
            transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.yellow;

        }
    }

    private void Shake() {
        float AngleAmount = (Mathf.Cos(Time.time * 50f) * 180) / Mathf.PI * 0.5f;
        AngleAmount = Mathf.Clamp(AngleAmount, -10, 10);
        transform.GetChild(6).transform.localRotation = Quaternion.Euler(0, 0, AngleAmount);
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
        Quaternion currentRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Bounds bounds = new Bounds(transform.position, Vector3.zero);

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
            bounds.Encapsulate(renderer.bounds);
        }

        Vector3 localCenter = bounds.center - transform.position;
        bounds.center = localCenter;
        

        transform.rotation = currentRotation;

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
                break;
            case 2: // Guard Settings
                obj.transform.localScale = new Vector3(obj.transform.localScale.x + Random.Range(-.5f, .5f), obj.transform.localScale.y, obj.transform.localScale.z);
                break;
            case 3: // Sword Settings
                obj.transform.position -= offset * 1.5f;

                obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y + Random.Range(-.5f, .5f), obj.transform.localScale.z);

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

    private void FixedUpdate() {
        transform.GetChild(6).transform.Rotate(Vector3.down);

        if (shake) {
            Shake();
        }
    }

    public void SetName() {
        name = itemValue.ToString();
    }
}
