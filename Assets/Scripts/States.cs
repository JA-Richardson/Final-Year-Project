using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class States
{
    public enum STATE { IDLE, PATROL, CHASE, ATTACK, DEAD, FLEE };

    public enum EVENT { ENTER, UPDATE, EXIT };

    public STATE name;
    protected EVENT stage;
    public GameObject NPC;
    public Transform player;
    protected NavMeshAgent agent;
    public States nextState;

    public float visDist = 20.0f;
    float visAngle = 50.0f;
    float shootDist = 10.0f;

    public States(GameObject sNPC, UnityEngine.AI.NavMeshAgent sAgent, Transform sPlayer)
    {
        NPC = sNPC;
        agent = sAgent;
        player = sPlayer;
        stage = EVENT.ENTER;
    }

    public virtual void Enter()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Update()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Exit()
    {
        stage = EVENT.EXIT;
    }

    public States Process()
    {
        if (stage == EVENT.ENTER)
        {
            Enter();
        }
        if (stage == EVENT.UPDATE)
        {
            Update();
        }
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public bool CanSeePlayer()
    {
        Vector3 dir = player.position - NPC.transform.position;
        float angle = Vector3.Angle(dir, NPC.transform.forward);
        if (dir.magnitude < visDist && angle < visAngle)
        {
            //Debug.Log("Can see");
            return true;
        }
        return false;
    }

    public bool IsPlayerBehind()
    {
        Vector3 dir = NPC.transform.position - player.position;
        float angle = Vector3.Angle(dir, NPC.transform.forward);
        if (dir.magnitude < 2 && angle < 30)
        {
            return true;
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 dir = player.position - NPC.transform.position;
        if(dir.magnitude < shootDist)
        {
            Debug.Log("Can attack");
            return true;
        }
        return false;
    }
}

public class Flee : States 
{
    GameObject safeLocation;
    public Flee(GameObject sNPC, UnityEngine.AI.NavMeshAgent sAgent, Transform sPlayer) : base(sNPC, sAgent, sPlayer)
    {
        name = STATE.FLEE;
        safeLocation = GameObject.FindGameObjectWithTag("Safe");
    }

    public override void Enter()
    {
        agent.isStopped = false;
        agent.speed = 6;
        agent.SetDestination(safeLocation.transform.position);
        base.Enter();
    }

    public override void Update()
    {
        if(agent.remainingDistance < 1)
        {
            nextState = new Idle(NPC, agent, player);
            stage = EVENT.EXIT;
        }
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

}






