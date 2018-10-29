using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeComponent : MonoBehaviour {

	public float[] size = new float[3] { 5,5,5};
	public int depth = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDrawGizmos()
	{
		var Octree = new Octree<int>(this.transform.position, size, depth);
		DrawNode(Octree.GetRoot());

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

		Gizmos.DrawWireCube(node.position, Vector3.one *node.size[0]);
	}

}
