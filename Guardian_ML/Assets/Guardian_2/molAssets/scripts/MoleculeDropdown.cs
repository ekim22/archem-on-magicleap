
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MoleculeDropdown : MonoBehaviour {

	public Dropdown dropdown;
	//public GuardianARController ar;

	public Dictionary<string, int> Molecules { get; set; } = new Dictionary<string, int>();

	// Start is called before the first frame update
	void Start() {
		PopulateList();

		//dropdown.onValueChanged.AddListener(delegate {
		//	DropdownValueChanged(dropdown);
		//});

	}

	//void DropdownValueChanged(Dropdown dropdown) {
	//	ar.MoleculeCID = dropdown.options.ElementAt(dropdown.value).text;
	//}

	void PopulateList() {
		Molecules.Add("<<from camera>>", -1);
		Molecules.Add("1 - (Bis(4 - fluorophenyl)methyl)piperazine", 152932);
		Molecules.Add("1 - Butanol", 263);
		Molecules.Add("2 - Butanol", 6568);
		Molecules.Add("2 - Butanone", 6569);
		Molecules.Add("Benzene", 241);
		Molecules.Add("Butanal", 261);
		Molecules.Add("Butane", 7843);
		Molecules.Add("Butanoic Acid", 264);
		Molecules.Add("Ciprofloxacin", 2764);
		Molecules.Add("Cyclohexane", 8078);
		Molecules.Add("Water", 962);
		Molecules.Add("Aspirin", 2244);
		Molecules.Add("Glucose", 5793);

		List<string> list = Molecules.Keys.ToList();

        dropdown.AddOptions(list);
       
        //ADDED BY RICHARD SMITH
        //
        Transform viewport = dropdown.template.transform.Find("Viewport");
        Transform content = viewport.transform.Find("Content");
        content.transform.Find("Item").GetComponent<Toggle>().image.color = Color.white;

		//dropdown.value = list.IndexOf("Glucose");
        if(CameraMolecule.moleculeCID == null) {
            dropdown.value = list.IndexOf("Glucose");
        }
        else {
            dropdown.value = list.IndexOf(CameraMolecule.moleculeCID);
        }
	}
}
