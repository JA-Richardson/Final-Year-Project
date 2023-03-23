using UnityEngine;
using System.Collections;

public class Edge
{
	public PathNode startNode;
	public PathNode endNode;
	
	public Edge(PathNode from, PathNode to)
	{
		startNode = from;
		endNode = to;
	}
}
