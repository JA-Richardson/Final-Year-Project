using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnvironmentManager
{
    private static EnvironmentManager instance;
    private List<GameObject> patrolPoints = new List<GameObject>();
    public List<GameObject> PatrolPoints { get { return patrolPoints; } }

    public static EnvironmentManager Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new EnvironmentManager();
                instance.PatrolPoints.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint"));
            }
            return instance;
        }
    }
}
