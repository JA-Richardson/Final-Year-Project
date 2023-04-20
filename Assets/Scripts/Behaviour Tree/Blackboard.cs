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
    public int openTime = 6;
    public int closeTime = 22;

    static Blackboard instance;
    public static Blackboard Instance
    {
        get 
        { 
            if (!instance)
            {
                Blackboard[] blackboards = GameObject.FindObjectsOfType<Blackboard>();
                if(blackboards != null)
                {
                    if(blackboards.Length ==1)
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
        StartCoroutine("UpdateTime");
    }

    IEnumerator UpdateTime()
    {
        while(true)
        {
            timeOfDay++;
            if (timeOfDay > 23)
                timeOfDay = 0;
            clock.text = timeOfDay + ":00";
            if(timeOfDay == closeTime)
                crowd.Clear();
            yield return new WaitForSeconds(1);
        }
    }

    public bool RegisterCustomer(GameObject c)
    {
        crowd.Push(c);
        return true;
    }

    //public void DeregisterCustomer()
    //{
    //    crowd = null;
    //}
}
