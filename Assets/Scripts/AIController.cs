using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{

    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance < 2)
        {
            agent.SetDestination(new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
        }
    }
    
}
