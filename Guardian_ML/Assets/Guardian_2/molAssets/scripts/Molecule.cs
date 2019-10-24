using System.Collections.Generic;
using UnityEngine;

public class Molecule : List<Atom>
{

	private string name;
	private List<Bond> bonds;

	public Molecule (MoleculePOCO root, string compound, double scale)
	{
		// coerce the json structure into something more usable: each compound is a
		// molecule which is a list of atoms
		List<int> elements = null;
		List<int> aid = null;
		List<double> xCoords = null, yCoords = null, zCoords = null;

		foreach (PCCompound cmp in root.PC_Compounds) {
			elements = cmp.atoms.element;
			aid = cmp.atoms.aid;
			foreach (Coord coord in cmp.coords) {
				foreach (Conformer conformer in coord.conformers) {
					xCoords = conformer.x; // get the xyz location for each atom, as a List
					yCoords = conformer.y;
					zCoords = conformer.z;
				}
			}
		}
		this.name = compound;
        for (int i = 0; i < elements.Count; i++) {
            Add(new Atom (elements [i], aid [i], (float)xCoords [i], (float)yCoords [i], 
				(float)zCoords [i], scale));
		}

		// process bonds, coerce into something more friendly
		bonds = new List<Bond>();
		foreach (PCCompound cmp in root.PC_Compounds) {
			for (int i = 0; i < cmp.bonds.aid1.Count; i++) {
				Atom start = getById (cmp.bonds.aid1 [i]); //aid1: array of starting points
				Atom end = getById (cmp.bonds.aid2 [i]);   //aid2: array of ending points
				int order = cmp.bonds.order [i];
				bonds.Add (new Bond(start, end, order));
			}
		}
	}

	public List<Bond> getBonds () {
		return bonds;
	}

	public List<Atom> getAtoms () {
		return this;
	}

	private Atom getById (int index) {
		foreach (Atom a in this)
			if (a.Id == index) return a;
		return null;
	}

	public override string ToString () {
		string result = string.Format ("[Molecule: name={0}]", name);
		foreach (Atom atom in this)
			result += atom.ToString ();
		return result;
	}
}

public class Bond {
	public Atom Start { get; set; }
	public Atom End { get; set; }
	public int Order { get; set; }

	public Bond (Atom s, Atom e, int o) {
		Start = s;
		End = e;
		Order = o;
	}

	public override string ToString () {
		return string.Format ("Bond {0} (id={1}) <=> {2} (id={3}) order={4}", 
		Start.Symbol, Start.Id, End.Symbol, End.Id, Order);
	}
}

public class Atom {

	private const double ANGSTROM = 1.0E-10D;
	private static Dictionary<int, Color> colors;
	private static Dictionary<int, float> radii;
	private static Dictionary<int, string> symbols;

	// note setters (set;} included for interop w/Holo
	public int Number { get; set; }        // atomic number
	public string Symbol { get; set; }     // line H, He, C, O
	public Color color { get; set; }       // customary color for 3d sphere
	public Vector3 Location { get; set; }
	public float Radius { get; set; }
	public int Id { get; set; }            // numeric identifier from input file
	public double Scale { get; set; }

