using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Blackboard singleton

public class Blackboard : MonoBehaviour
{
    public float timeOfDay;
    public Text clock;
    public Stack<GameObject> crowd = new();
    public float openTime = 6f;
    public float closeTime = 22f;

    static Blackboard instance;
    // Singleton instance of the blackboard
    public static Blackboard Instance
    {
        get
        {
            if (!instance)
            {
                Blackboard[] blackboards = GameObject.FindObjectsOfType<Blackboard>();
                if (blackboards != null)
                {
                    if (blackboards.Length == 1)
                    {
                        instance = blackboards[0];
                        return instance;
                    }
                }
                GameObject go = new("Blackboard", typeof(Blackboard));
                instance = go.GetComponent<Blackboard>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }

        set
        {
            instance = value as Blackboard;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(UpdateTime));
    }
    // Coroutine to update the time of day
    IEnumerator UpdateTime()
    {
        while (true)
        {
            timeOfDay += 0.2f;
            if (timeOfDay > 23f)
                timeOfDay = 0f;
            clock.text = timeOfDay + ":00";
            if (timeOfDay == closeTime)
                crowd.Clear();
            yield return new WaitForSeconds(1);
        }
    }
    // Register a customer to the crowd
    public bool RegisterCustomer(GameObject c)
    {
        crowd.Push(c);
        return true;
    }

    
}
