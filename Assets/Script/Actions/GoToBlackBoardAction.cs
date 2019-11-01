using System;
using System.Collections.Generic;
using UnityEngine;

public class GoToBlackBoardAction : GAction
{
    //adjust the cost to make the planner favor this more or less.
    //public float cost = 1f;

    //adjust duration in case you have an action that lasts a long time
    //or if you intend to make the costs relative to the duration
    private float duration = 1f;

    //Use this getter to run code if you want the duration to be dynamic
    //public new float Duration
    //{
    //    get { return duration; }
    //}
    
    public GoToBlackBoardAction()
    {
        //add preconditions and effects to the respective dictionaries so the planner 
        //knows what to add or remove

        AddEffect("nearBlackBoard", true);
    }

    public override bool IsDone() 
    {
        //return false while the action is being done 
        //and true after it has comitted the effects to the state.

        return false;
    }

    public override void ResetVariables()
    {
        //clear any saved variables so the action can be used again.
        isInitialized = false;
    }

    public override bool CheckProceduralPrecondition(GAgent agent)
    {
        //Cant move to BB if the agent has no BB
        if (agent.BlackBoard == null) return false;

        return true;
    }

    public override bool Perform(GAgent agent)
    {
        //run the action - return true while the action is being executed
        //return false if something makes it unable to finish executing it's task.
        if (!isInitialized) Initialize(agent);

        //run the action - return true while the action is being executed
        //return false if something makes it unable to finish executing it's task.

        float step = agent.moveSpeed * Time.deltaTime;

        agent.transform.position = Vector3.MoveTowards(agent.transform.position, target.transform.position, step);

        if (gameObject.transform.position.Equals(target.transform.position))
        {
            agent.NearTarget = done = true;
        }
        return true;
        
    }

    private void Initialize(GAgent agent)
    {
        isInitialized = true;

        agent.NearTarget = false;
        agent.Target = agent.BlackBoard.gameObject;
    }
}
