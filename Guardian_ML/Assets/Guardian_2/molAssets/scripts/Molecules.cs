using System.Collections;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using UnityEngine.Events;

public class Molecules : MonoBehaviour {

    private const string MOLECULE_TAG = "Molecule";
	private const string ATOM_TAG = "atom";
	private const string HALF_BOND_TAG = "half_bond";
	private const string BOND_TAG = "bond";

	private const string URL_BASE = "https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/cid/";
    private const string URL_PARAMS = "/JSON?record_type=3d";
    private const double ANGSTROM = 1.00E-10D;

	private string CACHE_PATH;
	private Shader shaderTransparentDiffuse;

	public GameObject colorKeyContent;
    public GameObject colorKey;

	// Start is called before the first frame update
	void Start() {
		CACHE_PATH = Application.persistentDataPath + "/";
		Debug.Log("Files will be cached at: " + CACHE_PATH);
		shaderTransparentDiffuse = Shader.Find("Legacy Shaders/Transparent/Diffuse");

		// preload the cache with some demo compounds
		string[] cids = { "241", "261", "263", "264", "962", "2244", "2764", "5793", "6568",
			"6569", "7843", "8078", "152932" };
		foreach (string cid in cids) {
			TextAsset t = Resources.Load<TextAsset>("PubchemCache/Molecules/" + cid);
			if (t != null) {
				Save(cid, t.text);
			}
		}
	}

    public void AppendMolecule(string cid, Pose pose) {
        StartCoroutine(GetMolecule(cid, pose));
    }

    IEnumerator GetMolecule(string compound, Pose pose) {

        string jsonString = "";

        if (IsCached(compound)) {
            jsonString = Load(compound);
        } else {
            string url = URL_BASE + compound + URL_PARAMS;    // Retrieving Molecule using CID
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
			if (www.isNetworkError || www.isHttpError)
				Debug.Log(www.error);
			else
				jsonString = Save(compound, www.downloadHandler.text);
        }

        GameObject go = CreateCompound(compound, jsonString);
		go.transform.Translate(pose.position);
	}

	void SetTransparency(GameObject go, float transparency) {
		foreach (Transform item in go.GetComponentsInChildren<Transform>()) {
			if (item.CompareTag(ATOM_TAG) || item.CompareTag(HALF_BOND_TAG)) {
				Color color = item.GetComponent<Renderer>().material.color;
				color.a = transparency; // sets the alpha value
				item.GetComponent<Renderer>().material.color = color;
			}
		}
	}

	private GameObject CreateCompound(string compound, string jsonString) {

		GameObject moleculeGameObject = new GameObject(compound);
        moleculeGameObject.tag = MOLECULE_TAG;
        moleculeGameObject.transform.parent = this.transform; // parent is GameObject that holds all spawned molecules
															 
	    //
        // Deleted all leanfinger code.
        //

        GameObject atomsGameObject = new GameObject("atoms");
        atomsGameObject.transform.parent = moleculeGameObject.transform;
		atomsGameObject.transform.SetParent(moleculeGameObject.transform, false);
		GameObject bondsGameObject = new GameObject("bonds");
        bondsGameObject.transform.parent = moleculeGameObject.transform;
		bondsGameObject.transform.SetParent(moleculeGameObject.transform, false);

		double scale = 1.0 / (ANGSTROM * 10.0); // essentially, mult by 1*10**10 then div by 10; C radius will be 0.67 m, about 2.6 inches
        Molecule molecule = new Molecule(JsonUtility.FromJson<MoleculePOCO>(jsonString), compound, scale);

		bool keyExists = false;

		foreach (Atom atom in molecule.getAtoms()) { // insert a sphere for each atom
            GameObject atomGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            atomGameObject.name = atom.Symbol;
			atomGameObject.tag = ATOM_TAG;
            atomGameObject.transform.localScale = Vector3.one * atom.Radius;
            atomGameObject.transform.position = atom.Location;
			Material mat = atomGameObject.GetComponent<Renderer>().material;
			mat.color = atom.color; // apply color
			mat.shader = shaderTransparentDiffuse;
			atomGameObject.transform.SetParent(atomsGameObject.transform, false);
				

			keyExists = false;
            for (int i = 0; i < colorKeyContent.transform.childCount; i++) {
                GameObject child = colorKeyContent.transform.GetChild(i).gameObject;
                if (child.GetComponentInChildren<Text>().text.Equals(atom.Symbol))
                    keyExists |= true;
            }
            if (!keyExists) {
                addColorKey(atom);
            }
        }

		// handle the bonds: // insert a cylinder for each bond
		foreach (Bond b in molecule.getBonds()) {
            GameObject bondGameObject = new GameObject(b.ToString());
			bondGameObject.tag = BOND_TAG;
			halfBond(0.75f, b, bondGameObject);   //each half-bond will match the color of neighboring atom
			halfBond(0.25f, b, bondGameObject);
            bondGameObject.transform.SetParent(bondsGameObject.transform, false);
        }
		return moleculeGameObject;
    }

