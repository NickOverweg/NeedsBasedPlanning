﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionTemplate : GAction
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


    public ActionTemplate()
    {
        //add preconditions and effects to the respective dictionaries so the planner 
        //knows what to add or remove
 
        //AddPrecondition("hasWood", false);
        //AddEffect("hasWood", true);
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
        //Check any preconditions that can possibly change 
        //from something other than this agents' actions

        return false;
    }

    public override bool Perform(GAgent agent)
    {
        //run the action - return true while the action is being executed
        //return false if something makes it unable to finish executing it's task.
        if (!isInitialized) Initialize(agent);


        return false;
    }

    private void Initialize(GAgent agent)
    {
        isInitialized = true;
        //Execute any code you only want to run once during this action here.

    }
}
