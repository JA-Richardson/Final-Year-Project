using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour

{
    List<NavMeshAgent> agents = new List<NavMeshAgent>();
    // Start is called before the first frame update
    void Start()
    {
       GameObject[] a = GameObject.FindGameObjectsWithTag("AI");
        foreach (GameObject obj in a)
        {
            agents.Add(obj.GetComponent<NavMeshAgent>());
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                foreach (NavMeshAgent agent in agents)
                {
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}