    private void halfBond(float bias, Bond bond, GameObject parent) {

        var r = (bond.Start.Radius + bond.End.Radius) / 4;
        var offset = new Vector3(0, 0, r / 2); // used for double and triple bonds
        var start = bond.Start.Location;
        switch (bond.Order)
        {
            case 1:
                halfCyl(bond, "Bond", start, bias, r, parent);
                break;
            case 2:
                halfCyl(bond, "Left Bond", start - offset, bias, r * 0.75f, parent);
                halfCyl(bond, "Right Bond", start + offset, bias, r * 0.75f, parent);
                break;
            case 3:
                halfCyl(bond, "Center Bond", start, bias, r / 3, parent);
                halfCyl(bond, "Left Bond", start - offset, bias, r * 0.5f, parent);
                halfCyl(bond, "Right Bond", start + offset, bias, r * 0.5f, parent);
                break;
            default:
                Debug.LogWarning("Warning: order of bond = " + bond.Order + ", not supported");
                break;
        }
    }

    private void halfCyl(Bond bond, string label, Vector3 beg, float bias, float r, GameObject parent) {

		var v3Bond = bond.End.Location - bond.Start.Location;
        var neighbor = bias < 0.5 ? bond.Start : bond.End;
        Quaternion bondAngle = Quaternion.FromToRotation(Vector3.up, v3Bond);
        GameObject cyl = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		cyl.tag = "half_bond";
        cyl.name = label + " " + neighbor.Symbol + " (id=" + neighbor.Id + ")";
        cyl.transform.position = bias * v3Bond + beg;
        cyl.transform.localScale = new Vector3(r, v3Bond.magnitude / 4, r);
        cyl.transform.rotation = bondAngle;
		Material mat = cyl.GetComponent<Renderer>().material;
		mat.color = neighbor.color;
		mat.shader = shaderTransparentDiffuse;
		cyl.transform.SetParent(parent.transform, false);
    }

    private string getFileName(string compound) {
        return CACHE_PATH + compound + ".txt";
    }

    private bool IsCached(string compound) {
        return File.Exists(getFileName(compound));
    }

    private string Save(string compound, string json) {

        string fileName = getFileName(compound);
		if (!File.Exists(fileName)) {
            File.WriteAllText(fileName, json);
        }
        return json;
    }

    private string Load(string compound)
    {
        string fileName = getFileName(compound);
        string json = null;

        if (File.Exists(fileName)) {
            json = File.ReadAllText(fileName);
        }
        return json;
    }

    private void addColorKey(Atom atom) {
        GameObject temp = Instantiate(colorKey);
        temp.transform.parent = colorKeyContent.transform;
        temp.transform.GetChild(0).GetComponent<Image>().color = atom.color;
        temp.transform.GetChild(1).GetComponent<Text>().text = atom.Symbol;
    }

}