	static Atom ()
	{
		radii = new Dictionary<int,float> ();
		symbols = new Dictionary<int,string> ();
		colors = new Dictionary<int,Color> ();
		radii.Add (1, 0.53f); // atomic number, atomic radius in angstroms
		radii.Add (2, 0.31f);
		radii.Add (3, 1.67f);
		radii.Add (4, 1.12f);
		radii.Add (5, 0.87f);
		radii.Add (6, 0.67f);
		radii.Add (7, 0.56f);
		radii.Add (8, 0.48f);
		radii.Add (9, 0.42f);
		radii.Add (10, 0.38f);
		radii.Add (11, 1.90f);
		radii.Add (12, 1.45f);
		radii.Add (13, 1.18f);
		radii.Add (14, 1.11f);
		radii.Add (15, 0.98f);
		radii.Add (16, 0.88f);
		radii.Add (17, 0.79f);
		radii.Add (18, 0.71f);
		radii.Add (19, 2.43f);
		radii.Add (20, 1.94f);
		radii.Add (21, 1.84f);
		radii.Add (22, 1.76f);
		radii.Add (23, 1.71f);
		radii.Add (24, 1.66f);
		radii.Add (25, 1.61f);
		radii.Add (26, 1.56f);
		radii.Add (27, 1.52f);
		radii.Add (28, 1.49f);
		radii.Add (29, 1.45f);
		radii.Add (30, 1.42f);
		radii.Add (31, 1.36f);
		radii.Add (32, 1.25f);
		radii.Add (33, 1.14f);
		radii.Add (34, 1.03f);
		radii.Add (35, 0.94f);
		radii.Add (36, 0.88f);
		radii.Add (37, 2.65f);
		radii.Add (38, 2.19f);
		radii.Add (39, 2.12f);
		radii.Add (40, 2.06f);
		radii.Add (41, 1.98f);
		radii.Add (42, 1.90f);
		radii.Add (43, 1.83f);
		radii.Add (44, 1.78f);
		radii.Add (45, 1.73f);
		radii.Add (46, 1.69f);
		radii.Add (47, 1.65f);
		radii.Add (48, 1.61f);
		radii.Add (49, 1.56f);
		radii.Add (50, 1.45f);
		radii.Add (51, 1.33f);
		radii.Add (52, 1.23f);
		radii.Add (53, 1.15f);
		radii.Add (54, 1.08f);
		radii.Add (55, 2.98f);
		radii.Add (56, 2.53f);
		radii.Add (57, 1.95f);
		radii.Add (58, 1.85f);
		radii.Add (59, 2.47f);
		radii.Add (60, 2.06f);
		radii.Add (61, 2.05f);
		radii.Add (62, 2.38f);
		radii.Add (63, 2.31f);
		radii.Add (64, 2.33f);
		radii.Add (65, 2.25f);
		radii.Add (66, 2.28f);
		radii.Add (67, 2.26f);
		radii.Add (68, 2.26f);
		radii.Add (69, 2.22f);
		radii.Add (70, 2.22f);
		radii.Add (71, 2.17f);
		radii.Add (72, 2.08f);
		radii.Add (73, 2.00f);
		radii.Add (74, 1.93f);
		radii.Add (75, 1.88f);
		radii.Add (76, 1.85f);
		radii.Add (77, 1.80f);
		radii.Add (78, 1.77f);
		radii.Add (79, 1.74f);
		radii.Add (80, 1.71f);
		radii.Add (81, 1.56f);
		radii.Add (82, 1.54f);
		radii.Add (83, 1.43f);
		radii.Add (84, 1.35f);
		radii.Add (85, 1.27f);
		radii.Add (86, 1.20f);
		radii.Add (87, 0.00f);
		radii.Add (88, 0.00f);
		radii.Add (89, 1.95f);
		radii.Add (90, 1.80f);
		radii.Add (91, 1.80f);
		radii.Add (92, 1.75f);
		radii.Add (93, 1.75f);
		radii.Add (94, 1.75f);
		radii.Add (95, 1.75f);
		radii.Add (96, 0.00f);
		
		
		symbols.Add (1, "H"); //hydrogen
		symbols.Add (2, "He"); //helium
		symbols.Add (3, "Li"); //lithium
		symbols.Add (4, "Be"); //beryllium
		symbols.Add (5, "B"); //boron
		symbols.Add (6, "C"); //carbon
		symbols.Add (7, "N"); //nitrogen
		symbols.Add (8, "O"); //oxygen
		symbols.Add (9, "F"); //fluorine
		symbols.Add (10, "Ne"); //neon
		symbols.Add (11, "Na"); //sodium
		symbols.Add (12, "Mg"); //magnesium
		symbols.Add (13, "Al"); //aluminium
		symbols.Add (14, "Si"); //silicon
		symbols.Add (15, "P"); //phosphorus
		symbols.Add (16, "S"); //sulphur
		symbols.Add (17, "Cl"); //chlorine
		symbols.Add (18, "Ar"); //argon
		symbols.Add (19, "K"); //potassium
		symbols.Add (20, "Ca"); //calcium
		symbols.Add (21, "Sc"); //scandium
		symbols.Add (22, "Ti"); //titanium
		symbols.Add (23, "V"); //vanadium
		symbols.Add (24, "Cr"); //chromium
		symbols.Add (25, "Mn"); //manganese
		symbols.Add (26, "Fe"); //iron
		symbols.Add (27, "Co"); //colbalt
		symbols.Add (28, "Ni"); //nickel
		symbols.Add (29, "Cu"); //copper
		symbols.Add (30, "Zn"); //zinc
		symbols.Add (31, "Ga"); //gallium
		symbols.Add (32, "Ge"); //germanium
		symbols.Add (33, "As"); //arsenic
		symbols.Add (34, "Se"); //selenium
		symbols.Add (35, "Br"); //bromine
		symbols.Add (36, "Kr"); //krypton
		symbols.Add (37, "Rb"); //rubidium
		symbols.Add (38, "Sr"); //strontium
		symbols.Add (39, "Y"); //yttrium
		symbols.Add (40, "Zr"); //zirconium
		symbols.Add (41, "Nb"); //niobium
		symbols.Add (42, "Mo"); //molybdenum
		symbols.Add (43, "Tc"); //techetium
		symbols.Add (44, "Ru"); //ruthenium
		symbols.Add (45, "Rh"); //rhodium
		symbols.Add (46, "Pd"); //palladium
		symbols.Add (47, "Ag"); //silver
		symbols.Add (48, "Cd"); //cadmium
		symbols.Add (49, "In"); //indium
		symbols.Add (50, "Sn"); //tin
		symbols.Add (51, "Sb"); //antimony
		symbols.Add (52, "Te"); //tellurium
		symbols.Add (53, "I"); //iodine
		symbols.Add (54, "Xe"); //xenon
		symbols.Add (55, "Cs"); //caesium
		symbols.Add (56, "Ba"); //barium
		symbols.Add (57, "La"); //lanthanum
		symbols.Add (58, "Ce"); //cerium
		symbols.Add (59, "Pr"); //praseodymium
		symbols.Add (60, "Nd"); //neodymium
		symbols.Add (61, "Pm"); //promethium
		symbols.Add (62, "Sm"); //samarium
		symbols.Add (63, "Eu"); //europium
		symbols.Add (64, "Gd"); //gadolinium
		symbols.Add (65, "Tb"); //terbium
		symbols.Add (66, "Dy"); //dysprosium
		symbols.Add (67, "Ho"); //holmium
		symbols.Add (68, "Er"); //erbium
		symbols.Add (69, "Tm"); //thulium
		symbols.Add (70, "Yb"); //ytterbium
		symbols.Add (71, "Lu"); //lutetium
		symbols.Add (72, "Hf"); //hafnium
		symbols.Add (73, "Ta"); //tantalum
		symbols.Add (74, "W"); //tungsten
		symbols.Add (75, "Re"); //rhenium
		symbols.Add (76, "Os"); //osmium
		symbols.Add (77, "Ir"); //iridium
		symbols.Add (78, "Pt"); //platinum
		symbols.Add (79, "Au"); //gold
		symbols.Add (80, "Hg"); //mercury
		symbols.Add (81, "Tl"); //thallium
		symbols.Add (82, "Pb"); //lead
		symbols.Add (83, "Bi"); //bismuth
		symbols.Add (84, "Po"); //polonium
		symbols.Add (85, "At"); //astatine
		symbols.Add (86, "Rn"); //radon
		symbols.Add (87, "Fr"); //franciium
		symbols.Add (88, "Ra"); //radium
		symbols.Add (89, "Ac"); //actinium
		symbols.Add (90, "Th"); //thorium 
		symbols.Add (91, "Pa"); //protactinium
		symbols.Add (92, "U"); //uranium
		symbols.Add (93, "Np"); //neptunium
		symbols.Add (94, "Pu"); //plutonium
		symbols.Add (95, "Am"); //americium
		symbols.Add (96, "Cm"); //curium
		
		/* additional elements on the table
		symbols.Add (97, "Bk"); //berkelium
		symbols.Add (98, "Cf"); //californium
		symbols.Add (99, "Es"); //einsteinium
		symbols.Add (100, "Fm"); //fermium
		symbols.Add (101, "Md"); //mendelevium
		symbols.Add (102, "No"); //nobelium
		symbols.Add (103, "Lr"); //lawrencium
		*/ 
		

		//color values according to Jmol.sourceforge.net
		colors.Add (1, Color.white);
		colors.Add (2, new Color32 (217, 255, 255, 255)); 
		colors.Add (3, new Color32 (204, 128, 255, 255));
		colors.Add (4, new Color32 (194, 255, 0, 255));
		colors.Add (5, new Color32 (255, 181, 181, 255));
		colors.Add (6, new Color32 (144, 144, 144, 255));
		colors.Add (7, new Color32 (48, 80, 248, 255));
		colors.Add (8, Color.red);
		colors.Add (9, new Color32 (144, 224, 80, 255));
		colors.Add (10, new Color32 (179, 227, 245, 255));
		colors.Add (11, new Color32 (171, 92, 242, 255));
		colors.Add (12, new Color32 (138, 255, 0, 255));
		colors.Add (13, new Color32 (191, 166, 166, 255));
		colors.Add (14, new Color32 (240, 200, 160, 255));
		colors.Add (15, new Color32 (255, 128, 0, 255));
		colors.Add (16, new Color32 (255, 255, 48, 255));
		colors.Add (17, new Color32 (31, 240, 31, 255));
		colors.Add (18, new Color32 (128, 209, 227, 255));
		colors.Add (19, new Color32 (143, 64, 212, 255));
		colors.Add (20, new Color32 (61, 255, 0, 255));
		colors.Add (21, new Color32 (230, 230, 230, 255));
		colors.Add (22, new Color32 (191, 194, 199, 255));
		colors.Add (23, new Color32 (166, 166, 171, 255));
		colors.Add (24, new Color32 (138, 153, 199, 255));
		colors.Add (25, new Color32 (156, 122, 199, 255));
		colors.Add (26, new Color32 (224, 102, 51, 255));
		colors.Add (27, new Color32 (240, 144, 160, 255));
		colors.Add (28, new Color32 (80, 208, 80, 255));
		colors.Add (29, new Color32 (200, 128, 51, 255));
		colors.Add (30, new Color32 (125, 128, 176, 255));
		colors.Add (31, new Color32 (194, 143, 143, 255));
		colors.Add (32, new Color32 (102, 143, 143, 255));
		colors.Add (33, new Color32 (189, 128, 227, 255));
		colors.Add (34, new Color32 (255, 161, 0, 255));
		colors.Add (35, new Color32 (166, 41, 41, 255));
		colors.Add (36, new Color32 (92, 184, 209, 255));
		colors.Add (37, new Color32 (112, 46, 176, 255));
		colors.Add (38, new Color32 (0, 255, 0, 255));
		colors.Add (39, new Color32 (148, 255, 255, 255));
		colors.Add (40, new Color32 (148, 224, 224, 255));
		colors.Add (41, new Color32 (115, 194, 201, 255));
		colors.Add (42, new Color32 (84, 181, 181, 255));
		colors.Add (43, new Color32 (59, 158, 158, 255));
		colors.Add (44, new Color32 (36, 143, 143, 255));
		colors.Add (45, new Color32 (10, 125, 140, 255));
		colors.Add (46, new Color32 (0, 105, 133, 255));
		colors.Add (47, new Color32 (192, 192, 192, 255));
		colors.Add (48, new Color32 (255, 217, 143, 255));
		colors.Add (49, new Color32 (166, 117, 115, 255));
		colors.Add (50, new Color32 (102, 128, 128, 255));
		colors.Add (51, new Color32 (158, 99, 181, 255));
		colors.Add (52, new Color32 (212, 122, 0, 255));
		colors.Add (53, new Color32 (148, 0, 148, 255));
		colors.Add (54, new Color32 (66, 158, 176, 255));
		colors.Add (55, new Color32 (87, 23, 143, 255));
		colors.Add (56, new Color32 (0, 201, 0, 255));
		colors.Add (57, new Color32 (112, 212, 255, 255));
		colors.Add (58, new Color32 (255, 255, 199, 255));
		colors.Add (59, new Color32 (217, 255, 199, 255));
		colors.Add (60, new Color32 (199, 255, 199, 255));
		colors.Add (61, new Color32 (163, 255, 199, 255));
		colors.Add (62, new Color32 (143, 255, 199, 255));
		colors.Add (63, new Color32 (97, 255, 199, 255));
		colors.Add (64, new Color32 (69, 255, 199, 255));
		colors.Add (65, new Color32 (48, 255, 199, 255));
		colors.Add (66, new Color32 (31, 255, 199, 255));
		colors.Add (67, new Color32 (0, 255,156, 255));
		colors.Add (68, new Color32 (0, 230, 117, 255));
		colors.Add (69, new Color32 (0, 212, 82, 255));
		colors.Add (70, new Color32 (0, 191, 56, 255));
		colors.Add (71, new Color32 (0, 171, 36, 255));
		colors.Add (72, new Color32 (77, 194, 255, 255));
		colors.Add (73, new Color32 (77, 166, 255, 255));
		colors.Add (74, new Color32 (33, 148, 214, 255));
		colors.Add (75, new Color32 (38, 125, 171, 255));
		colors.Add (76, new Color32 (38, 102, 150, 255));
		colors.Add (77, new Color32 (23, 84, 135, 255));
		colors.Add (78, new Color32 (208, 208, 224, 255));
		colors.Add (79, new Color32 (255, 209, 35, 255));
		colors.Add (80, new Color32 (184, 184, 208, 255));
		colors.Add (81, new Color32 (166, 84, 77, 255));
		colors.Add (82, new Color32 (87, 89, 97, 255));
		colors.Add (83, new Color32 (158, 79, 181, 255));
		colors.Add (84, new Color32 (171, 92, 0, 255));
		colors.Add (85, new Color32 (117, 79, 69, 255));
		colors.Add (86, new Color32 (66, 130, 150, 255));
		colors.Add (87, new Color32 (66, 0, 102, 255));
		colors.Add (88, new Color32 (0, 125, 0, 255));
		colors.Add (89, new Color32 (112, 171, 250, 255));
		colors.Add (90, new Color32 (0, 186, 255, 255));
		colors.Add (91, new Color32 (0, 161, 255, 255));
		colors.Add (92, new Color32 (0, 143, 255, 255));
		colors.Add (93, new Color32 (0, 128, 255, 255));
		colors.Add (94, new Color32 (0, 107, 255, 255));
		colors.Add (95, new Color32 (84, 92, 242, 255));
		colors.Add (96, new Color32 (120, 92, 227, 255));	
	}

	public Atom (int n, int id, double x, double y, double z) : this(n, id, x, y, z, 1.0D) {
		// 5 args with 6th set to 1 for life scale
	}

	public Atom (int n, int id, double x, double y, double z, double scale) {
		// create an atom
		// find the type (H, He, O, C, etc.) and look up the radius, color etc.
		Number = n;
		Id = id;
		Radius = (float)(radii [Number] * ANGSTROM * scale);
		Symbol = symbols [Number];
		color = colors [Number];
		Scale = scale;  // supply scale=1.0d for actual size and distance
		Location = new Vector3 ((float)(x * ANGSTROM * scale), (float)(y * ANGSTROM *scale), (float)(z * ANGSTROM * scale));
	}

	public override string ToString () {
		return string.Format ("[Atom: id={1} atomic number={0}, symbol={3}, color={4}, " +
			"location={5}, radius={2}, scale={6}]", Id, Number, Symbol, color, Location, Radius, Scale);
	}
}