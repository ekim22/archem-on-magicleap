using System.Collections.Generic;
using System.Linq;

// initially generated with http://json2csharp.com/
// had to further modify:
//  - convert all Properties to class fields, otherwise JSon Utility generated nulls
//  - added ToStrings to all classes
//  - made each class Serializable with [System.Serializable]

[System.Serializable]
public class MoleculePOCO {
	public List<PCCompound> PC_Compounds;
	public override string ToString () {
        string compounds = string.Join(",", PC_Compounds.Select(n => n.ToString()).ToArray());
        return string.Format ("[RootObject: PC_Compounds={0}]", compounds);
	}
}	

[System.Serializable]
public class Id2 {
	public int cid;
	public override string ToString () {
		return string.Format ("[Id2: cid={0}]", cid);
	}
}

[System.Serializable]
public class Id {
	public Id2 id;
	public override string ToString () {
		return string.Format ("[Id: id={0}]", id);
	}
}

[System.Serializable]
public class Atoms {
	public List<int> aid, element;
	public override string ToString () {
		return string.Format ("[Atoms: aid={0}, element={1}]", 
			string.Join (",", aid.Select (n => n.ToString ()).ToArray ()),
			string.Join (",", element.Select(n => n.ToString()).ToArray ()));
	}
}

[System.Serializable]
public class Bonds {
	public List<int> aid1, aid2, order;
	public override string ToString () {
		return string.Format ("[Bonds: aid1={0}, aid2={1}, order={2}]", 
			string.Join (",", aid1.Select (n => n.ToString ()).ToArray ()), 
			string.Join (",", aid2.Select (n => n.ToString ()).ToArray ()), 
			string.Join (",", order.Select (n => n.ToString ()).ToArray ()));
	}
}

[System.Serializable]
public class Urn {
	public string label, name, version, software, source, release;
	public int datatype;
	public override string ToString () {
		return string.Format ("[Urn: label={0}, name={1}, datatype={2}, version={3}, software={4}, " +
			"source={5}, release={6}]", label, name, datatype, version, software, source, release);
	}
}

[System.Serializable]
public class Value {
	public string sval;
	public double? fval;
	public List<string> slist;
	public List<double> fvec;
	public override string ToString () {
		return string.Format ("[Value: sval={0}, fval={1}, slist={2}, fvec={3}]", 
			sval, 
			fval, 
			string.Join (",", slist.Select (n => n.ToString ()).ToArray ()), 
			string.Join (",", fvec.Select (n => n.ToString ()).ToArray ()));
	}
}

[System.Serializable]
public class Datum {
	public Urn urn;
	public Value value;
	public override string ToString () {
		return string.Format ("[Datum: urn={0}, value={1}]", urn, value);
	}
}

[System.Serializable]
public class Conformer {
	public List<double> x, y, z;
	public List<Datum> data;
	public override string ToString () {
		return string.Format ("[Conformer: x={0}, y={1}, z={2}, data={3}]", 
			string.Join (",", x.Select (n => n.ToString ()).ToArray ()),
			string.Join (",", y.Select (n => n.ToString ()).ToArray ()),
			string.Join (",", z.Select (n => n.ToString ()).ToArray ()),
			string.Join (",", data.Select (n => n.ToString ()).ToArray ()));
	}
}

[System.Serializable]
public class Urn2 {
	public string label, name, release;
	public int datatype;
	public override string ToString () {
		return string.Format ("[Urn2: label={0}, name={1}, datatype={2}, release={3}]", 
			label, name, datatype, release);
	}
}

[System.Serializable]
public class Value2 {
	public double fval;
	public List<int> ivec;
	public override string ToString () {
		return string.Format ("[Value2: fval={0}, ivec={1}]", 
			fval, 
			string.Join (",", ivec.Select (n => n.ToString ()).ToArray ()));
	}
}

[System.Serializable]
public class Datum2 {
	public Urn2 urn;
	public Value2 value;
	public override string ToString () {
		return string.Format ("[Datum2: urn={0}, value={1}]", urn, value);
	}
}

[System.Serializable]
public class Coord {
	public List<int> type, aid;
	public List<Conformer> conformers;
	public List<Datum2> data;
	public override string ToString () {
		return string.Format ("[Coord: type={0}, aid={1}, conformers={2}, data={3}]", 
			string.Join (",", type.Select (n => n.ToString ()).ToArray ()), 
			string.Join (",", aid.Select (n => n.ToString ()).ToArray ()), 
			string.Join (",", conformers.Select (n => n.ToString ()).ToArray ()), 
			string.Join (",", data.Select (n => n.ToString ()).ToArray ()));
	}
}

[System.Serializable]
public class Urn3 {
	public string label, name, version, software, source, release, parameters;
	public int datatype;
	public override string ToString () {
		return string.Format ("[Urn3: label={0}, name={1}, datatype={2}, version={3}, " +
			"software={4}, source={5}, release={6}, parameters={7}]", label, name, 
			datatype, version, software, source, release, parameters);
	}
}

[System.Serializable]
public class Value3 {
	public List<string> slist;
	public int? fval;
	public override string ToString () {
		return string.Format ("[Value3: slist={0}, fval={1}]", 
			string.Join (",", slist.Select (n => n.ToString ()).ToArray()), 
			fval);
	}
}

[System.Serializable]
public class Prop {
	public Urn3 urn;
	public Value3 value;
	public override string ToString () {
		return string.Format ("[Prop: urn={0}, value={1}]", urn, value);
	}
}

[System.Serializable]
public class Count {
	public int heavy_atom, atom_chiral, atom_chiral_def, atom_chiral_undef, bond_chiral, bond_chiral_def, bond_chiral_undef, isotope_atom, covalent_unit, tautomers;
	public override string ToString () {
		return string.Format ("[Count: heavy_atom={0}, atom_chiral={1}, atom_chiral_def={2}, " +
			"atom_chiral_undef={3}, bond_chiral={4}, bond_chiral_def={5}, bond_chiral_undef={6}, " +
			"isotope_atom={7}, covalent_unit={8}, tautomers={9}]", heavy_atom, atom_chiral, 
			atom_chiral_def, atom_chiral_undef, bond_chiral, bond_chiral_def, bond_chiral_undef, 
			isotope_atom, covalent_unit, tautomers);
	}
}

[System.Serializable]
public class PCCompound {
	public Id id;
	public Atoms atoms;
	public Bonds bonds;
	public List<Coord> coords;
	public List<Prop> props;
	public Count count;
	public override string ToString () {
		return string.Format ("[PCCompound: id={0}, atoms={1}, bonds={2}, coords={3}, props={4}, count={5}]", 
			id, 
			atoms, 
			bonds, 
			string.Join (",", coords.Select (n => n.ToString ()).ToArray ()), 
			string.Join (",", props.Select (n => n.ToString ()).ToArray ()), 
			count);
	}
}