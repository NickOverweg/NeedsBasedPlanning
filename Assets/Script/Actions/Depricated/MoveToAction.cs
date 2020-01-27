using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAction : GAction 
{
    //adjust the cost to make the planner favor this more or less.
    //public float cost = 1f;

    //adjust duration in case you have an action that lasts a long time
    //or if you intend to make the costs relative to the duration
    //private float duration = 1f;

    //Use this getter to run code if you want the duration to be dynamic
    //public new float Duration
    //{
    //    get { return duration; }
    //}
    
    public MoveToAction()
    {
        //add preconditions and effects to the respective dictionaries so the planner 
        //knows what to add or remove

        AddPrecondition("hasTarget", true);
        AddEffect("nearTarget", true);
    }

    public override bool IsDone()
    {
        //return false while the action is being done 
        //and true after it has comitted the effects to the state.
        return done;
        
    }

    public override void ResetVariables()
    {
        //clear any saved variables so the action can be used again.
        target = null;
        done = false;

    }

    public override bool CheckProceduralPrecondition(GAgent agent)
    {
        //Cant move to target if the agent has no target
        //save the target to check if the target changed while moving.
        if (agent.Target == null) return false;
        else if(target == null) target = agent.Target;

        //cancel out if target changed
        if (target != agent.Target) return false;
        
        return true;
    }

    public override bool Perform(GAgent agent)
    {
        if (!isInitialized) Initialize(agent);

        //run the action - return true while the action is being executed
        //return false if something makes it unable to finish executing it's task.

        float step = agent.moveSpeed * Time.deltaTime;

        agent.transform.position = Vector3.MoveTowards(agent.transform.position, target.transform.position, step);

        if (gameObject.transform.position.Equals(target.transform.position))
        {
            //agent.NearTarget = done = true;
        }
        return true;
    }

    private void Initialize(GAgent agent)
    {
        //agent.NearTarget = false;
    }
}