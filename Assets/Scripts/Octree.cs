using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OctreeIndex
{
	BottomLeftFront = 0,//000,
	BottomRightFront = 2,//010,
	BottomRightBack = 3,//011,
	BottomLeftBack = 1,//001,
	TopLeftFront = 4,//100,
	TopRightFront = 6,//110,
	TopRightBack = 7,//111,
	TopLeftBack = 5,//101,
}

public class Octree<TType> {
	private OctreeNode<TType> node;
	private int depth;

	public Octree(Vector3 position, float size, int depth)
	{
		this.node = new OctreeNode<TType>(position, size);
		this.node.Subdivide(depth);

;	}

	public class OctreeNode<TType>
	{
		public Vector3 position;
		public float size; 
		OctreeNode<TType>[] subNodes;
		IList<TType> value;

		public OctreeNode(Vector3 pos, float size)
		{
			this.position = pos;
			this.size = size;
		}

		public IEnumerable Nodes
		{
			get { return subNodes; }
		}

		public void Subdivide(int d)
		{
			subNodes = new OctreeNode<TType>[8];
			for (int i = 0; i < subNodes.Length; i++)
			{
				Vector3 newPos = this.position;

				newPos.y += ( ( i & 4 ) == 4 ) ? size * 0.25f : -1.0f * size*0.25f;
				newPos.x += ( ( i & 2 ) == 2 ) ? size * 0.25f : -1.0f * size * 0.25f;
				newPos.z += ( ( i & 1 ) == 1 ) ? size * 0.25f : -1.0f * size * 0.25f;

				subNodes[i] = new OctreeNode<TType>(newPos, size * 0.5f);
				if (d > 0)
				{
					subNodes[i].Subdivide(d - 1);
				}
			}
		}

		public bool IsLeaf()
		{
			return subNodes == null;
		}
	}

	private int GetIndexOfPosition(Vector3 lookupPosition, Vector3 nodePosition)
	{ 
		int index = 0;
		index |= lookupPosition.y > nodePosition.y ? 4 : 0;
		index |= lookupPosition.x > nodePosition.x ? 2 : 0;
		index |= lookupPosition.z > nodePosition.z ? 1 : 0;

		return index;

	}

	public OctreeNode<TType> GetRoot()
	{
		return node;
	}

}
