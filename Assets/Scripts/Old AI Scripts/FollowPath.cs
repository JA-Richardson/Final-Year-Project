using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPath : MonoBehaviour
{

  
    public GameObject wpManager;
    GameObject[] wps;
    GameObject currentNode;
    NavMeshAgent agent;



    // Start is called before the first frame update
    void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        currentNode = wps[3];
        agent = GetComponent<NavMeshAgent>();
    }

    public void GoToHeli()
    {
        //g.AStar(currentNode, wps[1]);
        agent.SetDestination(wps[2].transform.position);
    }

    public void GoToRuin()
    {
        //g.AStar(currentNode, wps[4]);
        agent.SetDestination(wps[0].transform.position);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GoToHeli();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GoToRuin();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }
}
