using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//struct for easier data entry
[System.Serializable]
public struct Link //edges for the graph/nodes
{
    public enum direction {UNI, BI}; //uni can only move in on direction, bi can move both ways
    public GameObject node1;
    public GameObject node2;
    public direction dir;
}

public class WPManager : MonoBehaviour
{

    public GameObject[] waypoints; //array of waypoints/nodes
    public Link[] links; //array of links to show which connects and dir of travel
    public Graph graph = new Graph(); //

    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Length > 0)//if array not empty
        { 
            //loops through waypoints
            foreach (GameObject wp in waypoints)
            {
                //adds waypoints to grapg
                graph.AddNode(wp);
            }
            //loops through links
            foreach (Link l in links)
            {
                //adds edges to graph
                graph.AddEdge(l.node1, l.node2);
                //checks if link is bidirectional and adds a reutning edge
                if(l.dir == Link.direction.BI)
                {
                    graph.AddEdge(l.node2, l.node1);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        graph.debugDraw();
    }
}
