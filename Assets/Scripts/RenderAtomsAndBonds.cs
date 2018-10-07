using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderAtomsAndBonds : MonoBehaviour {
    public BondPrefab bondPrefab;
    public AtomPrefab atomPrefab;
    public string filePath;
    private Utility utility = new Utility();

    public void RenderAtoms()
    {
        for (int atomIndex = 0; atomIndex < utility.numAtoms; atomIndex++)
        {
            //Debug.Log(atomIndex + " , " +
            //    utility.atoms[atomIndex].iatom + " , " +
            //    utility.atoms[atomIndex].rr0[0] + " , " +
            //    utility.atoms[atomIndex].rr0[1] + " , " +
            //    utility.atoms[atomIndex].rr0[2]);
            AtomPrefab atom = (AtomPrefab)Instantiate(atomPrefab);
            atom.Coordinates = new Vector3(
                (float)utility.atoms[atomIndex].rr0[0],
                (float)utility.atoms[atomIndex].rr0[1],
                (float)utility.atoms[atomIndex].rr0[2]);
            switch (utility.atoms[atomIndex].iatom)
            {
                case "H":
                    atom.AtomColor = new Color(0, 1, 0, 1);
                    break;
                case "C":
                    atom.AtomColor = new Color(0, 0, 0, 1);
                    break;
                case "O":
                    atom.AtomColor = new Color(1, 0, 0, 1);
                    break;
                case "N":
                    atom.AtomColor = new Color(1, 1, 1, 1);
                    break;
                default:
                    atom.AtomColor = new Color(0, 1, 1, 1);
                    break;
            }
        }
    }

    public void RenderBonds()
    {
        for(int bondIndex = 0; bondIndex < utility.numAtoms; bondIndex++)
        {
            //Debug.Log(bondIndex);
            if (utility.atomBonds[bondIndex, 0] == 0)
                continue;
            for(int neighbourIndex = 1; neighbourIndex <= utility.atomBonds[bondIndex,0]; neighbourIndex++)
            {
                BondPrefab bond = (BondPrefab)Instantiate(bondPrefab);
                bond.StartPosition = new Vector3(
                    (float)utility.atoms[bondIndex].rr0[0],
                    (float)utility.atoms[bondIndex].rr0[1],
                    (float)utility.atoms[bondIndex].rr0[2]);
                bond.EndPosition = new Vector3(
                    (float)utility.atoms[utility.atomBonds[bondIndex, neighbourIndex]].rr0[0],
                    (float)utility.atoms[utility.atomBonds[bondIndex, neighbourIndex]].rr0[1],
                    (float)utility.atoms[utility.atomBonds[bondIndex, neighbourIndex]].rr0[2]);
                bond.BondColor = new Color(0, 0, 0, 0);
            }
        }
    } 
    // Use this for initialization
    void Start ()
    {
        utility.FilePath = filePath;
        utility.GetStructureData();
        //RenderAtoms();
        RenderBonds();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
