using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NanoCarbons : MonoBehaviour {
	public Transform bondPrefab;
	public Transform carbonAtom;
	public Transform hydrogenAtom;
	public Transform oxygenAtom;
	public Transform nitrogenAtom;
	public Transform SulphurAtom;
	public Transform MoAtom;

	public string filePath;
	public double[] newCenter = new double[3];
	private Utility utility = new Utility();

	public int frameCounter;
	public bool calledUtility = false;

	private int counter = 0;
	private int atomCount = 0;
	private int minAtomsPerRegion = 500;
	private int heightOctree = 0;
	private float[] maxSize = new float[3] {0,0,0};

	public Octree<int> octree;


	public void RenderAtoms()
	{
		for (int atomIndex = 0; atomIndex < utility.numAtoms; atomIndex++)
		{
			//Debug.Log(atomIndex + " , " +
			//    utility.atoms[atomIndex].iatom + " , " +
			//    utility.atoms[atomIndex].rr0[0] + " , " +
			//    utility.atoms[atomIndex].rr0[1] + " , " +
			//    utility.atoms[atomIndex].rr0[2]);


			float[] scaleFactorH = new float[3];
			for (int index = 0; index < 3; index++)
			{
				if (utility.scaleFactor[index] >= 1)
					scaleFactorH[index] = 2;
				else
					scaleFactorH[index] = (float)0.5;
			}

			switch (utility.atoms[atomIndex].iatom)
			{
				case "H": //white
					utility.atoms[atomIndex].atomInstance = Instantiate(hydrogenAtom);
					break;
				case "C": //dark cyan
					utility.atoms[atomIndex].atomInstance = Instantiate(carbonAtom);
					break;
				case "O": //red
					utility.atoms[atomIndex].atomInstance = Instantiate(oxygenAtom);
					break;
				case "N": //blue
					utility.atoms[atomIndex].atomInstance = Instantiate(nitrogenAtom);
					break;
				case "S":
					utility.atoms[atomIndex].atomInstance = Instantiate(SulphurAtom);
					break;
				case "Mo":
					utility.atoms[atomIndex].atomInstance = Instantiate(MoAtom);
					break;
				default:
					utility.atoms[atomIndex].atomInstance = Instantiate(hydrogenAtom);
					break;
			}
			utility.atoms[atomIndex].atomInstance.transform.parent = this.gameObject.transform;
			utility.atoms[atomIndex].atomInstance.transform.position = new Vector3(
				(float)utility.atoms[atomIndex].rr0[0],
				(float)utility.atoms[atomIndex].rr0[1],
				(float)utility.atoms[atomIndex].rr0[2]);
			switch (utility.atoms[atomIndex].iatom)
			{
				case "H":
					utility.atoms[atomIndex].atomInstance.transform.localScale = new Vector3(
						scaleFactorH[0] * utility.atoms[atomIndex].atomInstance.transform.localScale.x * (float)utility.scaleFactor[0],
						scaleFactorH[1] * utility.atoms[atomIndex].atomInstance.transform.localScale.y * (float)utility.scaleFactor[1],
						scaleFactorH[2] * utility.atoms[atomIndex].atomInstance.transform.localScale.z * (float)utility.scaleFactor[2]);
					break;
				default:
					utility.atoms[atomIndex].atomInstance.transform.localScale = new Vector3(
						utility.atoms[atomIndex].atomInstance.transform.localScale.x * (float)utility.scaleFactor[0],
						utility.atoms[atomIndex].atomInstance.transform.localScale.y * (float)utility.scaleFactor[0],
						utility.atoms[atomIndex].atomInstance.transform.localScale.z * (float)utility.scaleFactor[0]);
					break;
			}
		}
	}

	public void RenderBonds()
	{
		for (int bondIndex = 0; bondIndex < utility.numAtoms; bondIndex++)
		{
			//Debug.Log(utility.l[0] + "," + utility.l[1] + "," + utility.l[2] + ", " + bondIndex + "," + utility.atomBonds[bondIndex, 0]);
			if (utility.atomBonds[bondIndex, 0] == 0)
				continue;
			for (int neighbourIndex = 1; neighbourIndex <= utility.atomBonds[bondIndex, 0]; neighbourIndex++)
			{
				//Debug.Log("in");

				Transform bond = Instantiate(bondPrefab);
				//bond.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
				bond.transform.parent = this.gameObject.transform;

				Vector3 StartPosition = new Vector3(
					(float)utility.atoms[bondIndex].rr0[0],
					(float)utility.atoms[bondIndex].rr0[1],
					(float)utility.atoms[bondIndex].rr0[2]);
				Vector3 EndPosition = new Vector3(
					(float)utility.atoms[utility.atomBonds[bondIndex, neighbourIndex]].rr0[0],
					(float)utility.atoms[utility.atomBonds[bondIndex, neighbourIndex]].rr0[1],
					(float)utility.atoms[utility.atomBonds[bondIndex, neighbourIndex]].rr0[2]);

				Vector3 position = Vector3.Lerp(StartPosition, EndPosition, (float)0.5);
				bond.transform.position = position;
				bond.transform.up = EndPosition - StartPosition;

				float[] scale = new float[2] { (float)utility.scaleFactor[0], (float)utility.scaleFactor[2] };
				Vector3 newScale = bond.transform.localScale;
				newScale.x = (float)0.1 * scale[0];
				newScale.z = (float)0.1 * scale[1];
				newScale.y = Vector3.Distance(StartPosition, EndPosition) / 2;
				bond.transform.localScale = newScale;
				bond.gameObject.SetActive(false);
			}
		}
	}

	public void AssignAtomToRegion()
	{
		for (int atomIndex = 0; atomIndex < utility.numAtoms; atomIndex++)
		{
			Vector3 atomPosition = new Vector3(
				(float)utility.atoms[atomIndex].rr0[0],
				(float)utility.atoms[atomIndex].rr0[1],
				(float)utility.atoms[atomIndex].rr0[2]);
			var root = octree.GetRoot();
			while (!root.IsLeaf())
			{
				root = root.subNodes[octree.GetIndexOfPosition(atomPosition,root.position)];
			}
			root.value.Add(atomIndex);
		}

		//foreach(int index in rootCheck.value)
		//{
		//  utility.atoms[index].atomInstance.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
		//}

	}

	public void AllocateRegion()
	{
		heightOctree = (int)System.Math.Log((double)utility.numAtoms / minAtomsPerRegion, 8) + 1;

		//Debug.Log("Min: " + utility.minPos[0] + "," + utility.minPos[1] + "," + utility.minPos[2]);
		//Debug.Log("Max: " + utility.maxPos[0] + "," + utility.maxPos[1] + "," + utility.maxPos[2]);

		for (int index = 0;index < 3; index++)
		{
			maxSize[index] = (float)System.Math.Max(maxSize[index],(utility.maxPos[index] - utility.minPos[index]));
		}
		octree = new Octree<int>(new Vector3((float)newCenter[0], (float)newCenter[1], (float)newCenter[2]), maxSize, heightOctree);
		//Debug.Log("New Center: " + (float)newCenter[0] + " " + (float)newCenter[1] + " " + (float)newCenter[2]);
		AssignAtomToRegion();
		printLeaf(octree.GetRoot());

		Debug.Log("Total number of atoms in leaf " + atomCount + ", " + "\nTotal atom in system " + utility.numAtoms);
	}

	private void printLeaf(Octree<int>.OctreeNode<int> octreeNode)
	{
		if (octreeNode.IsLeaf()) { Debug.Log(octreeNode.value.Count+" , "+ counter); counter += 1;  atomCount += octreeNode.value.Count; }
		else
		{
			for (int index = 0; index < 8; index++)
			{
				printLeaf(octreeNode.subNodes[index]);
			}
		}


	}

	public void OnDrawGizmos()
	{
		DrawNode(octree.GetRoot());
	}


	private void DrawNode(Octree<int>.OctreeNode<int> node)
	{
		if (node.IsLeaf())
		{
			Gizmos.color = Color.green;
		}
		else
		{
			Gizmos.color = Color.blue;
			foreach (var subnode in node.Nodes)
			{

				DrawNode((Octree<int>.OctreeNode<int>)subnode);
			}
		}

		Gizmos.DrawWireCube(node.position,new Vector3(node.size[0], node.size[1], node.size[2]));
	}

	// Use this for initialization
	void Start()
	{
		utility.FilePath = filePath;
        utility.NewCenter = newCenter;
        utility.GetStructureData();
		calledUtility = true;
		RenderAtoms();
				AllocateRegion();

		//RenderBonds();
	}
    

    // Update is called once per frame
    void Update () {
		
	}
}
