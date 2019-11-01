using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GAction : MonoBehaviour
{
    private Dictionary<string, object> preconditions, effects;

    //adjust the cost to make the planner favor this more or less.
    public float cost = 1f;

    protected bool done = false;
    protected bool isInitialized = false;

    //adjust duration in case you have an action that lasts a long time
    //or if you intend to make the costs relative to the duration
    private float duration = 1f;
    
    
    public float Duration
    {
        get { return duration; }
    }

    //if the action performed needs a target, set it here.
    public GameObject target;

    public GAction()
    {
        effects = new Dictionary<string, object>();
        preconditions = new Dictionary<string, object>();
    }

    //reset variables for any new planning
    public abstract void ResetVariables();

    public abstract bool IsDone();

    //procedurally keeps checking if the action can run, if the action needs it.
    //only use this to check if it is even possible to run an action in the first place.
    public abstract bool CheckProceduralPrecondition(GAgent agent);

    //run the action - return value should be true if the action is performed succesfully and false if something happened that makes it no longer be able to perform its action.
    public abstract bool Perform(GAgent agent);

    public void AddPrecondition(string key, object value)
    {
        preconditions.Add(key, value);
    }

    public void RemovePrecondition(string key)
    {
        if (preconditions.ContainsKey(key))
            preconditions.Remove(key);
    }

    public void AddEffect(string key, object value)
    {
        effects.Add(key, value);
    }

    public void RemoveEffect(string key)
    {
        if (preconditions.ContainsKey(key))
            preconditions.Remove(key);
    }

    public Dictionary<string, object> Preconditions
    {
        get { return preconditions; }
    }
    public Dictionary<string, object> Effects
    {
        get { return effects; }
    }
}
