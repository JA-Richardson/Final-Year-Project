using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{

    public NavMeshAgent agent;
    public GameObject player;
    
    //visual system
    public bool enemyInLoS = false;
    public float fov = 50f;
    public float los = 45f;

    //Memory
    private bool playerInMemory = false;
    public float memoryTime = 5f;
    private float memoryTimer = 0f;

    //Hearing system
    Vector3 noisePos;
    public bool playerInHearing = false;
    public float hearingRange = 10f;
    public float searchSpeed = 3f;
    private bool canSearch = false;
    private float searchTimer = 0f;
    public float searchTime = 5f;


    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(agent.remainingDistance < 2)
        //{
        //    agent.SetDestination(new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
        //}
        Sight();
    }

    void Sight()
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < fov * 0.5f)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position + transform.up, direction.normalized * los, Color.green);
            if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, los))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    enemyInLoS = true;
                    playerInMemory = true;
                    memoryTimer = 0f;
                    Debug.Log("Player in LoS");
                }
                else
                {
                    enemyInLoS = false;
                    Debug.Log("Player not in LoS");
                }
            }
        }
        else
        {
            enemyInLoS = false;
            Debug.Log("Player not in LoS");
        }
    }
    
    //void Hearing()
    //{
    //    if (playerInHearing)
    //    {
    //        agent.SetDestination(noisePos);
    //        agent.speed = searchSpeed;
    //        canSearch = true;
    //    }
    //    else
    //    {
    //        agent.speed = 3f;
    //    }

    //    if (canSearch)
    //    {
    //        searchTimer += Time.deltaTime;
    //        if (searchTimer >= searchTime)
    //        {
    //            canSearch = false;
    //            searchTimer = 0f;
    //        }
    //    }
    //}
}

