using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderAtomsAndBonds : MonoBehaviour {
    public BondPrefab bondPrefab;
    public AtomPrefab atomPrefab;
    public string filePath;
    public double[] newCenter = new double[3];
    private Utility utility = new Utility();

    public bool calledUtility = false;

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
            atom.Parent = this.gameObject.name;
            atom.Coordinates = new Vector3(
                (float)utility.atoms[atomIndex].rr0[0],
                (float)utility.atoms[atomIndex].rr0[1],
                (float)utility.atoms[atomIndex].rr0[2]);

            float[] scaleFactorH = new float[3];
            for(int index = 0; index < 3; index++)
            {
                if (utility.scaleFactor[index] >= 1)
                    scaleFactorH[index] = 2;
                else
                    scaleFactorH[index] = (float)0.5;
            }

            switch (utility.atoms[atomIndex].iatom)
            {
                case "H":
                    atom.Scale = new Vector3(
                        scaleFactorH[0] * atom.transform.localScale.x * (float)utility.scaleFactor[0],
                        scaleFactorH[1] * atom.transform.localScale.y * (float)utility.scaleFactor[1],
                        scaleFactorH[2] * atom.transform.localScale.z * (float)utility.scaleFactor[2]);
                    break;
                default:
                    atom.Scale = new Vector3(
                        atom.transform.localScale.x * (float)utility.scaleFactor[0],
                        atom.transform.localScale.y * (float)utility.scaleFactor[1],
                        atom.transform.localScale.z * (float)utility.scaleFactor[2]);
                    break;
            }
            switch (utility.atoms[atomIndex].iatom)
            {
                case "H": //white
                    atom.AtomColor = new Color(1, 1, 1, 1);
                    break;
                case "C": //dark cyan
                    atom.AtomColor = new Color(0, 1, 1, 1);
                    break;
                case "O": //red
                    atom.AtomColor = new Color(1, 0, 0, 1);
                    break;
                case "N": //blue
                    atom.AtomColor = new Color(0, 0, 1, 1);
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
                bond.BondColor = new Color(1, 1, 1, 0);
                bond.Parent = this.gameObject.name;
                bond.Scale = new float[2] { (float)utility.scaleFactor[0], (float)utility.scaleFactor[2] };
            }
        }
    } 
    // Use this for initialization
    void Start ()
    {
        utility.FilePath = filePath;
        utility.NewCenter = newCenter;
        utility.GetStructureData();
        calledUtility = true;
        RenderAtoms();
        RenderBonds();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
